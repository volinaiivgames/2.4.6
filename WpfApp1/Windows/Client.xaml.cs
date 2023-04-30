using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Windows
{
    public partial class Client : Window
    {
        public Client(Socket socket,string name, string ip)
        {
            InitializeComponent();
            Title = $"{name} подключен к ({ip}) как (Пользователь)"; 
            Name = name; 
            Utils.Inits.Ip = ip; 
            Utils.Inits.Socket = socket;
            Utils.Methods.LoadSmiles(listBoxSmiles);
            RenderSockets();
        }

        private async Task RenderSockets()
        {
            SendData(new Utils.SendData(0, Name, null, "", DateTime.Now));
            while (true)
            {
                try
                {
                    await Utils.Inits.Socket.ReceiveAsync(new ArraySegment<byte>(Utils.Inits.MessageBytes), SocketFlags.None);
                    string message = Encoding.UTF8.GetString(Utils.Inits.MessageBytes);
                    if (message.Length != 0)
                    {
                        Utils.SendData data = JsonConvert.DeserializeObject<Utils.SendData>(message);
                        if (data.Type == 0 || data.Type == 1)
                        {
                            listBoxUsers.Items.Clear();
                            foreach (Utils.User user in data.Users) listBoxUsers.Items.Add(user.Name);
                        }
                        else
                        {
                            listBoxDialog.Items.Add($"[{data.Date}] {data.Name} ▶ {data.Message}");
                        }
                    }
                    Array.Clear(Utils.Inits.MessageBytes, 0, Utils.Inits.MessageBytes.Length);
                }
                catch (Exception) { Close(); break; }
            }
        }

        private async void SendData(Utils.SendData data)
        {
            try
            {
                string data_ = JsonConvert.SerializeObject(data);
                byte[] bytes = Encoding.UTF8.GetBytes(data_);
                await Utils.Inits.Socket.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось отправить запрос на сервер");
            }
        }

        private void textBoxMessage_GotFocus(object sender, RoutedEventArgs e) => Utils.Methods.ValidTextBox((TextBox)sender, "Сообщение", false);
        private void textBoxMessage_LostFocus(object sender, RoutedEventArgs e) => Utils.Methods.ValidTextBox((TextBox)sender, "Сообщение", true);
        private void Exit_Click(object sender, RoutedEventArgs e) => Close();
        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxMessage.Text.Length == 0) return;
            if (!Utils.Methods.IsValidTextBox(textBoxMessage)) return;
            if (textBoxMessage.Text.Equals("/disconect")) Close();
            else SendData(new Utils.SendData(2, Name, null, textBoxMessage.Text, DateTime.Now));
            textBoxMessage.Text = "";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Utils.Inits.Socket != null)
            {
                Utils.Inits.Socket.Close();
                Utils.Inits.Socket = null;
                new MainWindow().Show();
            }
        }

        private void Smiles_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxSmiles.Visibility == Visibility) listBoxSmiles.Visibility = Visibility.Hidden;
            else listBoxSmiles.Visibility = Visibility;
        }
    }
}
