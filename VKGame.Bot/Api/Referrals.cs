using System;
using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    public class Referrals
    {
        private Database.Data DB = null;
        private long id;
        
        public Referrals(long id)
        {
            DB = new Database.Data("Referrals");
            this.id = id;
        }

        public long Id => id;

        public static void Register(long userId)
        {
            var fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};
            Database.Data.Add(fields, values,"Referrals");
        }

        public List<long> RefList
        {
            get
            {
                string membersStr = (string)DB.GetFromId(id, "Ref");
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
                DB.EditFromId(id, "Ref", memberStr);
            }
        }

        public long MonthCash
        {
            get => (long) DB.GetFromId(id, "MonthCash");
            set => DB.EditFromId(id, "MonthCash", value);
        }
        
        public long SumCash
        {
            get => (long) DB.GetFromId(id, "SumCash");
            set => DB.EditFromId(id, "SumCash", value);
        }
    }
}