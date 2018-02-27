using System;
using System.Collections.Generic;
using VkNet.Model.RequestParams;

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
        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            var common = new Common();
            var user = Api.User.GetUser(msg.PeerId);
            if (user != null) return "Вы уже зарегистрированы в игре.";
            
            Api.User.NewUser(msg.PeerId);
            Api.Resources.Register(msg.PeerId);
            Api.Builds.Register(msg.PeerId);
            user = Api.User.GetUser(msg.PeerId);
            string resultStr =
                "Добро пожаловать, новичёк!🙂 Тебе предстоит создать свою армию!😏 Ты, конечно же, будешь командиром!😎 А теперь скажи своим солдатам как тебя называть!😜 Отправь своё имя! Для того чтобы отправить данные - используйте команду записи данных - ! Ваше имя. (пробел после ! обязателен.)";
            var vk = common.GetVk();
            var uservkdata = vk.Groups.IsMember("161965172", msg.PeerId, new List<long>() {msg.PeerId}, true)[0];

            if (uservkdata.Member) return resultStr;
            else resultStr += "\n❗ Я заметил, что ты не подписан на группу! ⚠ Пользоваться игрой можно и не подписавшись на группу, но подписчикам дают разные плюшки :) Так что советую подписаться! 😉";
            return resultStr;
        }

        public static string SetNick(LongPollVK.Models.AddNewMsg msg, string nick)
        {
            var user = Api.User.GetUser(msg.PeerId);
            if (user.isSetup) return "❌ Вы уже прошли этап установки.!";
                
            user.Name = nick;
            if (Api.User.SetUser(user))
            {
                OneRunGame(msg.PeerId);
                return $"✔ Так точно! Мы теперь будем называть Вас - {nick}! Вы всегда сможете изменить своё имя в настройках.";
            }
                
            else return "✖ Никак нет! Произошла ошибка при смене ника. Сочувствуем :( Попробуйте изменить в настройках.";
        }

        private static void OneRunGame(long id)
        {
            Api.MessageSend("Привет!😀 Сейчас я в кратце расскажу что да как.😉", id);
            Api.MessageSend("Самая главная валюта у нас это Монеты - 💰\nКроме монет существуют разные ресурсы, которые ты сможешь продавать:" +
                            "\n 💧 - Вода. У нас весь транспорт передвигается благодаря воде! Она реально редкий продукт!" +
                            "\n ⚡ - Энергия. Сейчас же будущее. Куда же без неё?" +
                            "\n 🍕 - Еда. А как вы думали? Чем ваша армия питаться будет-то?" +
                            "\n Монеты наличными вы редко будете встречать.😒 Все ваши монеты будут на банковском счету - 💳", id);
            Api.MessageSend("Чтобы посмотреть как там Ваша армия🐔, можно написать Армия", id);
            Api.MessageSend("Хотите поднять бабла💰? Можно сходить в казино😎! Напишите Казино", id);
            Api.MessageSend("Рвётесь в бой😨? Вас ничего не остановит✨! Пишите скорее Бой", id);
            Api.MessageSend("Забыли нужные команды🤨? Ничего😁! Напишите Помощь",id);
            Api.MessageSend("А сейчас переходите на домашний экран✋. Напишите Домой", id);
            var user = Api.User.GetUser(id);
            user.isSetup = true;
        }
    }
}