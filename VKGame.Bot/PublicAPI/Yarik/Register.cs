using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Registration
    {

        public static object Army(long userId)
        {
            if (Yarik.Army.Check(userId)) return new Models.Error() { Code = 3, Message = "Пользователь уже зарегестрирован." };

            var countObject = new Models.Count()
            { @List = new List<Models.SpecificCount>()
            {
                new Models.SpecificCount()
                {
                    Id= 1,
                    Count= 0
                }
            }
            };

            var fields = new List<string>() {"Id", "CurrentCount" };
            var values = new List<string>() { userId.ToString(), JsonConvert.SerializeObject(countObject)};

            Database.Public.Add(fields, values, "Army");
            return true;
        }

        public static object User(long id)
        {
            if(Users.Check(id))
                return new Models.Error() { Code = 3, Message = "Пользователь уже зарегестрирован." };

            var objectArmy = new Models.Units() {
                Army = new List<IArmy>()
                {
                    new Units.Soildery()
                    {
                        Level = 1,
                        isOpen = true
                    }
                }
            };
            var fields = new List<string> { "Id", "Units"};
            var values = new List<string> { id.ToString(), JsonConvert.SerializeObject(objectArmy)};

            Database.Public.Add(fields, values, "Users");

            Army(id);

            return true;
        }

        public static object Clan(string id, string name, long user)
        {
            if(Clans.Check(id))
                return new Models.Error() { Code = 11, Message = "Этот клан уже зарегестрирован." };
            Database.Public.Add(new List<string>() { "Id", "Name", "Members" }, new List<string>() { id, name, user.ToString()}, "Clans");

            return true;
        }
    }
}
