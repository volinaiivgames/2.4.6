using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Utils
{
    class SendData
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public SendData(int type, string name, List<User> users, string message, DateTime date)
        {
            Type = type;
            Name = name;
            Users = users;
            Message = message;
            Date = date;
        }

        public static string GetDataMessage(SendData data)
        {
            string message = "";
            switch (data.Type)
            {
                case 0:
                    message = $"[{data.Date}] {data.Name} подключился";
                    break;
                case 1:
                    message = $"[{data.Date}] {data.Name} отключился";
                    break;
                case 2:
                    message = $"[{data.Date}] {data.Name} ▶ {data.Message}";
                    break;
                default: break;
            }
            return message;
        }
    }
}
