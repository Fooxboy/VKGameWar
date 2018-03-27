using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class RootBotsLongPollVK
    {
        public long ts { get; set; }
        public List<Update> updates { get; set; }

        public class Update
        {
            public string type { get; set; }
            public object @object { get; set; }
            public long group_id { get; set; }
        }
    }
}
