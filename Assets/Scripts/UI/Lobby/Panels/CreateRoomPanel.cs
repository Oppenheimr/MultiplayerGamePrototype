using System.Threading.Tasks;
using Data;
using Network;
using Photon.Pun;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Lobby.Panels
{
    public class CreateRoomPanel : BasePanel
    {
        [SerializeField] private TMP_InputField _roomName;
        [SerializeField] private TMP_InputField _maxPlayers;

        public void Awake()
        {
            _roomName.text = "Room " + Random.Range(1000, 10000); 
            _maxPlayers.text = NetworkData.Instance.defaultOptions.MaxPlayers.ToString();
        }

        public async void OnCreateRoomButtonClicked()
        {
            var options = NetworkData.Instance.defaultOptions;
            options.MaxPlayers = byte.Parse(_maxPlayers.text);
            
            LoadingBar.Instance.ShowLoadingBar("Creating Room...");
            await PhotonManager.CreateRoom(_roomName.text, options);
            LoadingBar.Instance.HideLoadingBar();
            
            PanelManager.Instance.ShowPanel(typeof(InsideRoomPanel));
        }
    }
}