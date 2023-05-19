// Created by dylan@hathora.dev

using System.IO;
using System.Threading.Tasks;
using Hathora.Cloud.Sdk.Api;
using Hathora.Cloud.Sdk.Client;
using Hathora.Cloud.Sdk.Model;
using Hathora.Scripts.Net.Common;
using UnityEngine;

namespace Hathora.Scripts.SdkWrapper.Editor.ApiWrapper
{
    public class HathoraServerBuildApi : HathoraServerApiBase
    {
        private readonly BuildV1Api buildApi;
        
        public HathoraServerBuildApi(
            Configuration _hathoraSdkConfig, 
            NetHathoraConfig _netHathoraConfig) 
            : base(_hathoraSdkConfig, _netHathoraConfig)
        {
            Debug.Log("[HathoraServerBuildApi] Initializing API...");
            this.buildApi = new BuildV1Api(_hathoraSdkConfig);
        }
        
        
        
        #region Server Build Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateBuildAsync` to request an cloud build (tarball upload).
        /// </summary>
        /// <returns>Returns Build on success >> Pass this info to RunCloudBuildAsync()</returns>
        public async Task<Build> CreateBuildAsync()
        {
            Build createCloudBuildResult;
            
            try
            {
                createCloudBuildResult = await buildApi.CreateBuildAsync(
                    NetHathoraConfig.HathoraCoreOpts.AppId);
            }
            catch (ApiException apiException)
            {
                HandleServerApiException(
                    nameof(HathoraServerBuildApi),
                    nameof(CreateBuildAsync), 
                    apiException);
                return null;
            }

            Debug.Log($"[HathoraServerBuildApi.RunCloudBuildAsync] result == " +
                $"BuildId: '{createCloudBuildResult.BuildId}, " +
                $"Status: {createCloudBuildResult.Status}");

            return createCloudBuildResult;
        }
        
        /// <summary>
        /// Wrapper for `RunBuildAsync` to upload the tarball after calling 
        /// </summary>
        /// <param name="_buildId"></param>
        /// <param name="tarball"></param>
        /// <returns>Returns byte[] on success</returns>
        public async Task<byte[]> RunCloudBuildAsync(double _buildId, Stream tarball)
        {
            byte[] cloudRunBuildResultByteArr;
            
            try
            {
                cloudRunBuildResultByteArr = await buildApi.RunBuildAsync(
                    NetHathoraConfig.HathoraCoreOpts.AppId,
                    _buildId,
                    tarball);
            }
            catch (ApiException apiException)
            {
                HandleServerApiException(
                    nameof(HathoraServerBuildApi),
                    nameof(RunCloudBuildAsync), 
                    apiException);
                return null;
            }

            Debug.Log($"[HathoraServerBuildApi.RunCloudBuildAsync] result == " +
                $"isSuccess? '{cloudRunBuildResultByteArr is { Length: > 0 }}");

            return cloudRunBuildResultByteArr;
        }
        #endregion // Server Build Async Hathora SDK Calls
    }
}
