using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ParametrizedThreadStartExample2
{
    class Program
    {
        static void LisenerClient()
        {
            int Counter = 0;

            while (true)
            {
                Console.WriteLine("Нажмите любую клавишу для симуляции подключения!!!");
                Console.ReadKey(true);
                ParameterizedThreadStart UserDel = new ParameterizedThreadStart(UserThreadFunk);

                Thread UserWorkThread = new Thread(UserDel);

                UserWorkThread.Start((object)Counter.ToString());
                Counter++;

            }
        }

        static void UserThreadFunk(object a)
        {
            string UserName = (string)a;

            Console.WriteLine("пользователь\n# " + UserName + "подключился");

            while (true)
            {
                switch (GetUserCommand())
                {
                    case 0:
                        Console.WriteLine("# {0} подписался на новости", UserName);
                        break;
                    case 1:
                        Console.WriteLine("# {0} начал чат", UserName);
                        break;
                    case 2:
                        Console.WriteLine("# {0} купил продукцию в магазине", UserName);
                        break;
                    case 3:
                        Console.WriteLine("# {0} отправил письмо", UserName);
                        break;
                    case 4:
                        Console.WriteLine("# {0} отключился", UserName);
                        return;
                }
                Thread.Sleep(2000);
            }
        }

        static int GetUserCommand()
        {
            Random random = new Random();

            return random.Next(0, 5);

        }

        static void Main(string[] args)
        {
            ThreadStart Lis = new ThreadStart(LisenerClient);
            Thread LisenerThread = new Thread(Lis);
            LisenerThread.IsBackground = false;
            LisenerThread.Start();
        }
    }
}
