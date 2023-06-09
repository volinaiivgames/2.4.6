﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Utils
{
    class User
    {
        [JsonIgnore]
        public Socket Socket { get; set; }
        public string Name { get; set; }
        
        public User(Socket socket, string name)
        {
            Socket = socket;
            Name = name;
        }
    }
}
