// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Cloud.Sdk.Model;
using Hathora.Scripts.Net.Common;
using Hathora.Scripts.SdkWrapper.Editor.ApiWrapper;
using Hathora.Scripts.Utils.Extensions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Hathora.Scripts.Utils.Editor.ConfigStyle.PostAuth
{
    public class HathoraConfigPostAuthBodyRoomUI : HathoraConfigUIBase
    {
        #region Vars
        private HathoraConfigPostAuthBodyRoomLobbyUI roomLobbyUI { get; set; }
        public static CancellationTokenSource CreateRoomCancelTokenSrc { get; set; } // TODO
        private const int CREATE_ROOM_TIMEOUT_SECONDS = 30;
        private bool isCreatingRoom { get; set; }
        
        // Region lists
        private readonly List<string> displayOptsStrList;
        private readonly List<int> originalIndices;
        
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
            
            // ----------------------
            // REGION LISTS >> We want to be able to sort them, yet still refer to original Enum index
            Dictionary<int, string> regionDisplayMap = 
                Enum.GetValues(typeof(Region))
                    .Cast<Region>()
                    .Select((Region region, int index) => new { region, index })
                    .ToDictionary(x => x.index, x => 
                        x.region.ToString().SplitPascalCase());

            displayOptsStrList = regionDisplayMap.Values.OrderBy(s => s).ToList();
            originalIndices = displayOptsStrList.Select(s => regionDisplayMap.First(kvp => kvp.Value == s).Key).ToList();
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
            isCreateRoomLobbyFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(
                isCreateRoomLobbyFoldout, 
                "Create Room");
            
            if (!isCreateRoomLobbyFoldout)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
    
            EditorGUI.indentLevel++;
            InsertSpace2x();

            insertCreateRoomOrLobbyFoldoutComponents();

            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        
        private void insertCreateRoomOrLobbyFoldoutComponents()
        {
            insertRegionHorizPopupList();

            bool enableCreateRoomBtn = Config.MeetsCreateRoomBtnReqs();
            insertCreateRoomLobbyBtnHelpboxOnErr(enableCreateRoomBtn);

            bool showCancelBtn = isCreatingRoom && CreateRoomCancelTokenSrc.Token.CanBeCanceled; 
            if (showCancelBtn)
                insertCreateRoomLobbyCancelBtn(CreateRoomCancelTokenSrc);
            else
                insertCreateRoomLobbyBtn(enableCreateRoomBtn);

            bool hasLastRoomInfo = Config.HathoraLobbyRoomOpts.HasLastCreatedRoomInfo;
            if (hasLastRoomInfo)
                insertLastCreatedRoomInfoGroup();
            
            insertViewLogsMetricsLinkLbl();
        }

        private void insertLastCreatedRoomInfoGroup()
        {
            // throw new NotImplementedException("TODO");
        }

        private void insertCreateRoomLobbyCancelBtn(CancellationTokenSource _cancelTokenSrc)
        {
            string btnLabelStr = $"<color={HathoraEditorUtils.HATHORA_PINK_CANCEL_COLOR_HEX}>" +
                "<b>Cancel</b> (Creating Room/Lobby...)</color>";

            // USER INPUT >>
            bool clickedCancelBtn = GUILayout.Button(btnLabelStr, GeneralButtonStyle);
            if (clickedCancelBtn)
                onCreateRoomCancelBtnClick(_cancelTokenSrc);
        }

        /// <summary>
        /// Generally used for helpboxes to explain why a button is disabled.
        /// </summary>
        /// <param name="_config"></param>
        /// <param name="_includeMissingReqsFieldsPrefixStr">Useful if you had a combo of this </param>
        /// <returns></returns>
        public static StringBuilder GetCreateRoomMissingReqsStrb(
            NetHathoraConfig _config,
            bool _includeMissingReqsFieldsPrefixStr = true)
        {
            StringBuilder helpboxLabelStrb = new(_includeMissingReqsFieldsPrefixStr 
                ? "Missing required fields: " 
                : ""
            );
            
            // (!) Hathora SDK Enums start at index 1 (not 0)
            if (!_config.HathoraCoreOpts.HasAppId)
                helpboxLabelStrb.Append("`AppId, `");
            
            if (_config.HathoraLobbyRoomOpts.RegionSelectedIndex < 1)
                helpboxLabelStrb.Append("`Region, `");

            return helpboxLabelStrb;
        }
        
        private void insertCreateRoomLobbyBtnHelpboxOnErr(bool _enable)
        {
            if (_enable)
                return;
            
            // Explain why the button is disabled
            StringBuilder helpboxLabelStrb = GetCreateRoomMissingReqsStrb(Config);
            
            // Post the help box *before* we disable the button so it's easier to see (if toggleable)
            EditorGUILayout.HelpBox(helpboxLabelStrb.ToString(), MessageType.Error);
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

        private void insertCreateRoomLobbyBtn(bool _enable)
        {
            string btnLabelStr = isCreatingRoom 
                ? "Creating Room..." 
                : "Create Room/Lobby";

            EditorGUI.BeginDisabledGroup(disabled: !_enable);
            
            bool clickedCreateRoomLobbyBtn = InsertLeftGeneralBtn(btnLabelStr);

            EditorGUI.EndDisabledGroup();
            InsertSpace1x();

            if (!clickedCreateRoomLobbyBtn)
                return;

            onCreateRoomLobbyBtnClick(); // !await

            // Deployment deployment = await HathoraServerDeploy.DeployToHathoraAsync(Config);
            // Assert.That(deployment?.BuildId, Is.Not.Null,
            //     "Deployment failed: Check console for details.");
            
            EditorGUI.EndDisabledGroup();
        }

        private void insertRegionHorizPopupList()
        {
            int selectedIndex = Config.HathoraLobbyRoomOpts.RegionSelectedIndex;

            if (selectedIndex < 0 || selectedIndex >= originalIndices.Count)
            {
                Debug.LogError("[HathoraConfigPostAuthBodyRoomUI.insertRegionHorizPopupList] " + 
                    $"Invalid selected index: {selectedIndex}");
                return;
            }

            // Get display index from original index
            int displaySelectedIndex = originalIndices.IndexOf(selectedIndex);
            
            int newDisplaySelectedIndex = base.InsertHorizLabeledPopupList(
                _labelStr: "Region",
                _tooltip: "Default: `Seattle`",
                _displayOptsStrArr: displayOptsStrList.ToArray(),
                _selectedIndex: displaySelectedIndex,
                GuiAlign.SmallRight);

            bool isNewValidIndex = newDisplaySelectedIndex >= 0 &&
                newDisplaySelectedIndex != displaySelectedIndex &&
                newDisplaySelectedIndex < displayOptsStrList.Count;

            if (isNewValidIndex)
            {
                // Get the original index from display index
                int originalIndex = originalIndices[newDisplaySelectedIndex];
                onSelectedRegionPopupIndexChanged(originalIndex);
            }

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
            Debug.Log("[HathoraConfigPostAuthBodyRoomUI.onSelectedRegionPopupIndexChanged] " +
                $"Selected Region: {(Region)Config.HathoraLobbyRoomOpts.RegionSelectedIndex}" +
                $" (index {_newSelectedIndex})]");
        }
        
        /// <summary>
        /// On cancel, we'll set !isCreatingRoom so we can try again.
        /// </summary>
        private async Task onCreateRoomLobbyBtnClick()
        {
            isCreatingRoom = true;

            createNewCreateRoomCancelToken();
            HathoraServerRoomApi roomApi = new(Config);
            
            
            ConnectionInfoV2 connectionInfo = null;
            try
            {
                connectionInfo = await serverRoomApi.CreateRoomAsync(
                    _cancelToken: CreateRoomCancelTokenSrc.Token);
            }
            catch (Exception e)
            {
                // Could be a TaskCanceledException
                onCreateRoomDone();
                return;
            }
            

            Room room = null;
            try
            {
                room = await serverRoomApi.PollGetRoomUntilActiveAsync(
                    connectionInfo?.RoomId,
                    CreateRoomCancelTokenSrc.Token);
            }
            catch (Exception e)
            {
                // Could be a TaskCanceledException
                onCreateRoomDone();
                return;
            }
            
            
            Assert.That(connectionInfo?.RoomId, Is.Not.Null,
                "Failed to create room: See console");

            onCreateRoomDone();
        }

        private void onCreateRoomCancelBtnClick(CancellationTokenSource _cancelTokenSrc)
        {
            Debug.Log("[HathoraConfigPostAuthBodyRoomUI] onCreateRoomCancelBtnClick");
            onCreateRoomDone(_cancelTokenSrc);
        }

        private void onCreateRoomDone(CancellationTokenSource _cancelTokenSource = null)
        {
            Debug.Log("[HathoraConfigPostAuthBodyRoomUI.onCreateRoomDone] Done (or canceled)");
            CreateRoomCancelTokenSrc?.Cancel();
            isCreatingRoom = false;
        }
        #endregion // Event Logic

        
        #region Utils
        /// <summary>Cancel old, create new</summary>
        private static void createNewCreateRoomCancelToken()
        {
            // Cancel an old op 1st
            if (CreateRoomCancelTokenSrc != null && CreateRoomCancelTokenSrc.Token.CanBeCanceled)
                CreateRoomCancelTokenSrc.Cancel();
 
            CreateRoomCancelTokenSrc = new CancellationTokenSource(
                TimeSpan.FromSeconds(CREATE_ROOM_TIMEOUT_SECONDS));
        }
        #endregion // Utils
    }
}
