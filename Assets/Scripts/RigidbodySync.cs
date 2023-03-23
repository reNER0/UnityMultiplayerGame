using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodySync : MonoBehaviour
    {
        public Rigidbody Rigidbody { get; private set; }

        public RigidbodyState[] RigidbodyStates = new RigidbodyState[1024];

        private int currentState;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();

            SyncLoopTask();
        }

        private async Task SyncLoopTask()
        {
            while (true)
            {
#if UNITY_SERVER
                var objectId = NetworkRepository.GetGameObjectsId(gameObject);

                var syncCmd = new SyncRigidbodyCmd(
                    objectId, 
                    transform.position, 
                    transform.rotation, 
                    Rigidbody.velocity, 
                    Rigidbody.angularVelocity, 
                    NetworkSettings.CurrentTick
                    );

                NetworkBus.OnCommandSendToClients?.Invoke(syncCmd);
#else
                RigidbodyStates[currentState] = new RigidbodyState(
                    NetworkSettings.CurrentTick,
                    transform.position,
                    transform.rotation,
                    Rigidbody.velocity,
                    Rigidbody.angularVelocity
                    );

                currentState++;
                if (currentState >= 1024)
                    currentState = 0;
#endif

                await Task.Delay(NetworkSettings.ServerFixedUpdateTimeMilliseconds);
            }
        }
    }

    public class RigidbodyState
    {
        public int Tick { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Vector3 Velocity { get; private set; }
        public Vector3 AngularVelocity { get; private set; }

        public RigidbodyState(int tick, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
        {
            Tick = tick;
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
        }
    }
}
