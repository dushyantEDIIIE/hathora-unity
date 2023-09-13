// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using HathoraSdk;
using HathoraSdk.Models.Operations;
using HathoraSdk.Models.Shared;
using HathoraSdk.Utils;
using UnityEngine;
using CreateBuildRequest = HathoraSdk.Models.Shared.CreateBuildRequest;

namespace Hathora.Core.Scripts.Runtime.Server.ApiWrapper
{
    public class HathoraServerBuildApi : HathoraServerApiWrapperBase
    {
        private readonly BuildV1SDK buildApi;
        private volatile bool uploading;

        /// <summary>
        /// </summary>
        /// <param name="_hathoraServerConfig"></param>
        /// <param name="_hathoraSdkConfig">
        /// Set in base as `HathoraSdkConfig`; potentially null in child.
        /// </param>
        public HathoraServerBuildApi(
            HathoraServerConfig _hathoraServerConfig,
            SDKConfig _hathoraSdkConfig = null) 
            : base(_hathoraServerConfig, _hathoraSdkConfig)
        {
            Debug.Log("[HathoraServerBuildApi] Initializing API...");
            
            // TODO: Overloading VxSDK constructor with nulls, for now, until we know how to properly construct
            SpeakeasyHttpClient httpClient = null;
            string serverUrl = null;
            this.buildApi = new BuildV1SDK(
                httpClient,
                httpClient, 
                serverUrl,
                HathoraSdkConfig);
        }
        
        
        
        #region Server Build Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateBuildAsync` to request an cloud build (_tarball upload).
        /// </summary>
        /// <param name="_cancelToken"></param>
        /// <returns>Returns Build on success >> Pass this info to RunCloudBuildAsync()</returns>
        public async Task<Build> CreateBuildAsync(CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApi)}.{nameof(CreateBuildAsync)}]";

            HathoraSdk.Models.Operations.CreateBuildRequest createBuildRequest = new()
            {
                AppId = base.AppId,
                CreateBuildRequestValue = 
            };

            CreateBuildResponse createCloudBuildResponse = null;
            
            try
            {
                createCloudBuildResponse = await buildApi.CreateBuildAsync(
                    new CreateBuildSecurity { Auth0 = base.ServerAuth0 },
                    createBuildRequest);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(buildApi.CreateBuildAsync)} => Error: {e.Message}");
                return null; // fail
            }

            Debug.Log($"{logPrefix} Success: <color=yellow>" +
                $"{nameof(createCloudBuildResponse)}: {ToJson(createCloudBuildResponse)}</color>");

            return createCloudBuildResponse;
        }

        /// <summary>
        /// Wrapper for `RunBuildAsync` to upload the _tarball after calling CreateBuildAsync().
        /// (!) Temporarily sets the Timeout to 15min (900k ms) to allow for large builds.
        /// (!) After this is done, you probably want to call GetBuildInfoAsync().
        /// </summary>
        /// <param name="_buildId"></param>
        /// <param name="_pathToTarGzBuildFile">Ensure path is normalized</param>
        /// <param name="_cancelToken"></param>
        /// <returns>Returns streamLogs (List of chunks) on success</returns>
        public async Task<List<string>> RunCloudBuildAsync(
            double _buildId, 
            string _pathToTarGzBuildFile,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApi)}.{nameof(RunCloudBuildAsync)}]";
            byte[] cloudRunBuildResultLogsStream = null;

            #region Timeout Workaround
            // (!) TODO: SDKConfig.Timer no longer exists in the new SDK: Verify that Timeout is used!
            // Temporarily sets the Timeout to 15min (900k ms) to allow for large builds.
            // Since Timeout has no setter, we need to temporarily make a new api instance.
            SDKConfig highTimeoutConfig = HathoraUtils.DeepCopy(base.HathoraSdkConfig);
            highTimeoutConfig.Timeout = (int)TimeSpan.FromMinutes(15).TotalMilliseconds;
            
            // TODO: Overloading VxSDK constructor with nulls, for now, until we know how to properly construct
            SpeakeasyHttpClient httpClient = null;
            string serverUrl = null;
            BuildV1SDK highTimeoutBuildApi = new(
                httpClient,
                httpClient, 
                serverUrl,
                highTimeoutConfig);
            #endregion // Timeout Workaround
         
            uploading = true;

            try
            {
                _ = startProgressNoticeAsync(); // !await

                await using FileStream fileStream = new(
                    _pathToTarGzBuildFile,
                    FileMode.Open,
                    FileAccess.Read);

                // (!) Using the `highTimeoutBuildApi` workaround instance here
                cloudRunBuildResultLogsStream = await highTimeoutBuildApi.RunBuildAsync(
                    HathoraServerConfig.HathoraCoreOpts.AppId,
                    _buildId,
                    fileStream,
                    _cancelToken);
            }
            catch (TaskCanceledException)
            {
                Debug.Log($"{logPrefix} Task Cancelled || timed out");
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(highTimeoutBuildApi.RunBuildAsync)} => Error: {e.Message}");
                return null; // fail
            }
            finally
            {
                uploading = false;
            }

            Debug.Log($"{logPrefix} Done - to know if success, call buildApi.RunBuild");

            // (!) Unity, by default, truncates logs to 1k chars (including callstack).
            string encodedLogs = Encoding.UTF8.GetString(cloudRunBuildResultLogsStream);
            List<string> logChunks = onRunCloudBuildDone(encodedLogs);
            
            return logChunks;  // streamLogs 
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
                Debug.Log($"[HathoraServerBuildApi.onRunCloudBuildDone] result == chunk starting at line {i}: " +
                    $"\n<color=yellow>{chunkStr}</color>");
            }

            return lines;
        }

        /// <summary>
        /// Wrapper for `RunBuildAsync` to upload the _tarball after calling 
        /// </summary>
        /// <param name="_buildId"></param>
        /// <param name="_cancelToken"></param>
        /// <returns>Returns byte[] on success</returns>
        public async Task<Build> GetBuildInfoAsync(
            double _buildId,
            CancellationToken _cancelToken)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApi)}.{nameof(GetBuildInfoAsync)}]";
            
            Build getBuildInfoResult;
            
            try
            {
                getBuildInfoResult = await buildApi.GetBuildInfoAsync(
                    HathoraServerConfig.HathoraCoreOpts.AppId,
                    _buildId,
                    _cancelToken);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(buildApi.GetBuildInfoAsync)} => Error: {e.Message}");
                return null; // fail
            }

            // // TODO: `StatusEnum` to check for `Active` status no longer exists in the new SDK - how to check for status?
            // bool isSuccess = getBuildInfoResult is { Status: Build.StatusEnum.Succeeded };
            bool isSuccess = true;
            
            Debug.Log($"{logPrefix} Success? {isSuccess}, <color=yellow>" +
                $"{nameof(getBuildInfoResult)}: {ToJson(getBuildInfoResult)}</color>");

            return getBuildInfoResult;
        }
        #endregion // Server Build Async Hathora SDK Calls
    }
}
