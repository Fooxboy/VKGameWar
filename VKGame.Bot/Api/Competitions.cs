using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VKGame.Bot.Models;

namespace VKGame.Bot.Api
{
    public class Competitions
    {
        private Database.Data DB = null;
        private long id;

        public Competitions(long id)
        {
            DB = new Database.Data("Competitions");
            this.id = id;
        }

        public long Id => id;

        public static List<long> AllList
        {
            get
            {
                var DB = new Database.Data("Competitions");
                var r = DB.GetAll();
                var ids = new List<long>();
                while (r.Read())
                {
                    ids.Add((long)r["Id"]);
                }

                return ids;
            }
        }

        public string Name
        {
            get
            {
                string defaultValue = "Соревнование";
                var result = DB.GetFromId(id, "Name");
                if (result is DBNull) return defaultValue;
                return (string) result;
            }
            set => DB.EditFromId(id, "Name", value);
        }

        public List<Models.CompetitionsList.Member> Members
        {
            get
            {
                var result = DB.GetFromId(id, "Members");
                if (result is DBNull) return new List<CompetitionsList.Member>();

                var membersStr = (string) result;


                if (membersStr == "[]")
                {
                    var members = new List<Models.CompetitionsList.Member>();
                    return members;
                }
                else
                {
                    string[] membersArray = membersStr.Split(',');
                    var members = new List<Models.CompetitionsList.Member>();
                    foreach (var member in membersArray)
                        members.Add(JsonConvert.DeserializeObject<Models.CompetitionsList.Member>(member));
                    return members;
                }

            }
            set
            {
                var members = value;
                string memberStr = "";
                foreach (var member in members) memberStr += $"{JsonConvert.SerializeObject(member)},";
                memberStr = memberStr.Remove(memberStr.Length - 1);
                DB.EditFromId(id, "Members", memberStr);
            }
        }

        public long Price
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Price");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Price", value);
        }

        public long Time
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Time");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Time", value);
        }

        public List<Models.CompetitionsList.TopMember> Top
        {
            get
            {
                var result = DB.GetFromId(id, "Top");
                if (result is DBNull) return new List<Models.CompetitionsList.TopMember>();

                var membersStr = (string) result;

                if (membersStr == "[]")
                {
                    var members = new List<Models.CompetitionsList.TopMember>();
                    return members;
                }
                else
                {
                    string[] membersArray = membersStr.Split(',');
                    var members = new List<Models.CompetitionsList.TopMember>();
                    foreach (var member in membersArray)
                        members.Add(JsonConvert.DeserializeObject<Models.CompetitionsList.TopMember>(member));
                    return members;
                }
            }
            set
            {
                var members = value;
                string memberStr = "";
                foreach (var member in members) memberStr += $"{JsonConvert.SerializeObject(member)},";
                memberStr = memberStr.Remove(memberStr.Length - 1);
                DB.EditFromId(id, "Top", memberStr);
            }
        }

        public bool IsEnd
        {
            get => Convert.ToBoolean((long)DB.GetFromId(id, "IsEnd"));
            set => DB.EditFromId(id, "IsEnd", Convert.ToInt64(value));
        }
        
        public bool FreeBattle
        {
            get => Convert.ToBoolean((long)DB.GetFromId(id, "FreeBattle"));
            set => DB.EditFromId(id, "FreeBattle", Convert.ToInt64(value));
        }

        public static bool Check(long id) => Database.Data.CheckFromId(id, "Competitions");
        
        public static long New(string Name, long Price, long Time)
        {
            var db = new Database.Data("Competitions");
            var members = new List<Models.CompetitionsList.Member>();
            var membersJson = JsonConvert.SerializeObject(members);
            var top = new List<Models.CompetitionsList.TopMember>();
            var topJson = JsonConvert.SerializeObject(top);
            var id = (long)db.GetFromId(0, "Price") + 1;
            var fields = new List<string>() {"Id", "Name", "Members", "Price", "Time", "Top" };
            var value = new List<string>() { id.ToString(), Name, membersJson, Price.ToString(), Time.ToString(), topJson };
            db.EditFromId(0, "Price", id);
            Database.Data.Add(fields, value, "Competitions");
            return id;
        }
    }
}
