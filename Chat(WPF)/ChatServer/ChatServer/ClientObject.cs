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

                message = "entry/" + DateTime.Now.ToLongTimeString() + '/' + userName + '/' + Id;
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
                        message = String.Format("{0} {1} покинул чат", DateTime.Now.ToLongTimeString(), userName);
                        Console.WriteLine(message);

                        message = "leave/" + DateTime.Now.ToLongTimeString() + '/' + Id;

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

            if (message.Contains('/'))
            {
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
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + ' ' + message);
                    server.BroadcastMessage(code + '/' + DateTime.Now.ToLongTimeString() + '/' + message, this.Id);
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

}
