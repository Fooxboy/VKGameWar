using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Отчеты об ошибках
    /// </summary>
    public class Errors
    {
        private Database.Stat DB = null;

        public Errors()
        {
            DB = new Database.Stat("Errors");
        }

        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="e">экземпляр экзепшена</param>
        public static void Add(Exception e)
        {
            if(e is System.Net.WebException)
            {

            }else
            {
                Database.Stat.Add(new List<string> { "Exception", "Text", "Date", "StackTrace" },
                new List<string>() { e.GetType().Name, e.Message, DateTime.Now.ToString(), e.StackTrace }, "Errors");
            }
            
        }
    }
}
