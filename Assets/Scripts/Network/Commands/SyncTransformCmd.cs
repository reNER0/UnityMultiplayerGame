using System;
using UnityEngine;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class SyncTransformCmd : SerializableClass, ICommand
    {
        [SerializeField]
        private int _objectId;
        [SerializeField]
        private Vector3 _position;
        [SerializeField]
        private Quaternion _rotation;

        public SyncTransformCmd(int objectId, Vector3 position, Quaternion rotation)
        {
            _objectId = objectId;
            _position = position;
            _rotation = rotation;
        }

        public void Execute()
        {
            var gameObject = NetworkRepository.NetworkObjectById[_objectId].GameObject;

            gameObject.transform.position = _position;
            gameObject.transform.rotation = _rotation;

            Debug.LogError("Sync" + _position);
        }
    }
}
