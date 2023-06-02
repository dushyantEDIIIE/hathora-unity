// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Cloud.Sdk.Api;
using Hathora.Cloud.Sdk.Client;
using Hathora.Cloud.Sdk.Model;
using Hathora.Core.Scripts.Common.Utils;
using Hathora.Core.Scripts.Server.Models;
using UnityEngine;

namespace Hathora.Core.Scripts.Server.ApiWrapper
{
    public class HathoraServerDeployApi : HathoraServerApiBase
    {
        private readonly DeploymentV1Api deployApi;
        private HathoraDeployOpts deployOpts => HathoraServerConfig.HathoraDeployOpts;

        
        /// <summary>
        /// </summary>
        /// <param name="_hathoraServerConfig"></param>
        /// <param name="_hathoraSdkConfig">
        /// Passed along to base for API calls as `HathoraSdkConfig`; potentially null in child.
        /// </param>
        public HathoraServerDeployApi( 
            HathoraServerConfig _hathoraServerConfig,
            Configuration _hathoraSdkConfig = null)
            : base(_hathoraServerConfig, _hathoraSdkConfig)
        {
            Debug.Log("[HathoraServerDeployApi] Initializing API...");
            this.deployApi = new DeploymentV1Api(base.HathoraSdkConfig);
        }
        
        
        #region Server Deploy Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateDeploymentAsync` to upload and deploy a cloud deploy to Hathora.
        /// </summary>
        /// <param name="buildId"></param>
        /// <param name="_cancelToken"></param>
        /// <returns>Returns Deployment on success</returns>
        public async Task<Deployment> CreateDeploymentAsync(
            double buildId,
            CancellationToken _cancelToken = default)
        {
            List<ContainerPort> extraContainerPorts = 
                deployOpts.AdvancedDeployOpts.GetExtraContainerPorts();
            
            // (!) Throws on constructor Exception
            DeploymentConfig deployConfig = null;
            try
            {
                deployConfig = new DeploymentConfig(
                    parseEnvFromConfig() ?? new List<DeploymentConfigEnvInner>(),
                    deployOpts.RoomsPerProcess, 
                    deployOpts.SelectedPlanName, 
                    extraContainerPorts,
                    deployOpts.ContainerPortWrapper.TransportType,
                    deployOpts.ContainerPortWrapper.PortNumber
                );
                
                Debug.Log("[HathoraServerDeploy.CreateDeploymentAsync] " +
                    $"deployConfig == <color=yellow>{deployConfig.ToJson()}</color>");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error: {e}");
                throw;
            }

            Deployment createDeploymentResult;
            try
            {
                createDeploymentResult = await deployApi.CreateDeploymentAsync(
                    AppId,
                    buildId,
                    deployConfig,
                    _cancelToken);
            }
            catch (ApiException apiErr)
            {
                HandleServerApiException(
                    nameof(HathoraServerDeployApi),
                    nameof(CreateDeploymentAsync), 
                    apiErr);
                return null;
            }

            Debug.Log($"[HathoraServerDeployApi] Success - " +
                $"BuildId: '{createDeploymentResult?.BuildId}', " +
                $"DeployId: '{createDeploymentResult?.DeploymentId}");

            return createDeploymentResult;
        }

        /// <summary>
        /// Wrapper for `CreateDeploymentAsync` to upload and deploy a cloud deploy to Hathora.
        /// </summary>
        /// <param name="_cancelToken"></param>
        /// <returns>Returns Deployment on success</returns>
        public async Task<List<Deployment>> GetDeploymentsAsync(
            CancellationToken _cancelToken = default)
        {
            List<Deployment> getDeploymentsResult;
            try
            {
                getDeploymentsResult = await deployApi.GetDeploymentsAsync(AppId, _cancelToken);
            }
            catch (ApiException apiErr)
            {
                HandleServerApiException(
                    nameof(HathoraServerDeployApi),
                    nameof(GetDeploymentsAsync), 
                    apiErr);
                return null;
            }

            Debug.Log($"[HathoraServerDeployApi] num: '{getDeploymentsResult?.Count}'");

            return getDeploymentsResult;
        }
        #endregion // Server Deploy Async Hathora SDK Calls


        /// <summary>
        /// Convert List of HathoraEnvVars to List of DeploymentConfigEnvInner.
        /// </summary>
        /// <returns>
        /// Returns empty list (NOT null) on empty, since Env is required for DeploymentConfig.
        /// </returns>
        private List<DeploymentConfigEnvInner> parseEnvFromConfig()
        {
            // Validate
            List<HathoraEnvVars> envVars = HathoraServerConfig.HathoraDeployOpts.EnvVars;
            if (envVars == null || envVars.Count == 0) 
                return new List<DeploymentConfigEnvInner>();
            
            // Parse
            return HathoraServerConfig.HathoraDeployOpts.EnvVars.Select(env => 
                new DeploymentConfigEnvInner(env.Key, env.StrVal)).ToList();
        }
    }
}
