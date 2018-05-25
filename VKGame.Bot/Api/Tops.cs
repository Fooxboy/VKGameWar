using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Топ
    /// </summary>
    class Tops
    {
        private Database.Data DB = null;

        public Tops()
        {
            DB = new Database.Data("Tops");
        }

        /// <summary>
        /// Топ пользователей
        /// </summary>
        public List<long> Users
        {
            get
            {
                var stringUsers = (string)DB.GetFromKey("Users");
                var array = stringUsers.Split(',');
                var users = new List<long>();
                foreach(var user in array)
                    users.Add(Int64.Parse(user));

                return users;
            }
            set
            {
                var stringUsers = String.Empty;
                foreach (var user in value)
                    stringUsers += $"{user},";
                stringUsers.Remove(stringUsers.Length - 1);

                DB.EditFromKey("Users", stringUsers);
            }
        }

        /// <summary>
        /// Топ кланов
        /// </summary>
        public List<long> Clans
        {
            get
            {
                var stringClans = (string)DB.GetFromKey("Clans");
                var array = stringClans.Split(',');
                var users = new List<long>();
                foreach (var user in array)
                    users.Add(Int64.Parse(user));

                return users;
            }
            set
            {
                var stringClans = String.Empty;
                foreach (var user in value)
                    stringClans += $"{user},";
                stringClans.Remove(stringClans.Length - 1);

                DB.EditFromKey("Users", stringClans);
            }
        }

        /// <summary>
        /// Время последнего обновления
        /// </summary>
        public string DateUpdate
        {
            get => (string)DB.GetFromKey("LastUpdate");
            set => DB.EditFromKey("LastUpdate", value);
        }
    }
}
