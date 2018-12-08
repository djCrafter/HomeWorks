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


namespace TextFinder
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

          
        }


        private void GetAllDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo item in allDrives)
            {
                comboBox.Items.Add(item.Name);
            }

            comboBox.SelectedItem = comboBox.Items[0];
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
           textBox1.Text = core.OpenFile();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)

        {
            if (textBox1.Text.Length != 0)
            {
                core = new Core(comboBox.SelectionBoxItem.ToString(), textBox1.Text);
                Progress progress = new Progress(core);
                core.SetProgressWindow(progress);
                progress.WindowStartupLocation = WindowStartupLocation.CenterScreen;              
                progress.ShowDialog();
            }
            else
            {
                MessageBox.Show("Задайте слова для поиска!!!", "WordFinder message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    
    }        
  
}
