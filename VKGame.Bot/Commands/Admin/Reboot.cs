using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands.Admin
{
    public class Reboot : ICommand
    {
        public string Name => "Перезагрузка";
        public string Caption => "а";
        public string Arguments => "()";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>();
        public Access Access => Access.Admin;

        public object Execute(Models.Message msg)
        {

            return null;
        }
    }
}
