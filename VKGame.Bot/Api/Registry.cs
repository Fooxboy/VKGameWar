using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace VKGame.Bot.Api
{
    public class Registry
    {
        private Database.Data DB = null;
        private long id;

        public Registry(long id)
        {
            DB = new Database.Data("Registry");
            this.id = id;
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
            set => DB.EditFromId(id, "CountCreateBattles", value);
        }

        public string LastMessage
        {
            get => (string) DB.GetFromId(id, "LastMessage");
            set => DB.EditFromId(id, "LastMessage", value);
        }

        public bool StartThread
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "StartThread"));
            set => DB.EditFromId(id, "StartThread", value);
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

        public bool IsSetup
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isSetup"));
            set => DB.EditFromId(id, "isSetup", value);
        }

        public bool IsHelp
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isHelp"));
            set => DB.EditFromId(id, "isHelp", value);
        }

        public bool IsReferal
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isReferal"));
            set => DB.EditFromId(id, "isReferal", value);
        }

        public bool IsBonusInGroupJoin
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isBonusInGroupJoin"));
            set => DB.EditFromId(id, "isBonusInGroupJoin", value);
        }

        public bool IsLeaveIsGroup
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isLeaveIsGroup"));
            set => DB.EditFromId(id, "isLeaveIsGroup", value);
        }

        public bool IsTodaySendMessageLast
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "isTodaySendMessageLast"));
            set => DB.EditFromId(id, "isTodaySendMessageLast", value);
        }

        public bool ShowNotifyBoostArmy
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "ShowNotifyBoostArmy"));
            set => DB.EditFromId(id, "ShowNotifyBoostArmy", value);
        }

        public bool ShowNotifyBoostResources
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "ShowNotifyBoostResources"));
            set => DB.EditFromId(id, "ShowNotifyBoostResources", value);
        } 
        
        public bool ActivedBoostFood
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "ActivedBoostFood"));
            set => DB.EditFromId(id, "ActivedBoostFood", value);
        } 
        
        public bool ActivedBoostWater
        {
            get => Convert.ToBoolean((long) DB.GetFromId(id, "ActivedBoostWater"));
            set => DB.EditFromId(id, "ActivedBoostWater", value);
        } 
    }
}