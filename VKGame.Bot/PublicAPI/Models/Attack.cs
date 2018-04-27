using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class Attack
    {
        public int Status { get; set; }

        public int Damage { get; set; }

        public int HpEnemy { get; set; }

        public int IdBarrack { get; set; }

        public int Money { get; set; }
    }
}
