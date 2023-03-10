using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class NetworkObjectsSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _spawnPoints;

        private IServerHub _serverHub;

        // Delete this
        private int currentSpawnPoint;


        [Inject]
        public void Construct(IServerHub serverHub)
        {
            _serverHub = serverHub;
        }

        private void Awake()
        {
#if !UNITY_SERVER
            Destroy(this);
#endif

            NetworkBus.OnClientConnected += CreateNetworkObjects;
        }

        private void CreateNetworkObjects(NetworkClient client)
        {
            var spawnCmd = new SpawnCmd("Player", client.ClientId, _spawnPoints[currentSpawnPoint].position, _spawnPoints[currentSpawnPoint].rotation);

            _serverHub.PerformCommand(spawnCmd);
            _serverHub.SendCommandToAllClients(spawnCmd);

            // Delete this
            currentSpawnPoint++;
            if (currentSpawnPoint >= _spawnPoints.Length)
                currentSpawnPoint = 0;
        }

        private void OnDestroy()
        {
            NetworkBus.OnClientConnected -= CreateNetworkObjects;
        }
    }

    [Serializable]
    public struct NetworkObjectSpawnParameters
    {
        public NetworkObject NetworkObject;
        public Transform Transform;
    }
}
