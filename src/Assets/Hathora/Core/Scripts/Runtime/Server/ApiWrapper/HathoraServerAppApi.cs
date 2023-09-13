// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HathoraSdk;
using HathoraSdk.Models.Operations;
using HathoraSdk.Models.Shared;
using Debug = UnityEngine.Debug;

namespace Hathora.Core.Scripts.Runtime.Server.ApiWrapper
{
    public class HathoraServerAppApi : HathoraServerApiWrapperBase
    {
        private readonly AppV1SDK appApi;

        
        /// <summary>
        /// </summary>
        /// <param name="_hathoraServerConfig"></param>
        /// <param name="_hathoraSdkConfig">
        /// Passed along to base for API calls as `HathoraSdkConfig`; potentially null in child.
        /// </param>
        public HathoraServerAppApi(
            HathoraServerConfig _hathoraServerConfig,
            SDKConfig _hathoraSdkConfig = null)
            : base(_hathoraServerConfig, _hathoraSdkConfig)
        { 
            Debug.Log("[HathoraServerAppApi] Initializing API..."); 
            
            // TODO: Manually init w/out constructor, or add constructor support to model
            // TODO: `Configuration` is missing in the new SDK - cleanup, if permanently gone.
            this.appApi = new AppV1SDK(base.HathoraSdkConfig);
        }
        
        
        #region Server App Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateAppAsync` to upload and app a cloud app to Hathora.
        /// </summary>
        /// <param name="_cancelToken">TODO: This may be implemented in the future</param>
        /// <returns>Returns App on success</returns>
        public async Task<List<ApplicationWithDeployment>> GetAppsAsync(
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerAppApi)}.{nameof(GetAppsAsync)}]";
            
            GetAppsResponse getAppsResponse = null;
            
            try
            {
                GetAppsSecurity security = TODO;
                getAppsResponse = await appApi.GetAppsAsync(security);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(appApi.GetAppsAsync)} => Error: {e.Message}");
                return null; // fail
            }

            // Get inner response to return -> Log/Validate
            List<ApplicationWithDeployment> applicationWithDeployment = getAppsResponse.ApplicationWithDeployments;
            Debug.Log($"[HathoraServerAppApi.GetAppsAsync] num: '{applicationWithDeployment?.Count ?? 0}'");
            
            return applicationWithDeployment;
        }
        #endregion // Server App Async Hathora SDK Calls
    }
}
