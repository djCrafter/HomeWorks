using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace ChatServer
{
    public class ServerObject
    {
        static TcpListener tcpListener;
        List<ClientObject> clients = new List<ClientObject>();

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        protected internal void RemoveConnection(string Id)
        {
            ClientObject client = clients.FirstOrDefault(c => c.Id == Id);

            if (client != null)
                clients.Remove(client);
        }

        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                   

                    Thread clientThread = new Thread(clientObject.Process);
                    clientThread.Start();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }


        protected internal void BroadcastMessage(string message, string id)
        {
                byte[] data = Encoding.Unicode.GetBytes(message);
                for (int i = 0; i < clients.Count; i++)
                {
                    clients[i].Stream.Write(data, 0, data.Length);             
                }
        }

    

        protected internal void Disconnect()
        {
            tcpListener.Stop();

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);
        }

        public IEnumerable<ClientObject> GetClientObjects()
        {
            return clients;
        }

        
        public void UserEnterMessage(string name)
        {
            Console.WriteLine(name + " вошел в чат.");
        }
    }


    public class ClientObject
    {
        protected internal string Id { get; set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
        }

        TcpClient client;
        ServerObject server;

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }
       
        public void SendUserList()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("addlist");

            foreach (ClientObject item in server.GetClientObjects())
            {
                sb.Append('/' + item.UserName);               
                sb.Append('/' + item.Id);
            }

            byte[] data = Encoding.Unicode.GetBytes(sb.ToString());
            Stream.Write(data, 0, data.Length);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();

                string message = null;

                message = "takeid/" + Id;
                byte[] data = Encoding.Unicode.GetBytes(message);
                Stream.Write(data, 0, data.Length);

                Thread.Sleep(100);

                userName = GetMessage();

                SendUserList();

                Thread.Sleep(50);

                message = "entry/" + userName + '/' + Id;
                server.UserEnterMessage(userName);
                server.BroadcastMessage(message, Id);
                                                                   
                while (true)
                {
                    try
                    {
                            message = GetMessage();

                        if (message.Length == 0)
                            throw new Exception();

                       MessageDecode(message);                                                                               
                    }
                    catch
                    {
                        message = String.Format("{0} покинул чат", userName);
                        Console.WriteLine(message);

                        message = "leave/" + Id;

                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        public void MessageDecode(string message)
        {
            string code = "default";

            if (message.Contains('/')) {
                int index = message.IndexOf('/');

                code = message.Substring(0, index);
                message = message.Substring(++index);
            }

            switch (code)
            {
                case "test":
                    Console.WriteLine("Тест сработал");
                    break;
                default:                    
                    message = String.Format("{0}: {1}", userName, message);
                    Console.WriteLine(message);
                    server.BroadcastMessage(code + "/" + message, this.Id);
                    break;
            }
        }

        private string GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);                
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();           
        }

        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }


        class Program
    {
        static ServerObject server;
        static Thread listenThread;
      
        static void Main(string[] args)
        {
            try
            {
                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch(Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
