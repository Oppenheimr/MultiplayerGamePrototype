using System;
using Config;
using ExitGames.Client.Photon;
using Network.Photon;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame
{
    public class PlayerItem : MonoBehaviour
    {
        public Player player;
        
        [SerializeField] private Image _colorImage;
        [SerializeField] private TextMeshProUGUI _playerName;

        private void Awake()
        {
            PhotonCallbacks.Instance.OnPlayerPropertiesUpdateEvent += OnPlayerPropertiesUpdate;
        }

        public void Initialize(Player playerInfo, Color color)
        {
            player = playerInfo;
            _playerName.text = playerInfo.NickName;
            _colorImage.color = color;
        }
        
        private void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (player == null)
                 return;
            
            if (targetPlayer.ActorNumber != player.ActorNumber)
                return;

            SetColor(targetPlayer.GetPlayerColor());
        }
        
        public void SetColor(Color color) => _colorImage.color = color;
    }
}