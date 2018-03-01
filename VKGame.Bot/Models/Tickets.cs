using System.Collections.Generic;

namespace VKGame.Bot.Models
{
    /// <summary>
    /// Модель для взаимодействия со списком билетов.
    /// </summary>
    public class Tickets
    {
        public List<Ticket> List {get;set;}

        public class Ticket 
        {
            public long User {get;set;}
            public string Number {get;set;}
        }
        
    }
}