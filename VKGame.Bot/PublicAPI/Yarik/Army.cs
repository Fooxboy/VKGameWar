using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Army
    {
        public static string Create(int type, int count, long userId)
        {
            return null;
        }

        public static bool Check(long user) => Bot.Database.Public.CheckFromId(user, "Army");

        public static object EditCount(long userId, int type, int count)
        {
            var db = new Database.Public("Army");
            if(type == 1)
            {
                db.EditFromId(userId, "Soildery", count);
            }else if(type == 2)
            {
                db.EditFromId(userId, "Tanks", count);
            }else
            {
                Models.IError error = new Models.Error()
                {
                    Code = 1,
                    Message = "Неизвестный тип армии."
                };
                return error;
            }
            return true;
        }

        public static object EditLevel(long userId, int type, int level)
        {
            var db = new Database.Public("Army");
            if (type == 1)
            {
                db.EditFromId(userId, "LevelSoildery", level);
            }
            else if (type == 2)
            {
                db.EditFromId(userId, "LevelTanks", level);
            }
            else
            {
                Models.IError error = new Models.Error()
                {
                    Code = 1,
                    Message = "Неизвестный тип армии."
                };
                return error;
            }
            return true;
        } 

        public static object Current(long userId)
        {
            var db = new Database.Public("Army");
            var model = new Models.Army.Current()
            {
                Soildery = Convert.ToInt32((long)db.GetFromId(userId, "Soildery")),
                Tanks = Convert.ToInt32((long)db.GetFromId(userId, "Tanks")),
                LevelSoildery = Convert.ToInt32((long)db.GetFromId(userId, "LevelSoildery")),
                LevelTanks = Convert.ToInt32((long)db.GetFromId(userId, "LevelTanks"))
            };
            return model;
        }

        public static object Levels(long userId)
        {
            var db = new Database.Public("Army");
            var model = new Models.Army.Levels()
            {
                Soildery = Convert.ToInt32((long)db.GetFromId(userId, "LevelSoildery")),
                Tanks = Convert.ToInt32((long)db.GetFromId(userId, "Tanks"))
            };
            return model;
        }

        public static object Count(long userId)
        {
            var db = new Database.Public("Army");

            var model = new Models.Army.Count()
            {
                Soildery = Convert.ToInt32((long)db.GetFromId(userId, "Soildery")),
                Tanks = Convert.ToInt32((long)db.GetFromId(userId, "Tanks"))
            };
            return model;
        }
    }
}
