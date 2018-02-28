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
                   $"\n"+
                   $"\nВ разработке."+
                   $"\n"+
                   $"\n";


        }
    }
}