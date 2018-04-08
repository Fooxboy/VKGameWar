using System;
using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    public class Boxs
    {
        private Database.Data DB = null;
        private long id;
        
        public Boxs(long id)
        {
            DB = new Database.Data("Boxs");
            this.id = id;
        }

        public long Id => id;


        public static void Register(long userId)
        {
            List<string> fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};         
            Database.Data.Add(fields, values, "Boxs");
        }

        public long Battle
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Battle");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Battle", value);
        }
        
        public long Build
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Build");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Build", value);
        }
        
        public long Vip
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Vip");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Vip", value);
        }
        
        public long Test
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Test");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Test", value);
        }
    }
}