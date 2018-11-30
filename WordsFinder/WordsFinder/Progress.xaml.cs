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
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Threading;

namespace WordsFinder
{
    /// <summary>
    /// Interaction logic for Progress.xaml
    /// </summary>
    public partial class Progress : Window
    {
        Core core;

      
       

        bool isReady = false;

        public Progress(Core core)
        {
            InitializeComponent();
            this.core = core;

            LoadAllPath();
            //DispatcherTimer dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Tick += DispatcherTimer_Tick;
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            //dispatcherTimer.Start();
        }

        private async void LoadAllPath()
        {
            isReady = await core.LoadAllPath();
            if (isReady)
            {
                MessageBox.Show("IsReady");
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            core.CancelOperation();
        }

        //private void DispatcherTimer_Tick(object sender, EventArgs e)
        //{
        //    progressBar.Value++;
        //}


    }
}
