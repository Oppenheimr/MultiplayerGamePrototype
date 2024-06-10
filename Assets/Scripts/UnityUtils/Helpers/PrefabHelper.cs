using System.IO;
using UnityEditor;
using UnityEngine;
using UnityUtils.Extensions;

namespace UnityUtils.Helpers
{
    public static class PrefabHelper
    {
        #region Unity Editor
#if UNITY_EDITOR
        public static void SavePrefab(GameObject target, string saveLocation, bool select = true)
        {
            StringExtensions.CheckDuplicate(ref saveLocation);
            var saved = PrefabUtility.SaveAsPrefabAsset(target, saveLocation);
            Object.DestroyImmediate(target);
            target = (GameObject) PrefabUtility.InstantiatePrefab(saved);
            if (select) target.Select();
        }
        
        public static void SavePrefab(GameObject target)
        {
            var saveLocation = string.Format("Assets{0}Resources{0}{1}.prefab",
                Path.DirectorySeparatorChar, target.name);
         
            StringExtensions.CheckDuplicate(ref saveLocation);
            
            var saved = PrefabUtility.SaveAsPrefabAsset(target, saveLocation);
            Object.DestroyImmediate(target);
            target = (GameObject) PrefabUtility.InstantiatePrefab(saved);
            target.Select();
        }
        
        public static GameObject InstantiateAndUnpackPrefab(string folder, string name) => InstantiateAndUnpackPrefab(
                Resources.Load(Path.Combine(folder, name)) as GameObject);
        
        public static GameObject InstantiateAndUnpackPrefab(GameObject selected)
        {
            //Null check
            GameObject item = null;
            if (selected == null)
                return selected;
            
            //Instantiate
            if (selected.scene.IsValid())
                item = Object.Instantiate(selected);
            else
                item = PrefabUtility.InstantiatePrefab(selected) as GameObject;
            
            // Check if the selected object is a prefab instance
            if (PrefabUtility.IsPartOfPrefabInstance(item))
            {
                // Unpack the prefab completely
                PrefabUtility.UnpackPrefabInstance(item, 
                    PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            }
            return item;
        }
#endif
        #endregion
    }
}