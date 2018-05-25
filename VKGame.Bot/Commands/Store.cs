using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using VKGame.Bot.Models;

namespace VKGame.Bot.Commands
{
    public class Store :ICommand
    {
        public override string Name => "магазин";
        public override string Caption => "Эта команда предназначена для работы с разделом магазина";
        public override string Arguments => "(), (Вариант_выбора)";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() { "ресурс", "опыт", "билет"};
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";


        public override object Execute(Message msg) 
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetStoreText(msg);
            
            var type = typeof(Store);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        public static string GetStoreText(Message msg) 
        {
            var resource = new Api.Resources(msg.from_id);
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💳 Ваш баланс: {resource.MoneyCard}"+
                   $"\n"+
                   $"\n✨ Здесь Вы можете купить все, что угодно."+
                   $"\nСПИСОК ТОВАРОВ➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💵 Покупка монет за реальные деньги."+
                   $"\n➡ Цена: 10 монет за 1 Российский Рубль."+
                   $"\n❓ Для покупки обращаться к [fooxboy|адмену] (Да-да, автоматической покупки нет)."+
                   $"\n"+
                   $"\n🔝 Покупка опыта за монеты." +
                   $"\n➡ Цена: 1 монета для 1 опыта." +
                   $"\n❓ Для покупки написать: Магазин опыт 10" +
                   $"\n"+
                   $"\n🍕 Покупка ресурсов за монеты." +
                   $"\n➡ Цена: 10 ресурсов за 1 монету." +
                   $"\n❓ Для покупки написать: Магазин ресурс название_ресурса 10" +
                   $"\n❗ Пример: магазин ресурс вода 50" +
                   $"\n" +
                   $"\n🎁 Покупка кейсов за реальные деньги" +
                   $"\n➡ Цена: 1 бокс за 10 рублей" +
                   $"\n❓ Вы можете купить элитный, вип, стальной кейсы." +
                   $"\n❓ Описание кейсов Вы можете найти в группе." +
                   $"\n❓ Для покупки обращаться к [fooxboy|адмену] (Да-да, автоматической покупки нет)." +
                   $"\n" +
                   $"\n🎟 Покупка билета на соревнование." +
                   $"\n➡ Цена: 1 билет 300 монет" +
                   $"\n❓ Билеты нужны, чтобы учавствовать в соревнованиях." +
                   $"\n❓ ❓ Для покупки написать: Магазин билет" +

                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" ;
        }

        [Attributes.Trigger("билет")]
        public static string Ticket(Message msg)
        {
            var resources = new Api.Resources(msg.from_id);
            if (!Notifications.RemovePaymentCard(200,msg.from_id, "магазин")) return $"❌ У Вас недосточно монет для покупки. Ваш баланс: {resources.MoneyCard}. Необходимо: 200";
            resources.TicketsCompetitions = resources.TicketsCompetitions + 1;

            return "✅ Вы успешно купили билет на соревнование!";
        }
        

        [Attributes.Trigger("ресурс")]
        public static string Resources(Message msg)
        {
            var messageArray = msg.body.Split(' ');
            string resource = "";
            try
            {
                resource = messageArray[2];
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали нужный ресурс. Доступные ресурсы: еда, энергия, вода";
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

            var resources = new Api.Resources(msg.from_id);

            if (count < 10) return "❌ Минимальное количество: 10";
            if (resources.MoneyCard < count / 10) return $"❌ У Вас недосточно монет для покупки. Ваш баланс: {resources.MoneyCard}. Необходимо: {count / 10}";
            if (count > 100) return "❌ Максимальное доступное количество для покупки: 100";
            if (resource.ToLower() == "вода") resources.Water += count;
            else if (resource.ToLower() == "еда") resources.Food += count;
            else if (resource.ToLower() == "энергия") resources.Energy += count;
            else return "❌ Вы указали несуществующий ресурс. Доступные ресурсы: еда, энергия, вода";
            Notifications.RemovePaymentCard(Convert.ToInt32(count / 10), msg.from_id, "покупка ресурсов в магазине");
            return "✅ Вы успешно купили ресурсы! ";
        }

        [Attributes.Trigger("опыт")]
        public static string Exp(Message msg)
        {
            var messageArray = msg.body.Split(' ');
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
            var resources = new Api.Resources(msg.from_id);
            var user = new Api.User(msg.from_id);
            if (resources.MoneyCard < count) return $"❌ У Вас недостаточно денег для такой покупки! Ваш баланс: {resources.MoneyCard}. Необходимо: {count}";
            Notifications.RemovePaymentCard(Convert.ToInt32(count), user.Id, "Покупка опыта.");
            user.Experience = user.Experience + count;
            return $"✅ Вы успешно купили {count} опыта!";
        }
    }
}