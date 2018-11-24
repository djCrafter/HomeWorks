using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SemaphoreTest
{
    class MyThread
    {
        Thread thread;
        int count = 0;


        public MyThread()
        {
            thread = new Thread(Method);
            thread.Start();
        }

        public void Method()
        {
            while (count != 300)
            {
                Thread.Sleep(1000);
                count++;
            }
        }

        public int GetCount()
        {
            return count;
        }
    }
}
