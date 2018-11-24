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
using System.Collections.ObjectModel;
using System.Threading;
using eisiWare;

namespace SemaphoreTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Thread> creaedThreads = new ObservableCollection<Thread>();
        ObservableCollection<Thread> waitingThreads = new ObservableCollection<Thread>();
        ObservableCollection<Thread> workingThreads = new ObservableCollection<Thread>();

        ObservableCollection<ThreadState> ts = new ObservableCollection<ThreadState>();
        
        private static Semaphore pool;
        public int threadsCount = 0;
           

        public void Method()
        {
            pool.WaitOne();

          


            while (true)
            {
             
                

            }

            pool.Release();
        }

        public MainWindow()
        {
            InitializeComponent();


            pool = new Semaphore(3, 500, "MainSemaphore");

            CreatedThreadBox.ItemsSource = creaedThreads;
            WaitThreadBox.ItemsSource = waitingThreads;
            WorkThreadBox.ItemsSource = workingThreads;

            ts.CollectionChanged += Ts_CollectionChanged;
           
        }

        private void Ts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MessageBox.Show(e.Action.ToString());
        }

        public Thread CreateThread()
        {
            threadsCount++;
            Thread thread = new Thread(Method);
            thread.Name = "Поток " + threadsCount.ToString();
            ts.Add(thread.ThreadState);
            return thread;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = CreateThread();
            creaedThreads.Add(thread);
            MessageBox.Show(thread.ThreadState.ToString());

        }

        private void CreatedThreadBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(CreatedThreadBox.SelectedItem != null)
            {
                Thread thread = (Thread)CreatedThreadBox.SelectedItem;
                creaedThreads.Remove(thread);

                waitingThreads.Add(thread);

                thread.Start();

                Thread.Sleep(20);

                MessageBox.Show(thread.ThreadState.ToString());

            }
        }
    }
}
