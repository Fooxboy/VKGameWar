using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class Clan : IClan
    {
        string id = string.Empty;

        public Clan(string clan)
        {
            id = clan;
        }

        public override string Id => id;

        public override string BattleId
        {
            get => (string)Yarik.Clans.GetBattleId(id);
            set => Yarik.Clans.SetBattleId(id, value);
        }
        
        public override string Name
        {
            get => (string)Yarik.Clans.GetName(id);
        }

        public override List<long> Members
        {
            get => (List<long>)Yarik.Clans.GetMembers(id);
        }

        public override long Found
        {
            get => (long)Yarik.Clans.GetFound(id);
            set => Yarik.Clans.SetFound(id, value);
        }

        public override int CountMembers
        {
            get => Convert.ToInt32((long)Yarik.Clans.GetCountMembers(id));
            set => Yarik.Clans.SetCountMembers(id, value);
        }

        public override bool IsSearchBattle
        {
            get => Convert.ToBoolean((long)Yarik.Clans.GetIsSearchBattle(id));
        }

        public override bool IsStartBattle
        {
            get => Convert.ToBoolean((long)Yarik.Clans.GetIsSearchBattle(id));
        }

        public override int Level
        {
            get => Convert.ToInt32((long)Yarik.Clans.Level(id));
            set => Yarik.Clans.SetLevel(id, value);
        }
    }
}
