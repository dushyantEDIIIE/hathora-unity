
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
    using Newtonsoft.Json;
    using System;
    using UnityEngine;
    
    [Serializable]
    public class Organization
    {

        [SerializeField]
        [JsonProperty("isSingleTenant")]
        public bool IsSingleTenant { get; set; } = default!;

        /// <summary>
        /// System generated unique identifier for an organization. Not guaranteed to have a specific format.
        /// </summary>
        [SerializeField]
        [JsonProperty("orgId")]
        public string OrgId { get; set; } = default!;

        [SerializeField]
        [JsonProperty("stripeCustomerId")]
        public string StripeCustomerId { get; set; } = default!;
    }
}