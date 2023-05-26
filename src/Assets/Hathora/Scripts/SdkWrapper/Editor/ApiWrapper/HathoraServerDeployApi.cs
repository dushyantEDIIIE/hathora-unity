// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hathora.Cloud.Sdk.Api;
using Hathora.Cloud.Sdk.Client;
using Hathora.Cloud.Sdk.Model;
using Hathora.Scripts.Net.Common;
using Hathora.Scripts.SdkWrapper.Models;
using Hathora.Scripts.Utils;
using UnityEngine;

namespace Hathora.Scripts.SdkWrapper.Editor.ApiWrapper
{
    public class HathoraServerDeployApi : HathoraServerApiBase
    {
        private readonly DeploymentV1Api deployApi;
        private HathoraDeployOpts deployOpts => NetHathoraConfig.HathoraDeployOpts;

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_netHathoraConfig"></param>
        /// <param name="_hathoraSdkConfig">
        /// Passed along to base for API calls as `HathoraSdkConfig`; potentially null in child.
        /// </param>
        public HathoraServerDeployApi( 
            NetHathoraConfig _netHathoraConfig,
            Configuration _hathoraSdkConfig = null)
            : base(_netHathoraConfig, _hathoraSdkConfig)
        {
            Debug.Log("[HathoraServerDeployApi] Initializing API...");
            this.deployApi = new DeploymentV1Api(base.HathoraSdkConfig);
        }
        
        
        #region Server Deploy Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateDeploymentAsync` to upload and deploy a cloud deploy to Hathora.
        /// </summary>
        /// <returns>Returns Deployment on success</returns>
        public async Task<Deployment> CreateDeploymentAsync(double buildId)
        {
            List<ContainerPort> extraContainerPorts = deployOpts.AdvancedDeployOpts.GetExtraContainerPorts();
            
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
                    deployConfig);
            }
            catch (ApiException apiErr)
            {
                HandleServerApiException(
                    nameof(HathoraServerDeployApi),
                    nameof(CreateDeploymentAsync), 
                    apiErr);
                return null;
            }

            Debug.Log($"[HathoraServerDeployApi] " +
                $"BuildId: '{createDeploymentResult?.BuildId}', " +
                $"DeployId: '{createDeploymentResult?.DeploymentId}");

            return createDeploymentResult;
        }
        
        /// <summary>
        /// Wrapper for `CreateDeploymentAsync` to upload and deploy a cloud deploy to Hathora.
        /// </summary>
        /// <returns>Returns Deployment on success</returns>
        public async Task<List<Deployment>> GetDeploymentsAsync()
        {
            List<Deployment> getDeploymentsResult;
            try
            {
                getDeploymentsResult = await deployApi.GetDeploymentsAsync(AppId);
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
            List<HathoraEnvVars> envVars = NetHathoraConfig.HathoraDeployOpts.EnvVars;
            if (envVars == null || envVars.Count == 0) 
                return new List<DeploymentConfigEnvInner>();
            
            // Parse
            return NetHathoraConfig.HathoraDeployOpts.EnvVars.Select(env => 
                new DeploymentConfigEnvInner(env.Key, env.StrVal)).ToList();
        }
    }
}
