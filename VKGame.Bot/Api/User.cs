﻿
using System;
using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        private Database.Data DB = null;
        private long id;
        
        public User(long id)
        {
            DB = new Database.Data("Users");
            this.id = id;
        }

        /// <summary>
        /// Список всех пользователей
        /// </summary>
        public static List<long> AllList
        {
            get
            {
                var DB = new Database.Data("Users");
                var r = DB.GetAll();
                var ids = new List<long>();
                while (r.Read())
                {
                    ids.Add((long)r["Id"]);
                }
                return ids;
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="userId">ид</param>
        public static void Registration(long userId)
        {
            string name = String.Empty;
            try
            {
                name = Common.GetVk().Users.Get(new List<long> {userId})[0].FirstName;
            }
            catch
            {
                name = "Командир";
            }
            
            List<string> fields = new List<string>() {"Id", "Name"};
            var values = new List<string>() {userId.ToString(), name};         
            Database.Data.Add(fields, values, "Users");
        }

        /// <summary>
        /// Проверка на существоввание в бд
        /// </summary>
        /// <param name="userId"> ид пользователя</param>
        /// <returns></returns>
        public static bool Check(long userId) => Database.Data.CheckFromId(userId, "Users");
        
        public long Id => id;

        /// <summary>
        /// Имя
        /// </summary>
        public string Name
        {
            get
            {
                const string defaultValue = "Командир";
                var result = DB.GetFromId(id, "Name");
                if (result is DBNull) return defaultValue;
                return (string) result;
            }
            set => DB.EditFromId(id, "Name", value);
        }
        
        /// <summary>
        /// Уровень
        /// </summary>
        public long Level
        {
            get
            {
                const long defaultValue = 1;
                var result = DB.GetFromId(id, "Level");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Level", value);
        }

        /// <summary>
        /// Подарки
        /// </summary>
        public List<long> Gifts
        {
            get
            {
                var result = (string)DB.GetFromId(id, "Gifts");
                var list = new List<long>();
                if (result == "")
                    return list;
                string[] arrayResult = result.Split(',');
                foreach (var ids in arrayResult)
                    list.Add(Int64.Parse(ids));
                return list;
            }
            set
            {
                List<long> gifts = value;
                string memberStr = string.Empty;
                foreach (var gift in gifts) memberStr += $"{gift},";
                memberStr = memberStr.Remove(memberStr.Length - 1);
                DB.EditFromId(id, "Gifts", memberStr);
            }
        }
        

        /// <summary>
        /// Индентификтор битвы в которой учавствует пользователь
        /// </summary>
        public long BattleId
        {
            get
            {
                const long defaultValue = 0;
                var result = DB.GetFromId(id, "BattleId");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "BattleId", value);
        }
        
        /// <summary>
        /// Опыт пользователя
        /// </summary>
        public long Experience
        {
            get
            {
                const long defaultValue = 0;
                var result = DB.GetFromId(id, "Experience");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Experience", value);
        }
        

        /// <summary>
        /// Индентификатор клана
        /// </summary>
        public long Clan
        {
            get
            {
                const long defaultValue = 0;
                var result = DB.GetFromId(id, "Clan");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Clan", value);
        }
        

        /// <summary>
        /// Индитификатор соревнования
        /// </summary>
        public long Competition
        {
            get
            {
                const long defaultValue = 0;
                var result = DB.GetFromId(id, "Competition");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Competition", value);
        }
        
        /// <summary>
        /// Права или доступ
        /// </summary>
        public long Access
        {
            get
            {
                const long defaultValue = 0;
                var result = DB.GetFromId(id, "Access");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Access", value);
        }


        /// <summary>
        /// Индентификаторы друзей
        /// </summary>
        public List<long> Friends
        {
            get
            {
                 List<long> defaultValue = new List<long>();
                var result = DB.GetFromId(id, "Friends");
                if (result is DBNull) return defaultValue;
                if(result == "")
                    return defaultValue;
                var membersString = (string)result;
                string[] arrayResult = membersString.Split(',');
                foreach (var ids in arrayResult)
                    defaultValue.Add(Int64.Parse(ids));
                return defaultValue;
            }
            set
            {
                List<long> friends = value;
                string memberStr = string.Empty;
                foreach (var friend in friends) memberStr += $"{friend},";
                memberStr = memberStr.Remove(memberStr.Length - 1);
                DB.EditFromId(id, "Friends", memberStr);
            }
        }
        

        /// <summary>
        /// Индендификатор квеста
        /// </summary>
        public long Quest
        {
            get
            {
                const long defaultValue = 0;
                var result = DB.GetFromId(id, "Quest");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Quest", value);
        }
    }
}