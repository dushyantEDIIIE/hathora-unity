
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasyapi.dev). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace HathoraCloud.Models.Shared
{
    using Newtonsoft.Json;
    using System;
    using UnityEngine;
    
    [Serializable]
    public class LoginNicknameRequest
    {

        /// <summary>
        /// An alias to represent a player.
        /// </summary>
        [SerializeField]
        [JsonProperty("nickname")]
        public string Nickname { get; set; } = default!;
        
    }
}