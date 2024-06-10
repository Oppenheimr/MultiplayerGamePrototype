using Config;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace UI.Lobby
{
    public class ConnectionStatus : MonoBehaviour
    {
        public TextMeshProUGUI connectionStatusText;
        
        public void Update()
        {
            if (connectionStatusText)
                connectionStatusText.text = MultiplayerPrototypeConfig.CONNECTION_STATUS_MESSAGE_ROOT + PhotonNetwork.NetworkClientState;
        }
    }
}