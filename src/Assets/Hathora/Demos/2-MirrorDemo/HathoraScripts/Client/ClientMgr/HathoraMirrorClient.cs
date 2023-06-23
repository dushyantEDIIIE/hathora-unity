// Created by dylan@hathora.dev

using Hathora.Cloud.Sdk.Model;
using Hathora.Demos.Shared.Scripts.Client;
using Hathora.Demos.Shared.Scripts.Client.ClientMgr;
using Mirror;
using UnityEngine;

namespace Hathora.Demos._2_MirrorDemo.HathoraScripts.Client.ClientMgr
{
    /// <summary>
    /// - This spawns BEFORE the player, or even connected to the network.
    /// - This is the entry point to call Hathora SDK: Auth, lobby, rooms, etc.
    /// - To add API scripts: Add to the `ClientApis` serialized field.
    /// 
    /// MIRROR NOTES / NOTABLE PROPS + FUNCS:
    /// - Mirror.NetworkClient
    /// - Mirror.NetworkManager.singleton
    /// - Mirror.NetworkManager.ConnectState is INTERNAL, but broken up into individual props
    /// </summary>
    public class HathoraMirrorClient : HathoraClientBase
    {
        #region vars
        public static HathoraMirrorClient Singleton { get; private set; }
        
        private bool isConnected => NetworkClient.isConnected;
        private bool isConnecting => NetworkClient.isConnecting;
        #endregion // vars
        

        #region Init
        protected override void OnAwake()
        {
            base.OnAwake();
            setSingleton();
        }

        private void setSingleton()
        {
            if (Singleton != null)
            {
                Debug.LogError("[HathoraMirrorClient]**ERR @ setSingleton: Destroying dupe");
                Destroy(gameObject);
                return;
            }

            Singleton = this;
        }
        
        protected override void OnStart()
        {
            base.OnStart();
            
            // This is a Client manager script; listen for relative events
            Mirror.NetworkManager.singleton.transport.OnClientConnected += OnClientConnected;
            Mirror.NetworkManager.singleton.transport.OnClientDisconnected += OnClientDisconnected;
        }
        #endregion // Init
        
        
        #region NetCode Callbacks
        private void OnClientConnected()
        {
            Debug.Log("[HathoraMirrorClient] OnClientConnected");
            OnConnectSuccess();
        }

        private void OnClientDisconnected()
        {
            Debug.Log("[HathoraMirrorClient] OnClientDisconnected");
            OnConnectFailed("Disconnected");
        }
        #endregion // NetCode Callbacks
        
        
        #region Interactions from UI
        /// <summary>
        /// Connect to the Server as a Client via net code. Uses cached vals.
        /// Currently uses FishNet.Tugboat (UDP) transport.
        /// This will trigger `OnClientConnectionState(state)`
        /// </summary>
        /// <returns>isSuccess</returns>
        public bool Connect()
        {
            Debug.Log("[HathoraMirrorClient] ConnectAsync");

            IsConnecting = true;

            // -----------------
            // Validate; UI and err handling is handled within
            bool isReadyToConnect = ValidateIsReadyToConnect();
            if (!isReadyToConnect)
                return false; // !isSuccess

            // -----------------
            // Connect
            Debug.Log("[HathoraMirrorClient.ConnectAsync] Connecting to: " + 
                $"{HathoraClientSession.GetServerInfoIpPort()} via Mirror " +
                $"NetworkManager.{NetworkManager.singleton.transport.name} transport");

            ExposedPort connectInfo = HathoraClientSession.ServerConnectionInfo.ExposedPort;
            NetworkClient.Connect(connectInfo.Host);
            
            // TODO: How to validate success? Is this a synchronous connect?
            if (!isConnected)
            {
                OnConnectFailed("StartConnection !isSuccess");
                return false;
            }
            
            return true; // isSuccess => Continued at OnClientConnected()
        }

        private bool ValidateIsReadyToConnect()
        {
            if (!ValidateServerConfigConnectionInfo())
                return false;

            if (NetworkManager.singleton == null)
            {
                OnConnectFailed("!NetworkManager");
                return false; // !isSuccess
            }

            // Validate state
            if (isConnected || isConnecting)
            {
                NetworkClient.Disconnect();
                OnConnectFailed("Prior connection still active: Disconnecting... " +
                    "Try again soon");
                
                return false; // !isSuccess
            }
            
            // Success - ready to connect
            return true;
        }
        #endregion // Callbacks
    }
}
