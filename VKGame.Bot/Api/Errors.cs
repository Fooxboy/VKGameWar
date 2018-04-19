using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Api
{
    public class Errors
    {
        private Database.Stat DB = null;

        public Errors()
        {
            DB = new Database.Stat("Bugs");
        }

        public static void Add(Exception e)
        {
            Database.Stat.Add(new List<string> { "Exception", "Text", "Date", "StackTrace" },
                new List<string>() { e.ToString(), e.Message, DateTime.Now.ToString(), e.StackTrace }, "Errors");
        }
    }
}
