using System.Collections;
using System.Threading.Tasks;
using Config;
using Network;
using PlayFab;
using PlayFab.MultiplayerModels;
using UI.Base;
using UnityEngine;

namespace UI.Lobby.Panels
{
    public class ConnectedPanel : BasePanel
    {
        private string ticketId;
        private bool _checkMatchmakingStatus;
        
        public void ShowCreateRoomPanel() => PanelManager.Instance.ShowPanel(typeof(CreateRoomPanel));
        public void ShowRoomListPanel() => PanelManager.Instance.ShowPanel(typeof(RoomListPanel));
        
        public async void JoinRandomRoom()
        {
            LoadingBar.Instance.ShowLoadingBar("Joining random room...");
            await PhotonManager.JoinRandomRoom();

            LoadingBar.Instance.HideLoadingBar();
            PanelManager.Instance.ShowPanel(typeof(InsideRoomPanel));
        }

        public void Matchmaking()
        {
            LoadingBar.Instance.ShowLoadingBar("Matchmaking...");
            var createMatchmakingTicketRequest = new CreateMatchmakingTicketRequest
            {
                Creator = new MatchmakingPlayer
                {
                    Entity = new EntityKey
                    {
                        Id = PlayFabSettings.staticPlayer.EntityId,
                        Type = PlayFabSettings.staticPlayer.EntityType
                    }
                },
                GiveUpAfterSeconds = 60, // Kaç saniye sonra eşleşmeyi bırakacağı
                QueueName = MultiplayerPrototypeConfig.QUEUE_NAME // Kuyruk adı
            };

            PlayFabMultiplayerAPI.CreateMatchmakingTicket(createMatchmakingTicketRequest, OnMatchmakingTicketCreated, OnMatchmakingError);
        }
        
        private void OnMatchmakingTicketCreated(CreateMatchmakingTicketResult result)
        {
            LoadingBar.Instance.SetLoadingText("Matchmaking ticket created successfully!");
            Debug.Log("Matchmaking ticket created successfully!");
            ticketId = result.TicketId;
            StartCoroutine(CheckMatchmakingStatus());
        }

        private IEnumerator CheckMatchmakingStatus()
        {
            LoadingBar.Instance.SetLoadingText("Checking matchmaking status...");
            _checkMatchmakingStatus = true;
            while (_checkMatchmakingStatus)
            {
                var getMatchmakingTicketRequest = new GetMatchmakingTicketRequest
                {
                    TicketId = ticketId,
                    QueueName = MultiplayerPrototypeConfig.QUEUE_NAME
                };

                PlayFabMultiplayerAPI.GetMatchmakingTicket(getMatchmakingTicketRequest, OnGetMatchmakingTicketSuccess, OnMatchmakingError);
                yield return new WaitForSeconds(5); // 5 saniyede bir durumu kontrol et
                LoadingBar.Instance.SetLoadingText("Try checking matchmaking status...");
            }
        }

        private void OnGetMatchmakingTicketSuccess(GetMatchmakingTicketResult result)
        {
            if (result.Status == "Matched")
            {
                LoadingBar.Instance.SetLoadingText("Get match...");
                _checkMatchmakingStatus = false;
                
                Debug.Log("Match found!");
                StopCoroutine(CheckMatchmakingStatus()); // Durumu kontrol eden korutini durdur

                var getMatchRequest = new GetMatchRequest
                {
                    MatchId = result.MatchId,
                    QueueName = MultiplayerPrototypeConfig.QUEUE_NAME
                };

                PlayFabMultiplayerAPI.GetMatch(getMatchRequest, OnGetMatchSuccess, OnMatchmakingError);
            }
            else
            {
                Debug.Log("Matchmaking status: " + result.Status);
            }
        }

        private void OnMatchmakingError(PlayFabError error)
        {
            Debug.LogError("Error creating matchmaking ticket: " + error.GenerateErrorReport());
            LoadingBar.Instance.HideLoadingBar();
        }
        
        private async void OnGetMatchSuccess(GetMatchResult result)
        {
            LoadingBar.Instance.SetLoadingText("Match found!");
            // Sunucuya bağlanmak için gerekli işlemleri yapın
            await PhotonManager.JoinLobby();
            await PhotonManager.JoinOrCreateRoom(result.MatchId);
            LoadingBar.Instance.HideLoadingBar();
            PanelManager.Instance.ShowPanel(typeof(InsideRoomPanel));
        }
    }
}
