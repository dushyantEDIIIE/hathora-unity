// Created by dylan@hathora.dev

using System.Threading.Tasks;
using Hathora.Scripts.Net.Server;
using UnityEditor;
using UnityEngine;

namespace Hathora.Scripts.Utils.Editor
{
    /// <summary>
    /// The main editor for HathoraServerConfig, including all the button clicks and extra UI.
    /// </summary>
    [CustomEditor(typeof(HathoraServerConfig))]
    public class HathoraConfigObjectEditor : UnityEditor.Editor
    {
        private static bool devAuthLoginButtonInteractable = true;
        private const string HATHORA_GREEN_HEX = "#76FDBA";
        
        public HathoraServerConfig GetSelectedInstance() =>
            (HathoraServerConfig)target;
        
        // Create a new GUIStyle for the buttons with custom padding
        private GUIStyle buttonStyle;
        
        /// <summary>Hathora banner</summary>
        public override void OnInspectorGUI()
        {
            HathoraEditorUtils.InsertBanner(); // Place banner @ top
            insertEditingTemplateWarningMemo();
            base.OnInspectorGUI();
            insertButtons(); // Place btns @ bottom
        }

        /// <summary>
        /// Only insert memo if using the template file
        /// </summary>
        private void insertEditingTemplateWarningMemo()
        {
            bool isDefaultName = GetSelectedInstance().name == nameof(HathoraServerConfig);
            if (!isDefaultName)
                return;
            
            EditorGUILayout.HelpBox("You are editing a template! Best practice is to duplicate" +
                "this file and rename it >> then .gitignore it; treat the dupe like an `.env` file.", 
                MessageType.Warning);
            
            GUILayout.Space(10);
        }

        void initButtonStyle()
        {
            buttonStyle = new(GUI.skin.button);
            buttonStyle.padding = new RectOffset(10, 10, 10, 10);
            buttonStyle.richText = true;
            buttonStyle.fontSize = 13;
        }
        
        
        #region Core Buttons
        private void insertButtons()
        {
            HathoraServerConfig selectedConfig = GetSelectedInstance();
            
            initButtonStyle();
            GUILayout.Space(5);

            insertBuildBtn(selectedConfig);
            GUILayout.Space(10);

            insertDevAuthLoginBtn(selectedConfig);
            GUILayout.Space(10);

            insertHathoraDeployBtn(selectedConfig);
            GUILayout.Space(10);
        }

        private void insertHathoraDeployBtn(HathoraServerConfig selectedConfig)
        {
            GUI.enabled = selectedConfig.MeetsDeployBtnReqs();;
            if (GUI.enabled)
            {
                EditorGUILayout.HelpBox("1. Build a dedicated Linux server to " +
                    "`/Build-Server/Build-Server.x86_64`\n" +
                    "2. Deploy below.", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("" +
                    "* Ensure the core+deploy settings above are set.\n" +
                    "* Ensure you have a linux server built.\n" +
                    "* Auth above, if dev token is not set.", 
                    MessageType.Error);
            }
            
            if (GUILayout.Button("Deploy to Hathora", buttonStyle))
            {
                HathoraServerDeploy.DeployToHathoraAsync(selectedConfig);
                GUILayout.Space(20);
            }
        }

        private async Task insertDevAuthLoginBtn(HathoraServerConfig selectedConfig)
        {
            bool hasAuthToken = !string.IsNullOrEmpty(selectedConfig.HathoraCoreOpts.DevAuthOpts.DevAuthToken);
            bool pendingGuiEnable = devAuthLoginButtonInteractable && !hasAuthToken;

            string btnStr = "Developer Login";
            if (!pendingGuiEnable)
            {
                if (hasAuthToken)
                {
                    EditorGUILayout.HelpBox("Unset `dev auth token` to relogin", MessageType.Info);
                    btnStr = $"<color={HATHORA_GREEN_HEX}>Dev Auth Token Set!</color>";
                }
                else
                {
                    EditorGUILayout.HelpBox("Your default browser should have popped up.", MessageType.Info);
                    btnStr = "<color=yellow>Awaiting Auth...</color>";
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Launch a browser window to login and set your " +
                    "`dev auth token` above.", MessageType.Info);
            }

            GUI.enabled = pendingGuiEnable;
            EditorGUI.BeginDisabledGroup(!devAuthLoginButtonInteractable);
            if (GUILayout.Button(btnStr, buttonStyle))
            {
                GUI.FocusControl(null); // Unfocus to refresh UI // TODO: Is Repaint() better?
                devAuthLoginButtonInteractable = false;
                
                await HathoraServerAuth.DevAuthLogin(selectedConfig);
                
                devAuthLoginButtonInteractable = true;
                Repaint();
                EditorGUI.EndDisabledGroup();
            }
        }

        private void insertBuildBtn(HathoraServerConfig selectedConfig)
        {
            GUI.enabled = selectedConfig.MeetsBuildBtnReqs();
            EditorGUILayout.HelpBox("Build your dedicated Linux server in 1 click, " +
                "using the Auto-Build settings above.", MessageType.Info);
            
            if (GUILayout.Button("Build Linux Server", buttonStyle))
            {
                HathoraServerBuild.BuildHathoraLinuxServer(selectedConfig);
            }
        }
        #endregion // Core Buttons
    }
}
