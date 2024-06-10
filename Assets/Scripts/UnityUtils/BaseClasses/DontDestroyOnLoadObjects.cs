using System;
using UnityEngine;
using UnityUtils.Extensions;
using Object = UnityEngine.Object;

namespace UnityUtils.BaseClasses
{
    public class DontDestroyOnLoadObjects<T> : SingletonBehavior<T> where T : Object, new()
    {
        protected virtual void Awake() => gameObject.DontDestroyOnLoadIfSingle<T>();
    }
}