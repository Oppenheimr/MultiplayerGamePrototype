using System.Runtime.CompilerServices;
using UnityEngine;

namespace Data.Prefs
{
    public class IntPref
    {
        private readonly int _defaultValue;
        private readonly string _key;

        public IntPref(int defaultValue = 0, [CallerMemberName] string key = "")
        {
            this._defaultValue = defaultValue;
            this._key = key;
        }
        
        public int Value
        {
            // Anahtar olarak değişkenin adını kullanıyoruz
            get => PlayerPrefs.GetInt(_key, _defaultValue);
            set => PlayerPrefs.SetInt(_key, value);
        }
        
        public bool HasValue => PlayerPrefs.HasKey(_key);
    }
}