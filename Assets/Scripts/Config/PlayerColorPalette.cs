using Photon.Realtime;
using UnityEngine;

namespace Config
{
    public static class PlayerColorPalette
    {
        public const string PLAYER_COLOR_KEY = "PlayerColor";
        
        public static readonly Color Red = new Color(1, 0, 0);
        public static readonly Color Green = new Color(0, 1, 0);
        public static readonly Color Blue = new Color(0, 0, 1);
        public static readonly Color Yellow = new Color(1, 1, 0);
        public static readonly Color Magenta = new Color(1, 0, 1);
        public static readonly Color Cyan = new Color(0, 1, 1);
        public static readonly Color Orange = new Color(1, 0.5f, 0);
        public static readonly Color Purple = new Color(0.5f, 0, 1);
        public static readonly Color LightBlue = new Color(0, 0.5f, 1);  
        public static readonly Color LimeGreen = new Color(0.5f, 1, 0);    
        public static readonly Color Pink = new Color(1, 0, 0.5f);
        public static readonly Color OceanGreen = new Color(0, 1, 0.5f);    
        public static readonly Color Gray = new Color(0.5f, 0.5f, 0.5f);
        public static readonly Color DarknessRed = new Color(0.5f, 0, 0);    
        public static readonly Color DarknessGreen = new Color(0, 0.5f, 0);    
        public static readonly Color DarknessBlue = new Color(0, 0, 0.5f);
        public static readonly Color Olive = new Color(0.5f, 0.5f, 0);
        public static readonly Color DarknessPurple = new Color(0.5f, 0, 0.5f); 
        
        public static readonly Color[] Colors =
        {
            Red,
            Green,
            Blue,
            Yellow,
            Magenta,
            Cyan,
            Orange,
            Purple,
            LightBlue,
            LimeGreen,
            Pink,
            OceanGreen,
            Gray,
            DarknessRed,
            DarknessGreen,
            DarknessBlue,
            Olive,
            DarknessPurple
        };

        public static int GetColorIndex(Color color)
        {
            for (int i = 0; i < Colors.Length; i++)
            {
                if (Colors[i] == color)
                    return i;
            }

            return -1;
        }
        
        public static Color GetColor(int index) => Colors[index];
        public static Color GetRandomColorIndex() => Colors[Random.Range(0, Colors.Length)];
    }
}