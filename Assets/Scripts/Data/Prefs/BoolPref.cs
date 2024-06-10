using System.Runtime.CompilerServices;
using UnityEngine;

namespace Data.Prefs
{
    public class BoolPref
    {
        private readonly bool _defaultValue;
        private readonly string _key;

        public BoolPref(bool defaultValue = false, [CallerMemberName] string key = "")
        {
            this._defaultValue = defaultValue;
            this._key = key;
        }
        
        public bool Value
        {
            // Anahtar olarak değişkenin adını kullanıyoruz
            get => PlayerPrefs.GetInt(_key, _defaultValue ? 1 : 0) == 1;
            set => PlayerPrefs.SetInt(_key, value ? 1 : 0);
        }
        
        public bool HasValue => PlayerPrefs.HasKey(_key);
    }
}