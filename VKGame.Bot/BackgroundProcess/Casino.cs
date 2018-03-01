using System.IO;
using System.Threading;
using System;

namespace VKGame.Bot.BackgroundProcess
{
    /// <summary>
    /// Фоновые процессы для Казино.
    /// </summary>
    public class Casino
    {
        /// <summary>
        /// Вызывает метод конца игры.
        /// </summary>
        public static void TimerTriggerEndGame(object ticketObject)
        {
            Thread.Sleep(300000);
            Models.Tickets.Ticket ticket = (Models.Tickets.Ticket) ticketObject;
            long[] price = { 5, 10, 40, 50, 70, 100, 150, 200, 300, 350, 400, 600, 800, 1000 };
            var r = new Random();
                var resources = new Api.Resources(ticket.User);
                var moneyUser = resources.MoneyCard;
                var priceInt = r.Next(0, price.Length - 1);
                moneyUser += price[priceInt];
                resources.MoneyCard = moneyUser;
                Api.MessageSend($"✨ Денежный перевод! На Ваш банковский счёт было зачислено {price[priceInt]} 💳 от КАЗИНО \"ИСПЫТАЙ УДАЧУ\". ", ticket.User);

        }
    }
}