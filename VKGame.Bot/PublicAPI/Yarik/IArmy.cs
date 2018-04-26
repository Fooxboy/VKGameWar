using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public interface IArmy
    {
        int Type { get;}
        int Level { get; set; }
        long Damage { get; }
        bool isOpen { get; set; }
        long TimeCreate { get; }
    }
}
