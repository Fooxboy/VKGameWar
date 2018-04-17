using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Api
{
    class Tops
    {
        private Database.Data DB = null;

        public Tops()
        {
            DB = new Database.Data("Battles");
        }

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

        public string DateUpdate
        {
            get => (string)DB.GetFromKey("LastUpdate");
            set => DB.EditFromKey("LastUpdate", value);
        }
    }
}
