// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Cloud.Sdk.Model;
using Hathora.Scripts.Server.ApiWrapper;
using Hathora.Scripts.Server.Config;
using Hathora.Scripts.Server.Models;
using UnityEngine.Assertions;
using UnityEngine.TestTools.Constraints;
using Debug = UnityEngine.Debug;

namespace Hathora.Scripts.Server.Editor
{
    public delegate void ZipCompleteHandler();
    public delegate void OnBuildReqComplete(Build _buildInfo);
    public delegate void OnUploadComplete();
    
    public static class HathoraServerDeploy
    {
        public static event ZipCompleteHandler OnZipComplete;
        public static event OnBuildReqComplete OnBuildReqComplete;
        public static event OnUploadComplete OnUploadComplete;

        /// <summary>
        /// Deploys with HathoraServerConfig opts. Optionally sub to events:
        /// - OnZipComplete
        /// - OnBuildReqComplete
        /// - OnUploadComplete
        /// TODO: Support cancel token.
        /// </summary>
        /// <param name="_serverConfig">Find via menu `Hathora/Find UserConfig(s)`</param>
        /// <param name="_cancelToken"></param>
        public static async Task<Deployment> DeployToHathoraAsync(
            HathoraServerConfig _serverConfig,
            CancellationToken _cancelToken = default)
        {
            Debug.Log("[HathoraServerBuild.DeployToHathoraAsync] " +
                "<color=yellow>Starting...</color>");
            
            Assert.IsNotNull(_serverConfig, "[HathoraServerBuild.DeployToHathoraAsync] " +
                "Cannot find HathoraServerConfig ScriptableObject");
            
            // Prepare paths and file names that we didn't get from UserConfig
            HathoraServerDeployPaths serverDeployPaths = new(_serverConfig);

            
            #region Dockerfile >> Compress to .tar.gz
            // Generate the Dockerfile: Paths will be different for each collaborator
            string dockerFileContent = generateDockerFileStr(serverDeployPaths);
            await writeDockerFileAsync(
                serverDeployPaths.PathToDockerfile,
                dockerFileContent,
                _cancelToken);
            
            // Compress build into .tar.gz (gzipped tarball)
            List<string> filePathsToCompress = new()
            {
                serverDeployPaths.ExeBuildDir, 
                serverDeployPaths.PathToDockerfile,
            };
            
            await Hathora7z.TarballDeployFilesVia7zAsync(
                serverDeployPaths, 
                filePathsToCompress,
                _cancelToken);
            
            OnZipComplete?.Invoke();
            #endregion // Dockerfile >> Compress to .tar.gz


            #region Request to build
            // Get a buildId from Hathora
            HathoraServerBuildApi buildApi = new(_serverConfig);

            Build buildInfo = null;
            try
            {
                buildInfo = await getBuildInfoAsync(buildApi, _cancelToken);
            }
            catch (Exception e)
            {
                return null;
            }
            Assert.IsNotNull(buildInfo, "[HathoraServerBuild.DeployToHathoraAsync] Expected buildInfo");
            
            // Building seems to unselect Hathora _serverConfig on success
            HathoraServerConfigFinder.ShowWindowOnly();
            
            OnBuildReqComplete?.Invoke(buildInfo);
            #endregion // Request to build

            
            #region Upload Build
            // ----------------------------------------------
            // Upload the build to Hathora
            byte[] buildBytes = null;
            try
            {
                buildBytes = await uploadBuildAsync(
                    buildApi, 
                    buildInfo.BuildId, 
                    serverDeployPaths);
            }
            catch (Exception e)
            {
                return null;
            }
            Assert.IsNotNull(buildBytes, "[HathoraServerBuild.DeployToHathoraAsync] Expected buildBytes");
            
            OnUploadComplete?.Invoke();
            #endregion // Upload Build

            
            #region Deploy Build
            // ----------------------------------------------
            // Deploy the build
            HathoraServerDeployApi deployApi = new(_serverConfig);

            Deployment deployment = null;
            try
            {
                deployment = await deployBuildAsync(deployApi, buildInfo.BuildId);
            }
            catch (Exception e)
            {
                return null;
            }

            Assert.That(deployment?.BuildId, Is.Not.Null, 
                "[HathoraServerBuild.DeployToHathoraAsync] Deployment failed: " +
                "Check console for details.");
            
            return deployment;
            #endregion // Deploy Build
        }

