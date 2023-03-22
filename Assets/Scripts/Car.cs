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
        private bool _movable;

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!_movable)
                return;

            var forwardForce = transform.right * _speed * _playerInput.Y;
            var rotationForce = transform.up * _rotationSpeed * _playerInput.X;

            _rigidbody.AddForce(forwardForce);
            _rigidbody.AddTorque(rotationForce);
        }

        public void SetMovable(bool movable) 
        {
            _movable = movable;
        }
    }
}
