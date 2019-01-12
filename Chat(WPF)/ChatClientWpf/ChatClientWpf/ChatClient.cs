using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Windows;
using System.Collections.ObjectModel;

namespace ChatClientWpf
{  
    class Client
    {             
        MainWindow gui;

        string userName;
        private readonly string host;
        private readonly int port;
        static TcpClient client;
        static NetworkStream stream;
        string id;

        bool WithTime { get; set; }


        public Client(string userName, string host, int port, MainWindow gui)
        {
            this.userName = userName;
            this.host = host;
            this.port = port;
            this.gui = gui;
            WithTime = true;
        }

        public void Connect()
        {
            client = new TcpClient();

            try
            {
                client.Connect(host, port);
                stream = client.GetStream();

                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                ReciveMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Удаленный сервер не отвечает!\nНет соединения или вы забыли запустить сервер:)", "Error Code(1703)", MessageBoxButton.OK, MessageBoxImage.Stop);

                gui.Dispatcher.Invoke(DispatcherPriority.Background, new
               Action(() =>
               {
                   gui.Close();
               }));
            }
        }

        public async Task SendMessage(string message)
        {
            await Task.Run(() =>
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            });
        }

    

        public void ReciveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();


                    MessageDecode(message);
                   
                }
                catch(Exception ex)
                {                                 
                   Disconnect();
                }
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
                case "takeid":
                    id = message;                    
                    break;

                case "addlist":
                    AddList(message);              
                    break;

                case "entry":
                    Entry(message);
                    break;

                case "leave":
                    Leave(message);
                    break;

                case "history":
                    History(message);
                    break;

                case "server_messg":
                    PrintDefaultMessage(message);
                    break;

                case "private":
                    PrintDefaultMessage(message);
                    break;

                case "private_call_back":
                    PrintDefaultMessage(message);
                    break;

                default:
                    PrintDefaultMessage(message);
                    break;
            }
        }

        void TakeId(string id)
        {
            this.id = id;  
        }

        public void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            Environment.Exit(0);
        }

     
        public IEnumerable<ChatUser> UsersPlusIdDecode(string message)
        {
            List<ChatUser> list = new List<ChatUser>();

            while (message.Contains('/'))
            {
                int index = message.IndexOf('/');

                string name = message.Substring(0, index);
                message = message.Substring(++index);

                string id;

                if (message.Contains('/'))
                {
                    index = message.IndexOf('/');
                    id = message.Substring(0, index);
                    message = message.Substring(++index);
                }
                else
                {
                    id = message;
                }

                list.Add(new ChatUser(name, id));
            }
            return list;
        }



        private void AddList(string message)
        {
            gui.listBox.Dispatcher.Invoke(DispatcherPriority.Background, new
               Action(() =>
               {
                   foreach (ChatUser item in UsersPlusIdDecode(message))
                   {
                       gui.listBox.Items.Add(item);
                   }
               }));
        }

        private void Entry(string message)
        {
            int index = message.IndexOf('/');

            string time = message.Substring(0, index);          
            message = message.Substring(++index);
            index = message.IndexOf('/');

            string name = message.Substring(0, index);
            string id = message.Substring(++index);

            if (id != this.id)
            {
                gui.listBox.Dispatcher.Invoke(DispatcherPriority.Background, new
                 Action(() =>
                 {
                     gui.listBox.Items.Add(new ChatUser(name, id));
                 }));
            }

            if(WithTime)
                message = '\n' + time + ' ' + name + " вошел в чат.";
            else
                message = '\n' + name + " вошел в чат.";

            gui.textBox.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                gui.textBox.AppendText(message);
            }));
        }

        private void Leave(string message)
        {
            string name = "Unknown";

            int index = message.IndexOf('/');

            string time = message.Substring(0, index);
            id = message.Substring(++index);

            gui.listBox.Dispatcher.Invoke(DispatcherPriority.Background, new
                 Action(() =>
                 {                  
                   foreach(ChatUser item in gui.listBox.Items)
                     {
                         if (item.Id == id)
                         {
                             name = item.Name;
                             gui.listBox.Items.Remove(item);
                             break;
                         }
                     }                 
                 }));

            gui.textBox.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                if(WithTime)
                    gui.textBox.AppendText('\n' + time + ' ' + name + " покинул чат.");
                else
                    gui.textBox.AppendText('\n' + name + " покинул чат.");
            }));
        }


        private void PrintDefaultMessage(string message)
        {
            int index = message.IndexOf('/');

            string time = message.Substring(0, index);
            message = message.Substring(++index);

            gui.textBox.Dispatcher.Invoke(DispatcherPriority.Background, new
                  Action(() =>
                  {
                      if(WithTime)
                      gui.textBox.AppendText("\n" + time + ' ' + message);
                      else
                      gui.textBox.AppendText("\n" + message);
                  }));
        }



        private void History(string message)
        {
            while (message.Contains('/'))
            {
                int index = message.IndexOf('/');

                string time = message.Substring(0, index);
                message = message.Substring(++index);
                
                index = message.IndexOf('/');

                string str;

                if (index > 0)
                {
                    str = message.Substring(0, index);
                    message = message.Substring(++index);
                }
                else
                {
                    str = message;
                }
                gui.textBox.Dispatcher.Invoke(DispatcherPriority.Background, new
               Action(() =>
               {
                   if (WithTime)
                       gui.textBox.AppendText("\n" + time + ' ' + str);
                   else
                       gui.textBox.AppendText("\n" + str);
               }));
            }
        }
    }
}
