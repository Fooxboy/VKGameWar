using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class ChoiseMembers
    {
        public List<ChoiseMember> List { get; set; }
    }

    public class ChoiseMember
    {
        public int Number { get; set; }
        public User User { get; set; }
    }
}
