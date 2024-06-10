using System.Runtime.CompilerServices;
using UnityEngine;

namespace Data.Prefs
{
    public class FloatPref
    {
        private readonly float _defaultValue;
        private readonly string _key;

        public FloatPref(float defaultValue = 0, [CallerMemberName] string key = "")
        {
            this._defaultValue = defaultValue;
            this._key = key;
        }
        
        public float Value
        {
            // Anahtar olarak değişkenin adını kullanıyoruz
            get => PlayerPrefs.GetFloat(_key, _defaultValue);
            set => PlayerPrefs.SetFloat(_key, value);
        }
        
        public bool HasValue => PlayerPrefs.HasKey(_key);
    }
}