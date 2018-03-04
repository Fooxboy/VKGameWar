using System;
using Newtonsoft.Json;
using System.IO;

namespace VKGame.Bot.Commands
{
    public class Store :ICommand
    {
        public string Name => "магазин";
        public string Caption => "Эта команда предназначена для работы с разделом магазина";
        public string Arguments => "(), (Вариант_выбора)";
        public TypeResponse Type => TypeResponse.Text;
        public object Execute(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');
            if (messageArray.Length == 1)
                return GetStoreText(msg);
            else
            {
                var type = typeof(Store);
                object obj = Activator.CreateInstance(type);
                var methods = type.GetMethods();

                foreach (var method in methods)
                {
                    var attributesCustom = Attribute.GetCustomAttributes(method);

                    foreach (var attribute in attributesCustom)
                    {
                        if (attribute.GetType() == typeof(Attributes.Trigger))
                        {

                            var myAtr = ((Attributes.Trigger)attribute);

                            if (myAtr.Name == messageArray[1])
                            {

                                object result = method.Invoke(obj, new object[] { msg });
                                return (string)result;
                            }
                        }
                    }

                }
                return "❌ Неизвестная подкоманда.";
            }
        }

        public static string GetStoreText(LongPollVK.Models.AddNewMsg msg) 
        {
            var resource = new Api.Resources(msg.PeerId);
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💳 Ваш баланс: {resource.MoneyCard}"+
                   $"\n"+
                   $"\n✨ Здесь Вы можете купить все, что угодно."+
                   $"\nСПИСОК ТОВАРОВ➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💵 Покупка монет за реальные деньги."+
                   $"\n▶ Цена: 20 монет за 1 Российский Рубль."+
                   $"\n❓ Для покупки обращаться к [fooxboy|адмену] (Да-да, автоматической покупки нет)."+
                   $"\n"+
                   $"\n🔝 Покупка опыта за монеты." +
                   $"\n▶ Цена: 1 монета для 1 опыта." +
                   $"\n❓ Для покупки написать: Магазин опыт 10" +
                   $"\n"+
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" ;
        }

        [Attributes.Trigger("опыт")]
        public static string Exp(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            long count = 0;
            try
            {
                count = Int64.Parse(messageArray[2]);
            } catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали число опыта.";
            }catch (FormatException)
            {
                return "❌ Вы указали не число.";
            }
            var resources = new Api.Resources(msg.PeerId);
            var user = Api.User.GetUser(msg.PeerId);
            if (resources.MoneyCard < count) return "❌ У Вас недостаточно денег для такой покупки!";
            Notifications.RemovePaymentCard(Convert.ToInt32(count), msg.PeerId, "Покупка опыта.");
            user.Experience = user.Experience + count;
            Api.User.SetUser(user);
            return "✅ Вы успешно купили опыт!";
        }
    }
}