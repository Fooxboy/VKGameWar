using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Класс для работы с разделом Армия.
    /// </summary>
    public class Army:ICommand
    {
        public string Name => "армия";
        public string Caption => "Этот раздел предназначен для управления Вашей армией.";
        public string Arguments => "(), (вариант_выбора)";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            if (messageArray.Length == 1) return GetArmyText(msg, "Уведомление.");
            
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

        [Attributes.Trigger("создать")]
        public string CreateTanks(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');  
            if(messageArray.Length < 4) return "❌ Вы не указали аргумент. Помощь: Армия cоздать танков 10";
            var user = Api.User.GetUser(msg.PeerId);
            var resources = new Api.Resources(msg.PeerId);
            
            if (messageArray[2] != "танков") return $"❌ Невозможно создать {messageArray}.";
            int count = 0;
             try
            {
                count = System.Convert.ToInt32(messageArray[3]);
            }
            catch (System.FormatException)
            {
                return $"❌ {messageArray[3]} не является нужным числом.";
            }

            int price = 40*count;
            if(resources.MoneyCard < price) return "❌ На Вашем банковском счету недостаточно средств.";

            var builds = new Api.Builds(msg.PeerId);
            if( (resources.Tanks + count) > (builds.Hangars * 5)) return "❌ Вы не можете создать больше, чем у Вас вмещается.";
            var thread = new Thread(new ParameterizedThreadStart(BackgroundProcess.Army.CreateTanks));
            thread.Start(new Models.DataCreateSoldiery() { UserId = msg.PeerId, Count = count});
            return $"✔ Вы успешно создаёте {count} новых танков.";
        }
        
        [Attributes.Trigger("обучить")]
        public string Training(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');  
            if(messageArray.Length < 4) return "❌ Вы не указали аргумент. Помощь: Армия обучить солдат 10";
            var user = Api.User.GetUser(msg.PeerId);
            var resources = new Api.Resources(msg.PeerId);
            if (messageArray[2] != "солдат") return $"❌ Невозможно обучить {messageArray}.";
            int count = 0;
            try
            {
                count = System.Convert.ToInt32(messageArray[3]);
            }
            catch (System.FormatException)
            {
                return $"❌ {messageArray[3]} не является нужным числом.";
            }

            int price = 5*count;
            if(resources.MoneyCard < price) return "❌ На Вашем банковском счету недостаточно средств.";
            var builds = new Api.Builds(msg.PeerId);
            if ((resources.Soldiery + count) > (builds.Apartments * 10)) return "❌ Вы не можете создать больше, чем у Вас вмещается.";
            var thread = new Thread(new ParameterizedThreadStart(BackgroundProcess.Army.CreateSoldiery));
            thread.Start(new Models.DataCreateSoldiery() { UserId = msg.PeerId, Count = count});
            return $"✔ Вы успешно обучаете {count} солдат.";
        }

        public string GetArmyText(LongPollVK.Models.AddNewMsg msg, string notify)
        {
            var user = Api.User.GetUser(msg.PeerId);
            var resources = new Api.Resources(msg.PeerId);
            return $"‼{notify}" +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n" +
                   $"\n(время суток), наш командир 👦 {user.Name}." +
                   $"\n" +
                   $"\nВАША ПЕХОТА➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n❓Здесь находится все Ваши солдаты. Будем считать, что это их дом." +
                   $"\n👨 Количество солдат: {resources.Soldiery}" +
                   $"\n❗ Вам нехватает солдат? Обучите новых! Для обучения напишите: Армия обучить солдат 10" +
                   $"\n"+
                   $"\nВАША ТЕХНИКА➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n❓ Здесь \"живут\" Ваши любимые танки." +
                   $"\n💣 Количество танков: {resources.Tanks}" +
                   $"\n❗ Вам нехватает танков? Создайте новые! Для создания напишите: Армия создать танков 10" +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }
    }
}