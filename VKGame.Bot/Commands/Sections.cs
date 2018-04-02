using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Sections:ICommand
    {
        public string Name => "Разделы";
        public string Caption => "Раздел предназначен для вывода списка разделов.";
        public string Arguments => "()";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>();
        public Access Access => Access.User;


        public object Execute(Models.Message msg)
        {
            var listCommand = new List<ICommand>()
            {
            new Bot.Commands.Start(),
            new Bot.Commands.Home(),
            new Bot.Commands.Casino(),
            new Bot.Commands.Army(),
            new Bot.Commands.Buildings(),
            new Bot.Commands.Battle(),
            new Bot.Commands.Store(),
            new Bot.Commands.Promocode(),
            new Bot.Commands.Bank(),
            new Bot.Commands.Boxes(),
            new Bot.Commands.Quests(),
            new Bot.Commands.Referrals(),
            new Bot.Commands.Clans(),
            new Bot.Commands.Competitions(),
            new Bot.Commands.Settings(),
            new Bot.Commands.Sections(),
            new Bot.Commands.Balance(),
            new Bot.Commands.Feedback(),
            new Bot.Commands.Bug()
            };

            string text = $"➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\nСПИСОК ДОСТУПНЫХ РАЗДЕЛОВ:" +
                $"";

            foreach(var command in listCommand)
            {
                string commandsInCommand = String.Empty;

                foreach(var commandInCommand in command.Commands)
                {
                    commandsInCommand += $"{commandInCommand}, ";
                }

                text += $"\n 😀 Название: {command.Name}" +
                    $"\n 🎉 Аргументы: {command.Arguments}" +
                    $"\n ➡ Доступные подкоманды: {commandsInCommand}" +
                    $"\n ❓ Описание: {command.Caption}" +
                    $"\n";
            }

            text += "\n➖➖➖➖➖➖➖➖➖➖➖➖";

            return text;
        }
    }
}
