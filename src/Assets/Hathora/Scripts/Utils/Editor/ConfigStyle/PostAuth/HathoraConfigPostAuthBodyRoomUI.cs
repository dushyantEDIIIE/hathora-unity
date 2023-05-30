// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hathora.Cloud.Sdk.Model;
using Hathora.Scripts.Net.Common;
using Hathora.Scripts.SdkWrapper.Editor;
using NUnit.Framework;
using UnityEditor;

namespace Hathora.Scripts.Utils.Editor.ConfigStyle.PostAuth
{
    public class HathoraConfigPostAuthBodyRoomUI : HathoraConfigUIBase
    {
        #region Vars
        private HathoraConfigPostAuthBodyRoomLobbyUI roomLobbyUI;

        // Foldouts
        private bool isCreateRoomLobbyFoldout;
        #endregion // Vars


        #region Init
        public HathoraConfigPostAuthBodyRoomUI(
            NetHathoraConfig _config, 
            SerializedObject _serializedConfig)
            : base(_config, _serializedConfig)
        {
            if (!HathoraConfigUI.ENABLE_BODY_STYLE)
                return;

            initDrawUtils();
        }

        private void initDrawUtils()
        {
            roomLobbyUI = new HathoraConfigPostAuthBodyRoomLobbyUI(Config, SerializedConfig);
        }
        #endregion // Init
        
        
        #region UI Draw
        public void Draw()
        {
            if (!IsAuthed)
                return; // You should be calling HathoraConfigPreAuthBodyUI.Draw()

            insertCreateRoomOrLobbyFoldout();
        }

        private void insertCreateRoomOrLobbyFoldout()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            isCreateRoomLobbyFoldout = EditorGUILayout.Foldout(
                isCreateRoomLobbyFoldout, 
                "Create Room or Lobby");
            
            if (!isCreateRoomLobbyFoldout)
            {
                EditorGUILayout.EndVertical(); // End of foldout box skin
                return;
            }
    
            EditorGUI.indentLevel++;
            InsertSpace2x();

            insertCreateRoomOrLobbyFoldoutComponents();

            EditorGUILayout.EndVertical(); // End of foldout box skin
            InsertSpace3x();
            EditorGUI.indentLevel--;
        }

        private void insertCreateRoomOrLobbyFoldoutComponents()
        {
            insertRegionHorizPopupList();
            roomLobbyUI.Draw();
            
            EditorGUI.BeginDisabledGroup(disabled: true); // TODO: WIP - Enable after finished
            insertCreateRoomLobbyBtn();
            EditorGUI.EndDisabledGroup();
            
            insertViewLogsMetricsLinkLbl();
        }

        private void insertViewLogsMetricsLinkLbl()
        {
            InsertSpace2x();
            
            InsertCenterLabel("View logs and metrics for your active rooms and processes below:");

            string consoleAppUrl = HathoraEditorUtils.HATHORA_CONSOLE_APP_BASE_URL +
                $"/{Config.HathoraCoreOpts.AppId}";
            
            InsertLinkLabel(
                "Hathora Console",
                consoleAppUrl,
                _centerAlign: true);
            
            InsertSpace1x();
        }

        private async Task insertCreateRoomLobbyBtn()
        {
            bool clickedCreateRoomLobbyBtn = insertLeftGeneralBtn("Create Room/Lobby");
            if (!clickedCreateRoomLobbyBtn)
                return;

            throw new NotImplementedException("TODO");

            // Deployment deployment = await HathoraServerDeploy.DeployToHathoraAsync(Config);
            // Assert.That(deployment?.BuildId, Is.Not.Null,
            //     "Deployment failed: Check console for details.");
        }

        private void insertRegionHorizPopupList()
        {
            int selectedIndex = Config.HathoraLobbyRoomOpts.RegionSelectedIndex;
            
            // Get list of string names from Region Enum members. Set UPPER.
            List<string> displayOptsStrList = GetStrListOfEnumMemberKeys<Region>(
                EnumListOpts.PascalWithSpaces);

            int newSelectedIndex = base.insertHorizLabeledPopupList(
                _labelStr: "Region",
                _tooltip: "Default: `Seattle`",
                _displayOptsStrArr: displayOptsStrList.ToArray(),
                _selectedIndex: selectedIndex,
                GuiAlign.SmallRight);

            bool isNewValidIndex = selectedIndex >= 0 &&
                newSelectedIndex != selectedIndex &&
                selectedIndex < displayOptsStrList.Count;

            if (isNewValidIndex)
                onSelectedRegionPopupIndexChanged(newSelectedIndex);
            
            InsertSpace2x();
        }
        #endregion // UI Draw

        
        #region Event Logic
        private void onSelectedRegionPopupIndexChanged(int _newSelectedIndex)
        {
            Config.HathoraLobbyRoomOpts.RegionSelectedIndex = _newSelectedIndex;
            SaveConfigChange(
                nameof(Config.HathoraLobbyRoomOpts.RegionSelectedIndex), 
                _newSelectedIndex.ToString());
        }
        #endregion // Event Logic
    }
}
