// Created by dylan@hathora.dev

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hathora.Net.Common
{
    public class NetUI : MonoBehaviour
    {
        private void Start() =>
            setShowWelcomeLobbyMemoTxt();

        [SerializeField]
        private Button hostAsServerBtn;
        [SerializeField]
        private Button joinAsClientBtn;
        [SerializeField]
        private Button disconnectBtn;
        [SerializeField]
        private Button sendClientToServerPongBtn;
        [SerializeField]
        private Button sendServerToClientPingBtn;
        [SerializeField]
        private Image backgroundImg;
        [SerializeField]
        private TextMeshProUGUI debugMemoTxt;
        
        
        public void SetShowDebugMemoTxt(string memoStr)
        {
            debugMemoTxt.text = memoStr;
            debugMemoTxt.gameObject.SetActive(true);
            Debug.Log($"[NetCmdLine] Debug Memo: '{memoStr}'");
        }
        
        public void ToggleLobbyUi(bool show, NetCommonMgr.NetMode netMode)
        {
            ToggleHostJoinUiVisible(show);
            ToggleUiBackground(show);
            toggleRoomUi(!show, netMode);
        }
        
        private void toggleRoomUi(bool show, NetCommonMgr.NetMode netMode)
        {
            ToggleDisconnectBtnVisible(show);

            switch (netMode)
            {
                case NetCommonMgr.NetMode.Client:
                    SetShowDebugMemoTxt("Client");
                    sendClientToServerPongBtn.gameObject.SetActive(show);
                    break;
                
                case NetCommonMgr.NetMode.Server:
                    SetShowDebugMemoTxt("Server");
                    sendServerToClientPingBtn.gameObject.SetActive(show);
                    break;
                
                case NetCommonMgr.NetMode.None:
                    setShowWelcomeLobbyMemoTxt();
                    hidePingBtns();
                    break;
            }
        }

        private void hidePingBtns()
        {
            sendServerToClientPingBtn.gameObject.SetActive(false);
            sendClientToServerPongBtn.gameObject.SetActive(false);
        }

        private void setShowWelcomeLobbyMemoTxt() =>
            SetShowDebugMemoTxt("Lobby");
        
        private void ToggleUiBackground(bool show) =>
            backgroundImg.gameObject.SetActive(show);

        private void ToggleHostJoinUiVisible(bool show)
        {
            hostAsServerBtn.gameObject.SetActive(show);
            joinAsClientBtn.gameObject.SetActive(show);
        }
        
        private void ToggleDisconnectBtnVisible(bool show) =>
            disconnectBtn.gameObject.SetActive(show);
    }
}