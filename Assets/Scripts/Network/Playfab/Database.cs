using System.Collections.Generic;
using System.Threading.Tasks;
using Config;
using Network.PlayFab.Models;
using PlayFab;
using PlayFab.ClientModels;
#if UNITY_EDITOR
using PlayFab.PfEditor.Json;
#endif
using UnityEngine;
using JsonObject = PlayFab.Json.JsonObject;

namespace Network.PlayFab
{
    public class Database
    {
        public static int collectedWoods;

        
        public static async Task<ServerResultItem<GetAccountInfoResult>> GetAccountInfo(string playFabId)
        {
            TaskCompletionSource<ServerResultItem<GetAccountInfoResult>> tcs = new TaskCompletionSource<ServerResultItem<GetAccountInfoResult>>();
            
            // Kullanıcı adı almak için GetAccountInfo çağrısı yap
            var getAccountInfoRequest = new GetAccountInfoRequest { PlayFabId = playFabId };

            PlayFabClientAPI.GetAccountInfo(getAccountInfoRequest, accountInfoResult =>
            {
                // LoginResult içine kullanıcı adını ekle
                var loginResultItem = new ServerResultItem<GetAccountInfoResult>
                {
                    success = true,
                    item = accountInfoResult,
                };

                tcs.SetResult(loginResultItem);
            }, accountInfoError =>
            {
                accountInfoError.UnityReport();
                tcs.SetResult(new ServerResultItem<GetAccountInfoResult>(accountInfoError));
            });
            
            return await tcs.Task;
        }
        
        public static async Task<ServerResult> SetStats()
        {
            TaskCompletionSource<ServerResult> tcs = new TaskCompletionSource<ServerResult>();
            
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest 
                {
                    // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
                    Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate { StatisticName = "CollectedWoods", Value = collectedWoods },
                    }
                }, result => {
                    Debug.Log("User statistics updated");
                    tcs.SetResult(new ServerResult { success = true });
                }, error => {                
                    error.UnityReport();
                    tcs.SetResult(new ServerResult(error));});
            
            return await tcs.Task;
        }

        public static async Task<ServerResult> GetStats()
        {
            TaskCompletionSource<ServerResult> tcs = new TaskCompletionSource<ServerResult>();

            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
                result => {
                    OnGetStats(result);
                    tcs.SetResult(new ServerResult { success = true });
                },
                error => {                
                    error.UnityReport();
                    tcs.SetResult(new ServerResult(error));
                });
            
            return await tcs.Task;
        }

        private static void OnGetStats(GetPlayerStatisticsResult result)
        {
            Debug.Log("Received the following Statistics:");
            foreach (var eachStat in result.Statistics)
            {
                Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
                switch (eachStat.StatisticName)
                {
                    case MultiplayerPrototypeConfig.COLLECTION_WOOD_KEY:
                        collectedWoods = eachStat.Value;
                        Debug.Log("Database - Collected Woods: " + collectedWoods);
                        break;
                }
            }
        }
        
        public static async Task<ServerResult> UpdateGameLaunch()
        {
            TaskCompletionSource<ServerResult> tcs = new TaskCompletionSource<ServerResult>();
            
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "updateGameLaunch", // Arbitrary function name (must exist in your uploaded cloud.js file)
                    GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
                },
                result => {
                    //OnCloudUpdateStats(result);
                    tcs.SetResult(new ServerResult { success = true });
                },
                error => {                
                    error.UnityReport();
                    tcs.SetResult(new ServerResult(error));});

            return await tcs.Task;
        }
        
        // OnCloudHelloWorld defined in the next code block
        private static void OnCloudUpdateStats(ExecuteCloudScriptResult result)
        {
            // Cloud Script returns arbitrary results, so you have to evaluate them one step and one parameter at a time
            #if UNITY_EDITOR
            Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
            #endif
            JsonObject jsonResult = (JsonObject)result.FunctionResult;
            object messageValue;
            jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in Cloud Script
            Debug.Log((string)messageValue);
        }
    }
}