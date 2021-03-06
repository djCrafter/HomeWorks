﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operations
{
    class Program
    {
        static void Main(string[] args)
        {
            unsafe
            {
                int* x;
                int y = 10;

                x = &y;

                Console.WriteLine(*x);

                y = y + 20;
                Console.WriteLine(*x);

                *x = 50;

                Console.WriteLine(y);
            }

            Console.ReadLine();

            unsafe
            {
                int* x;

                int y = 10;

                x = &y;

                uint addr = (uint)x;

                Console.WriteLine("The address of the variable y: {0}", addr);
            }

            Console.ReadLine();
        }
    }
}
