﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Созадние объектов битв
    /// </summary>
    public class Battles
    {
        private Database.Data DB = null;
        private long id;
        
        //Публичный конструктор
        public Battles(long id)
        {
            DB = new Database.Data("Battles");
            this.id = id;
        }

        public long Id => id;

        public string Name => (string) DB.GetFromId(id, "Body");

        /// <summary>
        /// Проверка на активность
        /// </summary>
        public bool IsActive
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isActive"));
            set => DB.EditFromId(id, "isActive", Convert.ToInt64(value));
        }
        
        /// <summary>
        /// Проверка на страт битвы
        /// </summary>
        public bool IsStart
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isPlay"));
            set => DB.EditFromId(id, "isPlay", Convert.ToInt64(value));
        }
        
        /// <summary>
        /// Получает фонд 
        /// </summary>
        public long Found
        {
            get => (long) DB.GetFromId(id, "Found");
            set => DB.EditFromId(id, "Found", value);
        }

        /// <summary>
        /// Добавление нового участника 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="hp"></param>
        public void AddMember(long userId, long hp)
        {
            string membersStr = (string) DB.GetFromId(id, "Members");
            string hpStr = (string) DB.GetFromId(id, "HpMembers");

            membersStr += $",{userId}";
            hpStr += $",{hp}";
            DB.EditFromId(id, "Members", membersStr);
            DB.EditFromId(id, "HpMembers", hpStr);
        }

        //Types:
        //1 - обычная битва
        //2 - соревнование
        //3 - бой с ботом.
        public long Type
        {
            get
            {
                const long defaultValue = 1;
                var result = DB.GetFromId(id, "Type");
                if (result is DBNull) return defaultValue;
                return (long)result;
            }
            set => DB.EditFromId(id, "Type", value);
        }

        /// <summary>
        /// Проверка на существование
        /// </summary>
        /// <param name="id">индентификатор битвы</param>
        /// <returns></returns>
        public static bool Check(long id) => Database.Data.CheckFromId(id, "Battles");

        /// <summary>
        /// Создание битвы
        /// </summary>
        /// <param name="name">имя битвы</param>
        /// <param name="user">пользователь, который создал</param>
        /// <param name="price">цена или ставка</param>
        /// <param name="hp">хп пользователя</param>
        /// <returns> индентификатор битвы</returns>
        public static long Create(string name, long user, long price, long hp)
        {
            var fields = new List<string>() { "Id", "Body", "Members", "HpMembers", "Found", "Creator"};
            var DB = new Database.Data("Battles");
            long battleId = ((long)DB.GetFromId(0, "Found")) + 1;
            var values = new List<string>() { battleId.ToString(), name, user.ToString(), hp.ToString(), price.ToString(), user.ToString()};
            DB.EditFromId(0, "Found", battleId);
            Database.Data.Add(fields, values, "Battles");
            return battleId; 
        }

        /// <summary>
        /// получение активных битв
        /// </summary>
        public static List<long> GetActivesAll
        {
            get
            {
                var DB = new Database.Data("Battles");
                var rider = DB.GetAll();
                var battles = new List<long>();
                while (rider.Read())
                {
                    
                    var battle = new Api.Battles((long)rider["Id"]);

                    if (battle.IsActive && !battle.IsStart)
                        battles.Add(battle.Id);

                }
                rider.Close();
                return battles;
            }
        }

        /// <summary>
        /// Получение создателя
        /// </summary>
        public long Creator => (long)DB.GetFromId(id, "Creator");

        /// <summary>
        /// Получение атакующего пользователя
        /// </summary>
        public long UserAttack
        {
            get => (long) DB.GetFromId(id, "UserAttack");
            set => DB.EditFromId(id, "UserAttack", value);
        }

        /// <summary>
        /// ПОлучение участников
        /// </summary>
        public Dictionary<long, long> Members
        {
            get
            {
                string membersStr = (string) DB.GetFromId(id, "Members");
                string hpStr = (string) DB.GetFromId(id, "HpMembers");
                var members = new Dictionary<long,long>();
                var membersArray = membersStr.Split(',');
                var hpArray = hpStr.Split(',');
                Logger.NewMessage(membersStr + "а " + hpStr);
                Logger.NewMessage(membersArray[0] + "а " + hpArray[0]);
                Logger.NewMessage(membersArray.Length + "а " + hpArray.Length);

                for (var i = 0; i < membersArray.Length; i++)
                    members.Add(Convert.ToInt64(membersArray[i]), Convert.ToInt64(hpArray[i]));

                return members;
            }
        }

        /// <summary>
        /// Установка хп пользователю
        /// </summary>
        /// <param name="userId"> индентификатор пользолвателя</param>
        /// <param name="hp">хп</param>
        public void SetHp(long userId, long hp)
        {
            string membersStr = (string) DB.GetFromId(id, "Members");
            string hpStr = (string) DB.GetFromId(id, "HpMembers");
            var membersArray = membersStr.Split(',');
            var hpArray = hpStr.Split(',');
            for (var i = 0; i < membersArray.Length; i++)
            {
                if (Convert.ToInt64(membersArray[i]) == userId)
                    hpArray[i] = hp.ToString();
            }

            hpStr = hpArray.Aggregate(String.Empty, (current, hps) => current + $"{hps},");

            hpStr.Remove(hpStr.Length - 1);
            DB.EditFromId(id, "HpMembers", hpStr);
        }
    }
}