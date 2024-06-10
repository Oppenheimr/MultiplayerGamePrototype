using System.Collections.Generic;
using System.Linq;
using Network.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils.BaseClasses;
using UnityUtils.Extensions;

namespace UI.InGame
{
    public class PlayerListPanel : SingletonBehavior<PlayerListPanel>
    {
        [SerializeField] private PlayerItem _playerItemReference;
        [SerializeField] private List<PlayerItem> _playerItems = new List<PlayerItem>();
        
        private void Start()
        {
            foreach (var color in PhotonNetwork.CurrentRoom.Players.Values)
            {
                var playerItem = Instantiate(_playerItemReference, _playerItemReference.transform.parent);
                playerItem.Initialize(color, Color.white);
            }
            _playerItemReference.SetActivate(false);
            
            PhotonCallbacks.Instance.OnPlayerEnteredRoomEvent += OnPlayerEnteredRoom;
            PhotonCallbacks.Instance.OnPlayerLeftRoomEvent += RemovePlayer;
        }
        
        private void OnPlayerEnteredRoom(Player player)
        {
            if (_playerItems.Any(playerItem => Equals(playerItem.player, player)))
                return;
            
            AddPlayer(player, player.GetPlayerColor());
        }
        
        public void UpdatePlayerColor(int actorNumber, Color color)
        {
            foreach (var playerItem in _playerItems.Where(playerItem => playerItem.player.ActorNumber == actorNumber))
                playerItem.SetColor(color);
        }
        
        public void AddPlayer(Player player, Color color)
        {
            var playerItem = Instantiate(_playerItemReference, _playerItemReference.transform.parent);
            playerItem.Initialize(player, color);
            playerItem.SetActivate(true);
        }
        
        public void RemovePlayer(Player player)
        {
            foreach (Transform child in _playerItemReference.transform.parent)
            {
                if (!Equals(child.GetComponent<PlayerItem>().player, player))
                    continue;
                
                Destroy(child.gameObject);
                return;
            }
        }
    }
}