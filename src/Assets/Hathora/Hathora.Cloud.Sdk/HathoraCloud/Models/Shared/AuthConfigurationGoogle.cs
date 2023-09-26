
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
    
    /// <summary>
    /// Enable google auth for your application.
    /// </summary>
    [Serializable]
    public class AuthConfigurationGoogle
    {

        /// <summary>
        /// A Google generated token representing the developer&amp;apos;s credentials for &lt;a href=&quot;https://console.cloud.google.com/apis/dashboard?pli=1&amp;amp;project=discourse-login-388921&quot;&gt;Google&amp;apos;s API Console&lt;/a&gt;. Learn how to get a `clientId` &lt;a href=&quot;https://developers.google.com/identity/gsi/web/guides/get-google-api-clientid&quot;&gt;here&lt;/a&gt;.
        /// </summary>
        [SerializeField]
        [JsonProperty("clientId")]
        public string ClientId { get; set; } = default!;
        
    }
}