using System;
using Data;
using InGame.Player;
using Photon.Pun;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace Managers
{
    public class GameManager : SingletonBehavior<GameManager>
    {
        [SerializeField] private Transform _spawnPoint;
        private GameObject _localPlayer;

        private PlayerColorController _playerColorController;
        public PlayerColorController PlayerColorController => _playerColorController ? _playerColorController : (_playerColorController = _localPlayer.GetComponent<PlayerColorController>());
        
        private void Start()
        {
            _localPlayer = PhotonNetwork.Instantiate(GameData.Instance.GetSelectedCharacterName(), _spawnPoint.position, _spawnPoint.rotation);
        }
        
        public void ReturnToLobby()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
}