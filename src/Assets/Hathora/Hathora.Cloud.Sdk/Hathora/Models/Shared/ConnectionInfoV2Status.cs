
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
    /// `exposedPort` will only be available when the `status` of a room is "active".
    /// </summary>
    public enum ConnectionInfoV2Status
    {
    	[JsonProperty("starting")]
		Starting,
		[JsonProperty("active")]
		Active,
    }
    
    public static class ConnectionInfoV2StatusExtension
    {
        public static string Value(this ConnectionInfoV2Status value)
        {
            return ((JsonPropertyAttribute)value.GetType().GetMember(value.ToString())[0].GetCustomAttributes(typeof(JsonPropertyAttribute), false)[0]).PropertyName ?? value.ToString();
        }

        public static ConnectionInfoV2Status ToEnum(this string value)
        {
            foreach(var field in typeof(ConnectionInfoV2Status).GetFields())
            {
                var attribute = field.GetCustomAttributes(typeof(JsonPropertyAttribute), false)[0] as JsonPropertyAttribute;
                if (attribute != null && attribute.PropertyName == value)
                {
                    return (ConnectionInfoV2Status)field.GetValue(null);
                }
            }

            throw new Exception($"Unknown value {value} for enum ConnectionInfoV2Status");
        }
    }
    
}