﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik.Units
{
    public class UnitFour :IArmy
    {
        public int Type => 4;
        public int Level { get; set; }
        public long Damage => Level + 1;
        public bool isOpen { get; set; }
        public long TimeCreate => 1000;
    }
}
