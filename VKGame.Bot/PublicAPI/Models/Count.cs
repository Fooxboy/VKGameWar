using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class Count
    {
        public List<SpecificCount> List { get; set; }
    }

    public class SpecificCount
    {
        public int Id { get; set; }
        public int Count { get; set; }
    }
}
