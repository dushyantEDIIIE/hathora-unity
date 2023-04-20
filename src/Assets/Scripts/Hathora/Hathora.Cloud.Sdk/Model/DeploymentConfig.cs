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
    /// DeploymentConfig
    /// </summary>
    [DataContract(Name = "DeploymentConfig")]
    public partial class DeploymentConfig : IEquatable<DeploymentConfig>
    {

        /// <summary>
        /// Gets or Sets PlanName
        /// </summary>
        [DataMember(Name = "planName", IsRequired = true, EmitDefaultValue = true)]
        public PlanName PlanName { get; set; }

        /// <summary>
        /// Gets or Sets TransportType
        /// </summary>
        [DataMember(Name = "transportType", IsRequired = true, EmitDefaultValue = true)]
        public TransportType TransportType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeploymentConfig" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected DeploymentConfig()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeploymentConfig" /> class.
        /// </summary>
        /// <param name="env">env (required).</param>
        /// <param name="roomsPerProcess">roomsPerProcess (required).</param>
        /// <param name="planName">planName (required).</param>
        /// <param name="transportType">transportType (required).</param>
        /// <param name="containerPort">containerPort (required).</param>
        public DeploymentConfig(List<DeploymentConfigEnvInner> env = default(List<DeploymentConfigEnvInner>), double roomsPerProcess = default(double), PlanName planName = default(PlanName), TransportType transportType = default(TransportType), double containerPort = default(double))
        {
            // to ensure "env" is required (not null)
            if (env == null)
            {
                throw new ArgumentNullException("env is a required property for DeploymentConfig and cannot be null");
            }
            this.Env = env;
            this.RoomsPerProcess = roomsPerProcess;
            this.PlanName = planName;
            this.TransportType = transportType;
            this.ContainerPort = containerPort;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Env
        /// </summary>
        [DataMember(Name = "env", IsRequired = true, EmitDefaultValue = true)]
        public List<DeploymentConfigEnvInner> Env { get; set; }

        /// <summary>
        /// Gets or Sets RoomsPerProcess
        /// </summary>
        [DataMember(Name = "roomsPerProcess", IsRequired = true, EmitDefaultValue = true)]
        public double RoomsPerProcess { get; set; }

        /// <summary>
        /// Gets or Sets ContainerPort
        /// </summary>
        [DataMember(Name = "containerPort", IsRequired = true, EmitDefaultValue = true)]
        public double ContainerPort { get; set; }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class DeploymentConfig {\n");
            sb.Append("  Env: ").Append(Env).Append("\n");
            sb.Append("  RoomsPerProcess: ").Append(RoomsPerProcess).Append("\n");
            sb.Append("  PlanName: ").Append(PlanName).Append("\n");
            sb.Append("  TransportType: ").Append(TransportType).Append("\n");
            sb.Append("  ContainerPort: ").Append(ContainerPort).Append("\n");
            sb.Append("  AdditionalProperties: ").Append(AdditionalProperties).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as DeploymentConfig);
        }

        /// <summary>
        /// Returns true if DeploymentConfig instances are equal
        /// </summary>
        /// <param name="input">Instance of DeploymentConfig to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DeploymentConfig input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Env == input.Env ||
                    this.Env != null &&
                    input.Env != null &&
                    this.Env.SequenceEqual(input.Env)
                ) && 
                (
                    this.RoomsPerProcess == input.RoomsPerProcess ||
                    this.RoomsPerProcess.Equals(input.RoomsPerProcess)
                ) && 
                (
                    this.PlanName == input.PlanName ||
                    this.PlanName.Equals(input.PlanName)
                ) && 
                (
                    this.TransportType == input.TransportType ||
                    this.TransportType.Equals(input.TransportType)
                ) && 
                (
                    this.ContainerPort == input.ContainerPort ||
                    this.ContainerPort.Equals(input.ContainerPort)
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && !this.AdditionalProperties.Except(input.AdditionalProperties).Any());
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Env != null)
                {
                    hashCode = (hashCode * 59) + this.Env.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.RoomsPerProcess.GetHashCode();
                hashCode = (hashCode * 59) + this.PlanName.GetHashCode();
                hashCode = (hashCode * 59) + this.TransportType.GetHashCode();
                hashCode = (hashCode * 59) + this.ContainerPort.GetHashCode();
                if (this.AdditionalProperties != null)
                {
                    hashCode = (hashCode * 59) + this.AdditionalProperties.GetHashCode();
                }
                return hashCode;
            }
        }

    }

}
