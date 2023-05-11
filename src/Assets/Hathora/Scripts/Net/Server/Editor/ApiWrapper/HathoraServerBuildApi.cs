// Created by dylan@hathora.dev

using System;
using System.IO;
using System.Threading.Tasks;
using Hathora.Cloud.Sdk.Api;
using Hathora.Cloud.Sdk.Client;
using Hathora.Cloud.Sdk.Model;
using UnityEngine;

namespace Hathora.Scripts.Net.Server.Editor.ApiWrapper
{
    public class HathoraServerBuildApi : HathoraServerApiBase
    {
        private BuildV1Api buildApi;
        
        public override void Init(
            Configuration _hathoraSdkConfig, 
            HathoraServerConfig _hathoraServerConfig)
        {
            Debug.Log("[HathoraServerBuildApi] Initializing API...");
            base.Init(_hathoraSdkConfig, _hathoraServerConfig);
            this.buildApi = new BuildV1Api(_hathoraSdkConfig);
        }
        
        
        
        #region Server Build Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateBuildAsync` to upload the tarball.
        /// </summary>
        /// <param name="_buildId"></param>
        /// <param name="tarball"></param>
        /// <returns>Returns byte[] on success</returns>
        public async Task<byte[]> RunCloudBuild(double _buildId, Stream tarball)
        {
            byte[] cloudRunBuildResultByteArr;
            
            try
            {
                cloudRunBuildResultByteArr = await buildApi.RunBuildAsync(
                    hathoraServerConfig.AppId,
                    _buildId,
                    tarball);
            }
            catch (Exception e)
            {
                Debug.LogError($"[HathoraServerBuildApi.RunCloudBuild]" +
                    $"**ERR (RunBuildAsync): {e.Message}");
                await Task.FromException<Exception>(e);
                return null;
            }

            Debug.Log($"[HathoraServerBuildApi] result == " +
                $"isSuccess? '{cloudRunBuildResultByteArr is { Length: > 0 }}");

            return cloudRunBuildResultByteArr;
        }
        #endregion // Server Build Async Hathora SDK Calls
    }
}
