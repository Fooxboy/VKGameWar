using System.Collections.Generic;

namespace VKGame.Bot
{
    /// <summary>
    /// Базавый класс команд, от которого наследуются все команды
    /// </summary>
    public abstract class ICommand
    {
        /// <summary>
        /// Имя
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Доступные аргрументы
        /// </summary>
        public abstract string Arguments { get; }

        /// <summary>
        /// Краткое описание
        /// </summary>
        public abstract string Caption { get; }

        /// <summary>
        /// Тип ответа
        /// </summary>
        public abstract TypeResponse Type { get; }

        /// <summary>
        /// ЮРЛ помощи
        /// </summary>
        public abstract string HelpUrl { get; }

        /// <summary>
        /// Метод выполнения
        /// </summary>
        /// <param name="msg">объект сообщения</param>
        /// <returns>результат</returns>
        public abstract object Execute(Models.Message msg);

        /// <summary>
        /// Доступные команды
        /// </summary>
        public abstract List<string> Commands { get; }

        /// <summary>
        /// Права доступа для команды
        /// </summary>
        public abstract Access Access { get; }

        /// <summary>
        /// Подкоманда "помощь"
        /// </summary>
        /// <param name="msg">Объект сообщения</param>
        /// <returns>помощь по команде</returns>
        [Attributes.Trigger("помощь")]
        public object Help(Models.Message msg)
        {
            string commands = string.Empty;
            foreach (var command in Commands) commands += $"\n 👍 {command}";
            return $"❓ Помощь по разделу {Name}." +
                $"\n ➡ Описание: {Caption}" +
                $"\n ➡ Аргументы: {Arguments}" +
                $"\n ➡ Доступные подкоманды: {commands}" +
                $"\n 👀 Полное описание этого раздела: {HelpUrl}";
        }

        /// <summary>
        /// Поучение ссылки на помощь
        /// </summary>
        /// <param name="msg">объект сообщения</param>
        /// <returns></returns>
        [Attributes.Trigger("ссылка")]
        public object Urls(Models.Message msg)
        {
            return $"👀 Ссылка на подробное описание: {HelpUrl}";
        }
    }

    /// <summary>
    /// Тип ответа
    /// </summary>
    public enum TypeResponse
    {
        Text, Photo, TextAndPhoto, Console
    }
}