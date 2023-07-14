// Created by dylan@hathora.dev

using System;
using System.Threading.Tasks;
using Hathora.Cloud.Sdk.Client;
using Hathora.Cloud.Sdk.Model;
using Hathora.Core.Scripts.Runtime.Server.Models;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Server
{
    /// <summary>
    /// Inits and centralizes all Hathora Server [runtime] API wrappers.
    /// 
    /// Unlike HathoraClientMgrBase, we don't need a parent since Server is lower-level
    /// than Client (eg: No UI, Session or net code specific to a platform).
    /// TODO: If this gets more complex, make it an abstract base class; parity with Client.
    /// </summary>
    public class HathoraServerMgr : MonoBehaviour
    {
        #region Serialized Fields
        [Header("(!) Top menu: Hathora/ServerConfigFinder")]
        [SerializeField]
        private HathoraServerConfig hathoraServerConfig;
        
        [Header("API Wrappers for Hathora SDK")]
        [SerializeField]
        private ServerApiContainer serverApis;
        #endregion // Serialized Fields
        
        public static HathoraServerMgr Singleton { get; private set; }
        Process process;
        
        
        #region Init
        private void Awake()
        {
            #if !UNITY_SERVER && !UNITY_EDITOR
            Destroy(this);
            return;
            #endif // !UNITY_SERVER

            Debug.Log("[HathoraServerMgr] Awake");
            setSingleton();
        }

        private void setSingleton()
        {
            if (Singleton != null)
            {
                Debug.LogError("[HathoraServerMgrsetSingleton.] Error: " +
                    "setSingleton: Destroying dupe");
                
                Destroy(gameObject);
                return;
            }

            Singleton = this;
        }

        private void Start()
        {
            validateReqs();
            initApis(_hathoraSdkConfig: null); // Base will create this
            _ = getHathoraProcessAsync(); // !await
        }

        /// <summary>
        /// Gets the Server process info by a special env var that's
        /// *always* included (automatically) in Hathora deployments.
        /// </summary>
        private async Task getHathoraProcessAsync()
        {
            string HATHORA_PROCESS_ID = Environment.GetEnvironmentVariable("HATHORA_PROCESS_ID");
            
            Debug.Log($"[getHathoraProcessAsync.getHathoraProcessAsync] " +
                $"HATHORA_PROCESS_ID: `{HATHORA_PROCESS_ID}`");
            
            bool hasHathoraProcId = !string.IsNullOrEmpty(HATHORA_PROCESS_ID);
            if (!hasHathoraProcId)
                return;
            
            this.process = await serverApis.ServerProcessApi.GetProcessInfoAsync(HATHORA_PROCESS_ID);
        }

        private void validateReqs()
        {
            if (hathoraServerConfig == null)
            {
                Debug.LogError("[HathoraServerMgr] !HathoraServerConfig; " +
                    $"Serialize to {gameObject.name}.{nameof(HathoraServerMgr)}");
            }
        }
        
        /// <summary>
        /// Init all Server [runtime] API wrappers. Uses serialized HathoraServerConfig.
        /// </summary>
        /// <param name="_hathoraSdkConfig">We'll automatically create this, if empty</param>
        private void initApis(Configuration _hathoraSdkConfig = null)
        {
            if (serverApis.ServerProcessApi != null)
                serverApis.ServerProcessApi.Init(hathoraServerConfig, _hathoraSdkConfig);
        }
        #endregion // Init
    }
}
