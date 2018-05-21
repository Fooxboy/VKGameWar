using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Настрорйки бустеров
    /// </summary>
    public class ConfigBoosters
    {
        private Database.Data DB = null;
        private long id;
        
        public ConfigBoosters(long id)
        {
            DB = new Database.Data("ConfigBoosters");
            this.id = id;
        }

        public long Id => id;

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="userId">ид пользователя</param>
        public static void Register(long userId)
        {
            List<string> fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};         
            Database.Data.Add(fields, values, "ConfigBoosters");
        }
        
        /// <summary>
        /// еда
        /// </summary>
        public long CreateFood
        {
            get => (long) DB.GetFromId(id, "CreateFood");
            set => DB.EditFromId(id, "CreateFood", value);
        }

        /// <summary>
        /// вода
        /// </summary>
        public long CreateWater
        {
            get => (long) DB.GetFromId(id, "CreateWater");
            set => DB.EditFromId(id, "CreateWater", value);
        }
        
        /// <summary>
        /// солажды
        /// </summary>
        public long CreateSoldiery
        {
            get => (long) DB.GetFromId(id, "CreateSoldiery");
            set => DB.EditFromId(id, "CreateSoldiery", value);
        }
        
        /// <summary>
        /// танки
        /// </summary>
        public long CreateTanks
        {
            get => (long) DB.GetFromId(id, "CreateTanks");
            set => DB.EditFromId(id, "CreateTanks", value);
        }
    }
}