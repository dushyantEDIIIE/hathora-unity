// Created by dylan@hathora.dev

using Hathora.Scripts.Net.Common;
using Hathora.Scripts.SdkWrapper.Editor;
using Hathora.Scripts.SdkWrapper.Models;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Hathora.Scripts.Utils.Editor.ConfigStyle
{
    public class HathoraConfigFooterUI : HathoraConfigUIBase
    {
        public HathoraConfigFooterUI(
            NetHathoraConfig _config, 
            SerializedObject _serializedConfig) 
            : base(_config, _serializedConfig)
        {
        }

        public void Draw()
        {
            if (IsAuthed)
            {
                insertBuildUploadDeployComboBtn();
                return;
            }
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Learn more about Hathora Cloud", PreLinkLabelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            InsertLinkLabel("Documentation", HathoraEditorUtils.HATHORA_DOCS_URL);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
 
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            InsertLinkLabel("Demo Projects", HathoraEditorUtils.HATHORA_DOCS_DEMO_PROJECTS_URL);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        
        private async Task insertBuildUploadDeployComboBtn()
        {
            GUILayout.Space(15);
            
            bool meetsDeployBtnReqs = Config.MeetsDeployBtnReqs();
            MessageType helpMsgType = meetsDeployBtnReqs ? MessageType.Info : MessageType.Error;
            string helpMsg = meetsDeployBtnReqs
                ? "This action will create a new server build, upload to Hathora, " +
                  "and create a new development version of your application."
                : $"Requires set: {nameof(HathoraCoreOpts.AppId)}, " +
                  $"{nameof(HathoraAutoBuildOpts.ServerBuildExeName)}, " +
                  $"{nameof(HathoraAutoBuildOpts.ServerBuildDirName)}, ";

            // Post the help box *before* we disable the button so it's easier to see
            EditorGUILayout.HelpBox(helpMsg, helpMsgType);
            GUI.enabled = meetsDeployBtnReqs;
                
            if (GUILayout.Button("Build, Upload & Deploy New Version", GeneralButtonStyle))
            {
                BuildReport buildReport = HathoraServerBuild.BuildHathoraLinuxServer(Config);
                if (buildReport.summary.result != BuildResult.Succeeded)
                    return;
                
                await HathoraServerDeploy.DeployToHathoraAsync(Config);
                EditorGUILayout.Space(20);
            }
            
            GUI.enabled = true;
        }
    }
}
