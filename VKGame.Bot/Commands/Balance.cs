using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Balance:ICommand
    {
        public string Name => "Баланс";
        public string Arguments => "()";
        public string Caption => "Раздел для вывода информации о Вашем балансе.";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string> { "отнять", "прибавить", "узнать"};

        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetBalanceText(msg.from_id);
            else
            {
                var type = typeof(Balance);
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
                            if (myAtr.Name == messageArray[1])
                            {
                                object result = method.Invoke(obj, new object[] { msg });
                                return (string)result;
                            }
                        }
                    }
                }
            }
            var word = Common.SimilarWord(messageArray[0], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        [Attributes.Trigger("отнять")]
        public string RemoveMoney(Models.Message msg)
        {
            var user = Api.User.GetUser(msg.from_id);
            if (user.Access < 4) return "У Вас нет доступа.";
            long userId = 0;
            var messageArray = msg.body.Split(' ');
            var vk = Common.GetVk();
            var modelMessage = vk.Messages.GetById(new List<ulong> { Convert.ToUInt32(msg.id) });
            if (modelMessage[0].ForwardedMessages.Count != 0)
            {
                var userIdForw = modelMessage[0].ForwardedMessages[0].UserId;
                userId = userIdForw.Value;
            }
            var count = Int32.Parse(messageArray[2]);
            if (userId == 0) return "Перешлите сообщение с ним.";
            Notifications.RemovePaymentCard(count, userId, "Админ");

            return $"Вы успешно сняли с баланса пользователя {count} монет";
        }

        [Attributes.Trigger("прибавить")]
        public string AddMoney(Models.Message msg)
        {
            var user = Api.User.GetUser(msg.from_id);
            if (user.Access < 4) return "У Вас нет доступа.";
            long userId = 0;
            var messageArray = msg.body.Split(' ');
            var vk = Common.GetVk();
            var modelMessage = vk.Messages.GetById(new List<ulong> { Convert.ToUInt32(msg.id) });
            if (modelMessage[0].ForwardedMessages.Count != 0)
            {
                var userIdForw = modelMessage[0].ForwardedMessages[0].UserId;
                userId = userIdForw.Value;
            }
            var count = Int32.Parse(messageArray[2]);
            if (userId == 0) return "Перешлите сообщение с ним.";
            Notifications.EnterPaymentCard(count, userId, "Админ");

            return $"Вы успешно пополнили баланс пользователя на {count} монет";
        }

        [Attributes.Trigger("узнать")]
        public string Find(Models.Message msg)
        {
            var user = Api.User.GetUser(msg.from_id);
            if (user.Access < 4) return "У Вас нет доступа.";
            long userId = 0;
            var messageArray = msg.body.Split(' ');
            try
            {
                userId = Int64.Parse(messageArray[2]);
            }catch(IndexOutOfRangeException)
            {
                var vk = Common.GetVk();
                var modelMessage = vk.Messages.GetById(new List<ulong> { Convert.ToUInt32(msg.id) });
                if(modelMessage[0].ForwardedMessages.Count != 0)
                {
                    var userIdForw = modelMessage[0].ForwardedMessages[0].UserId;
                    userId = userIdForw.Value;
                }
            }

            if (userId == 0) return "Укажите ID пользователя или перешлите сообщение с ним.";

            return GetBalanceText(userId);
        }

        public static string GetBalanceText(long userId)
        {
            var resources = new Api.Resources(userId);
            string text = $"💰 Ваш баланс наличными монетами: {resources.Money}" +
                $"\n💳 Ваш баланс на банковском счёте: {resources.MoneyCard}" +
                $"\n❓ Деньги можно получить разными способами. Самый простой - взять кредит в банке.";
            return text;
        }
    }
}
