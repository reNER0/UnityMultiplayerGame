using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public static class NetworkSettings
    {
        public static readonly string ServerIP = "192.168.0.106";
        public static readonly float ServerTickrate = 30;

        public static float ServerFixedUpdateTime => 1 / ServerTickrate;
        public static int ServerFixedUpdateTimeMilliseconds => (int)Mathf.Round(1000 / ServerTickrate);
    }
}
