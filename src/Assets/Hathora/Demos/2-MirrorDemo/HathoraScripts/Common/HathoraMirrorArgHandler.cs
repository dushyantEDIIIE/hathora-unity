// Created by dylan@hathora.dev

using Hathora.Demos._2_MirrorDemo.HathoraScripts.Client.ClientMgr;
using Mirror;
using Hathora.Demos.Shared.Scripts.Common;
using kcp2k;
using UnityEngine;

namespace Hathora.Demos._2_MirrorDemo.HathoraScripts.Common
{
    /// <summary>
    /// Commandline helper - run via `-mode {server|client|host} -memo {someStr}`
    /// </summary>
    public class HathoraMirrorArgHandler : HathoraArgHandlerBase
    {
        [SerializeField]
        private NetworkManager manager;

        [SerializeField]
        private KcpTransport kcpTransport;

        
        private void Start() => base.InitArgs();

        protected override void InitArgMemo(string _memoStr)
        {
            base.InitArgMemo(_memoStr);
            HathoraMirrorClientMgrUi.Singleton.SetShowDebugMemoTxt(_memoStr);
        }

        protected override void ArgModeStartServer()
        {
            base.ArgModeStartServer();

            // It's very possible this already started, if Mirror's NetworkManager
            // start on headless checkbox is true
            if (NetworkServer.active)
                return;

            Debug.Log("[HathoraMirrorArgHandler] Starting Server ...");
            manager.StartServer();
        }

        protected override void ArgModeStartClient()
        {
            base.ArgModeStartClient();
            
            // It's very possible this already started, if Mirror's NetworkManager
            // Auto join clients checkbox is true
            if (NetworkClient.active)
                return; // We don't want to start 2x
            
            Debug.Log("[HathoraMirrorArgHandler] Starting Client to " +
                $"{manager.networkAddress}:{kcpTransport.Port} (TODO_PROTOCOL) ...");
            manager.StartClient();
        }
        
        protected override void ArgModeStartHost()
        {
            // base.StartHost(); // We don't want to just StartServer -> StartClient().

            // It's very possible this already started, if Mirror's NetworkManager
            // start on headless checkbox is true
            if (NetworkServer.active)
                return; // We don't want to start 2x
            
            Debug.Log("[HathoraMirrorArgHandler] Starting Host (Server+Client) ...");
            manager.StartHost(); // Different from FishNet
        }
    }
}