using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class User
    {
        public long Id { get; set; }
        public int Money { get; set; }
        public int Protection { get; set; }
        public Units Units { get; set; }
    }
}