        private static async Task<Deployment> deployBuildAsync(
            HathoraServerDeployApi _deployApi, 
            double _buildInfoBuildId)
        {
            Debug.Log("[HathoraServerDeploy.deployBuildAsync] " +
                "Deploying the uploaded build...");
            
            Deployment createDeploymentResult = null;
            try
            {
                createDeploymentResult = await _deployApi.CreateDeploymentAsync(_buildInfoBuildId);
            }
            catch (Exception e)
            {
                return null;
            }

            return createDeploymentResult;
        }

        private static async Task<byte[]> uploadBuildAsync(
            HathoraServerBuildApi _buildApi,
            double buildId,
            HathoraServerDeployPaths _serverDeployPaths)
        {
            Debug.Log("[HathoraServerDeploy.uploadBuildAsync] " +
                "Uploading the local build to Hathora...");
            
            // Pass BuildId and tarball (File stream) to Hathora
            string normalizedPathToTarball = Path.GetFullPath(
                $"{_serverDeployPaths.TempDirPath}/{_serverDeployPaths.ExeBuildName}.tar.gz");
            
            byte[] runBuildResult;
            await using (FileStream fileStream = new(normalizedPathToTarball, FileMode.Open, FileAccess.Read))
            {
                runBuildResult = await _buildApi.RunCloudBuildAsync(
                    buildId, 
                    fileStream);
            }

            string successStr = runBuildResult.Length > 0 ? "Success" : "Failed";
            Debug.Log($"runBuildResult=={successStr}");
            
            return runBuildResult;
        }

        private static async Task<Build> getBuildInfoAsync(
            HathoraServerBuildApi _buildApi,
            CancellationToken _cancelToken = default)
        {
            Debug.Log("[HathoraServerDeploy.getBuildInfoAsync] " +
                "Getting build info (notably for buildId)...");
            
            Build createBuildResult = null;
            try
            {
                createBuildResult = await _buildApi.CreateBuildAsync(_cancelToken);
            }
            catch (Exception e)
            {
                return null;
            }

            return createBuildResult;
        }


        #region Dockerfile
        /// <summary>
        /// Deletes an old one, if exists, to ensure updated paths.
        /// TODO: Use this to customize the Dockerfile without editing directly.
        /// </summary>
        /// <param name="pathToDockerfile"></param>
        /// <param name="dockerfileContent"></param>
        /// <param name="_cancelToken"></param>
        /// <returns>path/to/Dockerfile</returns>
        private static async Task writeDockerFileAsync(
            string pathToDockerfile, 
            string dockerfileContent,
            CancellationToken _cancelToken = default)
        {
            // TODO: if (!overwriteDockerfile)
            if (File.Exists(pathToDockerfile))
            {
                Debug.LogWarning("[HathoraServerDeploy.writeDockerFileAsync] " +
                    "Deleting old Dockerfile...");
                File.Delete(pathToDockerfile);
            }

            try
            {
                await File.WriteAllTextAsync(
                    pathToDockerfile, 
                    dockerfileContent, 
                    _cancelToken);
            }
            catch (Exception e)
            {
                Debug.LogError("[HathoraServerDeploy.writeDockerFileAsync] " +
                    $"Failed to write Dockerfile to {pathToDockerfile}:\n{e}");

                return;
            }
        }

        /// <summary>
        /// Writes dynamic paths
        /// TODO: Use this to customize the Dockerfile without editing directly.
        /// </summary>
        /// <param name="_serverDeployPaths"></param>
        /// <returns>"path/to/DockerFile"</returns>
        private static string generateDockerFileStr(HathoraServerDeployPaths _serverDeployPaths)
        {
            return $@"# This file is auto-generated by HathoraServerDeploy.cs

FROM ubuntu

COPY ./Build-Server .

CMD ./{_serverDeployPaths.ExeBuildName}.tar.gz -mode server -batchmode -nographics
";
        }
        #endregion // Dockerfile


    }
}