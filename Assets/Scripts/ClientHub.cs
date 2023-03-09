using Assets.Scripts.Commands;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

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

                    // Do anything with data
                    Debug.Log(data);
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

    public void PerformCommand(ICommand command) 
    {
        string json = JsonUtility.ToJson(command);

        _streamWriter.WriteLine(json);
    }
}
