using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands.Admin
{
    public class NotifyAll : ICommand
    {
        public override string Name => "Уведомление";
        public override string Caption => "аа";
        public override string Arguments => "";
        public override List<string> Commands => new List<string>();
        public override TypeResponse Type => TypeResponse.Text;
        public override Access Access => Access.Admin;
        public override string HelpUrl => "недоступно";

        public override object Execute(Models.Message msg)
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
