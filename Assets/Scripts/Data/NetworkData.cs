using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace Data
{
    [CreateAssetMenu(fileName = nameof(NetworkData), menuName = "Scriptables/Network Data", order = 1)]
    public class NetworkData : SingletonScriptable<NetworkData>
    {
        [Header("Photon Networking")]
        [SerializeField] private SerializableRoomOptions _defaultOptions = new SerializableRoomOptions()
        {
            maxPlayers = 4,
            cleanupCacheOnLeave = true,
            customRoomProperties = new Hashtable()
            {
                
            },
            isOpen = true,
            isVisible = true,
            playerTtl = 10000,
            emptyRoomTtl = 10000,
            publishUserId = true,
            suppressRoomEvents = false,
            selectedNullProperties = true,
        };
        
        [Header("PlayFab")]
        public string titleId = "64AF8";

        public RoomOptions defaultOptions => _defaultOptions.GetNonSerializableRoomOptions();
        public static string generatedRoomName => "Room " + Random.Range(1000, 10000);
    }
}