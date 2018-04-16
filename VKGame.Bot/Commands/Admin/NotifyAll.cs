using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands.Admin
{
    public class NotifyAll : ICommand
    {
        public string Name => "Уведомление";
        public string Caption => "аа";
        public string Arguments => "";
        public List<string> Commands => new List<string>();
        public TypeResponse Type => TypeResponse.Text;
        public AccessCommand Access => Access.Admin;

        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            int count = Int32.Parse(messageArray[1]);

            string text = String.Empty;

            for (int i = 2; i < messageArray.Length; i++)
                text += $"{messageArray[i]} ";

            Notifications.SendAllMessage(text, count);
            return $"Вы успешно уведомили {count} пользователей. Сообщение: {text} ";
        }
    }
}
