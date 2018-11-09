using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixed
{
    public class Person
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
                Person person = new Person();
                person.age = 28;
                person.height = 178;

                fixed (int* p = &person.age)
                {
                    if(*p < 30)
                    {
                        *p = 30;
                    }
                }

                Console.WriteLine(person.age);
            }

            Console.ReadLine();
        }
    }
}
