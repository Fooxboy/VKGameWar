using System.Collections.Generic;

namespace VKGame.Bot.Models
{
    /// <summary>
    /// Модель для взаимодействия  с таблом билетов.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Билеты
        /// </summary>
        public List<string> Tickets { get; }
        /// <summary>
        /// Пользователи
        /// </summary>
        public List<long> Users { get; }
        /// <summary>
        /// Выигрыш
        /// </summary>
        public List<long> Price { get; }
    }
}