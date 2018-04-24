using System;
using System.Collections.Generic;
using VKGame.Bot.Models;

namespace VKGame.Bot.Commands
{
    public class Boosters :ICommand
    {
        public override string Name => "Усилители";
        public override string Arguments => "(), (Вариант_выбора)";
        public override string Caption => "Здесь можно купить усилители или настроить их использование.";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>(){"купить"};
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";

        public override object Execute(Message msg)
        {
            throw new System.NotImplementedException();
        }

        [Attributes.Trigger("настройка")]
        public static string SettingsConfigBoosters(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            var booster = String.Empty;
            try
            {
                booster = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали id усилителя. Чтобы узнать id усилителей, напишите: усилители ";
            }

            var values = String.Empty;
            try
            {
                values = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали значение. Доступные значения: Вкл, Выкл";
            }

            long valueLong = 0;
            if (values.ToLower() == "вкл") valueLong = 1;
            else if (values.ToLower() == "выкл") valueLong = 0;
            else return "❌ Неизвестное значение. Доступные значения: вкл, выкл ";
            
            var config = new Api.ConfigBoosters(msg.from_id);
            
            if (booster == "1")
            {
                config.CreateFood = valueLong;
            }else if (booster == "2")
            {
                config.CreateWater = valueLong;
            }else if (booster == "3")
            {
                config.CreateSoldiery = valueLong;
            }else if (booster == "4")
            {
                config.CreateTanks = valueLong;
            }
            else return "❌ Вы ввели неверный id усилителя. Чтобы узнать id усилителей, напишите: усилители";
            
            return $"✅ Теперь для усилителя с id - {booster} значение по умолчанию - {values}";
        }


        [Attributes.Trigger("купить")]
        public static string BuyBoosters(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            
            var booster = String.Empty;
            try
            {
                booster = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали id усилителя. Чтобы узнать id усилителей, напишите: усилители ";
            }
            
            var boosters = new Api.Boosters(msg.from_id);
            
            if (booster == "1")
            {
                if (!Notifications.RemovePaymentCard(50, msg.from_id, "покупка усилителя"))
                    return "❌ У Вас на счету нет необходимого количества монет. Необходимо: 50 💳";
                boosters.CreateFood = ++boosters.CreateFood;

            }else if (booster == "2")
            {
                if (!Notifications.RemovePaymentCard(50, msg.from_id, "покупка усилителя"))
                    return "❌ У Вас на счету нет необходимого количества монет. Необходимо: 50 💳";
                boosters.CreateWater = ++boosters.CreateWater;
            }else if (booster == "3")
            {
                if (!Notifications.RemovePaymentCard(80, msg.from_id, "покупка усилителя"))
                    return "❌ У Вас на счету нет необходимого количества монет. Необходимо: 80 💳";
                boosters.CreateSoldiery = ++boosters.CreateSoldiery;
            }else if (booster == "4")
            {
                if (!Notifications.RemovePaymentCard(100, msg.from_id, "покупка усилителя"))
                    return "❌ У Вас на счету нет необходимого количества монет. Необходимо: 100 💳";
                boosters.CreateTanks = +boosters.CreateTanks;
            }
            else return "❌ Вы ввели неверный id усилителя. Чтобы узнать id усилителей, напишите: усилители";
            return "✅ Вы успешно купили усилитель! ";
        }

        public static string GetBoostersText(long userId)
        {
            var boosters = new Api.Boosters(userId);
            var configboosters = new Api.ConfigBoosters(userId);

            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n ВАШИ УСИЛИТЕЛИ" +
                   $"\n" +
                   $"\n1 - 🍕 Усилитель производства еды: {boosters.CreateFood}" +
                   $"\n➡ Использовать по умолчанию, если есть: {convertInString(configboosters.CreateFood)}" +
                   $"\n" +
                   $"\n2 - 💧 Усилитель производства воды: {boosters.CreateWater}" +
                   $"\n➡ Использовать по умолчанию, если есть: {convertInString(configboosters.CreateWater)}" +
                   $"\n" +
                   $"\n3 - 😀 Усилитель обучения солдат: {boosters.CreateSoldiery}" +
                   $"\n➡  Использовать по умолчанию, если есть: {convertInString(configboosters.CreateSoldiery)}" +
                   $"\n" +
                   $"\n4 - 💣 Усилитель производства танков: {boosters.CreateTanks}" +
                   $"\n➡  Использовать по умолчанию, если есть: {convertInString(configboosters.CreateTanks)} " +
                   $"\n" +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }

        private static string convertInString(long i)
        {
            if (i == 1) return "Да";
            else return "Нет";
        }
        
    }
}