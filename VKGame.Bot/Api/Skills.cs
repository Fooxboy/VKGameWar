﻿using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Навыки пользователя
    /// </summary>
    public class Skills
    {
        private Database.Data DB = null;
        private long id;
        
        public Skills(long id)
        {
            DB = new Database.Data("Skills");
            this.id = id;
        }

        public long Id => id;

        public static void Registration(long userId)
        {
            var fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};
            Database.Data.Add(fields, values, "Skills");

        }

        /// <summary>
        /// Удача
        /// </summary>
        public long Fortuna
        {
            get => (long) DB.GetFromId(id, "Fortuna");
            set => DB.EditFromId(id, "Fortuna", value);
        }
       
    }
}