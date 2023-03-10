using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using UnityEngine;

namespace Assets.Scripts
{
    public class Movement : MonoBehaviour
    {
        [SerializeField]
        private ClientHub hub;

        [SerializeField]
        private float _speed;

        private void Update()
        {
            if (!ClientProfile.IsOwnerOfObject(gameObject))
                return;

            var x = Input.GetAxis("Horizontal") * -_speed;
            var y = Input.GetAxis("Vertical") * -_speed;

            if (Mathf.Abs(x + y) > 1)
            {
                var moveCmd = new MoveCmd(0, x, y);

                GameBus.OnClientSendToServer?.Invoke(moveCmd);
            }
        }
    }
}
