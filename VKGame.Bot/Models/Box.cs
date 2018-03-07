using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class BattleBox
    {
        private long money = 0;
        private long food = 0;
        private long soldiery = 0;

        public BattleBox()
        {
            var r = new Random();
            money = r.Next(5, 80);
            food = r.Next(0, 20);
            soldiery = r.Next(0, 5);
        }

        public string Name => "Битвенный";
        public long Money => money;
        public long Food => food;
        public long Soldiery => soldiery;
    }


    public class BuildBox
    {
        long count = 0;

        public BuildBox()
        {
            var r = new Random();
            count = r.Next(0, 3);
        }

        public string Name => "Строительный";
        public long Count => count;
    }
}
