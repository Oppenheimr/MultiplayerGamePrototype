using System.Threading.Tasks;
using Network;
using Network.PlayFab;


namespace Core
{
    public static class Memory
    {
        public static async Task Initialize()
        {
            // await ServicesMemory.Initialize();
            // ObjectPoolManager.Create();
            PhotonManager.Initialize();
            await PlayFabManager.Initialize();
        }
        
        public static async Task Reset()
        {
            await PlayFabManager.Reset();
            // ServicesMemory.Reset();
            // LevelManager.Reset();
        }
    }
}