namespace Network.Photon.Synchronization
{
    public enum SyncMethods 
    {
        PhotonSerialize, //OnPhotonSerializeView fonksyonu photon tarafindan dogrudan cagirilir
        PhotonRaise, //start enable ve disable ile tetiklenir ardından dongu ile cagirilir
    }
}