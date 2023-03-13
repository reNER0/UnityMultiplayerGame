using Assets.Scripts.Network.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Zenject;

namespace Assets.Scripts.Network
{
    public interface IServerRepository
    {
        void AddCommandInCommandsTimeline(ICommand cmd);
        void AddClient(NetworkClient client);
        void RemoveClient(NetworkClient client);
        NetworkClient[] GetClients();
        ICommand[] GetCommands();
    }

    public class ServerRepository : Hub, IServerRepository, IInitializable, IDisposable
    {
        private List<CommandTimeFrame> _commandsHistory = new List<CommandTimeFrame>();
        private List<NetworkClient> _connectedClients = new List<NetworkClient>();


        public void Initialize()
        {
            NetworkBus.OnClientDisconnected += RemoveClient;
        }

        public void AddCommandInCommandsTimeline(ICommand cmd)
        {
            var cmdTimeFrame = new CommandTimeFrame
            {
                Command = cmd,
                Time = DateTime.Now,
            };

            _commandsHistory.Add(cmdTimeFrame);
        }

        public void AddClient(NetworkClient client)
        {
            _connectedClients.Add(client);
        }

        public void RemoveClient(NetworkClient client)
        {
            _connectedClients.Remove(client);
        }

        public NetworkClient[] GetClients()
        {
            return _connectedClients.ToArray();
        }

        public ICommand[] GetCommands()
        {
            return _commandsHistory.Select(x => x.Command).ToArray();
        }

        public void Dispose()
        {
            NetworkBus.OnClientDisconnected -= RemoveClient;
        }
    }

    public struct CommandTimeFrame
    {
        public ICommand Command;
        public DateTime Time;
    }

    public struct NetworkClient
    {
        public int ClientId;
        public TcpClient Client;
        public StreamReader StreamReader;
        public StreamWriter StreamWriter;
        public PlayerInputs PlayerInputs;
    }
}