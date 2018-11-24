using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace ThreadOne
{
    public delegate void ThreadDelegate(object[] collection);

    class Program
    {
        public static void ThreadMethod(object[] collection)
        {
            foreach (var item in collection)
              Console.WriteLine("ThreadID = " + Thread.CurrentThread.ManagedThreadId +
                  "    Item: " + item.ToString());                
        }


        static void Main(string[] args)
        {
            object[] myCollect = {
                                  new Point(2, 4),
                                  new Exception(),
                                  new Button(),
                                  new TextBox(),
                                  new Label()
                                  };

            ThreadDelegate td = new ThreadDelegate(ThreadMethod);

            Console.WriteLine("Main Threading ID is: " + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Begin Invoke!!!:");

            td.BeginInvoke(myCollect, null, null);

            Console.ReadLine();
        }
    }
}
