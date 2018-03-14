using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Server
{
    public class Connection
    {
        public class User
        {
            public List<Connection> Users { get; set; }
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public object Access { get; set; }
        public int id { get; set; }
    }
}
