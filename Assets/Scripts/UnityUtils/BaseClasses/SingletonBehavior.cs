using UnityEngine;

namespace UnityUtils.BaseClasses
{
    public class SingletonBehavior : MonoBehaviour
    {
    }

    /// <summary>
    /// Türetilecek tim sınıflara "Ins" tanımı ekler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonBehavior<T> : SingletonBehavior where T : Object, new()
    {
        private static T _instance;
        public static T Instance => _instance ? _instance : (_instance = FindObjectOfType<T>(false));

        protected static bool InstanceIsAvailable => _instance;
        public void SetInstance(T instance) => _instance = instance;
    }
}