using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands.Admin
{
    public class News: ICommand
    {
        public string Name => "Новость";
        public string Caption => "аа";
        public string Arguments => "";
        public List<string> Commands => new List<string>();
        public TypeResponse Type => TypeResponse.Text;
        public AccessCommand Access => Access.Admin;

        public object Execute(Models.Message msg)
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
