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
using System.IO;

namespace WordsFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Core core;
        
        public MainWindow()
        {
            InitializeComponent();
            GetAllDrives();

            core = new Core(comboBox.SelectedItem.ToString());
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = core.OpenFile();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            core = new Core(comboBox.SelectedItem.ToString());

            if (textBox1.Text.Length != 0)
            {
                core.Words = textBox1.Text;
                Progress progress = new Progress(core);
                progress.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                progress.ShowDialog();
            }
            else
            {
                MessageBox.Show("Задайте слова для поиска!!!", "WordFinder message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private async void temporaryButton_Click(object sender, RoutedEventArgs e)
        {            
            await core.LoadAllPath();
        }

        private void GetAllDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach(DriveInfo item in allDrives)
            {
                comboBox.Items.Add(item.Name);
            }

            comboBox.SelectedItem = comboBox.Items[0];
        }
    }
}
