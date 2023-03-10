using Assets.Scripts.Network.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class ServerHub : Hub
    {
        private List<ICommand> _commandsHistory = new List<ICommand>();

        private TcpListener _tcpListener;
        private List<NetworkClient> _connectedClients = new List<NetworkClient>();

        private void Start()
        {
#if !UNITY_SERVER
            //Destroy(this);
#endif

            ClientsConnectingTask();

            GameBus.OnGameStarted += SpawnPlayers;
        }


        private void SpawnPlayers() 
        {
            var spawnCmd = new SpawnCmd("Player", 0);

            SendCommandToClient(spawnCmd, _connectedClients.First());
        }


        private async Task ClientsConnectingTask()
        {
            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _tcpListener.Start();

            while (true)
            {
                try
                {
                    var client = await _tcpListener.AcceptTcpClientAsync();

                    var availableId = _connectedClients.Count;

                    var connectedClient = new NetworkClient
                    {
                        ClientId = availableId,
                        Client = client,
                        StreamReader = new StreamReader(client.GetStream()),
                        StreamWriter = new StreamWriter(client.GetStream()),
                    };

                    _connectedClients.Add(connectedClient);

                    connectedClient.StreamWriter.AutoFlush = true;

                    var initCmd = new InitClientCmd(availableId);

                    connectedClient.StreamWriter.WriteLine(CommandToString(initCmd));

                    ClientReadingTask(connectedClient);

                    Debug.Log($"{client.Client.RemoteEndPoint} connected!");

                    if (_connectedClients.Count < 1)
                        continue;

                    GameBus.OnGameStarted?.Invoke();
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        private async Task ClientReadingTask(NetworkClient client)
        {
            while (true)
            {
                if (client.Client.Connected == false)
                {
                    _connectedClients.Remove(client);
                    return;
                }

                var data = await client.StreamReader.ReadLineAsync();

                var cmd = StringToCommand(data);

                PerformCommand(cmd);
            }
        }

        public void PerformCommand(ICommand cmd)
        {
            try
            {
                cmd.Execute();

                _commandsHistory.Add(cmd);

                foreach (var client in _connectedClients)
                {
                    SendCommandToClient(cmd, client);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while performing command on server: {e}");
            }
        }

        private void SendCommandToClient(ICommand cmd, NetworkClient client) 
        {
            client.StreamWriter.WriteLine(CommandToString(cmd));
        }
    }
}