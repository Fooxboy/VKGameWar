using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class Error : IError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
