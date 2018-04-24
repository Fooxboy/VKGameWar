using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Settings: ICommand
    {
        public override string Name => "Настройки";
        public override string Arguments => "(), (Вариант_выбора)";
        public override string Caption => "Здесь можно настроить все под себя!";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() {"имя"};
        public override Access Access => Access.User;


        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetSettingsText();
            var type = typeof(Settings);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        [Attributes.Trigger("имя")]
        public static string NameEdit(Models.Message msg)
        {
            var resources = new Api.Resources(msg.from_id);

            if (resources.MoneyCard < 100) return $"❌ На балансе недостаточно средств. Баланс: {resources.MoneyCard}. Необходимо: 100";
            string text = "";
            string[] arrayText = msg.body.Split(' ');
            for (int i = 2; arrayText.Length > i; i++) text += $"{arrayText[i]} ";
            if (text == "") text = $"{msg.from_id}";
            var user = new Api.User(msg.from_id);
            user.Name = text;
            Notifications.RemovePaymentCard(100, user.Id, "изменение имени");
            return $"🎉 Вы успешно изменили Ваше имя на {text}!";
        }

        private string GetSettingsText()
        {
            string text = $"➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\n ⚙НАСТРОЙКИ ПОЛЬЗОВАТЕЛЯ." +
                $"\n" +
                $"\n ➡😀 ИМЯ" +
                $"\n➡❓ Чтобы изменить своё имя, напишите: настройки имя ваше имя" +
                $"\n➡💳 Цена: 100 монет." +
                $"\n" +
                $"\n➖➖➖➖➖➖➖➖➖➖➖➖";

            return text;
        }
    }
}
