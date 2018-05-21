using System.IO;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
           
            try
            {
                Task.Delay(300000);
                TriggerEndGameHelper(ticketObject);
            }
            catch(Exception e)
            {
                Logger.WriteError(e);
                Bot.Statistics.NewError();
            }
        }

        //триггер конца игры и получение деняк
        public static void TriggerEndGameHelper(object ticketObject)
        {
            Models.Tickets.Ticket ticket = (Models.Tickets.Ticket)ticketObject;
            long[] price = { 5, 8, 1, 10, 20, 30, 40, 50, 55, 70, 80, 100, 150, 200, 300 };
            var r = new Random();
            var resources = new Api.Resources(ticket.User);
            var moneyUser = resources.MoneyCard;
            var priceInt = r.Next(0, price.Length - 1);
            moneyUser += price[priceInt];
            resources.MoneyCard = moneyUser;
            Bot.Statistics.WinCasino(price[priceInt]);
            Api.Message.Send($"✨ Денежный перевод! На Ваш банковский счёт было зачислено {price[priceInt]} 💳 от КАЗИНО \"ИСПЫТАЙ УДАЧУ\". ", ticket.User);

        }


        public static void RouletteHelper(object lol)
        {
            var roulette = Bot.Common.Roulette;

            Dictionary<string, string> smiles = new Dictionary<string, string>();
            smiles.Add("❤", "сердце");
            smiles.Add("💥", "взрыв");
            smiles.Add("🍕", "пицца");
            smiles.Add("🌸", "цветок");
            smiles.Add("😀", "лицо");
            string[] smilesList =
            {
                "❤",
                "💥",
                "🍕",
                "🌸",
                "😀"
                };

            var registry = new RegistryBot();

            var r = new Random();
            var i = r.Next(0, smiles.Count - 1);
            var winSmile = smiles[smilesList[i]];
            int countWinners = 0;
            var winersTxt = "\n Победителей нет 😪";

            foreach (var winner in roulette.Prices)
            {
                if (winner.Smile == winSmile) ++countWinners;
            }

            long priceWinner = 0;
            if (countWinners == 0) priceWinner = 0;
            else
                priceWinner = roulette.Fund / countWinners;


            foreach (var price in roulette.Prices)
            {
                if (price.Smile == winSmile)
                {
                    Notifications.EnterPaymentCard(Convert.ToInt32(priceWinner), price.User, "победа в рулетке");

                    var userWin = new Api.User(price.User);
                    winersTxt = "";
                    winersTxt += $"\n😀 {userWin.Name} взял {priceWinner}";
                }
            }

            string winText = $"🎉 Результаты рулетки!🎉" +
                               $"\n" +
                               $"\n➡➡➡➡ ⬇ ⬅⬅⬅⬅" +
                               $"\n{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]} {smilesList[i]} {smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}" +
                               $"\n➡➡➡➡ ⬆ ⬅⬅⬅⬅\n" +
                               $"\n💳 Выигрыш: {priceWinner}" +
                               $"\nСписок победителей: {winersTxt}";
            foreach (var price in roulette.Prices)
            {
                var user = new Api.User(price.User);

                if (price.Smile == winSmile)
                {
                    Api.Message.Send(winText, price.User);
                }
            }
            Bot.Statistics.WinCasino(priceWinner);
            registry.PlayInRulette = false;
            roulette.Fund = 0;
            roulette.Prices = new List<Models.RoulettePrices>();
            Bot.Common.Roulette = roulette;
        }

        /// <summary>
        /// КРутит рулетку и выдаёт выигрыш!
        /// </summary>
        public static void TimerTriggerRoulette()
        {
            try
            {
                Task.Delay(120000);
                RouletteHelper(null);
            }
            catch(Exception e)
            {
                Logger.WriteError(e);
                Bot.Statistics.NewError();
            }
        }
    }
}