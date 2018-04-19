using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class Army
    {
        public class Current
        {
            public int Soildery { get; set; }
            public int Tanks { get; set; }
            public int LevelSoildery { get; set; }
            public int LevelTanks { get; set; }
        }

        public class Levels
        {
            public int Tanks { get; set; }
            public int Soildery { get; set; }
        }

        public class Count
        {
            public int Tanks { get; set; }
            public int Soildery { get; set; }
        }
    }
}
