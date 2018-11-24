using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace InTime
{
    class Program
    {
        public static void StopTime()
        {
            Console.WriteLine();
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("To start the test, press any key!");

            Console.ReadKey();

            for(int i = 3; i > 0; --i)
            {
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }
            Console.WriteLine("PRESS KEY!!!");

            stopwatch.Start();

            Console.ReadKey();

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine("Your Time is {0} milliseconds.", + stopwatch.Elapsed);
        }

        static void Main(string[] args)
        {
            char option = 'y';

            do
            {
                StopTime();

                Console.WriteLine("If you want to repeat the test, press 'y'!!!");
                option = (char)Console.ReadKey().Key;
            } while ( char.ToLower(option) == 'y');

            Console.WriteLine("\nProgram completed thank you for your attention.");
            Console.ReadLine();
        }
    }
}
