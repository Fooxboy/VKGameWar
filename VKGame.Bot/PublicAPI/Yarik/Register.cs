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
                },
                new Models.SpecificCount()
                {
                    Id= 2,
                    Count= 0
                },
                new Models.SpecificCount()
                {
                    Id= 3,
                    Count= 0
                },
                new Models.SpecificCount()
                {
                    Id= 4,
                    Count= 0
                },
                new Models.SpecificCount()
                {
                    Id= 5,
                    Count= 0
                }
            }
            };

            var fields = new List<string>() {"Id", "CurrentCount" };
            var values = new List<string>() { userId.ToString(), JsonConvert.SerializeObject(countObject)};

            Database.Public.Add(fields, values, "Army");
            return true;
        }

        public static object Barracks(long id)
        {
            var fields = new List<string> { "Id", "One", "Two", "Three", "Four", "Five" };
            var values = new List<string> { id.ToString(), "1", "0", "0", "0", "0" };
            Database.Public.Add(fields, values, "Barracks");
            return true;
        }

        public static object User(long id)
        {
            if(Users.Check(id))
                return new Models.Error() { Code = 3, Message = "Пользователь уже зарегестрирован." };

            var objectArmy = new Models.Units() {
                Army = new List<IArmy>()
                {
                    new Units.UnitOne()
                    {
                        Level = 1,
                        isOpen = true
                    },
                    new Units.UnitTwo()
                    {
                         Level = 1,
                        isOpen = false
                    },
                    new Units.UnitThree()
                    {
                         Level = 1,
                        isOpen = false
                    },
                    new Units.UnitFour()
                    {
                         Level = 1,
                        isOpen = false
                    },
                    new Units.UnitFive()
                    {
                         Level = 1,
                        isOpen = false
                    }
                }
            };
            var fields = new List<string> { "Id", "Units"};
            var values = new List<string> { id.ToString(), JsonConvert.SerializeObject(objectArmy)};

            Database.Public.Add(fields, values, "Users");

            Barracks(id);
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
