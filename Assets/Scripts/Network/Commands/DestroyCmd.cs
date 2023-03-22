using System;
using UnityEngine;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class DestroyCmd : SerializableClass, ICommand
    {
        [SerializeField]
        private int _objectId;

        public DestroyCmd(int objectId)
        {
            _objectId = objectId;
        }

        public void Execute()
        {
            var gameObject = NetworkRepository.NetworkObjectById[_objectId].GameObject;

            NetworkRepository.NetworkObjectById.Remove(_objectId);

            GameObject.Destroy(gameObject);
        }
    }
}
