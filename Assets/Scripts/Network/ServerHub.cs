using Assets.Scripts.Network.Commands;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Network
{
    public interface IServerHub
    {
        public void PerformCommand(ICommand cmd);
        public void SendCommandToClient(ICommand cmd, NetworkClient client);
        public void SendCommandToAllClients(ICommand cmd);
        public void SendCommandToAllClientsExcept(ICommand cmd, NetworkClient exceptClient);
        public void DisconnectClient(NetworkClient client);
        public void DisconnectAllClients();
    }

    public class ServerHub : Hub, IServerHub, IInitializable, IDisposable
    {
        private readonly IServerRepository _serverRepository;

        public ServerHub(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public void Initialize()
        {
            ConnectingClientsLoopTask();

            NetworkBus.OnCommandSendToClients += SendCommandToAllClients;
        }


        private async Task ConnectingClientsLoopTask()
        {
            var tcpListener = new TcpListener(IPAddress.Any, _port);
            tcpListener.Start();

            while (true)
            {
                try
                {
                    var client = await tcpListener.AcceptTcpClientAsync();

                    AddNewClient(client);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        private void AddNewClient(TcpClient client) 
        {
            var availableId = _serverRepository.GetClients().Length;

            var connectedClient = new NetworkClient
            {
                ClientId = availableId,
                Client = client,
                StreamReader = new StreamReader(client.GetStream()),
                StreamWriter = new StreamWriter(client.GetStream()),
            };

            connectedClient.StreamWriter.AutoFlush = true;

            const string FMT = "O";
            DateTime now1 = NetworkSettings.ServerStartupTime;
            string strDate = now1.ToString(FMT);
            var initCmd = new InitClientCmd(availableId, strDate);

            SendCommandToClient(initCmd, connectedClient);

            foreach (var cmd in _serverRepository.GetCommands().Where(x => x.GetType().Equals(typeof(SpawnCmd)))) 
            {
                SendCommandToClient(cmd, connectedClient);
            }

            ClientReadingTask(connectedClient);

            _serverRepository.AddClient(connectedClient);

            Debug.Log($"{client.Client.RemoteEndPoint} connected!");
            
            NetworkBus.OnClientConnected?.Invoke(connectedClient);
        }

        private async Task ClientReadingTask(NetworkClient client)
        {
            while (true)
            {
                if (client.Client.Connected == false)
                {
                    NetworkBus.OnClientDisconnected?.Invoke(client);
                    return;
                }

                var data = await client.StreamReader.ReadLineAsync();

                var cmd = StringToCommand(data);

                PerformCommand(cmd);
                SendCommandToAllClients(cmd);
            }
        }

        public void PerformCommand(ICommand cmd)
        {
            try
            {
                cmd.Execute();

                _serverRepository.AddCommandInCommandsTimeline(cmd);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while performing command on server: {e}");
            }
        }

        public void SendCommandToClient(ICommand cmd, NetworkClient client)
        {
            client.StreamWriter.WriteLine(CommandToString(cmd));
        }

        public void SendCommandToAllClients(ICommand cmd)
        {
            foreach (var client in _serverRepository.GetClients())
            {
                SendCommandToClient(cmd, client);
            }
        }

        public void SendCommandToAllClientsExcept(ICommand cmd, NetworkClient exceptClient)
        {
            foreach (var client in _serverRepository.GetClients())
            {
                if (client.ClientId == exceptClient.ClientId)
                    continue;

                SendCommandToClient(cmd, client);
            }
        }

        public void DisconnectClient(NetworkClient client)
        {
            client.Client.Close();
        }

        public void DisconnectAllClients()
        {
            foreach (var client in _serverRepository.GetClients())
            {
                DisconnectClient(client);
            }
        }

        public void Dispose()
        {
            DisconnectAllClients();

            NetworkBus.OnCommandSendToClients -= SendCommandToAllClients;
        }
    }
}