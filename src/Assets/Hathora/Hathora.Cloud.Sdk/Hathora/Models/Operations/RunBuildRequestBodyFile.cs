
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
    using HathoraSdk.Utils;
    using System;
    using UnityEngine;
    
    
    [Serializable]
    public class RunBuildRequestBodyFile
    {
        [SerializeField]
        [SpeakeasyMetadata("multipartForm:content")]
        public byte[] Content { get; set; } = default!;
        
        [SerializeField]
        [SpeakeasyMetadata("multipartForm:name=file")]
        public string File { get; set; } = default!;
        
    }
    
}