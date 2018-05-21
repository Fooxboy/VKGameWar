using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Кредиты
    /// </summary>
    public class Credits
    {
        private Database.Data DB = null;
        private long id;

        public Credits(long id)
        {
            DB = new Database.Data("Credits");
            this.id = id;
        }

        /// <summary>
        /// Получние всех кредитов
        /// </summary>
        public static List<long> All
        {
            get
            {
                var DB = new Database.Data("Credits");
                var r = DB.GetAll();
                var ids = new List<long>();
                while (r.Read())
                {
                    ids.Add((long)r["User"]);
                }
                return ids;
            }
        }

        public long Id => id;
        
        /// <summary>
        /// Создание нового
        /// </summary>
        /// <param name="userId">ид пользователя</param>
        /// <param name="price">цена</param>
        /// <returns></returns>
        public static long New(long userId, long price) 
        {
            var db = new Database.Data("Credits");
            var id = (long)db.GetFromId(0, "Price") +1;
            var fields = new List<string>() { "Id", "Price", "User"};
            var value = new List<string>() { id.ToString(), price.ToString(), userId.ToString()};
            db.EditFromId(0, "Price", id);
            Database.Data.Add(fields, value, "Credits");
            return id;
        }

        /// <summary>
        /// Удаление
        /// </summary>
        public void Delete() => DB.DeleteFromId(this.Id);
        
        /// <summary>
        /// Цена
        /// </summary>
        public long Price
        {
            get => (long) DB.GetFromId(id, "Price");
            set => DB.EditFromId(id, "Price", value);
        }

        /// <summary>
        /// Время
        /// </summary>
        public long Time
        {
            get => (long) DB.GetFromId(id, "Time");
            set => DB.EditFromId(id, "Price", value);
        }

        /// <summary>
        /// Пользователь
        /// </summary>
        public long User
        {
            get => (long) DB.GetFromId(id, "User");
            set => DB.EditFromId(id, "User", value);
        }
    }
}