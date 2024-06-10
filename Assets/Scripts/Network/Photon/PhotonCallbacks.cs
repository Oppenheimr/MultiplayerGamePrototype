using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UI.Lobby.Panels;
using UnityEngine;

namespace Network.Photon
{
    public class PhotonCallbacks : MonoBehaviourPunCallbacks
    {
        public Dictionary<string, RoomInfo> cachedRoomList;

        private Dictionary<int, GameObject> _playerListEntries;
        //Events
        public event Action OnConnectedToMasterEvent;
        public event Action<List<RoomInfo>> OnRoomListUpdateEvent;
        public event Action OnJoinedLobbyEvent;
        public event Action OnLeftLobbyEvent;
        public event Action<short, string> OnCreateRoomFailedEvent;
        public event Action<short, string> OnJoinRoomFailedEvent;
        public event Action<short, string> OnJoinRandomFailedEvent;
        public event Action OnJoinedRoomEvent;
        public event Action OnLeftRoomEvent;
        public event Action<Player> OnPlayerEnteredRoomEvent;
        public event Action<Player> OnPlayerLeftRoomEvent;
        public event Action<Player> OnMasterClientSwitchedEvent;
        public event Action<Player, Hashtable> OnPlayerPropertiesUpdateEvent;


        #region UNITY

        private void Start()
        {
            cachedRoomList = new Dictionary<string, RoomInfo>();
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnConnectedToMaster()
        {
            OnConnectedToMasterEvent?.Invoke();
            Debug.Log("OnConnectedToMaster");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            OnRoomListUpdateEvent?.Invoke(roomList);

            foreach (var room in roomList)
            {
                Debug.Log("On Room List Update, ROOM: " + room.Name + ", player count:  " + room.PlayerCount + "/" + room.MaxPlayers + ", is open: "
                          + room.IsOpen + ", is visible: " + room.IsVisible + ", removed from list: " + room.RemovedFromList);
            }

            UpdateCachedRoomList(roomList);
        }

        public override void OnJoinedLobby()
        {
            // whenever this joins a new lobby, clear any previous room lists
            Debug.Log("OnJoinedLobby");
            OnJoinedRoomEvent?.Invoke();
        }

        // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
        public override void OnLeftLobby()
        {
            OnLeftLobbyEvent?.Invoke();
            Debug.Log("OnLeftLobby");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            OnCreateRoomFailedEvent?.Invoke(returnCode, message);
            Debug.Log("OnCreateRoomFailed, return code: " + returnCode + ", message: " + message);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            OnJoinRoomFailedEvent?.Invoke(returnCode, message);
            Debug.Log("OnJoinRoomFailed, return code: " + returnCode + ", message: " + message);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            OnJoinRoomFailedEvent?.Invoke(returnCode, message);
            Debug.Log("OnJoinRandomFailed, return code: " + returnCode + ", message: " + message);
        }

        public override void OnJoinedRoom()
        {
            // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
            Debug.Log("OnJoinedRoom");
            cachedRoomList.Clear();
            OnJoinedRoomEvent?.Invoke();
        }

        public override void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
            OnLeftRoomEvent?.Invoke();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("OnPlayerEnteredRoom");
            OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("OnPlayerLeftRoom");
            OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log("OnMasterClientSwitched");
            if (PhotonNetwork.LocalPlayer.ActorNumber != newMasterClient.ActorNumber)
                return;

            OnMasterClientSwitchedEvent?.Invoke(newMasterClient);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Debug.Log("OnPlayerPropertiesUpdate");
            OnPlayerPropertiesUpdateEvent?.Invoke(targetPlayer, changedProps);
        }

        #endregion

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (var info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                        cachedRoomList.Remove(info.Name);
                    
                    continue;
                }

                // Update cached room info
                // Add new room info to cache
                cachedRoomList[info.Name] = info;
            }
        }

        //Singleton
        private static PhotonCallbacks _instance;
        public static PhotonCallbacks Instance => _instance ? _instance : (_instance = FindObjectOfType<PhotonCallbacks>());
    }
}