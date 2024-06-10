using UnityEngine;

namespace Network.Photon.Synchronization
{
    public abstract class SyncBase
    {
        protected Transform Transform => genericSync.transform;
        protected GenericSync genericSync;

        protected SyncBase(GenericSync sync)
        {
            genericSync = sync;
        }

        public abstract void OnDisable();
    }
}