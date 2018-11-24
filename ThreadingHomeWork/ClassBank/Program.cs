using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ClassBank
{
    public class Bank
    {
        public delegate void BankInfoDelegate(string n, int m, int p);
        private BankInfoDelegate bankInfoSaveDelegate;

        int saving = 0;


        private int money;
        private string name;
        private int percent;

        public int Money
        {
            get { return money; }
            set
            {
                if (money != value)
                {
                    bankInfoSaveDelegate.BeginInvoke(name, money, percent, null, null);
                    money = value;
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    bankInfoSaveDelegate.BeginInvoke(name, money, percent, null, null);
                    name = value;
                }
            }
        }
        public int Percent
        {
            get { return percent; }
            set
            {
                if (percent != value)
                {
                    bankInfoSaveDelegate.BeginInvoke(name, money, percent, null, null);
                    percent = value;
                }
            }
        }

        public Bank(string name, int money, int percent)
        {
            this.money = money;
            this.name = name;
            this.percent = percent;
            bankInfoSaveDelegate = new BankInfoDelegate(FieldSaver);
        }

        private void FieldSaver(string name, int money, int percent)
        {
            saving++;
            int id = Thread.CurrentThread.ManagedThreadId;

            string str = "Saving Operation " + saving + ":   BankName = " + name +
                "   AccountMoney = " + money + "   Percent = " + percent + "    ThreadID: " + id;

            StreamWriter sw = new StreamWriter("BankHistory.txt", true);
            sw.WriteLine(str);
            sw.Close();
        }

        public override string ToString()
        {
            return string.Format($"Name: {name}\nMoney: {money}\nPercent {percent}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bank myBank = new Bank("Avrora", 50000, 150);
            
            Console.WriteLine(myBank.ToString());

            myBank.Money = 5000;
            Console.WriteLine(myBank.ToString());
            Thread.Sleep(500);

            myBank.Name = "IdeaBank";
            Console.WriteLine(myBank.ToString());
            Thread.Sleep(500);

            myBank.Percent = 1000;
            Console.WriteLine(myBank.ToString());
            Thread.Sleep(500);

            Console.ReadLine();
        }
    }
}
