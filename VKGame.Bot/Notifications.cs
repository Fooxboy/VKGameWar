using System;
using VKGame.Bot;

namespace VKGame.Bot 
{
    public class Notifications 
    {
        public static bool EnterPaymentCard(int count, long id, string name) 
        {
            var resources = new Api.Resources(id);

            var balance = resources.MoneyCard;
            balance+=count;
            resources.MoneyCard = balance;
            Api.MessageSend($"✨ Платёжное уведомление!✨" +
                $"\n" +
                $"\n ➡ Вам поступил платёж в размере {count} 💳 от: {name}" +
                $"\n 💳 Ваш баланс: {resources.MoneyCard}", id);
            return true;
        }

        public static bool MainNotify(string notify)
        {
            Common.Notification = notify;
            return true;
        }

        public static bool SendAllMessage(string text, int count)
        {
            var vk = Common.GetVk();
            var dialogs = vk.Messages.GetDialogs(new VkNet.Model.RequestParams.MessagesDialogsGetParams
            {
                Count = Convert.ToUInt32(count),
                Offset = 0
            }).Messages;

            foreach(var dialog in dialogs)
            {
                var userId = dialog.UserId;

                Api.MessageSend(text, userId.Value);
            }
            return true;
        }

        public static bool RemovePaymentCard(int count, long id, string name) 
        {
            var resources = new Api.Resources(id);
            var balance = resources.MoneyCard;

            if(balance < count) return false;

            balance -= count;
            resources.MoneyCard = balance;
            Api.MessageSend($"✨ Платёжное уведомление!✨" +
                $"\n" +
                $"\n ➡ С Вашего банковского счёта было снято {count} 💳 на счёт: {name}" +
                $"\n 💳 Ваш баланс: {resources.MoneyCard}", id);
            return true;
        }
    }
}