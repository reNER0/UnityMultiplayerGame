using DG.Tweening.Core.Easing;
using System;
using UnityEngine;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class InputCmd : SerializableClass, ICommand
    {
        [SerializeField]
        private int _objectId;
        [SerializeField]
        private float _x;
        [SerializeField]
        private float _y;

        public InputCmd(int objectId, float x, float y)
        {
            _objectId = objectId;
            _x = x;
            _y = y;
        }

        public void Execute()
        {
            var input = NetworkRepository.NetworkObjectById[_objectId].GameObject.GetComponent<PlayerInput>();

            input.X = _x;
            input.Y = _y;
        }
    }
}
