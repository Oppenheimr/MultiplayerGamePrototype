using System;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class SerializableRoomOptions
    {
        public int maxPlayers = 4;
        public bool cleanupCacheOnLeave = true;
        public Hashtable customRoomProperties;
        public bool isOpen = true;
        public bool isVisible = true;
        public int playerTtl = 10000;
        public int emptyRoomTtl = 10000;
        public bool publishUserId = true;
        public bool suppressRoomEvents;
        public bool selectedNullProperties = true;
        
        public RoomOptions GetNonSerializableRoomOptions()
        {
            return new RoomOptions()
            {
                MaxPlayers = (byte) maxPlayers,
                CleanupCacheOnLeave = cleanupCacheOnLeave,
                CustomRoomProperties = customRoomProperties,
                IsOpen = isOpen,
                IsVisible = isVisible,
                PlayerTtl = playerTtl,
                EmptyRoomTtl = emptyRoomTtl,
                PublishUserId = publishUserId,
                SuppressRoomEvents = suppressRoomEvents,
                DeleteNullProperties = selectedNullProperties,
            };
        }
    }
}