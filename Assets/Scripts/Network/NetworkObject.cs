using System;
using UnityEngine;

namespace Assets.Scripts.Network
{
    [Serializable]
    public class NetworkObject
    {
        public GameObject GameObject;
        public int OwnerId;

        public NetworkObject(GameObject gameObject, int ownerId)
        {
            this.GameObject = gameObject;
            this.OwnerId = ownerId;
        }
    }
}
