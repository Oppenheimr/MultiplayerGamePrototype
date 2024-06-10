using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network.Photon.Synchronization
{
    public class PhotonRaise : SyncBase
    {
        public int raiseEventFrequency = 25;
        public int raiseAnimEventFrequency = 35;
        public float syncPositionSensitivity = 0.5f;
        public float syncRotationSensitivity = 10;
        
        private int _lastReceiveIndex = 0;
        private int _lastReceiveAnimIndex = 0;
        
        public PhotonRaise(GenericSync sync) : base(sync)
        {
            genericSync = sync;
            
            if (!Application.isPlaying)
                return;
            
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingClientEventReceived;
            genericSync.StartCoroutine(RaiseEventRefresh());
            genericSync.StartCoroutine(RaiseAnimEventRefresh());
        }
        
        private void NetworkingClientEventReceived(EventData obj)
        {
            switch (obj.Code)
            {
                case MultiplayerPrototypeConfig.SYNC_EVENT:
                    SyncTransformRead(obj);
                    break;
                case MultiplayerPrototypeConfig.SYNC_ANIM:
                    SyncAnimRead(obj);
                    break;
            }
        }
        
         private IEnumerator RaiseEventRefresh()
        {
            //TR : genericSync objesinde bulunan initialized değişkeni true olana kadar bekle
            //EN : Wait until the initialized variable in the genericSync object is true
            yield return new WaitUntil(() => genericSync.initialized);
            
            while (genericSync.initialized)
            {
                if (genericSync.photonView.IsMine)
                    OnPhotonRaise();
                
                yield return new WaitForSeconds(1f / raiseEventFrequency);
            }
        }

        private IEnumerator RaiseAnimEventRefresh()
        {
            //TR : genericSync objesinde bulunan initialized değişkeni true olana kadar bekle
            //EN : Wait until the initialized variable in the genericSync object is true
            yield return new WaitUntil(() => genericSync.initialized);
            
            while (genericSync.initialized)
            {
                if (genericSync.photonView.IsMine)
                        OnPhotonRaiseAnim();
                yield return new WaitForSeconds(1f / raiseAnimEventFrequency);
            }
        }

        public void OnPhotonRaise()
        {
            if (!genericSync.initialized || !PhotonNetwork.InRoom)
                return;

            try
            {
                List<object> datas = new List<object>();
                
                datas.Add(genericSync.photonView.ViewID);

                if (genericSync.syncPosition)
                {
                    datas.Add(Transform.position);
                    datas.Add((Transform.position - genericSync.lastPos) * Time.deltaTime);
                }

                datas.Add(PhotonNetwork.Time);
                if (genericSync.syncRotation)
                {
                    datas.Add(Transform.rotation);
                }

                PhotonNetwork.RaiseEvent(MultiplayerPrototypeConfig.SYNC_EVENT, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
            }
            catch
            {
            }
        }

        public void OnPhotonRaiseAnim()
        {
            if (!genericSync.initialized || !PhotonNetwork.InRoom)
                return;

            try
            {
                List<object> datas = new List<object>();
                
                datas.Add(genericSync.photonView.ViewID);

                if (genericSync.syncAnimations)
                {
                    foreach (var item in genericSync.animParams)
                    {
                        switch (item.Value)
                        {
                            case AnimatorControllerParameterType.Bool:
                                datas.Add(genericSync.Animator.GetBool(item.Key));
                                break;
                            case AnimatorControllerParameterType.Float:
                                float animf = (genericSync.Animator.GetFloat(item.Key) < 0.05f && genericSync.Animator.GetFloat(item.Key) > -0.05f)
                                    ? 0
                                    : genericSync.Animator.GetFloat(item.Key);
                                datas.Add(animf);
                                break;
                            case AnimatorControllerParameterType.Int:
                                datas.Add(genericSync.Animator.GetInteger(item.Key));
                                break;
                            case AnimatorControllerParameterType.Trigger:
                                datas.Add(genericSync.Animator.GetBool(item.Key));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                if (genericSync.syncAnimatorWeights)
                {
                    for (int i = 0; i < genericSync.Animator.layerCount; i++)
                    {
                        datas.Add(genericSync.Animator.GetLayerWeight(i));
                    }
                }

                PhotonNetwork.RaiseEvent(MultiplayerPrototypeConfig.SYNC_ANIM, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
            }
            catch
            {
            }
        }

        private void SyncTransformRead(EventData obj)
        {
            object[] datas = (object[])obj.CustomData;
            
            if ((int)datas[0] != genericSync.photonView.ViewID) return;

            _lastReceiveIndex = 1;

            if (genericSync.syncPosition)
            {
                Vector3 realPos = (Vector3)datas[ReceiveNextIndex()];
                Vector3 velocity = (Vector3)datas[ReceiveNextIndex()];

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - (double)datas[ReceiveNextIndex()]));

                if (Vector3.Distance(realPos, Transform.position) >
                    syncPositionSensitivity) //Harekete basla - update icinde
                {
                    genericSync.posTime = 0;
                    genericSync.lastPos = Transform.position;
                    genericSync.realPos = realPos;
                    genericSync.velocity = velocity;
                    genericSync.realPos = realPos + (genericSync.velocity * lag);
                }
            }

            if (!genericSync.syncRotation)
                return;
            
            Quaternion realRot = (Quaternion)datas[ReceiveNextIndex()];
            if (!(Quaternion.Angle(Transform.rotation, realRot) > syncRotationSensitivity))
                return;
            
            genericSync.rotTime = 0;
            genericSync.lastRot = Transform.rotation;
            genericSync.realRot = realRot;
        }

        private void SyncAnimRead(EventData obj)
        {
            object[] datas = (object[])obj.CustomData;
            
            if ((int)datas[0] != genericSync.photonView.ViewID) return;

            _lastReceiveAnimIndex = 1;

            if (genericSync.syncAnimations)
            {
                foreach (var item in genericSync.animParams)
                {
                    switch (item.Value)
                    {
                        case AnimatorControllerParameterType.Bool:
                            genericSync.Animator.SetBool(item.Key, (bool)datas[ReceiveAnimNextIndex()]);
                            break;
                        case AnimatorControllerParameterType.Float:
                            genericSync.Animator.SetFloat(item.Key, (float)datas[ReceiveAnimNextIndex()]);
                            break;
                        case AnimatorControllerParameterType.Int:
                            genericSync.Animator.SetInteger(item.Key, (int)datas[ReceiveAnimNextIndex()]);
                            break;
                        case AnimatorControllerParameterType.Trigger:
                            if ((bool)datas[ReceiveAnimNextIndex()])
                                genericSync.Animator.SetTrigger(item.Key);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            if (!genericSync.syncAnimatorWeights)
                return;
            
            for (int i = 0; i < genericSync.Animator.layerCount; i++)
            {
                genericSync.Animator.SetLayerWeight(i, (float)datas[ReceiveAnimNextIndex()]);
            }
        }
 
        private int ReceiveNextIndex() => _lastReceiveIndex++;
        private int ReceiveAnimNextIndex() => _lastReceiveAnimIndex++;

        
        public override void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClientEventReceived;
            genericSync.initialized = false;
        }
    }
}