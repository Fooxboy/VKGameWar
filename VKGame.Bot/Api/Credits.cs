using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    public class Credits
    {
        private Database.Data DB = null;
        private long id;

        public Credits(long id)
        {
            DB = new Database.Data("Credits");
            this.id = id;
        }

        public static List<long> All
        {
            get
            {
                var DB = new Database.Data("Credits");
                var r = DB.GetAll();
                var ids = new List<long>();
                while (r.Read())
                {
                    ids.Add((long)r["User"]);
                }
                return ids;
            }
        }

        public long Id => id;
        
        public static long New(long userId, long price) 
        {
            var db = new Database.Data("Credits");
            var id = (long)db.GetFromId(0, "Price") +1;
            var fields = new List<string>() { "Id", "Price", "User"};
            var value = new List<string>() { id.ToString(), price.ToString(), userId.ToString()};
            db.EditFromId(0, "Price", id);
            Database.Data.Add(fields, value, "Credits");
            return id;
        }

        public void Delete() => DB.DeleteFromId(this.Id);
        
        public long Price
        {
            get => (long) DB.GetFromId(id, "Price");
            set => DB.EditFromId(id, "Price", value);
        }

        public long Time
        {
            get => (long) DB.GetFromId(id, "Time");
            set => DB.EditFromId(id, "Price", value);
        }

        public long User
        {
            get => (long) DB.GetFromId(id, "User");
            set => DB.EditFromId(id, "User", value);
        }
    }
}