using Assets.Scripts.Commands;
using UnityEngine;

namespace Assets.Scripts
{
    public class Movement : MonoBehaviour
    {
        [SerializeField]
        private ClientHub hub;

        [SerializeField]
        private float _speed;

        private Rigidbody _rigidbody;


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            NetworkObjectsRepository.NetworkObjectById.Add(0, new NetworkObject(gameObject, 1));
        }

        private void Update()
        {
            // Check if owner

            var x = Input.GetAxis("Horizontal") * -_speed;
            var y = Input.GetAxis("Vertical") * -_speed;

            if (Mathf.Abs(x + y) > 1)
            {
                var moveCmd = new MoveCmd(0, x, y);

                hub.PerformCommand(moveCmd);
            }
        }
    }
}
