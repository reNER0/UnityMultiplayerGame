using System;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public static class NetworkSettings
    {
        public static readonly string ServerIP = "192.168.0.106";
        public static readonly float ServerTickrate = 30;
        public static readonly float SyncSpeed = 0.1f;
        public static DateTime ServerStartupTime { get; private set; } = DateTime.Now;

        public static float ServerFixedUpdateTime => 1 / ServerTickrate;
        public static int ServerFixedUpdateTimeMilliseconds => (int)Mathf.Round(1000 / ServerTickrate);
        public static int CurrentTick => (int)(DateTime.Now - ServerStartupTime).TotalMilliseconds / ServerFixedUpdateTimeMilliseconds;


        public static void SetServerStartupTime(DateTime time)
        { 
            ServerStartupTime = time; 
        }
    }
}
