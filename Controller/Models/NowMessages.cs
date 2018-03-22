using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Models
{
    public class NowMessages
    {
        public List<Message> Updates { get; set; }
        public ulong Ts { get; set; }
    }

    public class Message
    {
        public ulong Id { get; set; }
        public string Text { get; set; }
        public long PeerId { get; set; }
        public string Time { get; set; }
    }
}
