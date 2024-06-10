using Data.Prefs;
using UnityEngine;

namespace Data
{
    public static class LocalDatabase
    {
        public static readonly IntPref lastSelectedCharacter = new IntPref();
        public static readonly StringPref email = new StringPref();
        public static readonly StringPref password = new StringPref();
    }
}