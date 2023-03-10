using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using UnityEngine;

namespace Assets.Scripts
{
    public class Movement : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        private void Awake()
        {
#if UNITY_SERVER
            Destroy(this);
#endif
        }

        private void Update()
        {
            if (!NetworkRepository.IsCurrentClientOwnerOfObject(gameObject))
                return;

            var x = Input.GetAxis("Horizontal") * -_speed * Time.deltaTime;
            var y = Input.GetAxis("Vertical") * -_speed * Time.deltaTime;

            var moveCmd = new MoveCmd(NetworkRepository.GetGameObjectsId(gameObject), x, y);
            NetworkBus.OnCommandSend?.Invoke(moveCmd);
        }
    }
}
