using System.Threading.Tasks;
using Data;
using Network.PlayFab.Models;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using UnityEngine;

namespace Network.PlayFab
{
    public class Authentication
    {
        public static bool initialize;
        public static bool isLogged => PlayFabClientAPI.IsClientLoggedIn();
        
        public static async Task Initialize()
        {
            return;
            //This part for auto login
            if (LocalDatabase.email.HasValue)
            {
                await Login(LocalDatabase.email.Value, LocalDatabase.password.Value);
            }
            else
            {
#if UNITY_ANDROID
                await LoginWithAndroidId();
#elif UNITY_IOS
                await LoginWithIOSId();
#endif
            }

            initialize = true;
        }

        public static async Task<ServerResultItem<GetAccountInfoResult>> Login(string userEmail, string userPassword)
        {
            TaskCompletionSource<ServerResultItem<GetAccountInfoResult>> tcs = new TaskCompletionSource<ServerResultItem<GetAccountInfoResult>>();
            var request = new LoginWithEmailAddressRequest
            {
                Email = userEmail,
                Password = userPassword
            };

            PlayFabClientAPI.LoginWithEmailAddress(request, async result => {
                await Database.GetStats();
                var accountInfo = await Database.GetAccountInfo(result.PlayFabId);

                await Database.UpdateGameLaunch();
                // ExecuteFunctionRequest cloudFunction = new ExecuteFunctionRequest()
                // {
                //     FunctionName = "UpdateGameLaunch",
                //     FunctionParameter = new { partitionKey = result.PlayFabId}
                // };
                // PlayFabCloudScriptAPI.ExecuteFunction(cloudFunction,result =>
                //     {
                //         Debug.Log("Automation function executed successfully.");
                //         // Sonuçları işleyin
                //     },
                //     error =>
                //     {
                //         Debug.LogError("Error executing automation function: " + error.ErrorMessage);
                //         // Hata durumunda işlem yapın
                //     });
                
                tcs.SetResult(new ServerResultItem<GetAccountInfoResult> { success = true, item = accountInfo.item });
            }, error => {
                error.UnityReport();
                tcs.SetResult(new ServerResultItem<GetAccountInfoResult>(error));
            });
            return await tcs.Task;
        }
        
        public static async Task<ServerResult> LoginWithAndroidId()
        {
            TaskCompletionSource<ServerResult> tcs = new TaskCompletionSource<ServerResult>();
            
            var requestIOS = new LoginWithIOSDeviceIDRequest
            {
                DeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            };
           
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, async result => {
                await Database.GetStats();
                tcs.SetResult(new ServerResult { success = true });
            }, error => {
                tcs.SetResult(new ServerResult(error));
            });
            
            return await tcs.Task;
        }
        
        public static async Task<ServerResult> LoginWithIOSId()
        {
            TaskCompletionSource<ServerResult> tcs = new TaskCompletionSource<ServerResult>();
            
            var requestAndroid = new LoginWithAndroidDeviceIDRequest
            {
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            };
            
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, async result => {
                await Database.GetStats();
                tcs.SetResult(new ServerResult { success = true });
            }, error => {
                tcs.SetResult(new ServerResult(error));
            });
            
            return await tcs.Task;
        }

        public static async Task<ServerResult> Register(string userEmail, string userPassword, string username)
        {
            TaskCompletionSource<ServerResult> tcs = new TaskCompletionSource<ServerResult>();

            var registerRequest = new RegisterPlayFabUserRequest
            {
                Email = userEmail,
                Password = userPassword,
                Username = username
            };
            
            PlayFabClientAPI.RegisterPlayFabUser(registerRequest, async result => {
                await Database.GetStats();
                tcs.SetResult(new ServerResult { success = true });
            }, error => {
                error.UnityReport();
                tcs.SetResult(new ServerResult(error));
            });
            return await tcs.Task;
        }
        
        public static async Task Reset()
        {
            initialize = false;
        }
    }
}