using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static int port = 8005;

        static void Main(string[] args)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string conStr = @"Data Source=.\SQLEXPRESS; Initial Catalog=StreetsDB; Integrated Security=True;";
            SqlConnection connection = new SqlConnection(conStr);

            try
            {
                listenSocket.Bind(iPEndPoint);

                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {                   
                    Socket handler = listenSocket.Accept();
                    StringBuilder builder = new StringBuilder();

                    int bytes = 0;

                    byte[] data = new byte[2000];

                    do
                    {
                        bytes = handler.Receive(data);

                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));

                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": Запрошен индекс: " + builder.ToString());

                    SqlCommand cmd = new SqlCommand(@"SELECT Street FROM StreetIndex
                                                      Where PostIndex = '" +  builder.ToString() + 
                                                      "'", connection);
                    connection.Open();

                    string message;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        StringBuilder results = new StringBuilder();

                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; ++i)
                            {
                                results.Append(reader[i] + "\n");                            
                            }
                        }


                        if (results.Length > 0)
                            message = results.ToString();
                        else
                            message = "Данный индекс отсутствует в базе!";
                    }
                  
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);

                    handler.Shutdown(SocketShutdown.Both);

                    connection.Close();
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
