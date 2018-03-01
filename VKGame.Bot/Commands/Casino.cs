using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
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
            "Команда для работа с казино! Если написать Казино без аргуменов, то вы попадёте на главый 'экран казино.";

        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            if (messageArray.Length == 1)
                return GetCasinoText(msg.PeerId, $"Время последнего обновления: {DateTime.Now}");
            else
            {
                var type = typeof(Casino);
                object obj = Activator.CreateInstance(type);
                var methods = type.GetMethods();

                foreach (var method in methods)
                { 
                    var attributesCustom = Attribute.GetCustomAttributes(method);

                    foreach (var attribute in attributesCustom)
                    {
                        if (attribute.GetType() == typeof(Attributes.Trigger))
                        {

                            var myAtr = ((Attributes.Trigger) attribute);

                            if (myAtr.Name == messageArray[1])
                            {

                                object result = method.Invoke(obj, new object[] {msg});
                                return (string) result;
                            }
                        }
                    }
                    
                }

                return "❌ Неизвестная подкоманда.";
            }
        }
        
        /// <summary>
        /// Добавление нового билета.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="билет"></param>
        private void AddTicket(long id, string ticket)
        {
            var model = new Models.Tickets.Ticket {User = id, Number = ticket};
            var thread = new Thread(new ParameterizedThreadStart(BackgroundProcess.Casino.TimerTriggerEndGame));
            thread.Start(model);
        }
        

        [Attributes.Trigger("билет")]
        public string Ticket(LongPollVK.Models.AddNewMsg msg)
        {
            var id = msg.PeerId;
            var user = Api.User.GetUser(id);
            var resouces = new Api.Resources(id);
            if (resouces.MoneyCard < 200) return "❌ На Вашем счету недостаточно Монет 💳 ";
            string[] letters = {"a", "b", "c", "d", "f", "g", "k", "i"};
            var money = resouces.MoneyCard;
            money -= 200;
            resouces.MoneyCard = money;
            var r = new Random();
            string ticket = $"{letters[r.Next(0,7)]}{r.Next(1,50)}{r.Next(0,10)}{letters[r.Next(0,7)]}{letters[r.Next(0,7)]}";
            AddTicket(id, ticket);
            
            return $"✔ Вы успешно купили билет! Вот Ваш номер - {ticket}. Уведомление о выгрыше Вам придёт через определённое время.";
        }

        public string GetCasinoText(long id,string notify)
        {
            var user = Api.User.GetUser(id);
            Models.IResources resources = new Api.Resources(id);

            return        $"‼{notify}" +
                          $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n 🎰 КАЗИНО ИСПЫТАЙ УДАЧУ" +
                          $"\n" +
                          $"\n ⚠ Здесь можно играть только за БЕЗНАЛИЧНЫЙ расчёт." +
                          $"\n 💳 Баланс банковского счёта: {resources.MoneyCard}" +
                          $"\n" +
                          $"\n СПИСОК ДОСТУПНЫХ ИГР➖➖➖➖➖➖➖" +
                          $"\n 🎟 1.Счастливый билет. " +
                          $"\n ▶📝 Описание: пользователю выдаётся билет с определённым номером. Спустя определённое время Вам придёт уведомление о выгрыше." +
                          $"\n ▶💵 Стоимость: 200 💳" +
                          $"\n ▶💡 Как начать: напишите казино билет. Вам отправится Ваш билет. Чтобы вывести табло, напишите: казино билет табло" +
                          $"\n" +
                          $"\n NameGame" +
                          $"\n ➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n ⚠ Random help";
        }
    }
}