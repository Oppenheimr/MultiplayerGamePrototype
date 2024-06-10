using UnityEngine;
using UnityUtils.BaseClasses;
using UnityUtils.Extensions;

namespace Core
{
    public class CoreManager : SingletonBehavior<CoreManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Main()
        {
            // Create an empty GameObject
            GameObject gameObject = new GameObject("Core Manager");
            // Add this Component
            gameObject.AddComponent<CoreManager>();
        }

        protected async void Awake()
        {
            if (!gameObject.DontDestroyOnLoadIfSingle<CoreManager>())
                return;
            
            //TO DO : Bu fonksyon asenkron olacak bitince load scene geçecek
            await Memory.Initialize();
        }
        
        private async void OnApplicationQuit()
        {
            await Memory.Reset();
        }
    }
}