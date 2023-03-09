using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal static class NetworkObjectsRepository
    {
        public static Dictionary<int, NetworkObject> NetworkObjectById = new Dictionary<int, NetworkObject>();

        public static int GetAvailableId() => NetworkObjectById.Count;


        public static int GetGameObjectsId(GameObject gameObject)
        {
            return NetworkObjectById.First(x => x.Value.GameObject == gameObject).Key;
        }
    }
}
