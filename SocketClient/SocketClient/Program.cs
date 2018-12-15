using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static int port = 8005;

        static string adress = "127.0.0.1";

        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(adress), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(ipPoint);

                Console.WriteLine("Введите сообщение!");

                string message = Console.ReadLine();

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

                Console.WriteLine("Ответ сервера: " + builder.ToString());

                socket.Shutdown(SocketShutdown.Both);

                socket.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
