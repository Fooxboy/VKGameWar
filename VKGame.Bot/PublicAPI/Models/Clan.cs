using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Models
{
    public class Clan
    {
        string id = string.Empty;

        public Clan(string clan)
        {
            id = clan;
        }

        public string Id => id;

        public long Found
        {
            get => (long)Yarik.Clans.GetFound(id);
            set => Yarik.Clans.SetFound(id, value);
        }

        public int CountMembers
        {
            get => Convert.ToInt32((long)Yarik.Clans.GetCountMembers(id));
            set => Yarik.Clans.SetCountMembers(id, value);
        }

        public bool IsSearchBattle
        {
            get => Convert.ToBoolean((long)Yarik.Clans.GetIsSearchBattle(id));
        }

        public bool isStartBattle
        {
            get => Convert.ToBoolean((long)Yarik.Clans.GetIsSearchBattle(id));
        }

        public int Level
        {
            get => Convert.ToInt32((long)Yarik.Clans.Level(id));
            set => Yarik.Clans.SetLevel(id, value);
        }
    }
}
