// Created by dylan@hathora.dev

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Server;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle.PostAuth
{
    public class HathoraConfigPostAuthBodyBuildUI : HathoraConfigUIBase
    {
        #region Vars
        // Foldouts
        private bool isServerBuildFoldout;
        #endregion // Vars


        #region Init
        public HathoraConfigPostAuthBodyBuildUI(
            HathoraServerConfig _serverConfig, 
            SerializedObject _serializedConfig)
            : base(_serverConfig, _serializedConfig)
        {
            if (!HathoraConfigUI.ENABLE_BODY_STYLE)
                return;
        }
        #endregion // Init
        
        
        #region UI Draw
        public void Draw()
        {
            if (!IsAuthed)
                return; // You should be calling HathoraConfigPreAuthBodyUI.Draw()

            insertServerBuildSettingsFoldout();
        }

        private void insertServerBuildSettingsFoldout()
        {
            isServerBuildFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(
                isServerBuildFoldout,
                "Server Build Settings");

            if (!isServerBuildFoldout)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
            
            EditorGUI.indentLevel++;
            InsertSpace2x();
            
            insertBuildSettingsFoldoutComponents();
            
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void insertBuildSettingsFoldoutComponents()
        {
            insertBuildDirNameHorizGroup();
            insertBuildFileExeNameHorizGroup();

            InsertSpace2x();
            
            bool enableBuildBtn = ServerConfig.MeetsBuildAndDeployBtnReqs();
            if (!enableBuildBtn && !HathoraServerDeploy.IsDeploying)
                insertGenerateServerBuildBtnHelpboxOnMissingReqs();
            
            insertGenerateServerBuildBtn(enableBuildBtn); // !await
        }
        
        /// <summary>
        /// Generally used for helpboxes to explain why a button is disabled.
        /// </summary>
        /// <param name="_serverConfig"></param>
        /// <param name="_includeMissingReqsFieldsPrefixStr">Useful if you had a combo of this </param>
        /// <returns></returns>
        public static StringBuilder GetCreateBuildMissingReqsStrb(
            HathoraServerConfig _serverConfig,
            bool _includeMissingReqsFieldsPrefixStr = true)
        {
            StringBuilder helpboxLabelStrb = new(_includeMissingReqsFieldsPrefixStr 
                ? "Missing required fields: " 
                : ""
            );
            
            // (!) Hathora SDK Enums start at index 1 (not 0)
            if (!_serverConfig.HathoraCoreOpts.HasAppId)
                helpboxLabelStrb.Append("`AppId` ");
            
            if (!_serverConfig.LinuxHathoraAutoBuildOpts.HasServerBuildDirName)
                helpboxLabelStrb.Append("`Server Build Dir Name` ");
                
            if (!_serverConfig.LinuxHathoraAutoBuildOpts.HasServerBuildExeName)
                helpboxLabelStrb.Append("`Server Build Exe Name`");

            return helpboxLabelStrb;
        }

        /// <summary>
        /// </summary>
        /// <returns>enableBuildBtn</returns>
        private void insertGenerateServerBuildBtnHelpboxOnMissingReqs()
        {
            StringBuilder helpboxLabelStrb = GetCreateBuildMissingReqsStrb(ServerConfig);

            // Post the help box *before* we disable the button so it's easier to see (if toggleable)
            EditorGUILayout.HelpBox(helpboxLabelStrb.ToString(), MessageType.Error);
        }

        private void insertGenerateServerBuildBtn(bool _enableBuildBtn)
        {
            EditorGUI.BeginDisabledGroup(disabled: !_enableBuildBtn);
            
            // USER INPUT >>
            bool clickedBuildBtn = InsertLeftGeneralBtn("Generate Server Build");
            
            EditorGUI.EndDisabledGroup();
            InsertSpace1x();

            if (clickedBuildBtn)
                OnGenerateServerBuildBtnClick();
        }

        private void insertBuildDirNameHorizGroup()
        {
            string inputStr = base.InsertHorizLabeledTextField(
                _labelStr: "Build directory",
                _tooltip: "Parent directory to generate build into.\n\n" +
                "Default: `Build-Server`",
                _val: ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildDirName,
                _alignTextField: GuiAlign.SmallRight);

            bool isChanged = inputStr != ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildDirName;
            if (isChanged)
                onServerBuildDirChanged(inputStr);

            InsertSpace1x();
        }
        
        private void insertBuildFileExeNameHorizGroup()
        {
            string inputStr = base.InsertHorizLabeledTextField(
                _labelStr: "Build file name", 
                _tooltip: "Name for generated build.\n\n" +
                "Default: `Hathora-Unity-LinuxServer.x86_64`",
                _val: ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildExeName,
                _alignTextField: GuiAlign.SmallRight);
            
            bool isChanged = inputStr != ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildExeName;
            if (isChanged)
                onServerBuildExeNameChanged(inputStr);
            
            InsertSpace1x();
        }
        #endregion // UI Draw

        
        #region Event Logic
        private void onServerBuildDirChanged(string _inputStr)
        {
            ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildDirName = _inputStr;
            SaveConfigChange(
                nameof(ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildDirName), 
                _inputStr);
        }        
        
        private void onServerBuildExeNameChanged(string _inputStr)
        {
            ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildExeName = _inputStr;
            
            SaveConfigChange(
                nameof(ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildExeName), 
                _inputStr);
        }

        private void OnGenerateServerBuildBtnClick() =>
            GenerateServerBuildAsync(); // !await

        public async Task<BuildReport> GenerateServerBuildAsync()
        {
            CancellationTokenSource cancelTokenSrc = new();
            BuildReport buildReport = null;
            
            // TODO: Get from ServerConfig (for devs that have a custom Dockerfile they don't want overwritten each build)
            const bool overwriteExistingDockerfile = true;

            try
            {
                buildReport = await HathoraServerBuild.BuildHathoraLinuxServer(
                    ServerConfig,
                    overwriteExistingDockerfile, // TODO: 
                    cancelTokenSrc.Token);
            }
            catch (TaskCanceledException)
            {
                Debug.Log("Server build cancelled.");
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error: {e}");
                throw;
            }

            
            Assert.AreEqual(buildReport.summary.result, BuildResult.Succeeded,
                "Server build failed. Check console for details.");

            return buildReport;
        }
        #endregion // Event Logic
    }
}
