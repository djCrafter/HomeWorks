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

namespace WordsFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Core core = new Core();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = core.OpenFile();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            Progress progress = new Progress(core);
            progress.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            progress.ShowDialog();

        }

        private async void temporaryButton_Click(object sender, RoutedEventArgs e)
        {            
            await core.LoadAllPath();
        }
    }
}
