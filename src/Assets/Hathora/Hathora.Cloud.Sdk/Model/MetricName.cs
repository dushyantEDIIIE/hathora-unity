/*
 * Hathora Cloud API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 0.0.1
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using OpenAPIDateConverter = Hathora.Cloud.Sdk.Client.OpenAPIDateConverter;

namespace Hathora.Cloud.Sdk.Model
{
    /// <summary>
    /// Defines MetricName
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MetricName
    {
        /// <summary>
        /// Enum Cpu for value: cpu
        /// </summary>
        [EnumMember(Value = "cpu")]
        Cpu = 1,

        /// <summary>
        /// Enum Memory for value: memory
        /// </summary>
        [EnumMember(Value = "memory")]
        Memory = 2,

        /// <summary>
        /// Enum RateEgress for value: rate_egress
        /// </summary>
        [EnumMember(Value = "rate_egress")]
        RateEgress = 3,

        /// <summary>
        /// Enum TotalEgress for value: total_egress
        /// </summary>
        [EnumMember(Value = "total_egress")]
        TotalEgress = 4

    }

}