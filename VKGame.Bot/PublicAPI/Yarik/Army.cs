using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Army
    {

        public static object ConvertToId(IArmy army)
        {
            return army.Type;
        }

        public static IArmy ConvertToArmy(int id)
        {
            var army = (Models.Units)Users.GetArmy(id);
            var armys = army.Army.FindAll(u => u.Type == id);
            var type = armys.FirstOrDefault();
            if (type is null)
                return null;
            return type;
            
        }

        public static object CreateCompete(long userId)
        {
            var model = (Models.CreateDBModel)GetObjectCreate(userId);
            var count = (int)GetCountForType(userId, model.Type);
            count += model.Count;
            EditCount(userId, model.Type, count);

            return true;
        }

        public static object GetObjectCreate(long userId)
        {
            var db = new Database.Public("Army");
            var valueS = (string)db.GetFromId(userId, "Create");
            if(valueS == "")
                return new Models.Error() { Code = 2, Message = "Пользователь не имеет армии, которая сейчас обучается." };
            var model = JsonConvert.DeserializeObject<Models.CreateDBModel>(valueS);
           
            return model;
        }

        public static object Create(long userId, IArmy type, int count)
        {
            if (!Check(userId))
                return new Models.Error() { Code = 2, Message = "Пользователя с армией нет в базе данных." };
            var price = type.TimeCreate * count;
            var stroge = (int)Barracks.GetCount(userId, type.Type);
            if(stroge < ((Models.SpecificCount)GetCountForType(userId, type.Type)).Count + count)
                return new Models.Error() { Code = 21, Message = "У пользователя нет места для новых солдат." };
            var time = type.TimeCreate * count;
            return time;
        }

        public static bool Check(long user) => Bot.Database.Public.CheckFromId(user, "Army");

        public static void SetCount(long userId, Models.Count count)
        {
            var db = new Database.Public("Army");
            var stringCounts = JsonConvert.SerializeObject(count);
            db.EditFromId(userId, "CurrentCount", stringCounts);
        }

        public static object ResetCount(long userId)
        {
            var model = new Models.Count();
            var list = new List<Models.SpecificCount>();
            list.Add(new Models.SpecificCount() { Id = 1, Count = 0 });
            list.Add(new Models.SpecificCount() { Id = 2, Count = 0 });
            list.Add(new Models.SpecificCount() { Id = 3, Count = 0 });
            list.Add(new Models.SpecificCount() { Id = 4, Count = 0 });
            list.Add(new Models.SpecificCount() { Id = 5, Count = 0 });
            model.List = list;
            SetCount(userId, model);
            return true;
        }

        public static object GetCount(long userId)
        {
            if (!Check(userId))
                return new Models.Error() { Code = 2, Message = "Пользователя с армией нет в базе данных." };
            var db = new Database.Public("Army");
            var countStr = (string)db.GetFromId(userId, "CurrentCount");
            return JsonConvert.DeserializeObject<Models.Count>(countStr);
        }

        public static object SetPrice(long userId, int price)
        {
            var db = new Database.Public("Army");
            db.EditFromId(userId, "Price", price);
            return true;
        }

        public static object GetPrice(long userId)
        {
            var db = new Database.Public("Army");
            return (long)db.GetFromId(userId, "Price");
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
            long hp = 0;
            hp += army.Damage;
            return hp;
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

            counts.List.Add(value);
            SetCount(userId, counts);
            return true;
        }
    }
}
