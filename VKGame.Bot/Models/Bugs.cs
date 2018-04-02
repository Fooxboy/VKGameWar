using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class Bugs
    {
        public List<Bug> bugs { get; set; }
    }

    public class Bug
    {
        public long Id { get; set; }
        public string Time { get; set; }
        public long Status { get; set; }
        public string Text { get; set; }
        public long User { get; set; }
    }
}
