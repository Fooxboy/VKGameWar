using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Бустеры или УСИЛИТЕЛИ
    /// </summary>
    public class Boosters
    {
        private Database.Data DB = null;
        private long id;
        
        /// <summary>
        /// Публичный конструктор
        /// </summary>
        /// <param name="id"> ид пользователя</param>
        public Boosters(long id)
        {
            DB = new Database.Data("Boosters");
            this.id = id;
        }

        /// <summary>
        /// ПОлучение ид пользователя
        /// </summary>
        public long Id => id;

        /// <summary>
        /// Регистрация бустеров
        /// </summary>
        /// <param name="userId"></param>
        public static void Register(long userId)
        {
            List<string> fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};         
            Database.Data.Add(fields, values, "Boosters");
        }
        
        /// <summary>
        /// Количество создаваемой еды
        /// </summary>
        public long CreateFood
        {
            get => (long) DB.GetFromId(id, "CreateFood");
            set => DB.EditFromId(id, "CreateFood", value);
        }

        /// <summary>
        /// Кол-во создаваемой воды
        /// </summary>
        public long CreateWater
        {
            get => (long) DB.GetFromId(id, "CreateWater");
            set => DB.EditFromId(id, "CreateWater", value);
        }
        
        /// <summary>
        /// Кол-во создаваемых солдат
        /// </summary>
        public long CreateSoldiery
        {
            get => (long) DB.GetFromId(id, "CreateSoldiery");
            set => DB.EditFromId(id, "CreateSoldiery", value);
        }
        
        /// <summary>
        /// Кол-во создаваемых танков
        /// </summary>
        public long CreateTanks
        {
            get => (long) DB.GetFromId(id, "CreateTanks");
            set => DB.EditFromId(id, "CreateTanks", value);
        }
        
        
    }
}