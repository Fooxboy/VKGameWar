using System;
using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Уровни
    /// </summary>
    public class Levels
    {
        private Database.Data DB = null;
        private long id;
        
        public Levels(long id)
        {
            DB = new Database.Data("Levels");
            this.id = id;
        }

        public long Id => id;

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="userId">ид пользователя</param>
        public static void Registration(long userId)
        {
            var fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};
            Database.Data.Add(fields, values, "Levels");

        }


        public long Soldiery
        {
            get => (long) DB.GetFromId(id, "Soldiery");
            set => DB.EditFromId(id, "Soldiery", value);
        }
        
        public long Tanks
        {
            get => (long) DB.GetFromId(id, "Tanks");
            set => DB.EditFromId(id, "Tanks", value);
        }
        
        public long Apartments
        {
            get
            {
                long defaultValue = 2;
                var result = DB.GetFromId(id, "Apartments");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Apartments", value);
        }
        
        public long PowerGenerators
        {
            get
            {
                long defaultValue = 1;
                var result = DB.GetFromId(id, "PowerGenerators");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "PowerGenerators", value);
        }
        
        public long Mine
        {
            get
            {
                long defaultValue = 1;
                var result = DB.GetFromId(id, "Mine");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Mine", value);
        }
        
        
        public long WaterPressureStation
        {
            get
            {
                long defaultValue = 1;
                var result = DB.GetFromId(id, "WaterPressureStation");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "WaterPressureStation", value);
        }
        
        public long Eatery
        {
            get
            {
                long defaultValue = 1;
                var result = DB.GetFromId(id, "Eatery");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Eatery", value);
        }
        
        public long WarehouseEnergy
        {
            get
            {
                long defaultValue = 1;
                var result = DB.GetFromId(id, "WarehouseEnergy");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "WarehouseEnergy", value);
        }
        
        
        public long WarehouseWater
        {
            get
            {
                long defaultValue = 1;
                var result = DB.GetFromId(id, "WarehouseWater");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "WarehouseWater", value);
        }
        
        public long WarehouseEat
        {
            get
            {
                long defaultValue = 1;
                var result = DB.GetFromId(id, "WarehouseEat");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "WarehouseEat", value);
        }
        
        public long Hangars
        {
            get
            {
                long defaultValue = 1;
                var result = DB.GetFromId(id, "Hangars");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Hangars", value);
        }
        
    }
}