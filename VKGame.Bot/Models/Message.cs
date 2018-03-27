using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class Message
    {
        public long id { get; set; }
        public long user_id { get; set; }
        public long from_id { get; set; }
        public long date { get; set; }
        public long read_state { get; set; }
        public long @out { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public object geo { get; set; }
        public List<object> attachments { get; set; }
        public List<Message> fwd_messages { get; set; }
        public long emoji { get; set; }
        public long important { get; set; }
        public long deleted { get; set; }
        public long random_id { get; set; }
        public long chat_id { get; set; }
        public List<long> chat_active { get; set; }
        public object push_settings { get; set; }
        public long users_count { get; set; }
        public long admin_id { get; set; }
        public string action { get; set; }
        public long action_mid { get; set; }
        public string action_email { get; set; }
        public string action_text { get; set; }
        public string photo_50 { get; set; }
        public string photo_100 { get; set; }
        public string photo_200 { get; set; }
    }
}
