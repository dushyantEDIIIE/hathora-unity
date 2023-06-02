// Created by dylan@hathora.dev

using Newtonsoft.Json;

namespace Hathora.Core.Scripts.Server.Editor.Auth0.Models
{
    public class Auth0DeviceRequest
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }
    }
}