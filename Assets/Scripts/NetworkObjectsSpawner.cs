using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class NetworkObjectsSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<NetworkObjectSpawnParameters> networkObjectsByPositionList;

        private void Start()
        {
            CreateNetworkObjects();
        }

        private void CreateNetworkObjects()
        {
            foreach (var networkObjectsByPosition in networkObjectsByPositionList)
            {


            }
        }
    }

    [Serializable]
    public struct NetworkObjectSpawnParameters
    {
        public NetworkObject NetworkObject;
        public Transform Transform;
    }
}
