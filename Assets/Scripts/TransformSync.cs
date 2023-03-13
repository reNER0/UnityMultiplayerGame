using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class TransformSync : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_SERVER
            SyncLoopTask();
#endif
        }

        private async Task SyncLoopTask() 
        {
            while (true)
            {
                var objectId = NetworkRepository.GetGameObjectsId(gameObject);

                var syncCmd = new SyncTransformCmd(objectId, transform.position, transform.rotation);

                NetworkBus.OnCommandSendToClients?.Invoke(syncCmd);

                await Task.Delay(NetworkSettings.ServerFixedUpdateTimeMilliseconds);
            }
        }
    }
}
