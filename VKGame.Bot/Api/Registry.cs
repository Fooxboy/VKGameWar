using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Реестр пользователя
    /// </summary>
    public class Registry
    {
        private Database.Data DB = null;
        private long id;

        public Registry(long Id)
        {
            DB = new Database.Data("Registry");
            id = Id;
        }

        public static void Register(long userId)
        {
            var fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};
            Database.Data.Add(fields, values, "Registry");
        }

        public long Id => id;

        public long CountMessage
        {
            get => (long) DB.GetFromId(id, "CountMessage");
            set => DB.EditFromId(id, "CountMessage", value);
        }

        public long CountBattles
        {
            get => (long) DB.GetFromId(id, "CountBattles");
            set => DB.EditFromId(id, "CountBattles", value);
        }

        public long CountWinBattles
        {
            get => (long) DB.GetFromId(id, "CountWinBattles");
            set => DB.EditFromId(id, "CountWinBattles", value);
        }

        public long CountCreateBattles
        {
            get => (long) DB.GetFromId(id, "CountCreateBattles");
            set => DB.EditFromId(id, "CountCreateBattles", Convert.ToInt64(value));
        }

        public string LastMessage
        {
            get => (string) DB.GetFromId(id, "LastMessage");
            set => DB.EditFromId(id, "LastMessage", value);
        }

        /// <summary>
        /// Проверка на запущенность потока с добавлением ресурсов
        /// </summary>
        public bool StartThread
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "StartThread"));
            set => DB.EditFromId(id, "StartThread", Convert.ToInt64(value));
        }

        public long Credit
        {
            get => (long) DB.GetFromId(id, "Credit");
            set => DB.EditFromId(id, "Credit", value);
        }

        public string DateReg
        {
            get => (string) DB.GetFromId(id, "DateReg");
            set => DB.EditFromId(id, "DateReg", value);
        }

        /// <summary>
        /// Прошёл ли пользователь первоначальную  настройку
        /// </summary>
        public bool IsSetup
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isSetup"));
            set => DB.EditFromId(id, "isSetup", Convert.ToInt64(value));
        }

        public bool IsHelp
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isHelp"));
            set => DB.EditFromId(id, "isHelp", Convert.ToInt64(value));
        }

        public bool IsReferal
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isReferal"));
            set => DB.EditFromId(id, "isReferal", Convert.ToInt64(value));
        }

        public bool IsBonusInGroupJoin
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isBonusInGroupJoin"));
            set => DB.EditFromId(id, "isBonusInGroupJoin", Convert.ToInt64(value));
        }

        public bool IsLeaveIsGroup
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isLeaveIsGroup"));
            set => DB.EditFromId(id, "isLeaveIsGroup", Convert.ToInt64(value));
        }

        public bool IsTodaySendMessageLast
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isTodaySendMessageLast"));
            set => DB.EditFromId(id, "isTodaySendMessageLast", Convert.ToInt64(value));
        }

        public bool ShowNotifyBoostArmy
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "ShowNotifyBoostArmy"));
            set => DB.EditFromId(id, "ShowNotifyBoostArmy", Convert.ToInt64(value));
        }

        public bool ShowNotifyBoostResources
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "ShowNotifyBoostResources"));
            set => DB.EditFromId(id, "ShowNotifyBoostResources", Convert.ToInt64(value));
        } 
        
        public bool ActivedBoostFood
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "ActivedBoostFood"));
            set => DB.EditFromId(id, "ActivedBoostFood", Convert.ToInt64(value));
        } 
        
        public bool ActivedBoostWater
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "ActivedBoostWater"));
            set => DB.EditFromId(id, "ActivedBoostWater", Convert.ToInt64(value));
        }

        public long CountLoserBattle
        {
            get => (long) DB.GetFromId(id, "CountLoserBattle");
            set => DB.EditFromId(id, "CountLoserBattle", value);
        }
    }
}