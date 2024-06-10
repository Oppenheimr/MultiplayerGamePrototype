using System;
using System.Collections;
using System.Linq;
using Object = UnityEngine.Object;

#if FRAGMASTA
using InGame;
#endif

#if PHOTON_UNITY_NETWORKING
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
#endif

namespace UnityUtils.Helpers
{
#if PHOTON_UNITY_NETWORKING
    public static class PhotonHelper
    {
        public static string LocalName
        {
      
            get
            {
#if FRAGMASTA && MFPS
                if (bl_PhotonNetwork.LocalPlayer != null && OfflineManager.IsOffline)
                    return PhotonNetwork.LocalPlayer != null ? PhotonNetwork.LocalPlayer.NickName : "None";
                return OfflineManager.Instance.playerReferences.name;
#endif
                return PhotonNetwork.LocalPlayer != null ? PhotonNetwork.LocalPlayer.NickName : "None";
            }
        }

        public static PhotonView GetView(Player player)
        {
            var views = Object.FindObjectsOfType<PhotonView>();
            
            foreach (var view in views)
                if (player.NickName == view.name)
                    return view;
            
            // foreach (var view in views)
            //     if (player.ActorNumber == view.ControllerActorNr)
            //         return view;

//             Debug.Log($@"Can not find actor number! 
// Number of found objects : {views.Length}
// View ID : {player.ActorNumber}
// NickName : {player.NickName}");
            
            return null;
        }
        
        public static Player GetPlayer(int actorNumber) =>
            PhotonNetwork.PlayerList.FirstOrDefault(player => actorNumber == player.ActorNumber);

        public static IEnumerator WaitClientState(this ClientState state)
        {
            yield return CoroutineHelper.WaitCondition(() => PhotonNetwork.NetworkClientState == state);
        }
    }
#endif
}