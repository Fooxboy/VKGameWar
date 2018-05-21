using System.Collections.Generic;
using VKGame.Bot.Models;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Кэширование сообщений в базу данных
    /// </summary>
    public class CacheMessages : Models.MessageCache
    {
        private Database.Stat DB = null;
        private long id;

        public CacheMessages(long id)
        {
            DB = new Database.Stat("CacheMessages");
            this.id = id;
        }

        public long Ts => id;

        public string Time => (string) DB.GetFromId(id, "Time");

        public long FromType => (long) DB.GetFromId(id, "FromType");

        public long Type => (long) DB.GetFromId(id, "From");

        public long FromId => (long) DB.GetFromId(id, "FromId");

        public string Body => (string) DB.GetFromId(id, "Body");

        public long PeerId => (long) DB.GetFromId(id, "PeerId");

        //сбросить кэш
        public static void ResetCache() => Database.Stat.DeteleAll("CacheMessages");

        /// <summary>
        /// Добавить новое сообщение
        /// </summary>
        /// <param name="id">ид сообщения</param>
        /// <param name="time">время</param>
        /// <param name="fromType">от кого тип</param>
        /// <param name="Type">тип</param>
        /// <param name="userId">пользоваетль</param>
        /// <param name="Text">текст</param>
        /// <param name="peerId">пир иди</param>
        public static void AddMessage(long id, string time, long fromType, long Type, long userId, string Text, long peerId)
        {
            var fields = new List<string>() {"Id", "Time", "FromType", "Type", "FromId", "Body", "PeerId" };
            var values = new  List<string>(){id.ToString(), time, fromType.ToString(), Type.ToString(), userId.ToString(), Text, peerId.ToString()};
            Database.Stat.Add(fields, values, "CacheMessages");
        }

    }
}