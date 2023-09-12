
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasyapi.dev). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace HathoraSdk.Models.Operations
{
    using HathoraSdk.Models.Shared;
    using System;
    using UnityEngine.Networking;
    using UnityEngine;
    
    
    [Serializable]
    public class CreatePublicLobbyResponse: IDisposable
    {
        [SerializeField]
        public string? ContentType { get; set; } = default!;
        
        [SerializeField]
        public string? CreatePublicLobby400ApplicationJSONString { get; set; }
        
        [SerializeField]
        public string? CreatePublicLobby401ApplicationJSONString { get; set; }
        
        [SerializeField]
        public string? CreatePublicLobby404ApplicationJSONString { get; set; }
        
        [SerializeField]
        public string? CreatePublicLobby422ApplicationJSONString { get; set; }
        
        [SerializeField]
        public string? CreatePublicLobby429ApplicationJSONString { get; set; }
        
        [SerializeField]
        public string? CreatePublicLobby500ApplicationJSONString { get; set; }
        
        [SerializeField]
        public Lobby? Lobby { get; set; }
        
        [SerializeField]
        public int StatusCode { get; set; } = default!;
        
        [SerializeField]
        public UnityWebRequest? RawResponse { get; set; }
        
        public void Dispose() {
            if (RawResponse != null) {
                RawResponse.Dispose();
            }
        }
    }
    
}