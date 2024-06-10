using System;
using System.Threading.Tasks;
using Data;
using Network.PlayFab.Models;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Network.PlayFab
{
    public static class PlayFabManager
    {
        public static async Task Initialize()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = NetworkData.Instance.titleId;
            }
            await Authentication.Initialize();
        }
        
        private static void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            // Debug.Log("Congratulations, you made your first successful API call!");
            // PlayerPrefs.SetString("EMAIL", userEmail);
            // PlayerPrefs.SetString("PASSWORD", userPassword);
        }
        private static void OnLoginFailure(PlayFabError error)
        {
            //var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = username };
            //PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
        }
        private static void OnRegisterFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }
        
        public static void OnClickRegister(string userEmail, string userPassword)
        {
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            //PlayFabClientAPI.RegisterPlayFabUser();
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
        private static void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("Congratulations, you made your first successful API call!");
            // PlayerPrefs.SetString("EMAIL", userEmail);
            // PlayerPrefs.SetString("PASSWORD", userPassword);
        }
        
        public static async Task Reset()
        {
            await Authentication.Reset();
            // ServicesMemory.Reset();
            // LevelManager.Reset();
        }
        
    }
}