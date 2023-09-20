
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasyapi.dev). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace HathoraCloud
{
    using HathoraCloud.Utils;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;

    public interface IHathoraCloudSDK
    {
        public IAppV1SDK AppV1 { get; }
        public IAuthV1SDK AuthV1 { get; }
        public IBillingV1SDK BillingV1 { get; }
        public IBuildV1SDK BuildV1 { get; }
        public IDeploymentV1SDK DeploymentV1 { get; }
        public IDiscoveryV1SDK DiscoveryV1 { get; }
        public ILobbyV1SDK LobbyV1 { get; }
        public ILobbyV2SDK LobbyV2 { get; }
        public ILogV1SDK LogV1 { get; }
        public IManagementV1SDK ManagementV1 { get; }
        public IMetricsV1SDK MetricsV1 { get; }
        public IProcessesV1SDK ProcessesV1 { get; }
        public IRoomV1SDK RoomV1 { get; }
        public IRoomV2SDK RoomV2 { get; }
    }
    
    public class SDKConfig
    {
        public string? AppId;
        
        /// <summary>(!) TEMPORARY WORKAROUND FOR BEARER TOKEN --Dylan</summary>
        public static string? ClientAuthToken { get; set; }
    }

    public class HathoraCloudSDK: IHathoraCloudSDK
    {
        public SDKConfig Config { get; private set; }
        public static List<string> ServerList = new List<string>()
        {
            "https://api.hathora.dev",
            "/",
        };

        private const string _target = "unity";
        private const string _sdkVersion = "0.1.0";
        private const string _sdkGenVersion = "2.112.0";
        private const string _openapiDocVersion = "0.0.1";
        private string _serverUrl = "";
        private ISpeakeasyHttpClient _defaultClient;
        private ISpeakeasyHttpClient _securityClient;
        public IAppV1SDK AppV1 { get; private set; }
        public IAuthV1SDK AuthV1 { get; private set; }
        public IBillingV1SDK BillingV1 { get; private set; }
        public IBuildV1SDK BuildV1 { get; private set; }
        public IDeploymentV1SDK DeploymentV1 { get; private set; }
        public IDiscoveryV1SDK DiscoveryV1 { get; private set; }
        public ILobbyV1SDK LobbyV1 { get; private set; }
        public ILobbyV2SDK LobbyV2 { get; private set; }
        public ILogV1SDK LogV1 { get; private set; }
        public IManagementV1SDK ManagementV1 { get; private set; }
        public IMetricsV1SDK MetricsV1 { get; private set; }
        public IProcessesV1SDK ProcessesV1 { get; private set; }
        public IRoomV1SDK RoomV1 { get; private set; }
        public IRoomV2SDK RoomV2 { get; private set; }

        public HathoraCloudSDK(string? appId = null, string? serverUrl = null, ISpeakeasyHttpClient? client = null)
        {
            _serverUrl = serverUrl ?? HathoraCloudSDK.ServerList[0];

            _defaultClient = new SpeakeasyHttpClient(client);
            _securityClient = _defaultClient;
            
            Config = new SDKConfig()
            {
                AppId = appId,
            };

            AppV1 = new AppV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            AuthV1 = new AuthV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            BillingV1 = new BillingV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            BuildV1 = new BuildV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            DeploymentV1 = new DeploymentV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            DiscoveryV1 = new DiscoveryV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            LobbyV1 = new LobbyV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            LobbyV2 = new LobbyV2SDK(_defaultClient, _securityClient, _serverUrl, Config);
            LogV1 = new LogV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            ManagementV1 = new ManagementV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            MetricsV1 = new MetricsV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            ProcessesV1 = new ProcessesV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            RoomV1 = new RoomV1SDK(_defaultClient, _securityClient, _serverUrl, Config);
            RoomV2 = new RoomV2SDK(_defaultClient, _securityClient, _serverUrl, Config);
        }
    }
}