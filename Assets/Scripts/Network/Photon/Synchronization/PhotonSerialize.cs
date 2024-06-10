using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Network.Photon.Synchronization
{
    public class PhotonSerialize : SyncBase
    {
        public PhotonSerialize(GenericSync sync) : base(sync)
        {
            genericSync = sync;
        }

        public void View(PhotonStream stream, PhotonMessageInfo info) //this function called by Photon View component
        {
            if (!genericSync.initialized || !PhotonNetwork.InRoom)
                return;
            try
            {
                if (stream.IsWriting)
                    SerializeWrite(stream);
                else if (stream.IsReading)
                    SerializeRead(stream, info.SentServerTime);
            }
            catch
            { Debug.LogError("Error in OnPhotonSerializeView"); }
        }

        private void SerializeWrite(PhotonStream stream)
        {
            stream.SendNext(genericSync.syncPosition);
            stream.SendNext(genericSync.syncRotation);
            stream.SendNext(genericSync.syncAnimations);
            stream.SendNext(genericSync.syncAnimatorWeights);
            stream.SendNext(genericSync.positionLerpRate);
            stream.SendNext(genericSync.rotationLerpRate);

            if (genericSync.syncPosition)
            {
                stream.SendNext(Transform.position);
                stream.SendNext((Transform.position - genericSync.lastPos) * Time.deltaTime);
            }

            if (genericSync.syncRotation)
            {
                stream.SendNext(Transform.rotation);
            }

            if (genericSync.syncAnimations)
            {
                foreach (KeyValuePair<string, AnimatorControllerParameterType> item in genericSync.animParams)
                {
                    switch (item.Value)
                    {
                        case AnimatorControllerParameterType.Bool:
                            stream.SendNext(genericSync.Animator.GetBool(item.Key));
                            break;
                        case AnimatorControllerParameterType.Float:
                            stream.SendNext(
                                (genericSync.Animator.GetFloat(item.Key) < 0.05f && genericSync.Animator.GetFloat(item.Key) > -0.05f)
                                    ? 0
                                    : genericSync.Animator.GetFloat(item.Key));
                            break;
                        case AnimatorControllerParameterType.Int:
                            stream.SendNext(genericSync.Animator.GetInteger(item.Key));
                            break;
                        case AnimatorControllerParameterType.Trigger:
                            stream.SendNext(genericSync.Animator.GetBool(item.Key));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            if (!genericSync.syncAnimatorWeights)
                return;
            
            for (int i = 0; i < genericSync.Animator.layerCount; i++)
                stream.SendNext(genericSync.Animator.GetLayerWeight(i));
        }

        private void SerializeRead(PhotonStream stream, double SentServerTime)
        {
            genericSync.syncPosition = (bool)stream.ReceiveNext();
            genericSync.syncRotation = (bool)stream.ReceiveNext();
            genericSync.syncAnimations = (bool)stream.ReceiveNext();
            genericSync.syncAnimatorWeights = (bool)stream.ReceiveNext();
            genericSync.positionLerpRate = (float)stream.ReceiveNext();
            genericSync.rotationLerpRate = (float)stream.ReceiveNext();

            if (genericSync.syncPosition)
            {
                genericSync.realPos = (Vector3)stream.ReceiveNext();
                genericSync.velocity = (Vector3)stream.ReceiveNext();
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - SentServerTime));
                genericSync.realPos += (genericSync.velocity * lag);
                
                if (genericSync.realPos != Transform.position)
                {
                    genericSync.lastPos = Transform.position;
                    genericSync.posTime = 0;
                }
            }

            if (genericSync.syncRotation)
            {
                genericSync.realRot = (Quaternion)stream.ReceiveNext();
            }

            if (genericSync.syncAnimations)
            {
                foreach (KeyValuePair<string, AnimatorControllerParameterType> item in genericSync.animParams)
                {
                    switch (item.Value)
                    {
                        case AnimatorControllerParameterType.Bool:
                            genericSync.Animator.SetBool(item.Key, (bool)stream.ReceiveNext());
                            break;
                        case AnimatorControllerParameterType.Float:
                            genericSync.Animator.SetFloat(item.Key, (float)stream.ReceiveNext());
                            break;
                        case AnimatorControllerParameterType.Int:
                            genericSync.Animator.SetInteger(item.Key, (int)stream.ReceiveNext());
                            break;
                        case AnimatorControllerParameterType.Trigger:
                            if ((bool)stream.ReceiveNext())
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
                genericSync.Animator.SetLayerWeight(i, (float)stream.ReceiveNext());
        }
        
        public override void OnDisable()
        {
            
        }
    }
}