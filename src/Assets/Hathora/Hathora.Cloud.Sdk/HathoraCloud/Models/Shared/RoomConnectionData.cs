
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasy.com). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace HathoraCloud.Models.Shared
{
    using HathoraCloud.Models.Shared;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    
    /// <summary>
    /// Connection information for the default and additional ports.
    /// </summary>
    [Serializable]
    public class RoomConnectionData
    {

        [SerializeField]
        [JsonProperty("additionalExposedPorts")]
        public List<ExposedPort> AdditionalExposedPorts { get; set; } = default!;

        /// <summary>
        /// Connection details for an active process.
        /// </summary>
        [SerializeField]
        [JsonProperty("exposedPort")]
        public ExposedPort? ExposedPort { get; set; }

        /// <summary>
        /// System generated unique identifier to a runtime instance of your game server.
        /// </summary>
        [SerializeField]
        [JsonProperty("processId")]
        public string ProcessId { get; set; } = default!;

        /// <summary>
        /// Unique identifier to a game session or match. Use the default system generated ID or overwrite it with your own.<br/>
        /// 
        /// <remarks>
        /// Note: error will be returned if `roomId` is not globally unique.
        /// </remarks>
        /// </summary>
        [SerializeField]
        [JsonProperty("roomId")]
        public string RoomId { get; set; } = default!;

        [SerializeField]
        [JsonProperty("status")]
        public RoomReadyStatus Status { get; set; } = default!;
    }
}