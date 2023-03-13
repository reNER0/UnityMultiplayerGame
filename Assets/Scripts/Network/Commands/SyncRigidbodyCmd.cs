using System;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class SyncRigidbodyCmd : SerializableClass, ICommand
    {
        [SerializeField]
        private int _objectId;
        [SerializeField]
        private Vector3 _position;
        [SerializeField]
        private Quaternion _rotation;
        [SerializeField]
        private Vector3 _velocity;
        [SerializeField]
        private Vector3 _angularVelocity;

        public SyncRigidbodyCmd(int objectId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
        {
            _objectId = objectId;
            _position = position;
            _rotation = rotation;
            _velocity = velocity;
            _angularVelocity = angularVelocity;
        }

        public void Execute()
        {
            var rigidbody = NetworkRepository.NetworkObjectById[_objectId].Rigidbody;

            var moveTime = NetworkSettings.ServerFixedUpdateTime;

            rigidbody.DOKill();

            rigidbody.DOMove(_position, moveTime);
            rigidbody.transform.DORotateQuaternion(_rotation, moveTime);

            DOTween.To(() => rigidbody.velocity, x => rigidbody.velocity = x, _velocity, moveTime);
            DOTween.To(() => rigidbody.angularVelocity, x => rigidbody.angularVelocity = x, _angularVelocity, moveTime);
        }
    }
}
