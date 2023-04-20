// Created by dylan@hathora.dev
// Unity NGO: Getting Started | https://docs-multiplayer.unity3d.com/netcode/current/tutorials/get-started-ngo

using Hathora.Net.Client;
using Hathora.Net.Server;
using Unity.Netcode;
using UnityEngine;

namespace Hathora.Net.Common
{
    /// <summary>
    /// As soon as we connect to the server, we send a ping to the server.
    /// => Server will pong us back.
    /// </summary>
    public class PingTestRpc : NetworkBehaviour
    {
        [SerializeField]
        private NetClientMgr clientMgr;

        private NetServerMgr serverMgr => NetServerMgr.s_NetServerMgr;
        private int numTimesRpcd = 0;
        
        
        public override void OnNetworkSpawn()
        {
            Debug.Log($"[RpcTest] OnNetworkSpawn");

            // Only send an RPC to the server on the client that owns the
            // NetworkObject that owns this NetworkBehaviour instance
            if (IsServer || !IsOwner)
                return;
             
            // Owning (local) client (!server) should ping the server
            // => server should pong the client back.
            TestClientToServerPing();
        }

        public void TestClientToServerPing() =>
            clientMgr.TestClientToServerRpc(numTimesRpcd++, NetworkObjectId);
        
        public void TestServerToClientPong() =>
            serverMgr.TestServerToClientRpc(numTimesRpcd++, NetworkObjectId);
    }
}