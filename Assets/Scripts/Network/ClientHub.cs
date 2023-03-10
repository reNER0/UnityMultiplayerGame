using Assets.Scripts.Network.Commands;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class ClientHub : Hub
    {
        private static TcpClient _client;
        private static StreamReader _streamReader;
        private static StreamWriter _streamWriter;

        private void Start()
        {
#if UNITY_SERVER
        //Destroy(this);
#endif

            ConnectClient();

            GameBus.OnClientSendToServer += SendCommandToServer;
        }


        private async Task ConnectClient()
        {
            while (true)
            {
                try
                {
                    _client = new TcpClient();

                    await _client.ConnectAsync(_ip, _port);

                    _streamReader = new StreamReader(_client.GetStream());
                    _streamWriter = new StreamWriter(_client.GetStream());

                    _streamWriter.AutoFlush = true;

                    ClientLoop();
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);

                    await Task.Delay(3000);
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

                    await Task.Delay(10);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);

                    await Task.Delay(3000);
                }
            }
        }

        private void PerformCommand(ICommand cmd)
        {
            cmd.Execute();
        }

        public void SendCommandToServer(ICommand cmd)
        {
            _streamWriter.WriteLine(CommandToString(cmd));
        }
    }
}
