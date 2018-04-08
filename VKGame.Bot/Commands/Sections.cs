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
            new Start(),
            new Home(),
            new Casino(),
            new Army(),
            new Buildings(),
            new Battle(),
            new Store(),
            new Promocode(),
            new Bank(),
            new Boxes(),
            new Quests(),
            new Referrals(),
            new Clans(),
            new Competitions(),
            new Settings(),
            new Sections(),
            new Balance(),
            new Feedback(),
            new Bug(),
            new Skills()
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
