using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;

namespace MyTaskManager
{
    public struct Task
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public string MemoryUse { get; set; }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {      
        DispatcherTimer timer = new DispatcherTimer();

        int selectedId = 0;
        

        public MainWindow()
        {
            InitializeComponent();            
            Refresh();
            TimerSetUp();
        }


        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(long value, int decimalPlaces = 0)
        {
            if (value < 0)
            {
                throw new ArgumentException("Bytes should not be negative", "value");
            }
            var mag = (int)Math.Max(0, Math.Log(value, 1024));
            var adjustedSize = Math.Round(value / Math.Pow(1024, mag), decimalPlaces);
            return String.Format("{0} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        public void TimerSetUp()
        {
            timer.Interval = new TimeSpan(0,0,0,1,500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            taskList.Items.Clear();
            Refresh();
            
        }

        private void Refresh()
        {
            var processes = from g in Process.GetProcesses() orderby g.WorkingSet64 select g;

            foreach(var item in processes)
            {
                Task task = new Task() { Id = item.Id, ProcessName = item.ProcessName, MemoryUse = SizeSuffix(item.WorkingSet64) };
                
                taskList.Items.Add(task);
            }
             
            taskList.SelectedIndex = selectedId;
        }

        private void taskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(taskList.SelectedIndex != -1)
            selectedId = taskList.SelectedIndex;           
        }

      
    }
}
