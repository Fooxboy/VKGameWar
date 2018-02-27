using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

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
        /// Объявление выгрыша в билетную лотерею.
        /// </summary>
        public static void EndGameTicket(object obj)
        {
            var json = "";
            using (var reader = new StreamReader(@"Files\Tickets.json", System.Text.Encoding.Default))
            {
                json = reader.ReadToEnd();
            }

            Models.Tickets model = JsonConvert.DeserializeObject<Models.Tickets>(json);
            long[] price = {5, 10, 40, 50, 70, 100, 150, 200, 300, 350, 400, 600, 800, 1000};
            var r = new Random();
            
            foreach (var id in model.Users)
            {
                var resources = new Api.Resources(id);
                var moneyUser = resources.MoneyCard;
                var priceInt = r.Next(0, price.Length - 1);
                moneyUser += price[priceInt];
                resources.MoneyCard = moneyUser;
                Api.MessageSend($"✨ Денежный перевод! На Ваш банковский счёт было зачислено {price[priceInt]} 💳 от КАЗИНО \"ИСПЫТАЙ УДАЧУ\". ", id);
            }
            
            File.Delete(@"Files\Tickets.json");
        }
        
        /// <summary>
        /// Добавление нового билета.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="билет"></param>
        private void AddTicket(long id, string ticket)
        {
            if (!Directory.Exists("Files")) Directory.CreateDirectory("Files");
            
            Models.Tickets model =new  Models.Tickets();
            
            if (!File.Exists(@"Files\Tickets.json"))
            {
                using (var file = File.Create(@"Files\Tickets.json"))
                {
                }
                model.ListTickets = new List<string>();
                model.Users = new List<long>();
            }
            else
            {
                var json = "";
                using (var reader = new StreamReader(@"Files\Tickets.json", System.Text.Encoding.Default))
                {
                    json = reader.ReadToEnd();            
                    reader.Close();
                }

                model = JsonConvert.DeserializeObject<Models.Tickets>(json);
            }
            model.ListTickets.Add(ticket);
            model.Users.Add(id);
            string result = JsonConvert.SerializeObject(model);

            using (var writter = new StreamWriter(@"Files\Tickets.json", false, System.Text.Encoding.Default))
            {
                writter.Write(result);
                writter.Close();
            }
            
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