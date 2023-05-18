// Created by dylan@hathora.dev

using System.IO;
using Hathora.Scripts.SdkWrapper.Models;
using Hathora.Scripts.Utils;
using UnityEngine;

namespace Hathora.Scripts.Net.Common
{
    /// <summary>
    /// Sensitive info will not be included in Client builds.
    /// For meta objects (like the banner and btns), see HathoraConfigEditor.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(NetHathoraConfig), menuName = "Hathora/Config File")]
    public class NetHathoraConfig : ScriptableObject
    {
        #region Vars
        // ----------------------------------------
        [SerializeField]
        private HathoraCoreOpts _hathoraCoreOpts;
        public HathoraCoreOpts HathoraCoreOpts
        {
            get => _hathoraCoreOpts;
            set => _hathoraCoreOpts = value;
        }

        [SerializeField]
        private HathoraAutoBuildOpts linuxHathoraAutoBuildOpts;
        public HathoraAutoBuildOpts LinuxHathoraAutoBuildOpts
        {
            get => linuxHathoraAutoBuildOpts;
            set => linuxHathoraAutoBuildOpts = value;
        }

        [SerializeField] 
        private HathoraDeployOpts _hathoraDeployOpts;
        public HathoraDeployOpts HathoraDeployOpts
        {
            get => _hathoraDeployOpts;
            set => _hathoraDeployOpts = value;
        }
        
        [SerializeField]
        private HathoraLobbyRoomOpts _hathoraLobbyRoomOpts;
        public HathoraLobbyRoomOpts HathoraLobbyRoomOpts
        {
            get => _hathoraLobbyRoomOpts;
            set => _hathoraLobbyRoomOpts = value;
        }

        /// <summary>
        /// Explicit typings for FindNestedProperty() calls
        /// </summary>
        public struct SerializedFieldNames
        {
            public static string HathoraCoreOpts => nameof(_hathoraCoreOpts);
            public static string LinuxAutoBuildOpts => nameof(linuxHathoraAutoBuildOpts);
            public static string HathoraDeployOpts => nameof(_hathoraDeployOpts);
            public static string HathoraLobbyRoomOpts => nameof(_hathoraLobbyRoomOpts);
        }
        #endregion // Vars


        /// <summary>(!) Don't use OnEnable for ScriptableObjects</summary>
        private void OnValidate()
        {
        }

        public bool MeetsBuildBtnReqs() =>
            !string.IsNullOrEmpty(linuxHathoraAutoBuildOpts.ServerBuildDirName) &&
            !string.IsNullOrEmpty(linuxHathoraAutoBuildOpts.ServerBuildExeName);
                                                          
        public bool MeetsDeployBtnReqs() =>
            !string.IsNullOrEmpty(_hathoraCoreOpts.AppId) &&
            _hathoraCoreOpts.DevAuthOpts.HasAuthToken &&
            !string.IsNullOrEmpty(linuxHathoraAutoBuildOpts.ServerBuildDirName) &&
            !string.IsNullOrEmpty(linuxHathoraAutoBuildOpts.ServerBuildExeName) &&
            _hathoraDeployOpts.TransportInfo.PortNumber > 1024;

        /// <summary>
        /// Combines path, then normalizes
        /// </summary>
        /// <returns></returns>
        public string GetNormalizedPathToBuildExe() => Path.GetFullPath(Path.Combine(
            HathoraUtils.GetNormalizedPathToProjRoot(), 
            linuxHathoraAutoBuildOpts.ServerBuildDirName, 
            linuxHathoraAutoBuildOpts.ServerBuildExeName));
        

    }
}