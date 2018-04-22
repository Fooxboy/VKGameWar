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

            var result = (long)db.GetFromKey("Id", clan, "BattleId");
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

            var result = (long)db.GetFromKey("Id", clan, "Name");
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
