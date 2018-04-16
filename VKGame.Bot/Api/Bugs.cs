using System.Collections.Generic;
using System.Data;
using VkNet.Model;
using System;

namespace VKGame.Bot.Api
{
    public class Bugs
    {
        private Database.Stat DB = null;
        private long id;
        
        public Bugs(long id)
        {
            DB = new Database.Stat("Bugs");
            this.id = id;
        }

        public long Id => id;

        public string Time => (string) DB.GetFromId(id, "Time");

        public long Status
        {
            get => (long) DB.GetFromId(id, "Status");
            set => DB.EditFromId(id, "Status", value);
        }

        public string Text => (string) DB.GetFromId(id, "Text");

        public long User => (long) DB.GetFromId(id, "User");

        public static List<long> AllList
        {
            get
            {
                var list = new List<long>();
                var reader = new Database.Stat("Bugs").GetAll();
                while (reader.Read())
                {
                    list.Add((long)reader["Id"]);
                }

                return list;
            }
        }

        public static List<long> BugsFromUser(long userId)
        {
            var list = new List<long>();
            var reader = new Database.Stat("Bugs").GetAll();
            while (reader.Read())
            {
                if((long)reader["User"] == userId) list.Add((long)reader["Id"]);
            }

            return list;
        }

        public static long Add(string text, long user)
        {
            var fields = new List<string>(){"Id", "Time", "Status", "Text", "User"};
            var values = new List<string>() {text.GetHashCode().ToString(), DateTime.Now.ToString(), "0", text, user.ToString()};
            Database.Stat.Add(fields, values, "Bugs");
            return text.GetHashCode();
        }
    }
}