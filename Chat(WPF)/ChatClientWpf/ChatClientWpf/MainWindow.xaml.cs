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
using System.Threading;
using System.Collections.ObjectModel;

namespace ChatClientWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client chatClient;
     

        public MainWindow()
        {
            InitializeComponent();
            SetName();
            

            Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            chatClient.Disconnect();
        }

        

        private void SetName()
        {
            NameWindow nameWindow = new NameWindow();

            if(nameWindow.ShowDialog() == true)
            {
                chatClient = new Client(nameWindow.GetNewName, "127.0.0.1", 8888, this);

                Thread clientThread = new Thread(chatClient.Connect);
                clientThread.Start();
            }
            else
            {
                Close();
            }
         
        }
    
        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage("default");
        }

        private void sendTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage("default");
            }
        }

        private async void SendMessage(string tag)
        {
            if (sendTextBox.Text.Length > 0)
            {
                string message = tag + "/" + sendTextBox.Text;
                sendTextBox.Clear();

                await chatClient.SendMessage(message);
            }
        }

       
    }
}
