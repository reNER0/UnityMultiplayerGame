using System.IO;
using System.Net.Sockets;

namespace Assets.Scripts.Network
{
    internal class NetworkClient
    {
        public int ClientId;
        public TcpClient Client;
        public StreamReader StreamReader;
        public StreamWriter StreamWriter;
    }
}
