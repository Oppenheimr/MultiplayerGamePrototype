#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Extensions;

namespace Editor.MenuItems
{
    public class BackgroundMenuItem : MonoBehaviour
    {
        // Add a menu item to create custom GameObjects.
        // Priority 10 ensures it is grouped with the other menu items of the same kind
        // and propagated to the hierarchy dropdown and hierarchy context menus.
        [MenuItem("GameObject/UI/Background", false, 10)]
        private static void CreateCustomImageObject(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = (menuCommand.context as GameObject);
            go.AddComponent<RectTransform>();
            var image = go.InsImage("Background");
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(image, "Create " + image.name);
            Selection.activeObject = image;
            SetupFill(image.rectTransform);
            SetupImage(image);
        }
        
        [MenuItem("GameObject/UI/Filled Empty", false, 10)]
        private static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            var go = (menuCommand.context as GameObject);
            
            // Create a custom game object
            var filledObject = new GameObject("Filled UI GameObject");
            
            //Add RectTransform component
            var filledRect = filledObject.AddComponent<RectTransform>();
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(filledObject, go);
            
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(filledObject, "Create " + filledObject.name);
            Selection.activeObject = filledObject;
            SetupFill(filledRect);
        }

        private static void SetupFill(RectTransform transform)
        {
            transform.anchorMax = Vector2.one;
            transform.anchorMin = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
            transform.Save();
        }
        
        private static void SetupImage(Image image)
        {
            image.raycastTarget = false;
            image.maskable = false;
            image.Save();
        }
    }
#endif
}
