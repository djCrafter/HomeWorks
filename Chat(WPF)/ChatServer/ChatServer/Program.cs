﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace ChatServer
{   
    class Program
    {
        static ServerObject server;
        static Thread listenThread;
      
        static void Main(string[] args)
        {
            try
            {
                server = new ServerObject(args);

                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
