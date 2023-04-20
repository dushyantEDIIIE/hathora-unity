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
    /// Process
    /// </summary>
    [DataContract(Name = "Process")]
    public partial class Process : IEquatable<Process>
    {

        /// <summary>
        /// Gets or Sets Region
        /// </summary>
        [DataMember(Name = "region", IsRequired = true, EmitDefaultValue = true)]
        public Region Region { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Process" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected Process()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Process" /> class.
        /// </summary>
        /// <param name="egressedBytes">egressedBytes (required).</param>
        /// <param name="idleSince">idleSince (required).</param>
        /// <param name="activeConnections">activeConnections (required).</param>
        /// <param name="roomSlotsAvailable">roomSlotsAvailable (required).</param>
        /// <param name="draining">draining (required).</param>
        /// <param name="terminatedAt">terminatedAt (required).</param>
        /// <param name="stoppingAt">stoppingAt (required).</param>
        /// <param name="startedAt">startedAt (required).</param>
        /// <param name="startingAt">startingAt (required).</param>
        /// <param name="roomsPerProcess">roomsPerProcess (required).</param>
        /// <param name="port">port (required).</param>
        /// <param name="host">host (required).</param>
        /// <param name="region">region (required).</param>
        /// <param name="processId">processId (required).</param>
        /// <param name="deploymentId">deploymentId (required).</param>
        /// <param name="appId">appId (required).</param>
        public Process(double egressedBytes = default(double), DateTime? idleSince = default(DateTime?), double activeConnections = default(double), double roomSlotsAvailable = default(double), bool draining = default(bool), DateTime? terminatedAt = default(DateTime?), DateTime? stoppingAt = default(DateTime?), DateTime? startedAt = default(DateTime?), DateTime startingAt = default(DateTime), double roomsPerProcess = default(double), double port = default(double), string host = default(string), Region region = default(Region), string processId = default(string), double deploymentId = default(double), string appId = default(string))
        {
            this.EgressedBytes = egressedBytes;
            // to ensure "idleSince" is required (not null)
            if (idleSince == null)
            {
                throw new ArgumentNullException("idleSince is a required property for Process and cannot be null");
            }
            this.IdleSince = idleSince;
            this.ActiveConnections = activeConnections;
            this.RoomSlotsAvailable = roomSlotsAvailable;
            this.Draining = draining;
            // to ensure "terminatedAt" is required (not null)
            if (terminatedAt == null)
            {
                throw new ArgumentNullException("terminatedAt is a required property for Process and cannot be null");
            }
            this.TerminatedAt = terminatedAt;
            // to ensure "stoppingAt" is required (not null)
            if (stoppingAt == null)
            {
                throw new ArgumentNullException("stoppingAt is a required property for Process and cannot be null");
            }
            this.StoppingAt = stoppingAt;
            // to ensure "startedAt" is required (not null)
            if (startedAt == null)
            {
                throw new ArgumentNullException("startedAt is a required property for Process and cannot be null");
            }
            this.StartedAt = startedAt;
            this.StartingAt = startingAt;
            this.RoomsPerProcess = roomsPerProcess;
            this.Port = port;
            // to ensure "host" is required (not null)
            if (host == null)
            {
                throw new ArgumentNullException("host is a required property for Process and cannot be null");
            }
            this.Host = host;
            this.Region = region;
            // to ensure "processId" is required (not null)
            if (processId == null)
            {
                throw new ArgumentNullException("processId is a required property for Process and cannot be null");
            }
            this.ProcessId = processId;
            this.DeploymentId = deploymentId;
            // to ensure "appId" is required (not null)
            if (appId == null)
            {
                throw new ArgumentNullException("appId is a required property for Process and cannot be null");
            }
            this.AppId = appId;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets EgressedBytes
        /// </summary>
        [DataMember(Name = "egressedBytes", IsRequired = true, EmitDefaultValue = true)]
        public double EgressedBytes { get; set; }

        /// <summary>
        /// Gets or Sets IdleSince
        /// </summary>
        [DataMember(Name = "idleSince", IsRequired = true, EmitDefaultValue = true)]
        public DateTime? IdleSince { get; set; }

        /// <summary>
        /// Gets or Sets ActiveConnections
        /// </summary>
        [DataMember(Name = "activeConnections", IsRequired = true, EmitDefaultValue = true)]
        public double ActiveConnections { get; set; }

        /// <summary>
        /// Gets or Sets RoomSlotsAvailable
        /// </summary>
        [DataMember(Name = "roomSlotsAvailable", IsRequired = true, EmitDefaultValue = true)]
        public double RoomSlotsAvailable { get; set; }

        /// <summary>
        /// Gets or Sets Draining
        /// </summary>
        [DataMember(Name = "draining", IsRequired = true, EmitDefaultValue = true)]
        public bool Draining { get; set; }

        /// <summary>
        /// Gets or Sets TerminatedAt
        /// </summary>
        [DataMember(Name = "terminatedAt", IsRequired = true, EmitDefaultValue = true)]
        public DateTime? TerminatedAt { get; set; }

        /// <summary>
        /// Gets or Sets StoppingAt
        /// </summary>
        [DataMember(Name = "stoppingAt", IsRequired = true, EmitDefaultValue = true)]
        public DateTime? StoppingAt { get; set; }

        /// <summary>
        /// Gets or Sets StartedAt
        /// </summary>
        [DataMember(Name = "startedAt", IsRequired = true, EmitDefaultValue = true)]
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// Gets or Sets StartingAt
        /// </summary>
        [DataMember(Name = "startingAt", IsRequired = true, EmitDefaultValue = true)]
        public DateTime StartingAt { get; set; }

        /// <summary>
        /// Gets or Sets RoomsPerProcess
        /// </summary>
        [DataMember(Name = "roomsPerProcess", IsRequired = true, EmitDefaultValue = true)]
        public double RoomsPerProcess { get; set; }

        /// <summary>
        /// Gets or Sets Port
        /// </summary>
        [DataMember(Name = "port", IsRequired = true, EmitDefaultValue = true)]
        public double Port { get; set; }

        /// <summary>
        /// Gets or Sets Host
        /// </summary>
        [DataMember(Name = "host", IsRequired = true, EmitDefaultValue = true)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or Sets ProcessId
        /// </summary>
        [DataMember(Name = "processId", IsRequired = true, EmitDefaultValue = true)]
        public string ProcessId { get; set; }

        /// <summary>
        /// Gets or Sets DeploymentId
        /// </summary>
        [DataMember(Name = "deploymentId", IsRequired = true, EmitDefaultValue = true)]
        public double DeploymentId { get; set; }

        /// <summary>
        /// Gets or Sets AppId
        /// </summary>
        [DataMember(Name = "appId", IsRequired = true, EmitDefaultValue = true)]
        public string AppId { get; set; }

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
            sb.Append("class Process {\n");
            sb.Append("  EgressedBytes: ").Append(EgressedBytes).Append("\n");
            sb.Append("  IdleSince: ").Append(IdleSince).Append("\n");
            sb.Append("  ActiveConnections: ").Append(ActiveConnections).Append("\n");
            sb.Append("  RoomSlotsAvailable: ").Append(RoomSlotsAvailable).Append("\n");
            sb.Append("  Draining: ").Append(Draining).Append("\n");
            sb.Append("  TerminatedAt: ").Append(TerminatedAt).Append("\n");
            sb.Append("  StoppingAt: ").Append(StoppingAt).Append("\n");
            sb.Append("  StartedAt: ").Append(StartedAt).Append("\n");
            sb.Append("  StartingAt: ").Append(StartingAt).Append("\n");
            sb.Append("  RoomsPerProcess: ").Append(RoomsPerProcess).Append("\n");
            sb.Append("  Port: ").Append(Port).Append("\n");
            sb.Append("  Host: ").Append(Host).Append("\n");
            sb.Append("  Region: ").Append(Region).Append("\n");
            sb.Append("  ProcessId: ").Append(ProcessId).Append("\n");
            sb.Append("  DeploymentId: ").Append(DeploymentId).Append("\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
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
            return this.Equals(input as Process);
        }

        /// <summary>
        /// Returns true if Process instances are equal
        /// </summary>
        /// <param name="input">Instance of Process to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Process input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.EgressedBytes == input.EgressedBytes ||
                    this.EgressedBytes.Equals(input.EgressedBytes)
                ) && 
                (
                    this.IdleSince == input.IdleSince ||
                    (this.IdleSince != null &&
                    this.IdleSince.Equals(input.IdleSince))
                ) && 
                (
                    this.ActiveConnections == input.ActiveConnections ||
                    this.ActiveConnections.Equals(input.ActiveConnections)
                ) && 
                (
                    this.RoomSlotsAvailable == input.RoomSlotsAvailable ||
                    this.RoomSlotsAvailable.Equals(input.RoomSlotsAvailable)
                ) && 
                (
                    this.Draining == input.Draining ||
                    this.Draining.Equals(input.Draining)
                ) && 
                (
                    this.TerminatedAt == input.TerminatedAt ||
                    (this.TerminatedAt != null &&
                    this.TerminatedAt.Equals(input.TerminatedAt))
                ) && 
                (
                    this.StoppingAt == input.StoppingAt ||
                    (this.StoppingAt != null &&
                    this.StoppingAt.Equals(input.StoppingAt))
                ) && 
                (
                    this.StartedAt == input.StartedAt ||
                    (this.StartedAt != null &&
                    this.StartedAt.Equals(input.StartedAt))
                ) && 
                (
                    this.StartingAt == input.StartingAt ||
                    (this.StartingAt != null &&
                    this.StartingAt.Equals(input.StartingAt))
                ) && 
                (
                    this.RoomsPerProcess == input.RoomsPerProcess ||
                    this.RoomsPerProcess.Equals(input.RoomsPerProcess)
                ) && 
                (
                    this.Port == input.Port ||
                    this.Port.Equals(input.Port)
                ) && 
                (
                    this.Host == input.Host ||
                    (this.Host != null &&
                    this.Host.Equals(input.Host))
                ) && 
                (
                    this.Region == input.Region ||
                    this.Region.Equals(input.Region)
                ) && 
                (
                    this.ProcessId == input.ProcessId ||
                    (this.ProcessId != null &&
                    this.ProcessId.Equals(input.ProcessId))
                ) && 
                (
                    this.DeploymentId == input.DeploymentId ||
                    this.DeploymentId.Equals(input.DeploymentId)
                ) && 
                (
                    this.AppId == input.AppId ||
                    (this.AppId != null &&
                    this.AppId.Equals(input.AppId))
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
                hashCode = (hashCode * 59) + this.EgressedBytes.GetHashCode();
                if (this.IdleSince != null)
                {
                    hashCode = (hashCode * 59) + this.IdleSince.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.ActiveConnections.GetHashCode();
                hashCode = (hashCode * 59) + this.RoomSlotsAvailable.GetHashCode();
                hashCode = (hashCode * 59) + this.Draining.GetHashCode();
                if (this.TerminatedAt != null)
                {
                    hashCode = (hashCode * 59) + this.TerminatedAt.GetHashCode();
                }
                if (this.StoppingAt != null)
                {
                    hashCode = (hashCode * 59) + this.StoppingAt.GetHashCode();
                }
                if (this.StartedAt != null)
                {
                    hashCode = (hashCode * 59) + this.StartedAt.GetHashCode();
                }
                if (this.StartingAt != null)
                {
                    hashCode = (hashCode * 59) + this.StartingAt.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.RoomsPerProcess.GetHashCode();
                hashCode = (hashCode * 59) + this.Port.GetHashCode();
                if (this.Host != null)
                {
                    hashCode = (hashCode * 59) + this.Host.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.Region.GetHashCode();
                if (this.ProcessId != null)
                {
                    hashCode = (hashCode * 59) + this.ProcessId.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.DeploymentId.GetHashCode();
                if (this.AppId != null)
                {
                    hashCode = (hashCode * 59) + this.AppId.GetHashCode();
                }
                if (this.AdditionalProperties != null)
                {
                    hashCode = (hashCode * 59) + this.AdditionalProperties.GetHashCode();
                }
                return hashCode;
            }
        }

    }

}
