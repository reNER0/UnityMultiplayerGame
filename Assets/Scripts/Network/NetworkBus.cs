using Assets.Scripts.Network.Commands;
using System;

namespace Assets.Scripts.Network
{
    public static class NetworkBus 
    {
        public static Action<ICommand> OnCommandSend;
        public static Action<NetworkClient> OnClientConnected;
        public static Action<NetworkClient> OnClientDisconnected;
    }
}
