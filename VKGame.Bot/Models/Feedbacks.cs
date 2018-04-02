using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class Feedbacks
    {
        public List<FeedBack> Feedback { get; set; }
    }

    public class FeedBack
    {
        public string Time { get; set; }
        public long UserId { get; set; }
        public string Text { get; set; }
    }
}
