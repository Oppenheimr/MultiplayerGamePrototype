using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class CharacterData
    {
        public string name;
        public GameObject showModel;
        public PhotonView controller;
    }
}