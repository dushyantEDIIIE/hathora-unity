// dylan@hathora.dev

using Hathora.Cloud.Sdk.Model;
using UnityEngine;
using System;

namespace Hathora.Core.Scripts.Runtime.Server.Models
{
    [Serializable] 
    public class HathoraLobbyRoomOpts
    {
        #region Hathora Region
        /// <summary>
        /// (!) Hathora SDK Enums starts at index 1; not 0: Care of indexes.
        /// Since this Enum isn't alphabatized, also care if you Sort() the list.
        /// </summary>
        [SerializeField]
        private Region _hathoraRegion = Region.Seattle;
        
        /// <summary>
        /// (!) Hathora SDK Enums starts at index 1; not 0: Care of indexes.
        /// Since this Enum isn't alphabatized, also care if you Sort() the list.
        /// </summary>
        public Region HathoraRegion
        { 
            get => _hathoraRegion;
            set => _hathoraRegion = value;
        }

        /// <summary>
        /// (!) Hathora SDK Enums starts at index 1; not 0: Care of indexes.
        /// Since this Enum isn't alphabatized, also care if you Sort() the list.
        /// </summary>
        [SerializeField]
        private int _regionSelectedIndex = (int)Region.Seattle;
        
        /// <summary>
        /// (!) Hathora SDK Enums starts at index 1; not 0: Care of indexes.
        /// Since this Enum isn't alphabatized, also care if you Sort() the list.
        /// </summary>
        public int RegionSelectedIndex
        {
            get => _regionSelectedIndex;
            set => _regionSelectedIndex = value;
        }


        // [SerializeField] // While Rooms last only 5m, don't actually persist this
        private HathoraCachedRoomConnection _lastCreatedRoomConnection;
        public HathoraCachedRoomConnection LastCreatedRoomConnection
        {
            get => _lastCreatedRoomConnection;
            set => _lastCreatedRoomConnection = value;
        }

        /// <summary>
        /// We check if there's a RoomId, and null checking leading up to it.
        /// </summary>
        public bool HasLastCreatedRoomConnection => 
            !string.IsNullOrEmpty(_lastCreatedRoomConnection?.Room?.RoomId);
        #endregion // Hathora Region
    }
}
 