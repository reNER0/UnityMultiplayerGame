using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Network
{
    internal static class ClientProfile
    {
        public static int ClientId { get; private set; }

        public static void SetClientId(int id)
        {
            ClientId = id;
        }

        public static bool IsOwnerOfObject(GameObject gameObject) 
        {
            return NetworkObjectsRepository.NetworkObjectById
                .Values.First(x => x.GameObject == gameObject)
                .OwnerId == ClientId;
        }
    }
}
