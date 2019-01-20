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
        string userPass;

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

        public void SendSystemMessage(string message)
        {
            message = "server_messg/" + DateTime.Now.ToLongTimeString() + '/' + message; 

            byte[] data = Encoding.Unicode.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }

        private void SetNamePass(string message)
        {
            int index = message.IndexOf('/');

            userName = message.Substring(0, index);
            userPass = message.Substring(++index);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();

                string message = null;

                SetNamePass(GetMessage());

                if(server.Identification(userName, userPass))
                {
                    message = "takeid/" + Id;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    Stream.Write(data, 0, data.Length);

                    Thread.Sleep(100);

                    SendUserList();
                    Thread.Sleep(100);

                    message = "history/" + server.UploadHistory();
                    byte[] historyData = Encoding.Unicode.GetBytes(message);
                    Stream.Write(historyData, 0, historyData.Length);

                    Thread.Sleep(100);

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
                            server.UserOutMessage(userName);

                            message = "leave/" + DateTime.Now.ToLongTimeString() + '/' + Id;

                            server.BroadcastMessage(message, this.Id);
                            break;
                        }
                    }
                }
                else
                {
                    message = "novalid/" + Id;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    Stream.Write(data, 0, data.Length);
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

        private void PrivateCallBackMessage(string name, string message)
        {
            Console.Write(DateTime.Now.ToLongTimeString());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" Private message from " + userName + " to " + name + ": ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            
            message = "private_call_back/" + DateTime.Now.ToLongTimeString() + "/To " + name + ": " + message;

            byte[] data = Encoding.Unicode.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }

        private void PrivateDecode(string message)
        {
            int index = message.IndexOf('/');

            string name = message.Substring(0, index);
            message = message.Substring(++index);

            if (server.NameExist(name))
            {
                server.PrivateSend(userName, name, message);
                PrivateCallBackMessage(name, message);
            }
            else
            {
                SendSystemMessage("SERVER: --- Неверное имя пользователя!!! --- ");
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
                case "private":
                    PrivateDecode(message);
                    break;
                default:
                    message = String.Format("{0}: {1}", userName, message);
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + ' ' + message);
                    server.AddChatString(DateTime.Now, message);
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
