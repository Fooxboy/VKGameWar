using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Sections:ICommand
    {
        public string Name => "разделы";
        public string Caption => "Раздел предназначен для вывода списка разделов.";
        public string Arguments => "()";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
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
            new Bot.Commands.Balance()
            };

            string text = $"➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\nСПИСОК ДОСТУПНЫХ РАЗДЕЛОВ:" +
                $"";

            foreach(var command in listCommand)
            {
                text += $"\n 😀 Название: {command.Name}" +
                    $"\n 🎉 Аргументы: {command.Arguments}" +
                    $"\n ❓ Описание: {command.Caption}" +
                    $"\n";

            }

            text += "\n➖➖➖➖➖➖➖➖➖➖➖➖";

            return text;
        }
    }
}
