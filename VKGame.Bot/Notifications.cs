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
                $"\n Вам поступил платёж в размере {count} 💳 от: {name}", id);
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
                $"\n С Вашего банковского счёта было снято {count} 💳 на счёт: {name}", id);
            return true;
        }
    }
}