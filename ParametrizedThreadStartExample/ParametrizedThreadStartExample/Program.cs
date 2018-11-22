using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ParametrizedThreadStartExample
{

    class Program
    {
        public static void Encryption(object obj)
        {
            while (true)
            {
                
               
            }
        }

        public static void Decryption(object obj)
        {
            while (true)
            {
                
              
            }
        }


        static void Main(string[] args)
        {
            ParameterizedThreadStart Param = null;

            while (true)
            {
                Console.Clear();

                Console.WriteLine("1.Шифровать.");
                Console.WriteLine("2.Дешифровать.");

                ConsoleKeyInfo Select = Console.ReadKey(true);
                Console.Clear();

                if(ConsoleKey.D1 == Select.Key)
                {
                    Param = new ParameterizedThreadStart(Encryption);
                    Console.WriteLine("Введите путь к файлу который хотите зашифровать!");
                }
                else if(ConsoleKey.D2 == Select.Key)
                {
                    Param = new ParameterizedThreadStart(Decryption);
                    Console.WriteLine("Введите путь к файлу который хотите расшифровать!");
                }

                if(ConsoleKey.D1 == Select.Key || ConsoleKey.D2 == Select.Key)
                {
                    string FilePath = Console.ReadLine();

                    Thread thread = new Thread(Param);
                    thread.Start((object)FilePath);
                    Console.WriteLine("Нажмите символ чтобы выполнить действие!");

                    do
                    {
                        Console.WriteLine("[c] Отменить работу потока");
                        Console.WriteLine("[p] приостановить или возобновить работу потока");

                        ConsoleKeyInfo Selects = Console.ReadKey(true);

                        if (Selects.Key == ConsoleKey.C)
                        {
                            if (thread.ThreadState == ThreadState.Running)
                            {
                                thread.Abort();

                                Console.WriteLine("Поток остановлен!");
                            }
                        }
                        else if (Selects.Key == ConsoleKey.P)
                        {
                            if (thread.ThreadState == ThreadState.Running)
                            {
                                thread.Suspend();
                                Console.WriteLine("Поток приостановлен!");
                            }
                            else if (thread.ThreadState == ThreadState.Suspended)
                            {
                                thread.Resume();
                                Console.WriteLine("Поток восстановли работу!");
                            }
                            Thread.Sleep(100);
                        }
                    } while (thread.ThreadState == ThreadState.Suspended || thread.ThreadState == ThreadState.Running);
                    Console.ReadKey(true);
                    Console.Clear();
                }


            }
        }
    }
}
