using System;
using static Photon.Pun.UtilityScripts.PlayerNumbering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network.Photon.Synchronization
{
    [RequireComponent(typeof(PhotonView))]
    public class GenericSync : MonoBehaviourPun, IPunObservable
    {
        public SyncMethods syncMethod = SyncMethods.PhotonSerialize;

        [Header("Sync Options")]
        [Tooltip("Sync the position of this transform across the network.")]
        public bool syncPosition = true;

        [Tooltip("Sync the rotation of this transform across the network.")]
        public bool syncRotation = true;

        [Tooltip("Sync the animations of the object across the network.")]
        public bool syncAnimations = true;

        [Tooltip("Sync the animator layer weights across the network.")]
        public bool syncAnimatorWeights = true;

        [Tooltip("How fast to move the networked versions position to the desired location.")]
        public float positionLerpRate = 5;

        [Tooltip("How fast to move the networked versions rotation to the desired rotation.")]
        public float rotationLerpRate = 5;
        
        [Header("Animator Parameters")]
        [Tooltip("The animator parameters")] 
        public List<string> currentParams = new List<string>();
        
        [Tooltip("The animator parameters that you don't want to sync across the network")]
        public List<string> ignoreParams = new List<string>();
        
        [HideInInspector] public Vector3 lastPos;
        [HideInInspector] public Vector3 realPos;
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public Quaternion lastRot;
        [HideInInspector] public Quaternion realRot;
        [HideInInspector] public bool initialized;

        public Dictionary<string, AnimatorControllerParameterType> animParams =
            new Dictionary<string, AnimatorControllerParameterType>();
        
        [HideInInspector] public float posTime;
        [HideInInspector] public float rotTime;
        
        [HideInInspector] public PhotonRaise raise;
        private PhotonSerialize _serialize;

        private Animator _animator;
        public Animator Animator => _animator ? _animator : (_animator = GetComponent<Animator>());
        

#region Unity Event Function
        
        private void Awake()
        {
            _serialize = new PhotonSerialize(this);
            raise = new PhotonRaise(this);
        }
        
        private void OnEnable()
        {
            if (syncMethod == SyncMethods.PhotonSerialize) return;

            lastPos = transform.position;
            realPos = transform.position;
            realRot = transform.rotation;
            posTime = 0;
        }
        
        /// <summary>
        /// Attempts to find the animator, if none is found will disable syncing animations.
        /// Also builds the parameter dictionary to sync across the network as they change.
        /// Makes use of the `OnPlayerNumberingChanged` delegate provided by Photon to call
        /// `NewPlayerEnteredOrLeft` function.
        /// </summary>
        protected async void Start()
        {
            if (Animator)
                BuildAnimatorParamsDict();
            else
                syncAnimations = false;
            
            lastPos = transform.position;
            
            await Task.Delay(1000);
            initialized = true;
        }
        
        /// <summary>
        /// Used to smoothly rotate and move to the position of the owner. This is only run
        /// if this is the networked version.
        /// </summary>
        protected virtual void Update()
        {
            SyncPositionAndRotation();
        }
        
        private void OnDisable()
        {
            _serialize.OnDisable();
            raise.OnDisable();
        }
        
#endregion
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (syncMethod != SyncMethods.PhotonSerialize)
                return;
            
            _serialize.View(stream, info);
        }

        /// <summary>
        /// Will find all the animator parameters in the selected animator controller and store
        /// those types and names as a key value pair dictionary for later reading and syncing
        /// over the network. Also starts trigger watchers if `syncTriggers` is true.
        /// </summary>
        protected virtual void BuildAnimatorParamsDict()
        {
            if (!Animator)
                return;
            
            foreach (var param in Animator.parameters)
            {
                currentParams.Add(param.name);
                if (ignoreParams.Contains(param.name)) 
                    continue;
                
                //Syncing triggers this way is unreliable, send trigger events via RPC
                if (param.type != AnimatorControllerParameterType.Trigger) 
                    animParams.Add(param.name, param.type);
                else if (photonView.IsMine)
                    StartCoroutine(SyncTrigger(param.name));
            }
        }

        /// <summary>
        /// A simple thread that is dedicated to watching for when a trigger is fired. When fired
        /// it will fire and RPC to the other networked versions to update theirs. This is unreliable
        /// as it doesn't always catch the trigger. For a more reliable method find where the trigger
        /// is being fired and in that function make this RPC call.
        /// </summary>
        /// <param name="triggerName">The trigger name to watch and update over the network</param>
        /// <returns></returns>
        protected virtual IEnumerator SyncTrigger(string triggerName)
        {
            yield return new WaitUntil(() => Animator.GetBool(triggerName));
            photonView.RPC(nameof(GenericSyncSetTrigger), RpcTarget.Others, triggerName);
            StartCoroutine(SyncTrigger(triggerName));
            yield return null;
        }

        private void SyncPositionAndRotation()
        {
            if (photonView.IsMine)
                return;
            
            if (syncPosition)
            {
                posTime += Time.deltaTime * positionLerpRate;
                posTime = (posTime > 1) ? 1 : posTime;
                transform.position = Vector3.Lerp(lastPos, realPos, posTime);
            }

            if (!syncRotation)
                return;
            
            rotTime += Time.deltaTime * rotationLerpRate;
            rotTime = (rotTime > 1) ? 1 : rotTime;

            transform.rotation = Quaternion.Slerp(lastRot, realRot, rotTime);
        }
        
        [PunRPC]
        protected virtual void GenericSyncSetTrigger(string triggerName)
        {
            Animator.SetTrigger(triggerName);
        }
    }
}