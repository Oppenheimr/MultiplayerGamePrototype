using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Config;
using Data;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    public static class PhotonManager
    {
        public static List<PhotonView> GetRoomViewers
        {
            get
            {
                List<PhotonView> viewers = Object.FindObjectsOfType<PhotonView>().ToList();
                foreach (var viewer in 
                         PhotonNetwork.CurrentRoom.Players.Values.SelectMany(player => viewers.
                             Where(viewer => viewer.OwnerActorNr == player.ActorNumber)))
                {
                    viewers.Add(viewer);
                }

                return viewers;
            }
        }
        
        public static void Initialize()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        
        public static bool Login(string playerName)
        {
            if (!playerName.Equals(""))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                return PhotonNetwork.ConnectUsingSettings();
            }
            
            Debug.LogError("Player Name is invalid.");
            return false;
        }

        public static async Task<bool> JoinLobby()
        {
            if (PhotonNetwork.InLobby)
                return true;
            
            bool result = PhotonNetwork.JoinLobby();
            
            while (!PhotonNetwork.InLobby)
                await Task.Delay(1000);
            return true;
        }
        
        public static async Task<bool> CreateRoom(string roomName, RoomOptions roomOptions = null, TypedLobby typedLobby = null, string[] expectedUsers = null)
        {
            var res =  PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby, expectedUsers);
            
            if (!res)
                return false;
            
            while (PhotonNetwork.CurrentRoom == null)
                await Task.Delay(1000);
            return true;
        }
        
        public static async Task<bool> JoinOrCreateRoom(string roomName, RoomOptions roomOptions = null, TypedLobby typedLobby = null, string[] expectedUsers = null)
        {
            var res = PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby, expectedUsers);

            if (!res)
                return false;
            
            while (PhotonNetwork.CurrentRoom == null)
                await Task.Delay(1000);
            return true;
        }
        
        public static void StartGame()
        {
            if (!CheckPlayersReady())
                return;
            
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel("GameScene");
        }
        
        public static async Task<bool> LeaveRoom()
        {
            var res =  PhotonNetwork.LeaveRoom();
            
            while (PhotonNetwork.CurrentRoom != null)
                await Task.Delay(1000);
            return true;
        }
        
        public static async Task<bool> JoinRandomRoom()
        {
            var defaultOptions = NetworkData.Instance.defaultOptions;
            var res = PhotonNetwork.JoinOrCreateRoom(NetworkData.generatedRoomName, defaultOptions, TypedLobby.Default);
            
            if (!res)
                return false;
            
            while (PhotonNetwork.CurrentRoom == null)
                await Task.Delay(1000);
            return true;
        }
        
        public static bool CheckPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return false;
            }

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(MultiplayerPrototypeConfig.PLAYER_READY_KEY, out object isPlayerReady))
                {
                    if (!(bool) isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        public static async Task<bool> LeaveLobby()
        {
            bool res = PhotonNetwork.LeaveLobby();
            
            if (!res)
                return false;
            
            while (PhotonNetwork.InLobby)
                await Task.Delay(1000);
            return true;
        }
        
        public static void SetColorLocalPlayer(int getColorIndex)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() {{PlayerColorPalette.PLAYER_COLOR_KEY, getColorIndex}});
        }
        
        public static async Task WaitConnectToMaster()
        {
            while (!PhotonNetwork.IsConnectedAndReady)
                await Task.Delay(1000);
        }
    }
}