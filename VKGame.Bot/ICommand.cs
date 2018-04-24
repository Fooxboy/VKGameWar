using System.Collections.Generic;

namespace VKGame.Bot
{
    public abstract class ICommand
    {
        public abstract string Name { get; }
        public abstract string Arguments { get; }
        public abstract string Caption { get; }
        public abstract TypeResponse Type { get; }
        public abstract string HelpUrl { get; }
        public abstract object Execute(Models.Message msg);
        public abstract List<string> Commands { get; }
        public abstract Access Access { get; }

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