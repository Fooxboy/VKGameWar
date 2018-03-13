using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class CompetitionsList
    {
        public List<long> List { get; set; }

        public class Member
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public long WinBattles { get; set; }
        }

        public class TopMember : Member
        {
            public long Top { get; set; }
        }
    }
}
