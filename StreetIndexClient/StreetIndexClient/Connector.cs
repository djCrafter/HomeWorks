using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace StreetIndexClient
{
    static class Connector
    {
        static int port = 8005;
        static string adress = "127.0.0.1";

        public async static Task<string> Search(string message)
        {
            return await Task.Run(() =>
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(adress), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(ipPoint);

                byte[] data = Encoding.Unicode.GetBytes(message);

                socket.Send(data);

                data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available < 0);

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                message = builder.ToString();

                return message;
            });
        }
    }
}
