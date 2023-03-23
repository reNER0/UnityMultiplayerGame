using System;
using System.Linq;
using UnityEngine;

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
        [SerializeField]
        private int _tick;

        public SyncRigidbodyCmd(int objectId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity, int tick)
        {
            _objectId = objectId;
            _position = position;
            _rotation = rotation;
            _velocity = velocity;
            _angularVelocity = angularVelocity;
            _tick = tick;
        }

        public void Execute()
        {
            var networkObject = NetworkRepository.NetworkObjectById[_objectId];

            var clientRigidbodyState = networkObject.RigidbodySync.RigidbodyStates.FirstOrDefault(x => x?.Tick == _tick);
            if (clientRigidbodyState == null)
                return;

            var rigidbody = NetworkRepository.NetworkObjectById[_objectId].RigidbodySync.Rigidbody;

            var positionDelta = _position - clientRigidbodyState.Position;
            var rotationDelta = _rotation * Quaternion.Inverse(clientRigidbodyState.Rotation);
            var velocityDelta = _velocity - clientRigidbodyState.Velocity;
            var angularVelocityDelta = _angularVelocity - clientRigidbodyState.AngularVelocity;


            if (positionDelta.magnitude < 0.1f)
                return;

            var moveTime = NetworkSettings.SyncSpeed;

            rigidbody.transform.position = Vector3.Lerp(
                rigidbody.transform.position, 
                rigidbody.transform.position + positionDelta,
                moveTime
                );

            rigidbody.transform.rotation = Quaternion.Lerp(
                rigidbody.transform.rotation,
                _rotation,
                moveTime
                );

            rigidbody.velocity = Vector3.Lerp(
                rigidbody.velocity,
                rigidbody.velocity + velocityDelta,
                moveTime
                );
            rigidbody.angularVelocity = Vector3.Lerp(
                rigidbody.angularVelocity,
                rigidbody.angularVelocity + angularVelocityDelta,
                moveTime
                );

            for (int i = 0; i < networkObject.RigidbodySync.RigidbodyStates.Length; i++)
            {
                if (networkObject.RigidbodySync.RigidbodyStates[i] == null)
                    continue;

                if (networkObject.RigidbodySync.RigidbodyStates[i]?.Tick < _tick)
                {
                    networkObject.RigidbodySync.RigidbodyStates[i] = null;
                    continue;
                }

                var rbState = networkObject.RigidbodySync.RigidbodyStates[i];

                networkObject.RigidbodySync.RigidbodyStates[i] = new RigidbodyState(
                    rbState.Tick,
                    rbState.Position + positionDelta,
                    rbState.Rotation,
                    rbState.Velocity + velocityDelta,
                    rbState.AngularVelocity + angularVelocityDelta
                    );
            }
        }
    }
}
