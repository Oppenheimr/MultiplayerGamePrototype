using PlayFab;
using UnityEngine;

namespace Network.PlayFab
{
    public static class PlayFabExtensions
    {
        public static void UnityReport(this PlayFabError error) => Debug.Log(error.GenerateErrorReport());
    }
}