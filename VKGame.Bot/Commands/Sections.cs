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


        public override object Execute(Models.Message msg)
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
            new Skills(),
            new Top()
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
