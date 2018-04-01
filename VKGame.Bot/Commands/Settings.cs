using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Settings: ICommand
    {
        public string Name => "Настройки";
        public string Arguments => "(), (Вариант_выбора)";
        public string Caption => "Здесь можно настроить все под себя!";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>() {"имя"};

        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetSettingsText();
            else
            {
                var type = typeof(Settings);
                object obj = Activator.CreateInstance(type);
                var methods = type.GetMethods();

                foreach (var method in methods)
                {
                    var attributesCustom = Attribute.GetCustomAttributes(method);

                    foreach (var attribute in attributesCustom)
                    {
                        if (attribute.GetType() == typeof(Attributes.Trigger))
                        {
                            var myAtr = ((Attributes.Trigger)attribute);
                            if (myAtr.Name.ToLower() == messageArray[1].ToLower())
                            {

                                object result = method.Invoke(obj, new object[] { msg });
                                return (string)result;
                            }
                        }
                    }

                }
            }
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
            var user = Api.User.GetUser(msg.from_id);
            user.Name = text;
            Notifications.RemovePaymentCard(100, user.Id, "изменение имени");
            Api.User.SetUser(user);
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
