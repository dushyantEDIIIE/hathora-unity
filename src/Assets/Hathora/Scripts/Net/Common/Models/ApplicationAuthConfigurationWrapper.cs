// Created by dylan@hathora.dev

using System;
using Hathora.Cloud.Sdk.Model;

namespace Hathora.Scripts.Net.Common.Models
{
    /// <summary>
    /// This is a wrapper for Hathora SDK's `ApplicationWithDeployment` model.
    /// We'll eventually replace this with a [Serializable] revamp of the model.
    /// </summary>
    [Serializable]
    public class ApplicationAuthConfigurationWrapper
    {
        // [SerializeField] // TODO
        // private ApplicationAuthConfigurationGoogleWrapper _googleWrapper;
        // public ApplicationAuthConfigurationGoogle Google 
        // { 
        //     get => _google; 
        //     set => _google = value;
        // }
        
        
        // /// <summary>"Construct a type with a set of properties K of type T" --from SDK</summary>
        // [SerializeField] // TODO
        // private string _nicknameWrapper;
        //
        // /// <summary>"Construct a type with a set of properties K of type T" --from SDK</summary>
        // public System.Object Nickname // TODO: What's expected in this object, if not a string? 
        // { 
        //     get => _nicknameWrapper; 
        //     set => _nicknameWrapper = value;
        // }
        
        
        // /// <summary>"Construct a type with a set of properties K of type T" --from SDK</summary>
        // [SerializeField]
        // private System.Object _anonymous;
        //
        // /// <summary>"Construct a type with a set of properties K of type T" --from SDK</summary>
        // public System.Object Anonymous 
        // { 
        //     get => _anonymous; 
        //     set => _anonymous = value;
        // }
        
        
        // [SerializeField] // TODO
        // IDictionary<string, object> _additionalProperties;
        // public IDictionary<string, object> AdditionalProperties 
        // { 
        //     get => _additionalProperties;
        //     set => _additionalProperties = value;
        // }
        
        
        public ApplicationAuthConfigurationWrapper(ApplicationAuthConfiguration _appAuthConfig)
        {
            if (_appAuthConfig == null)
                return;

            // this.Google = _appAuthConfig.Google; // TODO
            // this.Nickname = _appAuthConfig.Nickname; // TODO
            // this.Anonymous = _appAuthConfig.Anonymous; // TODO
            // this.AdditionalProperties = _appWithDeployment.AdditionalProperties; // TODO
        }

        public ApplicationAuthConfiguration ToApplicationAuthConfigurationType() => new()
        {
            // Optional >>
            // Google = this.Google, // TODO
            // Nickname = this.Nickname, // TODO
            // Anonymous = this.Anonymous, // TODO
            // AdditionalProperties = this.AdditionalProperties, // TODO
        };
    }
}
