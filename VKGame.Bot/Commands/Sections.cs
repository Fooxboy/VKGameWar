using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Sections:ICommand
    {
        public override string Name => "Разделы";
        public override string Caption => "Раздел предназначен для вывода списка разделов.";
        public override string Arguments => "()";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>();
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";


        public override object Execute(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);

            var listCommand = Common.Commands;

            string text = $"➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\nСПИСОК ДОСТУПНЫХ РАЗДЕЛОВ:" +
                $"";

            foreach(var command in listCommand)
            {
                if ((int)command.Access <= user.Access)
                {
                    string commandsInCommand = String.Empty;

                    foreach (var commandInCommand in command.Commands)
                    {
                        commandsInCommand += $"{commandInCommand}, ";
                    }

                    text += $"\n 😀 Название: {command.Name}" +
                        $"\n ❓ Напишите: {command.Name} помощь, чтобы получить помощь о команде";
                }         
            }

            text += "\n➖➖➖➖➖➖➖➖➖➖➖➖";

            return text;
        }
    }
}
