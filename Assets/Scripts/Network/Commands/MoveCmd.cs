using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class MoveCmd : SerializableClass, ICommand
    {
        [SerializeField]
        private int _objectId;
        [SerializeField]
        private float _x;
        [SerializeField]
        private float _y;

        public MoveCmd(int objectId, float x, float y)
        {
            _objectId = objectId;
            _x = x;
            _y = y;
        }

        public void Execute()
        {
            var gameObject = NetworkRepository.NetworkObjectById[_objectId];

            var rigidbody = gameObject.GameObject.GetComponent<Rigidbody>();

            var force = new Vector3(_x, 0, _y);

            rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}
