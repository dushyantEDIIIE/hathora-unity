
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
    using System;
    using UnityEngine;
    
    /// <summary>
    /// Make all properties in T optional
    /// </summary>
    [Serializable]
    public class PaymentMethod
    {

        [SerializeField]
        [JsonProperty("ach")]
        public AchPaymentMethod? Ach { get; set; }

        [SerializeField]
        [JsonProperty("card")]
        public CardPaymentMethod? Card { get; set; }

        [SerializeField]
        [JsonProperty("link")]
        public LinkPaymentMethod? Link { get; set; }
    }
}