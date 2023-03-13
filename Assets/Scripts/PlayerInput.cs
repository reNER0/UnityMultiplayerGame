using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerInput : MonoBehaviour
    {
        public float X;
        public float Y;

        private void Update()
        {
            if (!NetworkRepository.IsCurrentClientOwnerOfObject(gameObject))
                return;

            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

            var moveCmd = new InputCmd(NetworkRepository.GetGameObjectsId(gameObject), x, y);
            NetworkBus.OnCommandSendToServer?.Invoke(moveCmd);
        }
    }

    public struct PlayerInputs 
    {
        public float X;
        public float Y;
    }
}
