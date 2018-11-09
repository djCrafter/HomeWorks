using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pointers
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

                uint addr = (uint)x;

                Console.WriteLine("The address of the variable y: {0}", addr);

                byte* bytePointers = (byte*)(addr + 4);
                Console.WriteLine("Byte value at {0}: {1}", addr + 4, *bytePointers);

                uint oldAddr = (uint)bytePointers - 4;
                int* intPointer = (int*)oldAddr;
                Console.WriteLine("Int value at {0}: {1}", oldAddr, *intPointer);

                double* doublePointer = (double*)(addr + 4);
                Console.WriteLine("Double value at {0}: {1}", addr + 4, *doublePointer);

                //***********************************************************************//

                char* charPointer = (char*)123500;
                charPointer += 4;
                Console.WriteLine("Adress: {0}", (uint)charPointer);

                //***********************************************************************//
            }

            Console.ReadLine();

            //Pointer to pointer.
            unsafe
            {
                int* x;
                int y = 10;

                x = &y;
                int** z = &x;
                **z = **z + 40;
                Console.WriteLine(y);

                Console.WriteLine(**z);
            }

            Console.ReadLine();
        }
    }
}
