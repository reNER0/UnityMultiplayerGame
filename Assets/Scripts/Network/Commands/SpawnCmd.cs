using System;
using UnityEngine;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class SpawnCmd : SerializableClass, ICommand
    {
        [SerializeField]
        private string _prefabName;
        [SerializeField]
        private int _ownerId;

        public SpawnCmd(string prefabName, int ownerId)
        {
            _prefabName = prefabName;
            _ownerId = ownerId;
        }

        public void Execute()
        {
            var gameObject = (GameObject)GameObject.Instantiate(Resources.Load(_prefabName));

            var networkObject = new NetworkObject(gameObject, _ownerId);

            NetworkObjectsRepository.NetworkObjectById.Add(NetworkObjectsRepository.GetAvailableId(), networkObject);
        }
    }
}
