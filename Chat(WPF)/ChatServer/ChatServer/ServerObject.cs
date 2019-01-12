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

        List<ChatString> history = new List<ChatString>(); 

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
            string message = name + " вошел в чат.";
            history.Add(new ChatString(DateTime.Now, message));


            Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + message);
        }

        public void UserOutMessage(string name)
        {
            string message = name + " покинул чат.";
            history.Add(new ChatString(DateTime.Now, message));


            Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + message);
        }

        public void AddChatString(DateTime dateTime, string str)
        {
            history.Add(new ChatString(dateTime, str));
        }

        public string UploadHistory()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < history.Count; ++i)
            {
                sb.Append(history[i].Time.ToLongTimeString() + '/' + history[i].Str);
                if (i + 1 != history.Count)
                    sb.Append('/');
            }
            return sb.ToString();
        }

        public bool NameExist(string name)
        {           
            foreach (ClientObject client in clients)
                if (name == client.UserName)
                    return true;

            return false;           
        }

        public void PrivateSend(string senderName, string recipientName, string message)
        {
            message = "private/" + DateTime.Now.ToLongTimeString() + '/' + "From " + senderName + ": " + message;

            foreach (ClientObject client in clients)
                if (recipientName == client.UserName)
                {
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    client.Stream.Write(data, 0, data.Length);
                    break;
                }

        }
    }

}
