using System.Collections.Generic;

namespace VKGame.Bot
{
    public class ICommand
    {
        public virtual string Name { get; }
        public virtual string Arguments { get; }
        public virtual string Caption { get; }
        public virtual TypeResponse Type { get; }
        public virtual string HelpUrl { get; }
        public virtual object Execute(Models.Message msg)
        {
            return "❌ Команда не имеет обработчик.";
        }
        public virtual List<string> Commands { get; }
        public virtual  Access Access { get; }

        [Attributes.Trigger("помощь")]
        public object Help(Models.Message msg)
        {
            string commands = string.Empty;
            foreach (var command in Commands) commands += $"\n 👍 {command}";
            return $"❓Помощь по разделу {Name}." +
                $"\n ➡ Описание: {Caption}" +
                $"\n ➡ Аргументы: {Arguments}" +
                $"\n ➡ Доступные подкоманды: {commands}" +
                $"\n 👀 Полное описание этого раздела: {HelpUrl}";
        }
    }

    public enum TypeResponse
    {
        Text, Photo, TextAndPhoto, Console
    }
}