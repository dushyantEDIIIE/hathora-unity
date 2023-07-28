// Created by dylan@hathora.dev

using System.Threading.Tasks;
using FishNet;
using FishNet.Managing.Client;
using FishNet.Transporting;
using FishNet.Transporting.Bayou;
using Hathora.Cloud.Sdk.Model;
using Hathora.Demos.Shared.Scripts.Client.ClientMgr;
using UnityEngine;

namespace Hathora.Demos._1_FishNetDemo.HathoraScripts.Client.ClientMgr
{
    /// <summary>
    /// - This spawns BEFORE the player, or even connected to the network.
    /// - This is the entry point to call Hathora SDK: Auth, lobby, rooms, etc.
    /// - To add API scripts: Add to the `ClientApis` serialized field.
    /// </summary>
    public class HathoraFishnetClientMgr : HathoraClientMgrBase
    {
        #region vars
        public static HathoraFishnetClientMgr Singleton { get; private set; }
        
        private Transport transport => 
            InstanceFinder.TransportManager.Transport; 

        /// <summary>Updates @ OnClientConnectionState</summary>
        private LocalConnectionState localConnectionState;
        #endregion // vars

        
        #region Init
        /// <summary>SetSingleton(), SetTransport()</summary>
        protected override void OnAwake() =>
            base.OnAwake();

        protected override void SetSingleton()
        {
            if (Singleton != null)
            {
                Debug.LogError("[HathoraFishnetClient]**ERR @ SetSingleton: Destroying dupe");
                Destroy(gameObject);
                return;
            }

            Singleton = this;
        }

        /// <summary>We want to use a different transport !UDP, such as WebGL.</summary>
        protected override void SetClientTransport()
        {
            base.SetClientTransport();
            
            // Default is Tugboat (UDP) >> We also want to consider WebGL builds
            string transportType = "UDP";
            
#if UNITY_WEBGL
            InstanceFinder.NetworkManager.TransportManager.Transport =
                InstanceFinder.NetworkManager.GetComponent<Bayou>();
            transportType = "WebGL";
#endif // UNITY_WEBGL
            
            // TODO: Consider other protocols

            Debug.Log("[HathoraFishnetClientMgrBase.SetTransport] " +
                $"Transport set to `{transport}` ({transportType})");
        }

        protected override void OnStart()
        {
            base.InitOnStart(HathoraFishnetClientMgrUi.Singleton);
            base.OnStart();
            
            // This is a Client manager script; listen for relative events
            InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }
        #endregion // Init
        
        
        #region Interactions from UI
        public override async Task StartHost()
        {
            await StartServer();
            await StartClient();
        }

        public override Task StartServer()
        {
            InstanceFinder.ServerManager.StartConnection();
            return Task.CompletedTask;
        }

        /// <param name="_hostPort">host:port provided by Hathora; eg: "1.proxy.hathora.dev:12345"</param>
        public override Task StartClient(string _hostPort = null)
        {
            (string hostNameOrIp, ushort port) hostPortContainer = SplitPortFromHostOrIp(_hostPort);
            bool hasHost = !string.IsNullOrEmpty(hostPortContainer.hostNameOrIp);
            bool hasPort = hostPortContainer.port > 0;

            // Start FishNet Client via selected Transport
            if (hasHost && hasPort)
            {
                InstanceFinder.ClientManager.StartConnection(
                    hostPortContainer.hostNameOrIp, 
                    hostPortContainer.port);    
            }
            else
                InstanceFinder.ClientManager.StartConnection();
            
            return Task.CompletedTask;
        }

        public override Task StopHost()
        {
            StopServer(); // StopServer() will also stop the client
            return Task.CompletedTask;
        }

        public override Task StopServer()
        {
            InstanceFinder.ServerManager.StopConnection(sendDisconnectMessage: true);
            return Task.CompletedTask;
        }

        public override Task StopClient()
        {
            InstanceFinder.ClientManager.StopConnection();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Connect to the Server as a Client via net code. Uses cached vals.
        /// Currently uses FishNet.Tugboat (UDP) transport.
        /// This will trigger `OnClientConnectionState(state)`
        /// </summary>
        /// <returns>
        /// startedConnection; to ATTEMPT the connection (isValid pre-connect vals); we're not connected yet.
        /// </returns>
        public override Task<bool> ConnectAsClient()
        {
            Debug.Log("[HathoraFishnetClient] ConnectAsync");
            
            // Set connecting state + log where we're connecting to
            base.SetConnectingState(transport.name);

            // -----------------
            // Validate; UI and err handling is handled within
            bool isReadyToConnect = ValidateIsReadyToConnect(InstanceFinder.ClientManager, transport);
            if (!isReadyToConnect)
                return Task.FromResult(false); // !startedConnection

            // -----------------
            // Set port + host (ip)
            ExposedPort connectInfo = HathoraClientSession.ServerConnectionInfo.ExposedPort;
            transport.SetPort((ushort)connectInfo.Port);
            transport.SetClientAddress(connectInfo.Host);
            
            // Connect now => cb @ OnClientConnected()
            bool startedConnection = InstanceFinder.ClientManager.StartConnection();
            return Task.FromResult(startedConnection);
        }

        private bool ValidateIsReadyToConnect(
            ClientManager _clientMgr, 
            Transport _transport)
        {
            if (!ValidateServerConfigConnectionInfo())
                return false;

            if (InstanceFinder.NetworkManager == null)
            {
                base.OnConnectFailed("!NetworkManager");
                return false; // !isSuccess
            }

            // Validate state
            LocalConnectionState currentState = _transport.GetConnectionState(server: false);
            if (currentState != LocalConnectionState.Stopped)
            {
                _clientMgr.StopConnection();
                base.OnConnectFailed("Prior connection !stopped: Try again soon");
                return false; // !isSuccess
            }
            
            // Success - ready to connect
            return true;
        }

        private void OnClientConnectionState(ClientConnectionStateArgs _state)
        {
            localConnectionState = _state.ConnectionState;
            Debug.Log($"[HathoraFishnetClient.OnClientConnectionState] " +
                $"New state: {localConnectionState}");
            
            // onConnectSuccess?
            if (localConnectionState == LocalConnectionState.Started)
                base.OnConnectSuccess();
            
            // onConnectFailed?
            bool stopped = localConnectionState == LocalConnectionState.Stopped; 
            bool stoppedConnecting = stopped && IsConnecting;
            if (stoppedConnecting)
                base.OnConnectFailed("Connection stopped");
        }
        #endregion // Callbacks
    }
}
