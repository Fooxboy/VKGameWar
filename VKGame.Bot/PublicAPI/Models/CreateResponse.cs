using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class CreateResponse
    {
        //Возможные статусы:
        // 1 - клан присоеденился в бой с кланом
        // 2 - клан в поиске боя.
        public int Status { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
