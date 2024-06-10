using System.Runtime.CompilerServices;
using UnityEngine;

namespace Data.Prefs
{
    public class StringPref
    {
        private readonly string _defaultValue;
        private readonly string _key;

        public StringPref(string defaultValue = "", [CallerMemberName] string key = "")
        {
            this._defaultValue = defaultValue;
            this._key = key;
        }
        
        public string Value
        {
            // Anahtar olarak değişkenin adını kullanıyoruz
            get => PlayerPrefs.GetString(_key, _defaultValue);
            set => PlayerPrefs.SetString(_key, value);
        }
        
        public bool HasValue => PlayerPrefs.HasKey(_key);
    }
}