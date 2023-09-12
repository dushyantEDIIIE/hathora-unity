
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
    /// Metadata on an allocated instance of a room.
    /// </summary>
    [Serializable]
    public class RoomAllocation
    {
        /// <summary>
        /// System generated unique identifier to a runtime instance of your game server.
        /// </summary>
        [SerializeField]
        [JsonProperty("processId")]
        public string ProcessId { get; set; } = default!;
        
        /// <summary>
        /// System generated unique identifier to an allocated instance of a room.
        /// </summary>
        [SerializeField]
        [JsonProperty("roomAllocationId")]
        public string RoomAllocationId { get; set; } = default!;
        
        [SerializeField]
        [JsonProperty("scheduledAt")]
        public DateTime ScheduledAt { get; set; } = default!;
        
        [SerializeField]
        [JsonProperty("unscheduledAt")]
        public DateTime UnscheduledAt { get; set; } = default!;
        
    }
    
}