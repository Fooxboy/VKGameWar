using System;
using System.Collections.Generic;
using VkNet.Model.RequestParams;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Команда отвечающая за старт.
    /// </summary>
    public class Start: ICommand
    {
        public string Name => "Старт";
        public string Arguments => "";
        public string Caption => "Команда для регистрации в игре.";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>();
        public Access Access => Access.User;


        [Attributes.Trigger("Старт")]
        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            long referral = 0;
            try
            {
                referral = Int64.Parse(messageArray[1]);
            }catch(IndexOutOfRangeException)
            {

            }catch(FormatException)
            {
                return "❌ Вы не были зарегистрированы. Введите верный ид реферала и попробуйте снова.";
            }
            if (referral == msg.from_id) return "❌ Вы не были зарегистрированы. Вы не можете использовать себя как реферала. Попробуйте снова, только введите другово реферала!";
            var common = new Common();
            if (!Api.User.Check(msg.from_id)) return "❌ Вы уже зарегистрированы в игре.";
            Bot.Api.User.Registration(msg.from_id);
            var user = new Api.User(msg.from_id);
            Api.Registry.Register(msg.from_id);
            var registry = new Api.Registry(msg.from_id);
            Api.Builds.Register(msg.from_id);
            Api.Referrals.Register(msg.from_id);
            Api.Levels.Registration(msg.from_id);
            Api.Skills.Registration(msg.from_id);
            Api.Boosters.Register(msg.from_id);
            Api.ConfigBoosters.Register(msg.from_id);
            Statistics.NewRegistation();
           

            registry.LastMessage = DateTime.Now.ToString();
            registry.StartThread = true;
            registry.DateReg = DateTime.Now.ToString();
            

            //Старт потока добавления ресурсов.
            Thread threadAddingResource = new Thread(new ParameterizedThreadStart(BackgroundProcess.Buildings.AddingResources));
            Logger.WriteDebug($"Старт потока AddResource_{user.Id}");
            threadAddingResource.Name = $"AddResource_{user.Id}";
            threadAddingResource.Start(user.Id);

            if (referral != 0)
            {
                Statistics.NewReferral();
                registry.IsReferal = true;
                
                var refferalsuser = new Api.Referrals(referral);
                var listref= refferalsuser.RefList;
                listref.Add(msg.from_id);
                refferalsuser.RefList = listref;
                
                var refuser = new Api.User(referral);
                
                Api.Message.Send($"✨ У Вас новый реферал по имени {refuser.Name}! Он вам принёс 100 💳", referral);
                Notifications.EnterPaymentCard(100, referral, "реферальная система");
                Notifications.EnterPaymentCard(100, msg.from_id, "реферальная система");
                Api.Message.Send("✨ На Вас счёт поступило 100 💳 за то, что вы указали реферала!", msg.from_id);
            }

            Api.Boxs.Register(msg.from_id);

            string resultStr =
                "🎉 Добро пожаловать, новичёк!🙂" +
                "\nТебе предстоит создать свою армию!😏 Ты, конечно же, будешь командиром!" +
                "\nА теперь скажи своим солдатам как тебя называть!😜" +
                "\nОтправь своё имя! Для того чтобы отправить данные - используйте команду записи данных - ! Ваше имя. (пробел после ! обязателен.)";
            var vk = Common.GetVk();
            var uservkdata = vk.Groups.IsMember("161965172", msg.from_id, new List<long>() {msg.from_id}, true)[0];

            if (uservkdata.Member)
            {
                Api.Message.Send("♥ Спасибо, что ты подписан на нашу группу, вот тебе бонус за это :)", user.Id);
                Notifications.EnterPaymentCard(100, user.Id, "бонус за группу.");
                registry.IsBonusInGroupJoin = true;
                return resultStr;
            }  
            else resultStr += "\n \n❗ Ого ого! Я заметил, что ты не подписан на группу! ⚠ Играть можно и не подписавшись на группу, но подписчикам дают разные плюшки :) Так что советую подписаться! 😉 \n И за подписку ты можешь получить бонус :)";
            return resultStr;
        }

        public static string SetNick(Models.Message msg, string nick)
        {
            var user = new Api.User(msg.from_id);
            var registry = new Api.Registry(user.Id);
            if (registry.IsSetup) return "❌ Вы уже прошли этап установки.!";
                
            user.Name = nick;
            registry.IsSetup = true;
            OneRunGame(user.Id);
            registry.IsHelp = true;
            return $"✅ Так точно! Мы теперь будем называть Вас - {nick}! Вы всегда сможете изменить своё имя в настройках.";
        }

        private static void OneRunGame(long id)
        {
            var textHelp = $"🎉 Привет! Сейчас я тебе кратко расскажу, что у нас тут и как.😉" +
                $"\n😨 У нас тут война идёт!" +
                $"\n😁 Как хорошо, что ты присоединился к нам! Тебя не хватает!" +
                $"\n" +
                $"\n💰 У нас тут все расплачиваются монетами. Но сейчас же будущее! Поэтому все хранят свои монеты на банковском счету." +
                $"\n😋 Кроме денег у нас есть и другие важные ресурсы:" +
                $"\n 💧 -- Вода. У нас весь транспорт передвигается благодаря воде!" +
                $"\n ⚡ -- Энергия. Сейчас же будущее. Куда же без неё?" +
                $"\n 🍕 -- Еда. А как вы думали? Чем ваша армия питаться будет-то?" +
                $"\n" +
                $"\n➡ В игре есть армия. Вы должны обучать солдат и делать танки. А как это делать?" +
                $"\n✅ Напишите: Армия" +
                $"\n" +
                $"\n➡ Надоело воевать? Или нет денег? Вы можете поднять монет в казино! Скорее!" +
                $"\n✅ Напишите: Казино" +
                $"\n" +
                $"\n➡ Где же живут Ваши солдаты? Где стоят танки? Где хранятся все ресурсы? В постройках!" +
                $"\n✅ Напишите: Постройки" +
                $"\n" +
                $"\n➡ А что делать с Вашей армией? Что же делать на войне как не воевать! Давай быстрей! Пока ты это читаешь, твои враги выигрывают!" +
                $"\n✅ Напишите: Бой" +
                $"\n❓ Чтобы узнать подробнее об логике битв, прочитайте статью в группе." +
                $"\n" +
                $"\n➡ Не хватает денег? Или хотите положить наличные монеты на свой счёт? Все это можно в банке!" +
                $"\n✅ Напишите: Банк" +
                $"\n" +
                $"\n➡ Хотите прикупить монет? Или купить новый кейс? Тогда Вам нужно зайти в магазин!" +
                $"\n✅ Напишите: Магазин" +
                $"\n" +
                $"\n➡ Любите кейсы? У нас тут их любят! Там куууча всяких плюшек! Открыть и купить можно в разделе кейсы!" +
                $"\n✅ Напишите: Кейсы" +
                $"\n" +
                $"\n❤ У нас ещё есть много чего интересного! Чтобы узнать, что за разделы существуют ещё, напишите: Разделы" +
                $"\n❓ Подробное описание или помощь по любому разделу нужно найти в группе!" +
                $"\n";


            Api.Message.Send(textHelp, id);
            Api.Message.Send("😁 Командир! Вот я Вам и рассказал кратко, что да как!" +
                "\n😎 А теперь перейдите на домашний экран. Напишите: домой", id);
        }
    }
}