using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public interface IError
    {
        int Code { get; set; }
        string Message { get; set; }
    }
}
