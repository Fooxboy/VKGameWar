using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik.Units
{
    public class UnitOne : Models.ArmyData, IArmy
    {
        public override int Type => 1;
        public override int Level { get; set; }
        public override long Damage => Level + 1;
        public override bool isOpen { get; set; }
        public override long TimeCreate => 1000;
        public override int Price => 20 * Level;
    }
}
