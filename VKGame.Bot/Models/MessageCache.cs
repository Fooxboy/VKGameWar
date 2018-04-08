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
        public long Ts { get; }
        public string Body { get;}
        public long PeerId { get;}
        public string Time { get;}
        public long Type { get;}
        public long FromType { get;}
        
    }
}
