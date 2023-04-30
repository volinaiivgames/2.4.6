using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace WpfApp1.Utils
{
    class Inits
    {
        public static byte[] MessageBytes = new byte[1024];
        public static string Name = "";
        public static string Ip = "";
        public static Socket Socket;
        public static List<User> Users = new List<User>();
        public static List<string> Smiles = new List<string>()
        {
            "😀", "😃", "😄", "😁", "😆", "😅", "🤣", "😂", "🙂", "🙃", "😉", "😊", "😇", "🥰", "😍", "🤩", "😘", "😗", "😚", "😙",
        };
    }
}
