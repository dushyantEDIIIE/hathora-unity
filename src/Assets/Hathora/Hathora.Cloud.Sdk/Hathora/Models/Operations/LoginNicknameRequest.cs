
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
    using HathoraSdk.Utils;
    using System;
    using UnityEngine;
    
    
    [Serializable]
    public class LoginNicknameRequest
    {
        [SerializeField]
        [SpeakeasyMetadata("request:mediaType=application/json")]
        public Models.Shared.LoginNicknameRequest LoginNicknameRequestValue { get; set; } = default!;
        
        [SerializeField]
        [SpeakeasyMetadata("pathParam:style=simple,explode=false,name=appId")]
        public string AppId { get; set; } = default!;
        
    }
    
}