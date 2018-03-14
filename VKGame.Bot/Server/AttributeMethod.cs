using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Server
{
    public class AttributeMethod:  System.Attribute
    {
        public string Name { get; }
        public AttributeMethod(string name)
        {
            Name = name;
        }
    }
}
