using Config;
using Photon.Realtime;
using UnityEngine;

namespace Network.Photon
{
    public static class PhotonExtensions
    {
        public static Color GetPlayerColor(this Player player)
        {
            if (player.CustomProperties.TryGetValue(PlayerColorPalette.PLAYER_COLOR_KEY, out object color))
                return PlayerColorPalette.GetColor((int)color);
            return Color.magenta;
        }
        
        public static int GetPlayerColorIndex(this Player player)
        {
            if (player.CustomProperties.TryGetValue(PlayerColorPalette.PLAYER_COLOR_KEY, out object color))
                return (int)color;
            return PlayerColorPalette.GetColorIndex(PlayerColorPalette.Magenta);
        }
    }
}