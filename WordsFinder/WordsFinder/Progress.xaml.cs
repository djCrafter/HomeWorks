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
            List<string> list = await core.LoadAllPath();
            

            MessageBox.Show("Messagae!!!!");

            if (list != null)
            {
                label.Content = "Сканирование файлов...";

                progressBar.Maximum = list.Count;

                foreach (string item in list)
                {
                    if (await core.TextWalk(item))                  
                        progressBar.Value++;
                    else
                    {
                        list = null;
                        break;
                    }
                }
            }


            if (list != null)
            {
                MessageBox.Show("IsReady");
            }
            else
            {
                this.Close();
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
