using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands.Admin
{
    public class AccessCommand : ICommand
    {
        public override string Name => "Доступ";
        public override string Arguments => "(), (вариант_выбора)";
        public override string Caption => "а";
        public override TypeResponse Type => TypeResponse.Text;

        public override List<string> Commands =>
            new List<string>() { };

        public override string HelpUrl => "недоступно";
        public override Access Access => Access.User;

        public override object Execute(Models.Message msg)
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
