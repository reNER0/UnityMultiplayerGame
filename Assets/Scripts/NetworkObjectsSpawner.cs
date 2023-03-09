using Assets.Scripts.Commands;
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
                var cmd = new SpawnCmd(
                    networkObjectsByPosition.NetworkObject.OwnerId,
                    networkObjectsByPosition.NetworkObject.GameObject.name,
                    networkObjectsByPosition.Transform.position,
                    networkObjectsByPosition.Transform.rotation
                    );


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
