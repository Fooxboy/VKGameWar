using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public interface IArmy
    {
        string Name { get;}
        Unit Type { get;}
        int Level { get; set; }
        long Damage { get; }
        bool isOpen { get; set; }
        long TimeCreate { get; }
    }

    public enum Unit
    {
        Soildery =1,
        a=2,
        b=3,
        c=4,
        d
    }
}
