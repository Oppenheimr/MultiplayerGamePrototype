using Config;
using Data;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UI.Lobby.Panels;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Extensions;

namespace UI.Lobby
{
    public class PlayerItem : MonoBehaviour
    {
        public TextMeshProUGUI PlayerNameText;
        public Image PlayerColorImage;
        public Button PlayerReadyButton;
        public Image PlayerReadyImage;

        private int _ownerId;
        private bool _isPlayerReady;

        #region UNITY

        public void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }

        public void Start()
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != _ownerId)
            {
                PlayerReadyButton.gameObject.SetActive(false);
            }
            else
            {
                var initialProps = new Hashtable()
                {
                    {MultiplayerPrototypeConfig.PLAYER_READY_KEY, _isPlayerReady},
                    {MultiplayerPrototypeConfig.SELECTED_CHARACTER_KEY, LocalDatabase.lastSelectedCharacter.Value}
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

                PlayerReadyButton.onClick.AddListener(() =>
                {
                    _isPlayerReady = !_isPlayerReady;
                    SetPlayerReady(_isPlayerReady);

                    var props = new Hashtable() {{MultiplayerPrototypeConfig.PLAYER_READY_KEY, _isPlayerReady}};
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if (PhotonNetwork.IsMasterClient)
                        InsideRoomPanel.Instance.CheckPlayersReadyAndUpdateStartButton();
                });
            }
        }

        public void OnDisable()
        {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }

        #endregion

        public void Initialize(int playerId, string playerName)
        {
            _ownerId = playerId;
            PlayerNameText.text = playerName;
        }

        public void Initialize(Player player) => Initialize(player.ActorNumber, player.NickName);
        
        private void OnPlayerNumberingChanged()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.ActorNumber != _ownerId)
                    return;
                    
                PlayerColorImage.color = MultiplayerPrototypeConfig.GetColor(player.GetPlayerNumber());
            }
        }
        
        public void SetPlayerReady(bool playerReady)
        {
            if (PlayerReadyButton.TryGetComponentInChildren(out TextMeshProUGUI textMeshProUGUI))
            {
                textMeshProUGUI.text = playerReady ? "Ready!" : "Ready?";
            }
            else if (PlayerReadyButton.TryGetComponentInChildren(out Text text))
            {
                text.text = playerReady ? "Ready!" : "Ready?";
            }
            
            PlayerReadyImage.enabled = playerReady;
        }
        
    }
}