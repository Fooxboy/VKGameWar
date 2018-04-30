using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands.Admin
{
    public class News: ICommand
    {
        public override string Name => "Новость";
        public override string Caption => "аа";
        public override string Arguments => "";
        public override List<string> Commands => new List<string>();
        public override TypeResponse Type => TypeResponse.Text;
        public override Access Access => Access.Admin;
        public override string HelpUrl => "Недоступно";

        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            string text = String.Empty;

            for (int i = 1; i < messageArray.Length; i++)
                text += $"{messageArray[i]} ";

            Notifications.MainNotify(text);

            return $"Теперь главная новость: {text}";
        }

    }
}
