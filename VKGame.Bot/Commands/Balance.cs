using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Balance:ICommand
    {
        public override string Name => "Баланс";
        public override string Arguments => "()";
        public override string Caption => "Раздел для вывода информации о Вашем балансе.";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string> { "отнять", "прибавить", "узнать"};
        public override Access Access => Access.User;

        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetBalanceText(msg.from_id);
            
            var type = typeof(Balance);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        [Attributes.Trigger("отнять")]
        public string RemoveMoney(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
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
            var user = new Api.User(msg.from_id);
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
            var user = new Api.User(msg.from_id);
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
