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

            unsafe
            {
                int[] nums = { 0, 1, 2, 3, 7, 88 };
                string str = "Hello world!!!";

                fixed(int* p = nums)
                {

                }
                fixed(char* p = str)
                {

                }

            }
        }
    }
}
