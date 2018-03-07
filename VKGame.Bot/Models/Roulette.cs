using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class Roulette
    {
        public List<RoulettePrices> Prices { get; set; }
        public long Fund { get; set; }
    }

    public class RoulettePrices
    {
        public long User { get; set; }
        public long Price { get; set; }
        public string Smile { get; set; }
    }
}
