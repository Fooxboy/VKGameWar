using System;
using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    //TODO: переписать кланы.
    public class Clans
    {
        private Database.Data DB = null;
        private long id;

        public Clans(long id)
        {
            DB = new Database.Data("Clans");
            this.id = id;
        }

        public long Id => id;


        public static bool Check(long id) => Database.Data.CheckFromId(id, "Clans");

        public string Name
        {
            get => (string) DB.GetFromId(id, "Name");
            set => DB.EditFromId(id, "Name", value);
        }

        public List<long> Members
        {
            get
            {
                string membersStr = (string)DB.GetFromId(id, "Members");
                string[] membersArray = membersStr.Split(',');
                List<long> members = new List<long>();
                foreach (var member in membersArray) members.Add(Int64.Parse(member));
                return members;
            }
            set
            {
                List<long> members = value;
                string memberStr = "";
                foreach (var member in members) memberStr += $"{member},";
                memberStr = memberStr.Remove(memberStr.Length - 1);
                DB.EditFromId(id, "Member", memberStr);
            }
        }

        public List<long> Moders
        {
            get
            {
                string membersStr = (string)DB.GetFromId(id, "Moders");
                string[] membersArray = membersStr.Split(',');
                List<long> members = new List<long>();
                foreach (var member in membersArray) members.Add(Int64.Parse(member));
                return members;
            }
            set
            {
                List<long> members = value;
                string memberStr = "";
                foreach (var member in members) memberStr += $"{member},";
                memberStr = memberStr.Remove(memberStr.Length - 1);
                DB.EditFromId(id, "Moders", memberStr);
            }
        }

        public long Resources => (long)DB.GetFromId(id, "Resources");

        public long Creator => (long) DB.GetFromId(id, "Creator");
        
        public static void Delete(long clanId)
        {
            Database.Methods db = new Database.Methods("Clans");
            db.Delete(clanId);
        }

        public static long New(long userId, string Name)
        {
            Database.Data db = new Database.Data("Clans");
            var id = (long)db.GetFromId(0, "Creator") + 1;
            var fields = new List<string>() {"Id", "Name", "Members", "Creator" };
            var value = new List<string>() {id.ToString(), Name, userId.ToString(), userId.ToString(),};
            db.EditFromId(0, "Creator", id);
            Database.Data.Add(fields, value, "Clans");
            return id;
        }
    }
}