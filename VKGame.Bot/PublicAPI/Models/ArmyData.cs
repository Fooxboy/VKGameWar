using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class ArmyData :Yarik.IArmy
    {
        public int Type { get; }
        public int Level { get; set; }
        public long Damage { get; }
        public bool isOpen { get; set; }
        public long TimeCreate { get; }
        public int Price { get; }
    }
}
