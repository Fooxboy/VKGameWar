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
        public string Caption => "Команда для регистрации игры.";
        public TypeResponse Type => TypeResponse.Text;
        
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
                return "❌ Вы не были зарегистрированы. Введите валидный ид реферала и попробуйте снова.";
            }
            if (referral == msg.from_id) return "❌ Вы не были зарегистрированы. Вы не можете использовать себя как реферала. Попробуйте снова, только введите другово реферала!";
            var common = new Common();
            var user = Api.User.GetUser(msg.from_id);
            if (user != null) return "❌ Вы уже зарегистрированы в игре.";
            Api.User.NewUser(msg.from_id);
            Api.Resources.Register(msg.from_id);
            Api.Builds.Register(msg.from_id);
            var listusers = Api.UserList.GetList();
            listusers.Users.Add(msg.from_id);
            Api.UserList.SetList(listusers);
            user = Api.User.GetUser(msg.from_id);
            user.isSetup = true;
            using (File.Create($@"Files/ReferralsFiles/Refferals_{msg.from_id}.json"))
            {

            }
            var modelRefferals = new Models.Referrals();
            modelRefferals.ReferralsList = new List<Models.Referrals.Referral>();
            Statistics.NewRegistation();
            var json = JsonConvert.SerializeObject(modelRefferals);
            using(var writer = new StreamWriter($@"Files/ReferralsFiles/Refferals_{msg.from_id}.json",false, System.Text.Encoding.Default))
            {
                writer.Write(json);
            }

            //старт потока добавления ресурсов.
            Thread threadAddingResource = new Thread(new ParameterizedThreadStart(BackgroundProcess.Buildings.AddingResources));
            Logger.WriteDebug($"Старт потока AddResource_{user.Id}");
            threadAddingResource.Name = $"AddResource_{user.Id}";
            threadAddingResource.Start(user.Id);

            if (referral != 0)
            {
                Statistics.NewReferral();
                var userRef = Api.User.GetUser(referral);
                if (userRef != null)
                {
                    var listReferral = Api.Referrals.GetList(referral);
                    listReferral.ReferralsList.Add(new Models.Referrals.Referral { Name = userRef.Name, DateRegistration = DateTime.Now.ToString(), FarmMoney = 100, Id = userRef.Id });
                    Api.MessageSend($"✨ У Вас новый реферал по имени {userRef.Name}! Он вам принёс 100 💳", referral);
                    Notifications.EnterPaymentCard(100, referral, "реферальная система");
                    Notifications.EnterPaymentCard(100, msg.from_id, "реферальная система");
                    Api.MessageSend("✨ На Вас счёт поступило 100 💳 за то, что вы указали реферала!", msg.from_id);
                }
            }

            Api.User.SetUser(user);
            Api.Boxes.Register(msg.from_id);

            string resultStr =
                "🎉 Добро пожаловать, новичёк!🙂" +
                "\nТебе предстоит создать свою армию!😏 Ты, конечно же, будешь командиром!" +
                "\nА теперь скажи своим солдатам как тебя называть!😜" +
                "\nОтправь своё имя! Для того чтобы отправить данные - используйте команду записи данных - ! Ваше имя. (пробел после ! обязателен.)";
            var vk = Common.GetVk();
            var uservkdata = vk.Groups.IsMember("161965172", msg.from_id, new List<long>() {msg.from_id}, true)[0];

            if (uservkdata.Member) return resultStr;
            else resultStr += "\n \n❗ Ого ого! Я заметил, что ты не подписан на группу! ⚠ Играть можно и не подписавшись на группу, но подписчикам дают разные плюшки :) Так что советую подписаться! 😉 \n И за подписку ты можешь получить бонус :)";
            return resultStr;
        }

        public static string SetNick(Models.Message msg, string nick)
        {
            var user = Api.User.GetUser(msg.from_id);
            if (user.isSetup) return "❌ Вы уже прошли этап установки.!";
                
            user.Name = nick;
            if (Api.User.SetUser(user))
            {
                OneRunGame(user.Id);
                return $"✅ Так точно! Мы теперь будем называть Вас - {nick}! Вы всегда сможете изменить своё имя в настройках.";
            }
                
            else return "❌ Никак нет! Произошла ошибка при смене ника. Попробуйте изменить в настройках.";
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


            Api.MessageSend(textHelp, id);
            Api.MessageSend("😁 Командир! Вот я Вам и рассказал кратко, что да как!" +
                "\n😎 А теперь перейдите на домашний экран. Напишите: домой", id);
        }
    }
}