using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Квесты
    /// </summary>
    public class Quests
    {
        private Database.Data DB = null;
        private long id;

        public Quests(long id)
        {
            DB = new Database.Data("Quests");
            this.id = id;
        }

        public long Id => id;


        /// <summary>
        /// Пользователи квеста
        /// </summary>
        public Models.Quests.Users Users
        {
            get
            {
                var json = (string)DB.GetFromId(id, "Users");
                Models.Quests.Users model;
                model = json == " " ? new Models.Quests.Users() { List = new List<Models.Quests.User>() } : JsonConvert.DeserializeObject<Models.Quests.Users>(json);
                return model;
            }
            set
            {
                var json = JsonConvert.SerializeObject(value);
                DB.EditFromId(id, "Users", json);
            }
        }

        public string Name => (string) DB.GetFromId(id, "Name");

        public long Price => (long) DB.GetFromId(id, "Price");


        /// <summary>
        /// Доступен ли квест
        /// </summary>
        public bool IsOnline => Convert.ToBoolean((long) DB.GetFromId(id, "isOnline"));
        
        public static bool Check(long id) => Database.Data.CheckFromId(id, "Quests");
    }
}