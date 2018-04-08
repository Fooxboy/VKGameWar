using System;
using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    public class Resources
    {
        private Database.Data DB = null;
        private long id;
        
        public Resources(long id)
        {
            DB = new Database.Data("Resources");
            this.id = id;
        }


        public static void Registration(long userId)
        {
            List<string> fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};         
            Database.Data.Add(fields, values, "Resources");
        }
        
        
        public long Id => id;

        public long Food
        {
            get
            {
                long defaultValue = 100;
                var result = DB.GetFromId(id, "Food");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Food", value);
        }
        
        public long Money
        {
            get
            {
                long defaultValue = 100;
                var result = DB.GetFromId(id, "Money");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Money", value);
        }
        
        public long MoneyCard
        {
            get
            {
                long defaultValue = 100;
                var result = DB.GetFromId(id, "MoneyCard");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "MoneyCard", value);
        }
        
        public long Energy
        {
            get
            {
                long defaultValue = 100;
                var result = DB.GetFromId(id, "Energy");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Energy", value);
        }
        
        public long Water
        {
            get
            {
                long defaultValue = 100;
                var result = DB.GetFromId(id, "Water");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Water", value);
        }

        public long Soldiery
        {
            get
            {
                long defaultValue = 15;
                var result = DB.GetFromId(id, "Soldiery");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Soldiery", value);
        }

        public long Tanks
        {
            get
            {
                long defaultValue = 5;
                var result = DB.GetFromId(id, "Tanks");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Tanks", value);
        }

        public long TicketsCompetitions
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "TicketsCompetitions");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "TicketsCompetitions", value);
        }
    }
}