
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
    
    public enum InviteStatusRescindedType
    {
        [JsonProperty("rescinded")]
        Rescinded,
    }

    public static class InviteStatusRescindedTypeExtension
    {
        public static string Value(this InviteStatusRescindedType value)
        {
            return ((JsonPropertyAttribute)value.GetType().GetMember(value.ToString())[0].GetCustomAttributes(typeof(JsonPropertyAttribute), false)[0]).PropertyName ?? value.ToString();
        }

        public static InviteStatusRescindedType ToEnum(this string value)
        {
            foreach(var field in typeof(InviteStatusRescindedType).GetFields())
            {
                var attributes = field.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                if (attributes.Length == 0)
                {
                    continue;
                }

                var attribute = attributes[0] as JsonPropertyAttribute;
                if (attribute != null && attribute.PropertyName == value)
                {
                    return (InviteStatusRescindedType)field.GetValue(null);
                }
            }

            throw new Exception($"Unknown value {value} for enum InviteStatusRescindedType");
        }
    }

}