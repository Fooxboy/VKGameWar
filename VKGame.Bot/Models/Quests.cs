using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class Quests
    {
        public class Users
        {
            public List<User> List { get; set; }
        }

        public class User
        {
            public long Id { get; set; }
            public long Progress { get; set; }
            public long Status { get; set; }
        }
    }
}
