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

namespace TextFinder
{
    /// <summary>
    /// Interaction logic for Progress.xaml
    /// </summary>
    public partial class Progress : Window
    {
        Core core;

        private bool isCanceled = false;
     

        public Progress(Core core)
        {          
            InitializeComponent();
            this.core = core;

            StartProgress();
        }

        private async void StartProgress()
        {
            List<string> list = await core.LoadAllPath();

            progressBar.Maximum = list.Count;

            labelStatic.Content = "Сканирование файлов";

            if(!isCanceled)
            await core.TextWalk(list);




            if (isCanceled)
                Close();          
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            core.CancelSearch();
            isCanceled = true;
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            core.PauseSearch();
        }
    }
}
