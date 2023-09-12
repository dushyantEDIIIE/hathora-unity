
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasyapi.dev). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace HathoraSdk.Models.Shared
{
    using Newtonsoft.Json;
    using System;
    using UnityEngine;
    
    
    /// <summary>
    /// A lobby object allows you to store and manage metadata for your rooms.
    /// </summary>
    [Serializable]
    public class Lobby
    {
        /// <summary>
        /// System generated unique identifier for an application.
        /// </summary>
        [SerializeField]
        [JsonProperty("appId")]
        public string AppId { get; set; } = default!;
        
        /// <summary>
        /// When the lobby was created.
        /// </summary>
        [SerializeField]
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = default!;
        
        /// <summary>
        /// Email address for the user that created the lobby.
        /// </summary>
        [SerializeField]
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; } = default!;
        
        /// <summary>
        /// User input to initialize the game state. Object must be smaller than 64KB.
        /// </summary>
        [SerializeField]
        [JsonProperty("initialConfig")]
        public LobbyInitialConfig InitialConfig { get; set; } = default!;
        
        [Obsolete("This field will be removed in a future release, please migrate away from it as soon as possible")]
        [SerializeField]
        [JsonProperty("local")]
        public bool Local { get; set; } = default!;
        
        [SerializeField]
        [JsonProperty("region")]
        public Region Region { get; set; } = default!;
        
        /// <summary>
        /// Unique identifier to a game session or match. Use either a system generated ID or pass in your own.
        /// </summary>
        [SerializeField]
        [JsonProperty("roomId")]
        public string RoomId { get; set; } = default!;
        
        /// <summary>
        /// JSON blob to store metadata for a room. Must be smaller than 1MB.
        /// </summary>
        [SerializeField]
        [JsonProperty("state")]
        public LobbyState? State { get; set; }
        
        /// <summary>
        /// Types of lobbies a player can create.
        /// 
        /// <remarks>
        /// 
        /// `private`: the player who created the room must share the roomId with their friends
        /// 
        /// `public`: visible in the public lobby list, anyone can join
        /// 
        /// `local`: for testing with a server running locally
        /// </remarks>
        /// </summary>
        [SerializeField]
        [JsonProperty("visibility")]
        public LobbyVisibility Visibility { get; set; } = default!;
        
    }
    
}