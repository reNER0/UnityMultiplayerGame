using UnityEngine;

namespace Assets.Scripts
{
    public class SpawnPoints : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _playerSpawnPoints;
        [SerializeField]
        private Transform _ballSpawnPoint;

        public Transform[] PlayerSpawnPoints => _playerSpawnPoints;
        public Transform BallSpawnPoint => _ballSpawnPoint;
    }
}
