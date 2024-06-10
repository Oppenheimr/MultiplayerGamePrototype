using System.Collections.Generic;
using System.Linq;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Network;
using Network.Photon;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using UI.Base;
using UnityEngine;
using UnityUtils.Extensions;

namespace UI.Lobby.Panels
{
    public class RoomListPanel : BasePanel
    {
        [SerializeField] private RoomItem _roomItemPrefab;
        [SerializeField] private Transform _roomItemRoot;

        private Dictionary<string, GameObject> _roomListItems;

        protected override void Start()
        {
            PhotonCallbacks.Instance.OnRoomListUpdateEvent += UpdateRoomList;
            _roomListItems = new Dictionary<string, GameObject>();
            base.Start();
        }

        public override TweenerCore<float, float, FloatOptions> ShowPanel()
        {
            PhotonManager.JoinLobby();
            if (PhotonCallbacks.Instance.cachedRoomList != null)
                UpdateRoomList(PhotonCallbacks.Instance.cachedRoomList.Values.ToList());
            return base.ShowPanel();
        }

        public void UpdateRoomList(List<RoomInfo> rooms)
        {
            if (_roomListItems == null)
                return;

            foreach (var item in _roomListItems.Values)
                Destroy(item);
            _roomListItems.Clear();

            foreach (var info in rooms)
            {
                var roomItem = Instantiate(_roomItemPrefab, _roomItemRoot);
                roomItem.SetActivate(true);
                roomItem.Initialize(info);
                _roomListItems.Add(info.Name, roomItem.gameObject);
            }
        }
    }
}