using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateWindow1(bool admin)
        {
            if (admin)
            {
                try
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Bind(new IPEndPoint(IPAddress.Parse(textBoxIp.Text), 25565));
                    socket.Listen(1000);
                    new Windows.Server(socket, textBoxName.Text, textBoxIp.Text).Show();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось создать чат");
                    return;
                }
            }
            else
            {
                try
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.ConnectAsync(textBoxIp.Text, 25565);
                    new Windows.Client(socket, textBoxName.Text, textBoxIp.Text).Show();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось подключится");
                }
            }
            Close();
        }

        private void textBoxName_GotFocus(object sender, RoutedEventArgs e) => Utils.Methods.ValidTextBox((TextBox)sender, "Введите имя", false);

        private void textBoxName_LostFocus(object sender, RoutedEventArgs e) => Utils.Methods.ValidTextBox((TextBox)sender, "Введите имя", true);

        private void textBoxIp_GotFocus(object sender, RoutedEventArgs e) => Utils.Methods.ValidTextBox((TextBox)sender, "Введите ip чата", false);

        private void textBoxIp_LostFocus(object sender, RoutedEventArgs e) => Utils.Methods.ValidTextBox((TextBox)sender, "Введите ip чата", true);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Utils.Methods.VerifyName(textBoxName.Text) && Utils.Methods.VerifyIP(textBoxIp.Text)) CreateWindow1(false);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Utils.Methods.VerifyName(textBoxName.Text) && Utils.Methods.VerifyIP(textBoxIp.Text)) CreateWindow1(true);
        }
    }
}
