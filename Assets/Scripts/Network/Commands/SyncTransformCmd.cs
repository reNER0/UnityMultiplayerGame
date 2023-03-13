using System;
using UnityEngine;
using DG.Tweening;

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

            gameObject.transform.DOKill();
            gameObject.transform.DOMove(_position, NetworkSettings.ServerFixedUpdateTime / 2);
            gameObject.transform.DORotateQuaternion(_rotation, NetworkSettings.ServerFixedUpdateTime / 2);
        }
    }
}
