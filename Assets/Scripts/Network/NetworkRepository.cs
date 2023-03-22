using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public static class NetworkRepository
    {
        public static int CurrentCliendId { get; private set; } = -1;

        public static Dictionary<int, NetworkObject> NetworkObjectById = new Dictionary<int, NetworkObject>();

        public static int GetAvailableNetworkObjectId() => NetworkObjectById.Count;

        public static int GetGameObjectsId(GameObject gameObject) => NetworkObjectById.First(x => x.Value.GameObject == gameObject).Key;

        public static NetworkObject GetNetworkObject(GameObject gameObject) => NetworkObjectById.First(x => x.Value.GameObject == gameObject).Value;

        public static void SetClientId(int id)
        {
            CurrentCliendId = id;
        }

        public static bool IsCurrentClientOwnerOfObject(GameObject gameObject)
        {
            return NetworkObjectById
                .Values.First(x => x.GameObject == gameObject)
                .OwnerId == CurrentCliendId;
        }
    }
}
