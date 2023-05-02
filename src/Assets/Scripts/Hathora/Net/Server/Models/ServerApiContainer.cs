// Created by dylan@hathora.dev

using System;
using Hathora.Cloud.Sdk.Client;
using UnityEngine;

namespace Hathora.Net.Server.Models
{
    /// <summary>
    /// API wrapper container to serialize in HathoraPlayer
    /// </summary>
    [Serializable]
    public struct ServerApiContainer
    {
        // [SerializeField]
        // public NetHathoraServerTodo TodoApi;

        
        public void InitAll(
            Configuration _hathoraSdkConfig, 
            HathoraServerConfig _hathoraServerConfig, 
            NetSession _playerSession)
        {
            Debug.Log("[ServerApiContainer] TODO: InitAll()");
            // TodoApi.Init(_hathoraSdkConfig, _hathoraServerConfig, _playerSession);
        }
    }
}
