using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Users
    {
        public static object Money(long userId)
        {
            if (!Check(userId))
                return new Models.Error() { Code = 12, Message = "Этот пользователь не зарегестирован." };
            var db = new Database.Public("Users");
            return (long)db.GetFromId(userId, "Money");
        }

        public static object SetMoney(long userId, int count)
        {
            if (!Check(userId))
                return new Models.Error() { Code = 12, Message = "Этот пользователь не зарегестирован." };
            var db = new Database.Public("Users");
            db.EditFromId(userId, "Money", count);
            return true;
        }

        public static object GetArmy(long userId)
        {
            if (!Check(userId))
                return new Models.Error() { Code = 12, Message = "Этот пользователь не зарегестирован." };

            var db = new Database.Public("Users");
            return (string)db.GetFromId(userId, "Units");
        }

        public static object EditUnit(long user, int type, int level = -1)
        {
            if (!Check(user))
                return new Models.Error() { Code = 2, Message = "Пользователя с армией нет в базе данных." };
            var db = new Database.Public("Army");
            var armys = JsonConvert.DeserializeObject<Models.Units>((string)db.GetFromId(user, "Units"));
            var value = armys.Army.FindAll(u => (int)u.Type == type).FirstOrDefault();
            if (value is null)
                return new Models.Error() { Code = 1, Message = "Неизвестный тип армии." };
            armys.Army.Remove(value);
           

            if (level != -1) value.Level = level;

            armys.Army.Add(value);
            SetArmy(user, armys);
            return true;
        }

        public static object EditStatusUnit(long user, int type, bool status)
        {
            if (!Check(user))
                return new Models.Error() { Code = 2, Message = "Пользователя с армией нет в базе данных." };
            var db = new Database.Public("Army");
            var armys = JsonConvert.DeserializeObject<Models.Units>((string)db.GetFromId(user, "Units"));
            var value = armys.Army.FindAll(u => (int)u.Type == type).FirstOrDefault();
            armys.Army.Remove(value);
            if (value is null)
                return new Models.Error() { Code = 1, Message = "Неизвестный тип армии." };

            value.isOpen = status;

            armys.Army.Add(value);
            SetArmy(user, armys);
            return true;
        }

        public static void SetArmy(long userId, Models.Units units)
        {
            var db = new Database.Public("Users");
            var stringUnits = JsonConvert.SerializeObject(units);
            db.EditFromId(userId, "Units", stringUnits);
        }

        public static bool Check(long user) => Database.Public.CheckFromId(user, "Users");
    }
}
