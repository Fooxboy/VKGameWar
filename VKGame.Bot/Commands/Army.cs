using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using VKGame.Bot.Api;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Класс для работы с разделом Армия.
    /// </summary>
    public class Army : ICommand
    {
        public override string Name => "Армия";
        public override string Caption => "Этот раздел предназначен для управления Вашей армией.";
        public override string Arguments => "(), (вариант_выбора)";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() {"создать", "обучить", "улучшить"};
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";


        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            var notify = String.Empty;
            if (Common.Notification == null) notify = $"Последнее обновление: {DateTime.Now}";
            else notify = Common.Notification;
            if (messageArray.Length == 1) return GetArmyText(msg, notify);
            var result = Helpers.Command.CheckMethods(typeof(Army), messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        public static class Api
        {
            public static int PriceSoldiery(int count)
            {
                return 7 * count;
            }

            public static int PriceTanks(int count)
            {
                return 50 * count;
            }
            public static bool CreateSoldiery(int count, long userId, bool boost = false)
            {
                new Thread(new ParameterizedThreadStart(BackgroundProcess.Army.CreateSoldiery)).Start(new Models.DataCreateSoldiery() { UserId = userId, Count = count, Boost = boost });
                return true;
            }

            public static int priceUpgSoldiery => 100;

            public static int priceUpgTanks => 500;

            public static bool CreateTanks(int count, long userId, bool boost = false)
            {
                new Thread(new ParameterizedThreadStart(BackgroundProcess.Army.CreateTanks)).Start(new Models.DataCreateSoldiery() { UserId = userId, Count = count, Boost = boost });
                return true;
            }
        }

        [Attributes.Trigger("улучшить")]
        public string UpgrateLevel(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            var typeArmy = string.Empty;

            try
            {
                typeArmy = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали тип армиии для улучшения. Доступные типы: солдат, танки." +
                       "\n -- Пример: армия улучшить солдат";
            }

            int price = 0;

            if (typeArmy == "солдат") price = Api.priceUpgSoldiery;
            else if (typeArmy == "танки") price = Api.priceUpgTanks;
            else return $"❌ {typeArmy} не является типом армии! Доступные типы: солдат, танки";
            if (!Notifications.RemovePaymentCard(price, msg.from_id, "улучшение армии"))
                return $"❌ На Вашем счету недостаточно монет! Необходимо: {price}. Узнать свой баланс - напишите: баланс";       
            var levels = new Bot.Api.Levels(msg.from_id);        
            if (typeArmy == "солдат") ++levels.Soldiery;
            else if (typeArmy == "танки") ++levels.Tanks;      
            return "✅ Вы успешно провели улучшение армии!";
        }

        [Attributes.Trigger("создать")]
        public string CreateTanks(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');  
            var user = new Bot.Api.User(msg.from_id);
            var resources = new Bot.Api.Resources(msg.from_id);
            var typeArmy = String.Empty;
            try
            {
                typeArmy = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали тип армии. Доступные типы армии: танков";
            }        
            if (typeArmy != "танков")
                return $"❌ Невозможно создать армию типа {messageArray[2]}, возможно, указан неверный тип Доступные типы: Солдат или Танков. Пример: Армия cоздать танков 10";
            int count = 0;
            var builds = new Bot.Api.Builds(msg.from_id);
            try
            {
                count = Convert.ToInt32(messageArray[3]);
            }
            catch (IndexOutOfRangeException)
            {
                if ((resources.Tanks) >= (Buildings.Api.MaxTanks(builds.Hangars)))
                {
                    return "❌ Вы использовали упрощённое создание танков. У Вас нет места для новых танков";
                }
                count = Convert.ToInt32((Buildings.Api.MaxTanks(builds.Hangars) - resources.Tanks));
            }
            catch (FormatException)
            {
                return $"❌ {messageArray[3]} не является числовым значением.";
            }
            int price = Api.PriceTanks(count);
            var boosters = new Bot.Api.Boosters(user.Id);
            if( (resources.Tanks + count) > Buildings.Api.MaxTanks(builds.Hangars))
                return $"❌ Вы не можете создать больше, чем у Вас вмещается. Ваша вместимость: [{resources.Tanks}/{Buildings.Api.MaxTanks(builds.Hangars)}]";     
            if (!Notifications.RemovePaymentCard(price, user.Id, "создание танков")) return $"❌ На Вашем банковском счету недостаточно средств. Ваш баланс: {resources.MoneyCard}. Необходимо: {price}. Заработайте деньги в казино или возьмите кредит в банке!";
            try
            {
                Common.CountCreateArmyTanks[user.Id] = count;
            }
            catch (KeyNotFoundException)
            {
                Common.CountCreateArmyTanks.Add(user.Id, count);
            }
            var registry = new Bot.Api.Registry(user.Id);
            var boost = false;
            if (boosters.CreateTanks != 0)
            {
                if (registry.ShowNotifyBoostArmy)
                    return
                        "❗ У Вас есть усилитель создания танков! Хотите его использовать? Напишите: ! Да или ! Нет или ! Умолчание (пробел после ! обязателен)" +
                        "\n❓ Да - Использовать, Нет - не ипользовать, Умолчание - использовать всегда значение по умолчанию." +
                        "\n➡ Чтобы изменить значение по умолчанию - перейдите в раздел Усилители";
                else boost = Convert.ToBoolean(new ConfigBoosters(user.Id).CreateTanks); 
            }

            if (boost) boosters.CreateTanks -= 1; 
            Api.CreateTanks(count, user.Id, boost);
            Common.CountCreateArmyTanks.Remove(user.Id);
            return $"✅ Вы успешно создаёте {count} новых танков. По окончанию создания, Вам придёт уведомление.";
        }
        
        [Attributes.Trigger("обучить")]
        public string Training(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');  
            var user = new Bot.Api.User(msg.from_id);
            var resources = new Bot.Api.Resources(msg.from_id);
            var typeArmy = String.Empty;
            try
            {
                typeArmy = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали тип армии. Доступные типы: солдат";
            }          
            if (typeArmy != "солдат") return $"❌ Невозможно обучить армию типа {typeArmy}. Доступные типы армии: Содат или Танков. Пример: Армия cоздать танков 10";
            int count = 0;
            var builds = new Bot.Api.Builds(msg.from_id);
            try
            {
                count = System.Convert.ToInt32(messageArray[3]);
            }
            catch (System.FormatException)
            {
                return $"❌ {messageArray[3]} не является числом.";
            }
            catch (IndexOutOfRangeException)
            {
                if ((resources.Soldiery) >= (Buildings.Api.MaxSoldiery(builds.Apartments)))
                {
                    return "❌ Вы использовали упрощённое обучение солдат. У Вас нет места для новых солдат.";
                }
                count = Convert.ToInt32((Buildings.Api.MaxSoldiery(builds.Apartments) - resources.Soldiery));
            }
            int price = Api.PriceSoldiery(count);
            if ((resources.Soldiery + count) > (Buildings.Api.MaxSoldiery(builds.Apartments)))
                return $"❌ Вы не можете создать больше, чем у Вас вмещается. У Вас вмещается: [{resources.Soldiery}/{Buildings.Api.MaxSoldiery(builds.Apartments)}]";
            var boosters = new Bot.Api.Boosters(user.Id);
            if(!Notifications.RemovePaymentCard(price, user.Id, "обучение солдат")) return $"❌ На Вашем банковском счету недостаточно средств. Ваш баланс: {resources.MoneyCard}. Необходимо: {price}. Заработайте деньги в казино или возьмите кредит в банке!";
            try
            {
                Common.CountCreateArmySoldiery[user.Id] = count;
            }
            catch (KeyNotFoundException)
            {
                Common.CountCreateArmySoldiery.Add(user.Id, count);
            }     
            var registry = new Bot.Api.Registry(user.Id);
            var boost = false;
            if (boosters.CreateSoldiery != 0)
            {
                if (registry.ShowNotifyBoostArmy)
                    return
                        "❗ У Вас есть усилитель обучения солдат! Хотите его использовать? Напишите: ! Да или ! Нет или ! Умолчание (пробел после ! обязателен)" +
                        "\n❓ Да - Использовать, Нет - не ипользовать, Умолчание - использовать всегда значение по умолчанию." +
                        "\n➡ Чтобы изменить значение по умолчанию - перейдите в раздел Усилители";
                else boost = Convert.ToBoolean(new ConfigBoosters(user.Id).CreateTanks); 
            }       
            if (boost) boosters.CreateSoldiery -= 1; 
            Common.CountCreateArmySoldiery.Remove(user.Id);
            Api.CreateSoldiery(count, user.Id, boost);
            return $"✅ Вы успешно обучаете {count} солдат. После окончания обучения, Вам прийдёт уведомление о готовности.";
        }

        public string GetArmyText(Models.Message msg, string notify)
        {
            var user = new Bot.Api.User(msg.from_id);
            var resources = new Bot.Api.Resources(user.Id);
            var levels = new Bot.Api.Levels(user.Id);
            var builds = new Bot.Api.Builds(user.Id);
            return $"‼{notify}" +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n" +
                   $"\nПривет, наш командир 👦 {user.Name}." +
                   $"\n" +
                   $"\nВАША ПЕХОТА➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n❓Здесь находится все Ваши солдаты. Будем считать, что это их дом." +
                   $"\n👨 Количество солдат: {resources.Soldiery} из {Buildings.Api.MaxSoldiery(builds.Apartments)}" +
                   $"\n🔝 Уровень солдат: {levels.Soldiery}" +
                   $"\n❗ Вам нехватает солдат? Обучите новых! Стоимость за одного солдата: 5 💳 Для обучения напишите: Армия обучить солдат 10" +
                   $"\n☢ Хотите увелить уровень? Напишите: Армия улучшить солдат " +
                   $"\n"+
                   $"\nВАША ТЕХНИКА➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n❓ Здесь \"живут\" Ваши любимые танки." +
                   $"\n💣 Количество танков: {resources.Tanks} из {Buildings.Api.MaxTanks(builds.Hangars)}" +
                   $"\n🔝 Уровень такнов: {levels.Tanks}" +
                   $"\n❗ Вам нехватает танков? Создайте новые! Стоимость за один танк: 40 💳 Для создания напишите: Армия создать танков 10" +
                   $"\n☢ Хотите увелить уровень? Напишите: Армия улучшить танки " +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }
    }
}