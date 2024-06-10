using UnityEngine;

namespace Config
{
    public static class MultiplayerPrototypeConfig
    {
        //Photon
        public const byte SYNC_EVENT = 0;
        public const byte SYNC_ANIM = 1;
        public const string SELECTED_CHARACTER_KEY = "SelectedCharacter";
        public const string PLAYER_READY_KEY = "IsPlayerReady";
        public const string PLAYER_LOADED_LEVEL_KEY = "PlayerLoadedLevel";
        public const string CONNECTION_STATUS_MESSAGE_ROOT = "  Connection Status: ";
        
        //PlayFab
        public const string QUEUE_NAME = "TestCollectedWoods";
        
        //Database
        public const string COLLECTION_WOOD_KEY = "CollectedWoods";

        public static Color GetColor(int colorChoice) => colorChoice switch
        {
            0 => Color.red,
            1 => Color.green,
            2 => Color.blue,
            3 => Color.yellow,
            4 => Color.cyan,
            5 => Color.grey,
            6 => Color.magenta,
            7 => Color.white,
            _ => Color.black
        };
    }
}