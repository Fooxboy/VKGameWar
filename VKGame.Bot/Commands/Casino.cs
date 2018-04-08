using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Казино
    /// </summary>
    public class Casino:ICommand
    {
        public string Name => "Казино";
        public string Arguments => "(), (вариант_выбора)";

        public string Caption =>
            "Раздел с казино! Если написать Казино без аргуменов, то вы попадёте на главый экран казино.";

        public TypeResponse Type => TypeResponse.Text;

        public List<string> Commands => new List<string>() { "карты", "билет", "рулетка"};
        public Access Access => Access.User;


        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetCasinoText(msg.from_id, $"Время последнего обновления: {DateTime.Now}");
            
            var type = typeof(Casino);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }
        
        /// <summary>
        /// Добавление нового билета.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="билет"></param>
        private void AddTicket(long id, string ticket)
        {
            var model = new Models.Tickets.Ticket {User = id, Number = ticket};
            new Task(() => BackgroundProcess.Casino.TimerTriggerEndGame(model));
        }

        [Attributes.Trigger("карты")]
        public string Cards(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');

            return "Карты в разработке. И будут там долго :)";
        }
        

        [Attributes.Trigger("билет")]
        public string Ticket(Models.Message msg)
        {
            var id = msg.from_id;
            var user = new Api.User(id);
            var resouces = new Api.Resources(id);
            if (resouces.MoneyCard < 50) return $"❌ На Вашем счету недостаточно Монет. Ваш баланс: {resouces.MoneyCard} 💳 Необходимо: 50 💳 ";
            string[] letters = {"a", "b", "c", "d", "f", "g", "k", "i"};
            var money = resouces.MoneyCard;
            money -= 50;
            resouces.MoneyCard = money;
            var r = new Random();
            string ticket = $"{letters[r.Next(0,7)]}{r.Next(1,50)}{r.Next(0,10)}{letters[r.Next(0,7)]}{letters[r.Next(0,7)]}";
            AddTicket(id, ticket);
            
            return $"✔ Вы успешно купили билет! Вот Ваш номер - {ticket}. Уведомление о выгрыше Вам придёт через несколько минут.";
        }

        [Attributes.Trigger("рулетка")]
        public static string Roulette(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
            var resources = new Api.Resources(user.Id);
            var messageArray = msg.body.Split(' ');
            string smile = "";
            long price = 0;
            try
            {
                smile = messageArray[2].ToLower();
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали смайл!";
            }

            try
            {
                price = Int64.Parse(messageArray[3]);
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали сумму!";
            }catch(FormatException)
            {
                return "❌ Вы указали неверную сумму!";
            }

            if (price < 10) return "❌ Сумма должна быть больше 10!";
            if (price > resources.MoneyCard) return $"❌ Вы ставите больше, чем у Вас есть на балансе. Ваш баланс: {resources.MoneyCard}";
            var roulette = Common.Roulette;
            bool userUsed = false;
            foreach (var rouletteItem in roulette.Prices)
            {
                if(user.Id == rouletteItem.User)
                {
                    userUsed = true;
                    break;
                }
            }
            if (userUsed) return "❌ Вы уже сделали ставку.";
            Notifications.RemovePaymentCard(Convert.ToInt32(price), user.Id, "Ставка в рулетке.");
            if(roulette.Prices.Count == 0)
            {
                new Task(() => BackgroundProcess.Casino.TimerTriggerRoulette()).Start();
                roulette.Fund = 0;
            }
            roulette.Prices.Add(new Models.RoulettePrices { User = user.Id, Price = price, Smile = smile });
            roulette.Fund = roulette.Fund + price;
            Common.Roulette = roulette;
            string users = "";

            foreach(var priceUser in roulette.Prices)
            {
                var userModel = new Api.User(priceUser.User);
                users += $"\n➡ {userModel.Name} поставил {priceUser.Price} на {priceUser.Smile}";
                Api.Message.Send($"❗ К игре присоединился новый игрок! Фонд рулетки теперь: {roulette.Fund}", userModel.Id);
            }
            return $"✅  Вы успешно поставили на {smile}!" +
                $"\n" +
                $"\n💰 Фонд текущей рулетки: {roulette.Fund}" +
                $"\n😀 Список игроков:" +
                $"{users}";
        }

        public string GetCasinoText(long id,string notify)
        {
            var user = new Api.User(id);
            var resources = new Api.Resources(id);

            return        $"‼{notify}" +
                          $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n 🎰 КАЗИНО ИСПЫТАЙ УДАЧУ" +
                          $"\n" +
                          $"\n ⚠ Здесь можно играть только за БЕЗНАЛИЧНЫЙ расчёт." +
                          $"\n 💳 Баланс банковского счёта: {resources.MoneyCard}" +
                          $"\n" +
                          $"\n СПИСОК ДОСТУПНЫХ ИГР➖➖➖➖➖➖➖" +
                          $"\n 🎟 1. Счастливый билет " +
                          $"\n ➡📝 Описание: пользователю выдаётся билет с определённым номером. Спустя определённое время Вам придёт уведомление о выгрыше." +
                          $"\n ➡💵 Стоимость: 50 💳" +
                          $"\n ➡💡 Как начать: напишите казино билет. Вам отправится Ваш билет." +
                          $"\n" +
                          $"\n 🎱 2. Рулетка" +
                          $"\n ➡📝 Описание: вы ставите сумму на смайл. Спустя время рулетка начинает крутится. И тем, кто угадал нужный смайл выплачивается сумма с фонда." +
                          $"\n ➡💵 Стоимость: вы сами определяете ставку. От 10 монет." +
                          $"\n ➡💡 Как начать: достаточно написать казино рулетка смайл сумма. Например: казино рулетка сердце 10" +
                          $"\n ➡❓ Список доступных смайлов для ставки:" +
                          $"\n ➡➡Сердце -- ❤" +
                          $"\n ➡➡Взрыв -- 💥" +
                          $"\n ➡➡Пицца -- 🍕" +
                          $"\n ➡➡Цветок -- 🌸" +
                          $"\n ➡➡Лицо -- 😀" +
                          $"\n ➡❓ Чтобы поставить нужно писать НАЗВАНИЕ смайла, а не сам смайл." +
                          $"\n ➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n ⚠ {Common.GetRandomHelp()}";
        }
    }
}