using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data.SqlClient;

namespace ChatServer
{
    public class ServerObject
    {
        string[] args;

        SqlConnection sqlConnection;

        static TcpListener tcpListener;
        List<ClientObject> clients = new List<ClientObject>();

        List<ChatString> history = new List<ChatString>();


        public ServerObject(string[] args)
        {
            this.args = args;
        }

        public bool ArgsExist()
        {
            if (args.Length > 0)
                return true;
            else
                return false;
        }

        public bool DbConnect(string dbName)
        {
            string SqlServerName = @".\SQLEXPRESS";

            string conStr;

            if (ArgsExist())
                conStr = @"Data Source=" + args[0] + "; Initial Catalog=master; Integrated Security=True;";
            else
                conStr = @"Data Source=" + SqlServerName + "; Initial Catalog=master; Integrated Security=True;";

            try
            {
                sqlConnection = new SqlConnection(conStr);
                sqlConnection.Open();

                if (!DbExist(sqlConnection, dbName))                            
                    CreateDB(sqlConnection, dbName);

                UseDb(sqlConnection, dbName);

                if (!TableExist(sqlConnection, "UsersTable"))          
                    CreateUsersTable(sqlConnection, "UsersTable");
              
                Console.WriteLine("SqlConnection status: " + sqlConnection.State);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DbExist(SqlConnection connection, string dbName)
        {

            SqlCommand cmd = new SqlCommand("DECLARE @bit Bit = 0; " +
                "IF DB_ID('" + dbName + "') IS NOT NULL " +
                " SET @bit = 1; " +
                " ELSE SET @bit = 0; " +
                " SELECT @bit", connection);

            return (bool)cmd.ExecuteScalar();
        }

        public bool TableExist(SqlConnection connection, string tableName)
        {
            SqlCommand cmd = new SqlCommand("DECLARE @bit Bit = 0; " +
                "IF object_id('" + tableName + "') IS NOT NULL " +
                " SET @bit = 1; " +
                " ELSE SET @bit = 0; " +
                " SELECT @bit", connection);

            return (bool)cmd.ExecuteScalar();
        }

        public void CreateDB(SqlConnection connection, string dbName)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("CREATE DATABASE " + dbName, connection);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Users DataBase created!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UseDb(SqlConnection connection, string dbName)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("USE " + dbName, connection);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateUsersTable(SqlConnection connection, string tableName)
        {
       
            string cmdString = "CREATE TABLE " + tableName + "(Id int PRIMARY KEY IDENTITY NOT NULL, " +
           "UserName nvarchar(30) UNIQUE NOT NULL, [Password] nvarchar(30) CHECK(Len([Password]) > 7) NOT NULL);";

            try
            {
                SqlCommand cmd = new SqlCommand(cmdString, connection);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Users Table created!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool Identification(string name, string pass)
        {
            string cmdString = "DECLARE @bit bit = 0; " +
                "IF EXISTS(SELECT UserName FROM UsersTable WHERE UsersTable.UserName = '" +
                 name + "' AND UsersTable.[Password] = '" +
                 pass + "') " +
                "SET @bit = 1 ELSE SET @bit = 0; " +
                "SELECT @bit";

            SqlCommand cmd = new SqlCommand(cmdString, sqlConnection);

            return (bool)cmd.ExecuteScalar();
        }

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
                if (DbConnect("WpfChatIdBase"))
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
                else
                {
                    Console.WriteLine("Сервер не был запущен. Проверьте входящие параметры.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                sqlConnection.Close();
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
