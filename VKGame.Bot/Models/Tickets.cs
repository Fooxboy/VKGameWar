using System.Collections.Generic;

namespace VKGame.Bot.Models
{
    /// <summary>
    /// Модель для взаимодействия со списком билетов.
    /// </summary>
    public class Tickets
    {
        /// <summary>
        /// Список билетов
        /// </summary>
        public List<string> ListTickets { get; set; }
        /// <summary>
        /// Пользователи
        /// </summary>
        public List<long> Users { get; set; }
        
    }
}