using System;
using System.Threading;
using VKGame.Bot;

namespace VKGame.Bot 
{
    /// <summary>
    /// Класс уведомлений пользователей
    /// </summary>
    public class Notifications 
    {



        /// <summary>
        /// Уведомление и пополнение баланса пользователя
        /// </summary>
        /// <param name="count">Количество монет</param>
        /// <param name="Индентификатор пользователя"></param>
        /// <param name="Имя раздела, который пополнил"></param>
        /// <returns></returns>
        public static bool EnterPaymentCard(int count, long id, string name) 
        {
            var resources = new Api.Resources(id);
            var balance = resources.MoneyCard;
            balance+=count;
            resources.MoneyCard = balance;
            Api.Message.Send($"✨ Платёжное уведомление!✨" +
                $"\n" +
                $"\n ➡ Вам поступил платёж в размере {count} 💳 от: {name}" +
                $"\n 💳 Ваш баланс: {resources.MoneyCard}", id);
            return true;
        }

        /// <summary>
        /// Уведомление на домашней странице вверху
        /// </summary>
        /// <param name="notify">текст</param>
        /// <returns></returns>
        public static bool MainNotify(string notify)
        {
            Common.Notification = notify;
            return true;
        }

        /// <summary>
        /// Отправить сообщение всем пользователям бота
        /// </summary>
        /// <param name="text"> Текст</param>
        /// <param name="count">Количество</param>
        /// <returns></returns>
        public static bool SendAllMessage(string text, int count)
        {
            var vk = Common.GetVk();
            int wait = 100;

            if (count > 15) wait = 1500;
            var dialogs = vk.Messages.GetDialogs(new VkNet.Model.RequestParams.MessagesDialogsGetParams
            {
                Count = Convert.ToUInt32(count),
                Offset = 0
            }).Messages;

            foreach(var dialog in dialogs)
            {
                var userId = dialog.UserId;

                Api.Message.Send($"❤ Уведомление от администрации!❤" +
                                "\n{text}", userId.Value);
                Thread.Sleep(wait);
            }
            return true;
        }

        /// <summary>
        /// Уведомление и снятие денег с счёта
        /// </summary>
        /// <param name="count">Количество монет</param>
        /// <param name="id"> Индентификатор пользователя</param>
        /// <param name="name"> Имя раздела, который сънял деньки</param>
        /// <returns>Результат снятия</returns>
        public static bool RemovePaymentCard(int count, long id, string name) 
        {
            var resources = new Api.Resources(id);
            var balance = resources.MoneyCard;

            if(balance < count) return false;

            balance -= count;
            resources.MoneyCard = balance;
            Api.Message.Send($"✨ Платёжное уведомление!✨" +
                $"\n" +
                $"\n ➡ С Вашего банковского счёта было снято {count} 💳 на счёт: {name}" +
                $"\n 💳 Ваш баланс: {resources.MoneyCard}", id);
            return true;
        }
    }
}