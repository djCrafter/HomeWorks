using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointersOnArrayAndStacalloc
{
    class Program
    {
       unsafe static void Main(string[] args)
        {           
            const int size = 7;

            int* factorial = stackalloc int[size];

            int* p = factorial;

            *(p++) = 1;

            for (int i = 2; i <= size; i++, p++)
            {
                *p = p[-1] * i;
            }
            for (int i = 1; i <= size; ++i)
            {
                Console.WriteLine(factorial[i - 1]);
            }  
            Console.ReadLine();
        }     
    }
}
