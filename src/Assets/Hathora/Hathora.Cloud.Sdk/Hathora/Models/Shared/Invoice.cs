
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
    /// Billing types
    /// </summary>
    [Serializable]
    public class Invoice
    {
        [SerializeField]
        [JsonProperty("amountDue")]
        public double AmountDue { get; set; } = default!;
        
        [SerializeField]
        [JsonProperty("dueDate")]
        public DateTime DueDate { get; set; } = default!;
        
        [SerializeField]
        [JsonProperty("id")]
        public string Id { get; set; } = default!;
        
        [SerializeField]
        [JsonProperty("month")]
        public double Month { get; set; } = default!;
        
        [SerializeField]
        [JsonProperty("pdfUrl")]
        public string PdfUrl { get; set; } = default!;
        
        [SerializeField]
        [JsonProperty("status")]
        public InvoiceStatus Status { get; set; } = default!;
        
        [SerializeField]
        [JsonProperty("year")]
        public double Year { get; set; } = default!;
        
    }
    
}