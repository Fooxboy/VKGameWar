using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VKGame.WebAPI.Areas.publicAPI.Models
{
    public class Error : VKGame.Bot.PublicAPI.Models.IError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
