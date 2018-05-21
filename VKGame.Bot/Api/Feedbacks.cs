using System;
using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Отзывы
    /// </summary>
    public class Feedbacks
    {
        private Database.Stat DB = null;
        private long id;
        
        public Feedbacks(long id)
        {
            DB = new Database.Stat("Feedbacks");
            this.id = id;
        }

        public long Id => id;

        public long User => (long) DB.GetFromId(id, "User");

        public string Time => (string) DB.GetFromId(id, "Time");

        public string Text => (string) DB.GetFromId(id, "Text");

        /// <summary>
        /// Добавить отзыв
        /// </summary>
        /// <param name="text">текст</param>
        /// <param name="userId">ид пользователя</param>
        /// <returns></returns>
        public static long Add(string text, long userId)
        {
            var fields = new List<string>() {"Id", "User", "Text", "Time"};
            var values = new List<string>() {text.GetHashCode().ToString(), userId.ToString(), text, DateTime.Now.ToString()};
            Database.Stat.Add(fields, values, "Feedbacks");
            return text.GetHashCode();
        }

        /// <summary>
        /// Получние всех отзывов
        /// </summary>
        public static List<long> Alllist
        {
            get
            {
                var list = new List<long>();
                
                var reader = new Database.Stat("Feedbacks").GetAll();

                while (reader.Read()) 
                    list.Add((long)reader["Id"]);

                return list;
            }
        }

    }
}