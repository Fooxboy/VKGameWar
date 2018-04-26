﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik.Units
{
    public class UnitThree: IArmy
    {
        public int Type => 3;
        public int Level { get; set; }
        public long Damage => Level + 1;
        public bool isOpen { get; set; }
        public long TimeCreate => 1000;
    }
}
