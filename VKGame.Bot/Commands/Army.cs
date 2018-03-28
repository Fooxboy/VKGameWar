using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Collections.Generic;
using System;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Класс для работы с разделом Армия.
    /// </summary>
    public class Army : ICommand
    {
        public string Name => "Армия";
        public string Caption => "Этот раздел предназначен для управления Вашей армией.";
        public string Arguments => "(), (вариант_выбора)";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>() {"создать", "обучить"};

        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1) return GetArmyText(msg, "Бета тест.");
            
            var type = typeof(Army);
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

                            if (myAtr.Name.ToLower() == messageArray[1].ToLower())
                            {

                                object result = method.Invoke(obj, new object[] {msg});
                                return (string) result;
                            }
                        }
                    }
                    
                }
            var word = Common.SimilarWord(messageArray[0], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        public static class Api
        {
            public static int PriceSoldiery(int count)
            {
                return 5 * count;
            }

            public static int PriceTanks(int count)
            {
                return 40 * count;
            }
            public static bool CreateSoldiery(int count, long userId)
            {
                var thread = new Thread(new ParameterizedThreadStart(BackgroundProcess.Army.CreateSoldiery));
                thread.Start(new Models.DataCreateSoldiery() { UserId = userId, Count = count });
                return true;
            }

            public static bool CreateTanks(int count, long userId)
            {
                var thread = new Thread(new ParameterizedThreadStart(BackgroundProcess.Army.CreateTanks));
                thread.Start(new Models.DataCreateSoldiery() { UserId = userId, Count = count });
                return true;
            }
        }

        [Attributes.Trigger("создать")]
        public string CreateTanks(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');  
            if(messageArray.Length < 4) return "❌ Вы не указали какой либо аргумент. Пример: Армия cоздать танков 10 ";
            var user = Bot.Api.User.GetUser(msg.from_id);
            var resources = new Bot.Api.Resources(msg.from_id);
            
            if (messageArray[2] != "танков") return $"❌ Невозможно создать армию типа {messageArray[2]}, возможно, указан неверный тип Доступные типы: Солдат или Танков. Пример: Армия cоздать танков 10";
            int count = 0;
             try
             {
                count = Convert.ToInt32(messageArray[3]);
             }
             catch (FormatException)
             {
                return $"❌ {messageArray[3]} не является числовым значением.";
             }

            int price = Api.PriceTanks(count);
            if (resources.MoneyCard < price) return $"❌ На Вашем банковском счету недостаточно средств. Ваш баланс: {resources.MoneyCard}. Необходимо: {price}. Заработайте деньги в казино или возьмите кредит в банке!";
            var builds = new Bot.Api.Builds(msg.from_id);
            if( (resources.Tanks + count) > Buildings.Api.MaxTanks(builds.Hangars))
                return $"❌ Вы не можете создать больше, чем у Вас вмещается. Ваша вместимость: [{resources.Tanks}/{Buildings.Api.MaxTanks(builds.Hangars)}]";
            Api.CreateTanks(count, user.Id);
            return $"✅ Вы успешно создаёте {count} новых танков. По окончанию создания, Вам прийдёт уведомление.";
        }
        
        [Attributes.Trigger("обучить")]
        public string Training(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');  
            if(messageArray.Length < 4) return "❌ Вы не указали какой-либо аргумент. Пример: Армия обучить солдат 10";
            var user = Bot.Api.User.GetUser(msg.from_id);
            var resources = new Bot.Api.Resources(msg.from_id);
            if (messageArray[2] != "солдат") return $"❌ Невозможно обучить армию типа {messageArray[2]}. Доступные типы армии: Содат или Танков. Пример: Армия cоздать танков 10";
            int count = 0;
            try
            {
                count = System.Convert.ToInt32(messageArray[3]);
            }
            catch (System.FormatException)
            {
                return $"❌ {messageArray[3]} не является числом.";
            }

            int price = Api.PriceSoldiery(count);
            if(resources.MoneyCard < price) return $"❌ На Вашем банковском счету недостаточно средств. Ваш баланс: {resources.MoneyCard}. Необходимо: {price}. Заработайте деньги в казино или возьмите кредит в банке!";
            var builds = new Bot.Api.Builds(msg.from_id);
            if ((resources.Soldiery + count) > (Buildings.Api.MaxSoldiery(builds.Apartments)))
                return $"❌ Вы не можете создать больше, чем у Вас вмещается. У Вас вмещается: [{resources.Soldiery}/{Buildings.Api.MaxSoldiery(builds.Apartments)}]";
            Api.CreateSoldiery(count, user.Id);
            return $"✅ Вы успешно обучаете {count} солдат. После окончания обучения, Вам прийдёт уведомление о готовности.";
        }

        public string GetArmyText(Models.Message msg, string notify)
        {
            var user = Bot.Api.User.GetUser(msg.from_id);
            var resources = new Bot.Api.Resources(user.Id);
            var builds = new Bot.Api.Builds(user.Id);
            return $"‼{notify}" +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n" +
                   $"\nПривет, наш командир 👦 {user.Name}." +
                   $"\n" +
                   $"\nВАША ПЕХОТА➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n❓Здесь находится все Ваши солдаты. Будем считать, что это их дом." +
                   $"\n👨 Количество солдат: {resources.Soldiery} из {Buildings.Api.MaxSoldiery(builds.Apartments)}" +
                   $"\n❗ Вам нехватает солдат? Обучите новых! Стоимость за одного солдата: 5 💳 Для обучения напишите: Армия обучить солдат 10" +
                   $"\n"+
                   $"\nВАША ТЕХНИКА➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n❓ Здесь \"живут\" Ваши любимые танки." +
                   $"\n💣 Количество танков: {resources.Tanks} из {Buildings.Api.MaxTanks(builds.Hangars)}" +
                   $"\n❗ Вам нехватает танков? Создайте новые! Стоимость за один танк: 40 💳 Для создания напишите: Армия создать танков 10" +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }
    }
}