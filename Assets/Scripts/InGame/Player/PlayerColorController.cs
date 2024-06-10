using System;
using Config;
using ExitGames.Client.Photon;
using Network;
using Network.Photon;
using Photon.Pun;
using UI.InGame;
using UnityEngine;
using UnityEngine.Serialization;

namespace InGame.Player
{
    public class PlayerColorController : MonoBehaviour
    {
        [HideInInspector] public int colorIndex;
        [SerializeField] private Renderer _targetRenderer;
        
        private PhotonView _photonView;
        private PhotonView PhotonView => _photonView ? _photonView : (_photonView = GetComponent<PhotonView>());

        private void Start()
        {
            PhotonCallbacks.Instance.OnPlayerPropertiesUpdateEvent += OnPlayerPropertiesUpdate;
            
            if (!PhotonView.IsMine)
                SetColor(PhotonView.Controller.GetPlayerColor());
            else
                SyncColor(PlayerColorPalette.GetRandomColorIndex());
        }
        
        private void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.ActorNumber != PhotonView.ControllerActorNr)
                return;
            
            SetColor(targetPlayer.GetPlayerColor());
        }
        
        public void SyncColor(Color color)
        {
            PhotonManager.SetColorLocalPlayer(PlayerColorPalette.GetColorIndex(color));
            
            //Örnek RPC kullanımı. Bu şekilde de renk senkronizasyonu yapılabilir.
            //AllBuffered kullanılmasının sebebi, sonradan oyuna katılan oyuncuların da renkleri görmesi içindir.
            //PhotonView.RPC(nameof(SetColor), RpcTarget.AllBuffered, PlayerColorPalette.GetColorIndex(color), PhotonView.ViewID);
        }

        [PunRPC] //Örnek RPC kullanımı. Bu şekilde de renk senkronizasyonu yapılabilir.
        public void SetColor(object[] dataList)
        {
            if ((int)dataList[1] != PhotonView.ViewID)
                return;
            
            var color = PlayerColorPalette.GetColor((int)dataList[0]);
            colorIndex = (int)dataList[0];
            SetColor(color);
            PlayerListPanel.Instance.UpdatePlayerColor(PhotonView.ControllerActorNr, color);
        }
        
        private void SetColor(Color color) => _targetRenderer.material.color = color;
    }
}