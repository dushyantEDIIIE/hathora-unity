
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
    public class ListActivePublicLobbiesRequest
    {
        [SerializeField]
        [SpeakeasyMetadata("pathParam:style=simple,explode=false,name=appId")]
        public string AppId { get; set; } = default!;
        
        /// <summary>
        /// Region to filter by. If omitted, active public lobbies in all regions will be returned.
        /// </summary>
        [SerializeField]
        [SpeakeasyMetadata("queryParam:style=form,explode=true,name=region")]
        public Region? Region { get; set; }
        
    }
    
}