using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityUtils.Extensions
{
    public static class ComponentExtensions
    {
        public static bool CompareTags(this Component component, params string[] tags) =>
            tags.Any(component.CompareTag);
        
        public static bool IsPlayer(this Component comp) => comp.CompareTag("Player");
        public static bool IsBot(this Component comp) => comp.CompareTag("AI");
        
        public static void DestroyImmediateInChildren(this Component target)
        {
            for (int i = 0; i < target.transform.childCount; i++)
                Object.DestroyImmediate(target.transform.GetChild(i).gameObject);
        }

        public static bool SafeComponentCast<T>(this object target, out T castedComponent) where T : Component
        {
            castedComponent = null;
            
            if ((T)target == null)
                return false;

            castedComponent = (T)target;
            return true;
        }
    }
}