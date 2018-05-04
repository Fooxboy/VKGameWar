using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class ArmyData :Yarik.IArmy
    {
        public virtual int Type { get; }
        public virtual int Level { get; set; }
        public virtual long Damage { get; }
        public virtual bool isOpen { get; set; }
        public virtual long TimeCreate { get; }
        public virtual int Price { get; }
    }
}
