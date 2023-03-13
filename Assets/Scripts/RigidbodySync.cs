using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodySync : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Start()
        {
#if UNITY_SERVER
            _rigidbody = GetComponent<Rigidbody>();

            SyncLoopTask();
#endif
        }

        private async Task SyncLoopTask()
        {
            while (true)
            {
                var objectId = NetworkRepository.GetGameObjectsId(gameObject);

                var syncCmd = new SyncRigidbodyCmd(objectId, transform.position, transform.rotation, _rigidbody.velocity, _rigidbody.angularVelocity);

                NetworkBus.OnCommandSendToClients?.Invoke(syncCmd);

                await Task.Delay(NetworkSettings.ServerFixedUpdateTimeMilliseconds);
            }
        }
    }
}
