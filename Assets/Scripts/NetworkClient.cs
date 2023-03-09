using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class NetworkClient
    {
        public TcpClient Client;
        public StreamReader StreamReader;
        public StreamWriter StreamWriter;
    }
}
