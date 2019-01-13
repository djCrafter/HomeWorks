using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UDP_Chat
{
    class Program
    {
        static int RemotePort;
        static int LocalPort;
        static IPAddress RemoteIPAddres;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.SetWindowSize(40, 20);
                Console.Title = "Chat";
                Console.WriteLine("Введите удаленный IP.");
                RemoteIPAddres = IPAddress.Parse(Console.ReadLine());
                Console.WriteLine("Введите удаленный порт.");
                RemotePort = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите локальный порт.");
                LocalPort = Convert.ToInt32(Console.ReadLine());

                Thread thread = new Thread(new ThreadStart(ThreadFuncRecive));
                thread.IsBackground = true;
                thread.Start();
                Console.ForegroundColor = ConsoleColor.Red;
                while (true)
                {
                    SendData(Console.ReadLine());
                }
            }
            catch(FormatException formEcx)
            {
                Console.WriteLine("Преобразование невозможно: " + formEcx);
            }
            catch(Exception exc)
            {
                Console.WriteLine("Ошибка:" + exc.Message);
            }
        }

        static void ThreadFuncRecive()
        {
            try
            {
                while (true)
                {
                    UdpClient uClient = new UdpClient(LocalPort);
                    IPEndPoint ipEnd = null;

                    byte[] responce = uClient.Receive(ref ipEnd);
                    string strResult = Encoding.Unicode.GetString(responce);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(strResult);
                    Console.ForegroundColor = ConsoleColor.Red;
                    uClient.Close();
                }
            }
            catch(SocketException sockEx)
            {
                Console.WriteLine("Ошибка сокета." + sockEx.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка:" + ex.Message);
            }
        }

        static void SendData(string datagramm)
        {
            UdpClient uClient = new UdpClient();
            IPEndPoint iPEnd = new IPEndPoint(RemoteIPAddres, RemotePort);

            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(datagramm);
                uClient.Send(bytes, bytes.Length, iPEnd);
            }
            catch(SocketException sockEx)
            {
                Console.WriteLine("Ошибка сокета: " + sockEx.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            finally
            {
                uClient.Close();
            }
        }
    }
}
