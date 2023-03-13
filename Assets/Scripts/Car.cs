using UnityEngine;

namespace Assets.Scripts
{
    public class Car : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _rotationSpeed;

        private PlayerInput _playerInput;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var forwardForce = transform.right * _speed * _playerInput.Y;
            var rotationForce = transform.up * _rotationSpeed * _playerInput.X;

            _rigidbody.AddForce(forwardForce);
            _rigidbody.AddTorque(rotationForce);
        }
    }
}
