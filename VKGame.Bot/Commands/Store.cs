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
            return "❌ Неизвестная подкоманда.";
        }

        public static string GetTextStore(LongPollVK.Models.AddNewMsg msg) 
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
                   $"\n" +
                   $"\n" +
                   $"\n" +
                   $"\n"+
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" ;




        }
    }
}