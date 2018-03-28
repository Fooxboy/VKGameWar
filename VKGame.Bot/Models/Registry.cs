using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class Registry
    {
        public long Id { get; set; }
        public long CountMessage { get; set; }
        public long CountBattles { get; set; }
        public long CountWinBattles { get; set; }
        public long CountCreateBattles { get; set; }
        public string LastMessage { get; set; }
        public bool StartThread { get; set; }
        public long Credit { get; set; }
        public string DateReg { get; set; }
        public bool isSetup { get; set; }
        public bool isHelp { get; set; }
        public bool isReferal { get; set; }
        public bool isBonusInGroupJoin { get; set; }
        public bool isLeaveIsGroup { get; set; }
    }
}
