using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik.Units
{
    public class Soildery : IArmy
    {
        public string Name => "Солдат";
        public Unit Type => Unit.Soildery;
        public int Level { get; set; }
        public long Damage => Yarik.Damage.Soildery + (Yarik.Damage.SoilderyLevel * Level);
        public bool isOpen { get; set; }
        public long TimeCreate => 1000;
    }
}
