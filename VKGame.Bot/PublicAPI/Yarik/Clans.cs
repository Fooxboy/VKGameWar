using System;
using System.Collections.Generic;
using System.Text;
using VKGame.Bot.PublicAPI.Models;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Clans
    {
        public static object GetFound(string clan)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            var result = (long)db.GetFromKey("Id", clan, "Found");
            return result;
        }

        public static object EndBattle(string clan)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };
            SetBattleId(clan, "0");
            var members = (List<long>)GetMembers(clan);
            foreach(var member in members)
            {
                Users.SetBattleId(member, "0");
            }

            return true;
        }

        public static object SetBattleId(string clan, string value)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            db.EditFromKey("Id", clan, "BattleId", value);
            return true;
        }

        public static object GetMembers(string clan)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };
            var db = new Database.Public("Clans");

            var objMembers = db.GetFromKey("Id", clan, "Level");

            if(objMembers is long)
            {
                List<long> members = new List<long>();
                members.Add((long)objMembers);
                return members;
            }else
            {
                var strMembers = (string)objMembers;

                string[] membersArray = strMembers.Split(',');
                List<long> members = new List<long>();
                foreach (var member in membersArray) members.Add(Int64.Parse(member));
                return members;
            }



            
        }

        public static object AddMember(string clan, long user)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var members = (List<long>)GetMembers(clan);
            members.Add(user);
            SetMembers(clan, members);
            return true;
        }

        public static object Remove(string clan, long user)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var members = (List<long>)GetMembers(clan);
            members.Remove(user);
            SetMembers(clan, members);
            return true;
        }

        public static object SetMembers(string clan, List<long> members)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            string memberStr = string.Empty;
            foreach (var member in members)
                memberStr += $"{member},";

            memberStr = memberStr.Remove(memberStr.Length - 1);
            db.EditFromKey("Id", clan, "Members", memberStr);

            return true;
        }

        public static object SetFound(string clan, long found)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            db.EditFromKey("Id", clan, "Found", found);
            return true;
        }

        public static bool Check(string clan)
        {
            return Database.Public.CheckFromKey("Id", clan, "Clans");
        }

        public static object Level(string clan)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            return (long) db.GetFromKey("Id", clan, "Level");
        }

        public static object SetIsSearchBattle(string clan, int value)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");
            db.EditFromKey("Id", clan, "isSearch", value);
            return true;
        }

        public static object SetCountMembers(string clan, int count)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");
            db.EditFromKey("Id", clan, "CountMembers", count);
            return true;
        }

        public static object GetCountMembers(string clan)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            return (long)db.GetFromKey("Id", clan, "Level");
        }

        public static List<string> AllClans()
        {
            var db = new Database.Public("Clans");
            var list = new List<string>();
            var reader = db.GetAll();
            while(reader.Read())
            {
                list.Add((string)reader["Id"]);
            }
            return list;
        }

        public static object GetIsSearchBattle(string clan)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            var result = (long)db.GetFromKey("Id", clan, "isSearchBattle");
            return result;
        }

        public static object SetLevel(string clan, int level)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            db.EditFromKey("Id", clan, "Level", level);
            return true;
        }

        public static object GetIsStartBattle(string clan)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            var result = (long)db.GetFromKey("Id", clan, "isStartBattle");
            return result;
        }

        public static object GetBattleId(string clan)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            var result = (string)db.GetFromKey("Id", clan, "BattleId");
            return result;
        }

        public static object GetName(string clan)
        {
            if (!Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var db = new Database.Public("Clans");

            var result = (string)db.GetFromKey("Id", clan, "Name");
            return result;
        }

        public static List<string> GetClansIsSearchBattle()
        {
            var db = new Database.Public("Clans");
            var list = new List<string>();
            var reader = db.GetAll();
            while (reader.Read())
            {
                if((long)reader["isSearchBattle"] == 1)
                    list.Add((string)reader["Id"]);

            }
            return list;
        }
    }
}
