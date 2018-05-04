using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class IClan
    {
        public virtual string Id { get; }
        public virtual long Found { get; set; }
        public virtual int Level { get; set; }
        public virtual int CountMembers { get; set; }
        public virtual bool IsSearchBattle { get; }
        public virtual bool IsStartBattle { get; }
        public virtual string BattleId { get; set; }
        public virtual string Name { get; }
        public virtual List<long> Members { get; }
    }
}
