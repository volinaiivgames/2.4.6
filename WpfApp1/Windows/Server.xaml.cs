using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Windows
{
    public partial class Server : Window
    {
        public Server(Socket socket, string name, string ip)
        {
            InitializeComponent();
            Title = $"{name} подключен к ({ip}) как (Админ)";
            Utils.Inits.Name = name;
            Utils.Inits.Ip = ip;
            Utils.Inits.Socket = socket;
            EditUsers(Utils.Inits.Socket, Utils.Inits.Name);
            Utils.Methods.LoadSmiles(listBoxSmiles);
            RenderSockets();
        }

        private void UdateListUsers()
        {
            listBoxUsers.Items.Clear();
            foreach (Utils.User user_ in Utils.Inits.Users)
            {
                listBoxUsers.Items.Add(user_.Name);
            }
        }

        private void EditUsers(Socket socket, string name)
        {
            Utils.Inits.Users.Add(new Utils.User(socket, name));
            UdateListUsers();
        }

        private void DeleteUserList(Socket socket)
        {
            Utils.User user = Utils.Inits.Users.Find(e => e.Socket == socket);
            if(user != null)
            {
                Utils.SendData data = new Utils.SendData(1, user.Name, Utils.Inits.Users, "", DateTime.Now);
                string newMessage = Utils.SendData.GetDataMessage(data);
                Utils.Inits.Users.Remove(user);
                user.Socket.Close();

                listBoxLog.Items.Add(newMessage);
                SendDataAll(data);
            }
            UdateListUsers();
        }

        private async Task RenderSockets()
        {
            while (true)
            {
                try
                {
                    Socket socket = await Utils.Inits.Socket.AcceptAsync();
                    if (socket?.Connected == true) RenderSocketData(socket);
                }
                catch (Exception) { Close(); break; }
            }
        }

        private async void RenderSocketData(Socket user)
        {
            while (true)
            {
                try
                {
                    await user.ReceiveAsync(new ArraySegment<byte>(Utils.Inits.MessageBytes), SocketFlags.None);
                    string message = Encoding.UTF8.GetString(Utils.Inits.MessageBytes);
                    if (message.Length == 0) continue;
                    Utils.SendData data = JsonConvert.DeserializeObject<Utils.SendData>(message);
                    if (data == null) continue;
                    string newMessage = Utils.SendData.GetDataMessage(data);
                    switch (data.Type)
                    {
                        case 0:
                            EditUsers(user, data.Name);
                            listBoxLog.Items.Add(newMessage);
                            SendDataAll(data);
                            break;
                        case 2:
                            SendDataAll(data);
                            listBoxDialog.Items.Add(newMessage);
                            break;
                        default:break;
                    }
                    Array.Clear(Utils.Inits.MessageBytes, 0, Utils.Inits.MessageBytes.Length);
                }
                catch (Exception)
                {
                    DeleteUserList(user);
                    break;
                }
            }
        }

        private async void SendDataAll(Utils.SendData data)
        {
            if (data.Users == null) data.Users = Utils.Inits.Users;
            foreach (var user in data.Users)
            {
                if (user.Socket == Utils.Inits.Socket) continue;
                byte[] dataByte = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                await user.Socket.SendAsync(new ArraySegment<byte>(dataByte), SocketFlags.None);
            }
        }

        private void InviteDataMessage(Utils.SendData data)
        {
            string newMessage = Utils.SendData.GetDataMessage(data);
            listBoxDialog.Items.Add(newMessage);
            SendDataAll(data);
        }

        private void textBoxMessage_GotFocus(object sender, RoutedEventArgs e) => Utils.Methods.ValidTextBox((TextBox)sender, "Сообщение", false);
        private void textBoxMessage_LostFocus(object sender, RoutedEventArgs e) => Utils.Methods.ValidTextBox((TextBox)sender, "Сообщение", true);
        private void Exit_Click(object sender, RoutedEventArgs e) => Close();
        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxUsers.Visibility == Visibility)
            {
                listBoxLog.Visibility = Visibility;
                listBoxUsers.Visibility = Visibility.Hidden;
                logsButton.Content = "Посмотреть пользователей чата";
            }
            else
            {
                listBoxUsers.Visibility = Visibility;
                listBoxLog.Visibility = Visibility.Hidden;
                logsButton.Content = "Посмотреть логи чата";
            }
        }
        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxMessage.Text.Length == 0) return;
            if (!Utils.Methods.IsValidTextBox(textBoxMessage)) return;
            if (textBoxMessage.Text.Equals("/disconect")) Close();
            else InviteDataMessage(new Utils.SendData(2, Name, Utils.Inits.Users, textBoxMessage.Text, DateTime.Now));
            textBoxMessage.Text = "";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var user in Utils.Inits.Users) user.Socket.Close();
            Utils.Inits.Users = new List<Utils.User>();
            Utils.Inits.Socket = null;
            new MainWindow().Show();
        }

        private void Smiles_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxSmiles.Visibility == Visibility) listBoxSmiles.Visibility = Visibility.Hidden;
            else listBoxSmiles.Visibility = Visibility;
        }
    }
}
