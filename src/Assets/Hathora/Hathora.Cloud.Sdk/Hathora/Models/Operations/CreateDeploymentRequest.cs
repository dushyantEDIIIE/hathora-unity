
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
    public class CreateDeploymentRequest
    {
        [SerializeField]
        [SpeakeasyMetadata("request:mediaType=application/json")]
        public DeploymentConfig DeploymentConfig { get; set; } = default!;
        
        [SerializeField]
        [SpeakeasyMetadata("pathParam:style=simple,explode=false,name=appId")]
        public string AppId { get; set; } = default!;
        
        [SerializeField]
        [SpeakeasyMetadata("pathParam:style=simple,explode=false,name=buildId")]
        public int BuildId { get; set; } = default!;
        
    }
    
}