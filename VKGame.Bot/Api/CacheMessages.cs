using System.Collections.Generic;
using VKGame.Bot.Models;

namespace VKGame.Bot.Api
{
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


        public static void ResetCache() => Database.Stat.DeteleAll("CacheMessages");

        public static void AddMessage(long id, string time, long fromType, long Type, long userId, string Text, long peerId)
        {
            var fields = new List<string>() {"Id", "Time", "FromType", "Type", "FromId", "Body", "PeerId" };
            var values = new  List<string>(){id.ToString(), time, fromType.ToString(), Type.ToString(), userId.ToString(), Text, peerId.ToString()};
            Database.Stat.Add(fields, values, "CacheMessages");
        }

    }
}