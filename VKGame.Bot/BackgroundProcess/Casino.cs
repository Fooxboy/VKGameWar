using System.IO;
using System.Threading;
using System;
using System.Collections.Generic;

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
                Thread.Sleep(300000);
                Models.Tickets.Ticket ticket = (Models.Tickets.Ticket)ticketObject;
                long[] price = { 5, 8, 1, 10, 20, 30, 40, 50, 55, 70, 80, 100, 150, 200, 300 };
                var r = new Random();
                var resources = new Api.Resources(ticket.User);
                var moneyUser = resources.MoneyCard;
                var priceInt = r.Next(0, price.Length - 1);
                moneyUser += price[priceInt];
                resources.MoneyCard = moneyUser;
                Bot.Statistics.WinCasino(price[priceInt]);
                Api.MessageSend($"✨ Денежный перевод! На Ваш банковский счёт было зачислено {price[priceInt]} 💳 от КАЗИНО \"ИСПЫТАЙ УДАЧУ\". ", ticket.User);

            }catch(Exception e)
            {
                Logger.WriteError($"{e.Message} \n {e.StackTrace}");
                Bot.Statistics.NewError();

            }


        }

        /// <summary>
        /// КРутит рулетку и выдаёт выигрыш!
        /// </summary>
        public static void TimerTriggerRoulette()
        {
            Thread.Sleep(120000);

            try
            {
                var roulette = Api.Roulette.GetList();

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

               
                var r = new Random();
                var i = r.Next(0, smiles.Count - 1);
                var winSmile = smiles[smilesList[i]];
                int countWinners = 0;
                var winersTxt = " Победителей нет 😪";

                foreach (var winner in roulette.Prices)
                {
                    if (winner.Smile == winSmile)
                    {
                        countWinners += 1;
                    }
                }

                long priceWinner = 0;
                if (countWinners == 0)
                {
                    priceWinner = 0;
                }else
                {
                    priceWinner = roulette.Fund / countWinners;
                }


                foreach (var price in roulette.Prices)
                {
                    if (price.Smile == winSmile)
                    {
                        Notifications.EnterPaymentCard(Convert.ToInt32(priceWinner), price.User, "победа в рулетке");

                        var userWin = Api.User.GetUser(price.User);
                        winersTxt = "";
                        winersTxt += $"\n😀 {userWin.Name} взял {priceWinner}";
                    }
                }
               
                string winText = $"🎉 Результаты рулетки!🎉" +
                                   $"\n" +
                                   $"\n➡➡➡➡ ⬇ ⬅⬅⬅⬅" +
                                   $"\n{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]} {smilesList[i]} {smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}{smilesList[r.Next(0, smiles.Count)]}" +
                                   $"\n➡➡ ⬆ ⬅⬅\n" +
                                   $"\n💳 Выигрыш: {priceWinner}" +
                                   $"\nСписок победителей: {winersTxt}";
                foreach (var price in roulette.Prices)
                {
                    var user = Api.User.GetUser(price.User);

                    if (price.Smile == winSmile)
                    {

                    }

                    Api.MessageSend(winText, price.User);
                }
                Bot.Statistics.WinCasino(priceWinner);

                roulette.Fund = 0;
                roulette.Prices = new List<Models.RoulettePrices>();
                Api.Roulette.SetList(roulette);
            }catch(Exception e)
            {
                Logger.WriteError($"{e.Message} \n {e.StackTrace}");
                Bot.Statistics.NewError();
            }

        }
    }
}