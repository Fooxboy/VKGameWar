using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    public class Boosters
    {
        private Database.Data DB = null;
        private long id;
        
        public Boosters(long id)
        {
            DB = new Database.Data("Boosters");
            this.id = id;
        }

        public long Id => id;


        public static void Register(long userId)
        {
            List<string> fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};         
            Database.Data.Add(fields, values, "Boosters");
        }
        
        public long CreateFood
        {
            get => (long) DB.GetFromId(id, "CreateFood");
            set => DB.EditFromId(id, "CreateFood", value);
        }

        public long CreateWater
        {
            get => (long) DB.GetFromId(id, "CreateWater");
            set => DB.EditFromId(id, "CreateWater", value);
        }
        
        public long CreateSoldiery
        {
            get => (long) DB.GetFromId(id, "CreateSoldiery");
            set => DB.EditFromId(id, "CreateSoldiery", value);
        }
        
        public long CreateTanks
        {
            get => (long) DB.GetFromId(id, "CreateTanks");
            set => DB.EditFromId(id, "CreateTanks", value);
        }
        
        
    }
}