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
                   $"\n▶ Цена: 10 монет за 1 Российский Рубль."+
                   $"\n❓ Для покупки обращаться к [fooxboy|адмену] (Да-да, автоматической покупки нет)."+
                   $"\n"+
                   $"\n🔝 Покупка опыта за монеты." +
                   $"\n▶ Цена: 1 монета для 1 опыта." +
                   $"\n❓ Для покупки написать: Магазин опыт 10" +
                   $"\n"+
                   $"\n🍕 Покупка ресурсов за монеты." +
                   $"\n▶ Цена: 10 ресурсов за 1 монету." +
                   $"\n❓ Для покупки написать: Магазин ресурс название_ресурса 10" +
                   $"\n❗ Пример: магазино ресурс вода 50" +

                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" ;
        }
        

        [Attributes.Trigger("ресурс")]
        public static string Resources(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            string resource = "";
            try
            {
                resource = messageArray[2];
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали нужный ресурс.";
            }
            long count = 0;
            try
            {
                count = Int64.Parse(messageArray[3]);
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали количество.";

            }catch(FormatException)
            {
                return "❌ Вы указали неверное количество.";
            }

            var resources = new Api.Resources(msg.PeerId);

            if (count < 10) return "❌ Минимальное количество: 10";
            if (resources.MoneyCard < count / 10) return "❌ У Вас недосточно монет для покупки.";
            if (count > 100) return "❌ Максимальное количество: 100";
            if (resource.ToLower() == "вода") resources.Water = resources.Water + count;
            else if (resource.ToLower() == "еда") resources.Food = resources.Food + count;
            else if (resource.ToLower() == "энергия") resources.Energy = resources.Energy + count;
            else return "❌ Вы указали несуществующий ресурс.";
            Notifications.RemovePaymentCard(Convert.ToInt32(count / 10), msg.PeerId, "покупка ресурсов в магазине");
            return "✅ Вы успешно купили ресурсы!";
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