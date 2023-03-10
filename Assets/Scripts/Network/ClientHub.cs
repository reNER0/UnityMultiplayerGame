using Assets.Scripts.Network.Commands;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Network
{
    public interface IClientHub
    {
        public void PerformCommand(ICommand cmd);
        public void SendCommandToServer(ICommand cmd);
    }
    public class ClientHub : Hub, IClientHub, IInitializable, IDisposable
    {
        private static TcpClient _client;
        private static StreamReader _streamReader;
        private static StreamWriter _streamWriter;

        public void Initialize()
        {
            ConnectClient();

            NetworkBus.OnCommandSendToServer += PerformCommand;
            NetworkBus.OnCommandSendToServer += SendCommandToServer;
        }

        public void PerformCommand(ICommand cmd)
        {
            cmd.Execute();
        }

        public void SendCommandToServer(ICommand cmd)
        {
            _streamWriter.WriteLine(CommandToString(cmd));
        }

        private async Task ConnectClient()
        {
            while (true)
            {
                try
                {
                    _client = new TcpClient();

                    await _client.ConnectAsync(NetworkSettings.ServerIP, _port);

                    _streamReader = new StreamReader(_client.GetStream());
                    _streamWriter = new StreamWriter(_client.GetStream());

                    _streamWriter.AutoFlush = true;

                    ClientLoop();
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return;
                }
            }
        }

        private async Task ClientLoop()
        {
            Debug.Log("Starting client loop");

            while (true)
            {
                try
                {
                    if (_client?.Connected == true)
                    {
                        var data = await _streamReader.ReadLineAsync();

                        var cmd = StringToCommand(data);

                        PerformCommand(cmd);
                    }
                    else
                    {
                        Debug.LogError("Client disconnected!");
                        return;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);

                    await Task.Delay(3000);
                }
            }
        }

        public void Dispose()
        {
            NetworkBus.OnCommandSendToServer -= PerformCommand;
            NetworkBus.OnCommandSendToServer -= SendCommandToServer;

            _client?.Dispose();
            _streamReader?.Dispose();
            _streamWriter?.Dispose();
        }
    }
}
