// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;
using HathoraCloud.Utils;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Server.ApiWrapper
{
    /// <summary>
    /// Operations that allow you create and manage your builds.
    /// Build Concept | https://hathora.dev/docs/concepts/hathora-entities#build
    /// API Docs | https://hathora.dev/api#tag/BuildV1
    /// </summary>
    public class HathoraServerBuildApiWrapper : HathoraServerApiWrapperBase
    {
        protected BuildV1SDK BuildApi { get; }
        private volatile bool uploading;

        public HathoraServerBuildApiWrapper(
            HathoraCloudSDK _hathoraSdk,
            HathoraServerConfig _hathoraServerConfig)
            : base(_hathoraSdk, _hathoraServerConfig)
        {
            Debug.Log($"[{nameof(HathoraServerBuildApiWrapper)}.Constructor] " +
                "Initializing Server API...");
            
            this.BuildApi = _hathoraSdk.BuildV1 as BuildV1SDK;
        }
        
        
        #region Server Build Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateBuildAsync` to request an cloud build (_tarball upload).
        /// </summary>
        /// <param name="_buildTag">
        /// Build tag to associate a version with a build. It is accessible via getBuildInfo().
        /// </param>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns Build on success >> Pass this info to RunCloudBuildAsync()</returns>
        public async Task<Build> CreateBuildAsync(
            string _buildTag = null,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApiWrapper)}.{nameof(CreateBuildAsync)}]";
            
            // Prep request
            HathoraCloud.Models.Shared.CreateBuildRequest createBuildRequest = new()
            {
                BuildTag = _buildTag,
            };
            
            HathoraCloud.Models.Operations.CreateBuildRequest createBuildRequestWrapper = new()
            {
                AppId = base.AppId,
                CreateBuildRequestValue = createBuildRequest,
            };
            
            // Get response async =>
            CreateBuildResponse createCloudBuildResponse = null;
            
            try
            {
                createCloudBuildResponse = await BuildApi.CreateBuildAsync(
                    new CreateBuildSecurity { HathoraDevToken = base.HathoraDevToken }, // TODO: Redundant - already has Auth0 from constructor via SDKConfig.HathoraDevToken
                    createBuildRequestWrapper);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(BuildApi.CreateBuildAsync)} => Error: {e.Message}");
                return null; // fail
            }

            Debug.Log($"{logPrefix} Success: <color=yellow>" +
                $"{nameof(createCloudBuildResponse)}: {base.ToJson(createCloudBuildResponse)}</color>");

            return createCloudBuildResponse.Build;
        }

        /// <summary>
        /// Wrapper for `RunBuildAsync` to upload the _tarball after calling CreateBuildAsync().
        /// (!) Temporarily sets the Timeout to 15min (900k ms) to allow for large builds.
        /// (!) After this is done, you probably want to call GetBuildInfoAsync().
        /// </summary>
        /// <param name="_buildId"></param>
        /// <param name="_pathToTarGzBuildFile">Ensure path is normalized</param>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns streamLogs (List of chunks) on success</returns>
        public async Task<List<string>> RunCloudBuildAsync(
            int _buildId, 
            string _pathToTarGzBuildFile,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApiWrapper)}.{nameof(RunCloudBuildAsync)}]";

            #region Timeout Workaround // (!) TODO: SDKConfig.Timer no longer exists in the new SDK: How to raise timeout now?
            Debug.Log($"{logPrefix} (!) TODO: The new SDK can no longer set (raise) Timeout");
            // // Temporarily sets the Timeout to 15min (900k ms) to allow for large builds.
            // // Since Timeout has no setter, we need to temporarily make a new api instance.
            // SDKConfig highTimeoutConfig = HathoraUtils.DeepCopy(base.HathoraSdkConfig);
            // highTimeoutConfig.Timeout = (int)TimeSpan.FromMinutes(15).TotalMilliseconds;
            //
            // // TODO: Overloading VxSDK constructor with nulls, for now, until we know how to properly construct
            // SpeakeasyHttpClient httpClient = null;
            // string serverUrl = null;
            // BuildV1SDK highTimeoutBuildApi = new(
            //     httpClient,
            //     httpClient, 
            //     serverUrl,
            //     highTimeoutConfig);
            #endregion // Timeout Workaround
         
            // Prep upload request
            RunBuildRequestBodyFile requestFile = new()
            {
                File = _pathToTarGzBuildFile,
                Content = await File.ReadAllBytesAsync(_pathToTarGzBuildFile, _cancelToken),
            };
            
            RunBuildRequestBody runBuildRequest = new()
            {
                File = requestFile,
            };
            
            RunBuildRequest runBuildRequestWrapper = new()
            {
                AppId = base.AppId, // TODO: SDK already has Config via constructor - redundant
                BuildId = _buildId,
                RequestBody = runBuildRequest,
            };
                
            // Get response async =>
            RunBuildResponse runBuildResponse = null;
            uploading = true;

            try
            {
                _ = startProgressNoticeAsync(); // !await

                // TODO: No more need for file stream in the new SDK?
                // await using FileStream fileStream = new(
                //     _pathToTarGzBuildFile,
                //     FileMode.Open,
                //     FileAccess.Read);

                // // (!) TODO: SDKConfig.Timer no longer exists in the new SDK: How to raise timeout now?
                // runBuildResponse = await highTimeoutBuildApi.RunBuildAsync(
                //     new RunBuildSecurity { HathoraDevToken = base.HathoraDevToken }, // TODO: Redundant - already has Auth0 from constructor via SDKConfig.HathoraDevToken
                //     runBuildRequestWrapper);

                runBuildResponse = await BuildApi.RunBuildAsync(
                    new RunBuildSecurity { HathoraDevToken = base.HathoraDevToken }, // TODO: Redundant - already has Auth0 from constructor via SDKConfig.HathoraDevToken
                    runBuildRequestWrapper);
            }
            catch (TaskCanceledException)
            {
                Debug.Log($"{logPrefix} Task Cancelled || timed out");
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(BuildApi.RunBuildAsync)} => Error: {e.Message}");
                return null; // fail
            }
            finally
            {
                uploading = false;
            }

            Debug.Log($"{logPrefix} Done - to know if success, call BuildApi.RunBuild");

            // (!) Unity, by default, truncates logs to 1k chars (callstack-inclusive).
            string encodedLogs = await readStreamToStringAsync(runBuildResponse?.RunBuild200TextPlainBinaryString);
            List<string> logChunks = onRunCloudBuildDone(encodedLogs);
            
            return logChunks;  // streamLogs 
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_memoryQueueBufferStream">From runBuildResponse.RunBuild200TextPlainBinaryString</param>
        /// <returns></returns>
        private static async Task<string> readStreamToStringAsync(MemoryQueueBufferStream _memoryQueueBufferStream)
        {
            if (_memoryQueueBufferStream == null)
                return string.Empty;

            await using (_memoryQueueBufferStream)
            {
                byte[] buffer = new byte[_memoryQueueBufferStream.Length];
                await _memoryQueueBufferStream.ReadAsync(buffer.AsMemory(start:0, buffer.Length));
                
                return Encoding.UTF8.GetString(buffer);
            }
        }

        private async Task startProgressNoticeAsync()
        {
            TimeSpan delayTimespan = TimeSpan.FromSeconds(5);
            StringBuilder sb = new("...");
            
            while (uploading)
            {
                Debug.Log($"[HathoraServerBuild] Uploading {sb}");
                
                await Task.Delay(delayTimespan);
                sb.Append(".");
            }
        }

        /// <summary>
        /// DONE - not necessarily success. Log stream every 500 lines
        /// (!) Unity, by default, truncates logs to 1k chars (including callstack).
        /// </summary>
        /// <param name="_cloudRunBuildResultLogsStr"></param>
        /// <returns>List of log chunks</returns>
        private static List<string> onRunCloudBuildDone(string _cloudRunBuildResultLogsStr)
        {
            // Split string into lines
            string[] linesArr = _cloudRunBuildResultLogsStr.Split(new[] 
                { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List<string> lines = new (linesArr);

            // Group lines into chunks of 500
            const int chunkSize = 500;
            for (int i = 0; i < lines.Count; i += chunkSize)
            {
                IEnumerable<string> chunk = lines.Skip(i).Take(chunkSize);
                string chunkStr = string.Join("\n", chunk);
                Debug.Log($"[HathoraServerBuildApiWrapper.onRunCloudBuildDone] result == chunk starting at line {i}: " +
                    $"\n<color=yellow>{chunkStr}</color>");
            }

            return lines;
        }

        /// <summary>
        /// Wrapper for `RunBuildAsync` to upload the _tarball after calling 
        /// </summary>
        /// <param name="_buildId"></param>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns byte[] on success</returns>
        public async Task<Build> GetBuildInfoAsync(
            int _buildId,
            CancellationToken _cancelToken)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApiWrapper)}.{nameof(GetBuildInfoAsync)}]";

            // Prepare request
            GetBuildInfoRequest getBuildInfoRequest = new()
            {
                AppId = base.AppId, // TODO: SDK already has Config via constructor - redundant
                BuildId = _buildId,
            };
            
            // Get response async =>
            GetBuildInfoResponse getBuildInfoResponse = null;
            
            try
            {
                getBuildInfoResponse = await BuildApi.GetBuildInfoAsync(
                    new GetBuildInfoSecurity { HathoraDevToken = base.HathoraDevToken }, // TODO: Redundant - already has Auth0 from constructor via SDKConfig.HathoraDevToken
                    getBuildInfoRequest);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(BuildApi.GetBuildInfoAsync)} => Error: {e.Message}");
                return null; // fail
            }

            Build build = getBuildInfoResponse.Build;
            bool isSuccess = build is { Status: BuildStatus.Succeeded };
            
            Debug.Log($"{logPrefix} Success? {isSuccess}, <color=yellow>" +
                $"{nameof(getBuildInfoResponse)}: {base.ToJson(getBuildInfoResponse)}</color>");

            return build;
        }
        #endregion // Server Build Async Hathora SDK Calls
    }
}
