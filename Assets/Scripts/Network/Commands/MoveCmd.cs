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
            var gameObject = NetworkRepository.NetworkObjectById[_objectId].GameObject;

            var moveVector = new Vector3(_x, 0, _y);

            gameObject.transform.position += moveVector;
        }
    }
}
