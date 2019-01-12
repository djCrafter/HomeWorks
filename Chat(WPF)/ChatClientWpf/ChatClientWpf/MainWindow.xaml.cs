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
            textBox.TextChanged += TextBox_TextChanged;
            listBox.MouseDoubleClick += ListBox_MouseDoubleClick;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            while (obj != null && obj != listBox)
            {
                if (obj.GetType() == typeof(ListBoxItem))
                {
                    ListBoxItem boxItem = (ListBoxItem)obj;
                    ChatUser chatUser = (ChatUser)boxItem.Content;
                    PrivateStringConstructor(chatUser.Name);
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox.ScrollToEnd();
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

                Title += " {" + nameWindow.GetNewName + '}';

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
            if (sendTextBox.Text.Contains("private/"))
            {
                SendMessage();
            }
            else
            {
                SendMessage("default");
            }
        }

        private void sendTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
       {
            if (e.Key == Key.Enter)
            {
                if (sendTextBox.Text.Contains("private/"))
                {
                    SendMessage();
                }
                else
                {
                    SendMessage("default");
                }
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

        private async void SendMessage()
        {
            if (sendTextBox.Text.Length > 0)
            {
                string message = sendTextBox.Text;
                sendTextBox.Clear();

                await chatClient.SendMessage(message);
            }
        }

        private void PrivateStringConstructor(string name)
        {
            string str = "private/" + name + '/';
            sendTextBox.Text = str;
        }

        private void sendPersonal_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null && sendTextBox.Text.Length > 0)
            {
                ///TO DO:!!!!!   
            }
        }
    }
}
