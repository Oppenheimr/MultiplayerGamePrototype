using Network;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class RoomItem : MonoBehaviour
    {
        public TextMeshProUGUI RoomNameText;
        public TextMeshProUGUI RoomPlayersText;
        public Button JoinRoomButton;

        private string _roomName;

        public void Initialize(string name, byte currentPlayers, byte maxPlayers)
        {
            _roomName = name;

            RoomNameText.text = name;
            RoomPlayersText.text = currentPlayers + " / " + maxPlayers;

            JoinRoomButton.onClick.AddListener(async () => 
            {
                LoadingBar.Instance.ShowLoadingBar("Joining Room...");
                if (PhotonNetwork.InLobby)
                    await PhotonManager.LeaveLobby();

                await PhotonManager.JoinOrCreateRoom(_roomName);
                LoadingBar.Instance.HideLoadingBar();
            });
        }
        public void Initialize(RoomInfo info) => Initialize(info.Name, (byte)info.PlayerCount, (byte)info.MaxPlayers);
    }
}