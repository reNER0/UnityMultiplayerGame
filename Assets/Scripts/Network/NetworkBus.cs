using Assets.Scripts.Network.Commands;
using System;

namespace Assets.Scripts.Network
{
    public static class NetworkBus 
    {
        public static Action<ICommand> OnCommandSendToServer;
        public static Action<ICommand> OnCommandSendToClients;
        public static Action<NetworkClient> OnClientConnected;
        public static Action<NetworkClient> OnClientDisconnected;
        public static Action<PlayerInputs> OnClientInput;
    }
}
