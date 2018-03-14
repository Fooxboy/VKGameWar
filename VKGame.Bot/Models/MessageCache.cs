using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class MessagesCache
    {
        public List<MessageCache> Message { get; set; }
    }

    public class MessageCache
    {
        public ulong Id { get; set; }
        public string Text { get; set; }
        public long PeerId { get; set; }
        public string Time { get; set; }
    }
}
