using UnityEngine;

namespace Assets.Scripts.Commands
{
    internal class SpawnCmd : ICommand
    {
        private readonly int _ownerId;
        private readonly string _prefabName;
        private readonly Vector3 _position;
        private readonly Quaternion _rotation;

        public SpawnCmd(int ownerId, string prefabName, Vector3 pos, Quaternion rot) 
        {
            _ownerId = ownerId;
            _prefabName = prefabName;
            _position = pos;
            _rotation = rot;
        }

        public void Execute()
        {
            var spawnedObject = GameObject.Instantiate(Resources.Load(_prefabName), _position, _rotation) as GameObject;

            var networkObject = new NetworkObject(spawnedObject, _ownerId);

            NetworkObjectsRepository.NetworkObjectById.Add(NetworkObjectsRepository.GetAvailableId(), networkObject);
        }
    }
}
