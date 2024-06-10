using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Config;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using ExitGames.Client.Photon;
using Network;
using Network.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityUtils.Extensions;

namespace UI.Lobby.Panels
{
    public class InsideRoomPanel : BasePanel<InsideRoomPanel>
    {
        [SerializeField] private PlayerItem _playerListEntryPrefab;
        [SerializeField] private Transform _playerItemRoot;
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private GameObject _startGameButton;
        
        private readonly Dictionary<int, PlayerItem> _playerListEntries = new Dictionary<int, PlayerItem>();

        private void Awake()
        {
            PhotonCallbacks.Instance.OnLeftRoomEvent += ClearPlayerList;  
            PhotonCallbacks.Instance.OnPlayerEnteredRoomEvent += AddPlayerToPlayerList;
            PhotonCallbacks.Instance.OnPlayerEnteredRoomEvent += (player) => {CheckPlayersReadyAndUpdateStartButton();};
            PhotonCallbacks.Instance.OnMasterClientSwitchedEvent += (player) => {CheckPlayersReadyAndUpdateStartButton();};
            PhotonCallbacks.Instance.OnPlayerPropertiesUpdateEvent += (player, Hashtable) => {CheckPlayersReadyAndUpdateStartButton();};
            PhotonCallbacks.Instance.OnPlayerPropertiesUpdateEvent += CheckPlayerReady;
            
            PhotonCallbacks.Instance.OnPlayerLeftRoomEvent += RemovePlayerToPlayerList;
            
        }

        public override TweenerCore<float, float, FloatOptions> ShowPanel()
        {
            PhotonManager.JoinLobby();
            UpdatePlayerList();
            CheckPlayersReadyAndUpdateStartButton();
            _header.text = PhotonNetwork.CurrentRoom.Name;
            
            return base.ShowPanel();
        }

        public void ClearPlayerList()
        {
            foreach (var item in _playerListEntries.Values)
                Destroy(item.gameObject);
            _playerListEntries.Clear();
        }
        
        public void UpdatePlayerList()
        {
            ClearPlayerList();
            
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var item = Instantiate(_playerListEntryPrefab, _playerItemRoot);
                item.SetActivate(true);
                item.Initialize(player);
                
                if (player.CustomProperties.TryGetValue(MultiplayerPrototypeConfig.PLAYER_READY_KEY, out object isPlayerReady))
                    item.SetPlayerReady((bool) isPlayerReady);

                _playerListEntries.Add(player.ActorNumber, item);
            }

            //StartGameButton.gameObject.SetActive(CheckPlayersReady());

            var props = new Hashtable { {MultiplayerPrototypeConfig.PLAYER_LOADED_LEVEL_KEY, false} };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        
        public void AddPlayerToPlayerList(Player player)
        {
            var item = Instantiate(_playerListEntryPrefab, _playerItemRoot);
            item.SetActivate(true);
            item.Initialize(player);
                
            if (player.CustomProperties.TryGetValue(MultiplayerPrototypeConfig.PLAYER_READY_KEY, out object isPlayerReady))
                item.SetPlayerReady((bool) isPlayerReady);

            _playerListEntries.Add(player.ActorNumber, item);
        }
        
        public void RemovePlayerToPlayerList(Player player)
        {
            Destroy(_playerListEntries[player.ActorNumber].gameObject);
            _playerListEntries.Remove(player.ActorNumber);
            
            CheckPlayersReadyAndUpdateStartButton();
        }
        
        public void CheckPlayersReadyAndUpdateStartButton() =>
            _startGameButton.SetActive(PhotonManager.CheckPlayersReady());

        public void CheckPlayerReady(Player player, Hashtable changedProps)
        {
            if (!_playerListEntries.TryGetValue(player.ActorNumber, out var item))
                return;

            if (changedProps.TryGetValue(MultiplayerPrototypeConfig.PLAYER_READY_KEY, out object isPlayerReady))
                item.SetPlayerReady((bool) isPlayerReady);
        }
        
        public void OnClickStartButton() => PhotonManager.StartGame();

        public async void OnLeaveButtonClicked()
        {
            await PhotonManager.LeaveRoom();
            PanelManager.Instance.ShowPreviousPanel();
        }
    }
}