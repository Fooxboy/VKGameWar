using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Army
    {
        public static string Create(int type, int count, long userId)
        {
            return null;
        }

        public static bool Check(long user) => Bot.Database.Public.CheckFromId(user, "Army");

        public static void SetCount(long userId, Models.Count count)
        {
            var db = new Database.Public("Army");
            var stringCounts = JsonConvert.SerializeObject(count);
            db.EditFromId(userId, "CurrentCount", stringCounts);
        }

        public static object GetCount(long userId)
        {
            if (!Check(userId))
                return new Models.Error() { Code = 2, Message = "Пользователя с армией нет в базе данных." };
            var db = new Database.Public("Army");
            var countStr = (string)db.GetFromId(userId, "CurrentCount");
            return JsonConvert.DeserializeObject<Models.Count>(countStr);
        }

        public static object GetCountForType(long userId, int type)
        {
            if (!Check(userId))
                return new Models.Error() { Code = 2, Message = "Пользователя с армией нет в базе данных." };
            var db = new Database.Public("Army");
            var counts = JsonConvert.DeserializeObject<Models.Count>((string)db.GetFromId(userId, "CurrentCount"));
            var value = counts.List.FindAll(u => u.Id == type).FirstOrDefault();
            if (value is null)
                return new Models.Error() { Code = 1, Message = "Неизвестный тип армии." };
            return value;
        }

        public static object GetSummaryDamage(long user, IArmy army)
        {
            return null;
        }

        public static object EditCount(long userId, int type, int count)
        {
            if (!Check(userId))
                return new Models.Error() { Code = 2, Message = "Пользователя с армией нет в базе данных." };
            var db = new Database.Public("Army");
            var counts = JsonConvert.DeserializeObject<Models.Count>((string)db.GetFromId(userId, "CurrentCount"));
            var value = counts.List.FindAll(u => u.Id == type).FirstOrDefault();
            if (value is null)
                return new Models.Error() { Code = 1, Message = "Неизвестный тип армии." };
            counts.List.Remove(value);

            value.Count = count;

            SetCount(userId, counts);
            return true;
        }
    }
}
