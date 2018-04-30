using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands.Admin
{
    public class Reboot : ICommand
    {
        public override string Name => "Перезагрузка";
        public override string Caption => "а";
        public override string Arguments => "()";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>();
        public override Access Access => Access.Admin;
        public override string HelpUrl => "недоступно";

        public override object Execute(Models.Message msg)
        {
            return "Ошибка";
        }
    }
}
