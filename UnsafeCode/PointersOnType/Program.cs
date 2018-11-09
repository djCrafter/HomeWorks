using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointersOnType
{
    public struct Person
    {
        public int age;
        public int height;
    }

    class Program
    {
        static void Main(string[] args)
        {
            unsafe
            {
                Person person;
                person.age = 29;
                person.height = 176;
                Person* p = &person;
                p->age = 30;
                Console.WriteLine(p->age);

                (*p).height = 180;               
                Console.WriteLine((*p).height);
            }

            Console.ReadLine();
        }
    }
}
