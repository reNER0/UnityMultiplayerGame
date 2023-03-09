using Assets.Scripts;
using Assets.Scripts.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

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

                var connectedClient = new NetworkClient 
                {
                    Client = client,
                    StreamReader = new StreamReader(client.GetStream()),
                    StreamWriter = new StreamWriter(client.GetStream()),
                };

                _connectedClients.Add(connectedClient);

                ClientReadingTask(connectedClient);

                Debug.Log($"{client.Client.RemoteEndPoint} connected!");
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
        }
        catch (Exception e) 
        {
            Debug.LogError($"Error while performing command on server: {e}");
        }
    }

    public static ICommand StringToCommand(string msg)
    {
        SerializableCommandClass ctype = JsonUtility.FromJson<SerializableCommandClass>(msg);
        Type t = Type.GetType(ctype.GetClassName);
        ICommand gc = (ICommand)JsonUtility.FromJson(msg, t);
        return gc;
    }
}
