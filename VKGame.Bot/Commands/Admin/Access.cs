using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands.Admin
{
    public class AccessCommand : ICommand
    {
        public string Name => "Доступ";
        public string Arguments => "(), (вариант_выбора)";
        public string Caption => "а";
        public TypeResponse Type => TypeResponse.Text;

        public List<string> Commands =>
            new List<string>() { "бот", "атака", "вступить", "покинуть", "мой", "создать", "список" };

        public Access Access => Access.User;

        public object Execute(Models.Message msg)
        {
            if (msg.from_id != 308764786) return "Вам недоступна текущая команда!";
            var messageArray = msg.body.Split(' ');

            var access = Int64.Parse(messageArray[1]);

            var user = new Api.User(msg.from_id);
            user.Access = access;
            return $"Вы успешно установили себе {access}";
        }
    }
}
