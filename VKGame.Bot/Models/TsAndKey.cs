using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class TsAndKey
    {
        public class ResponseModel
        {
            public TsAndKeyModel response { get; set; } 
        }

        public class TsAndKeyModel
        {
            public string key { get; set; }
            public string server { get; set; }
            public long ts { get; set; }
        }
    }
}
