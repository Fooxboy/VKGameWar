using System;
using System.Collections.Generic;
using System.Text;
using VKGame.Bot.Commands;

namespace VKGame.Bot
{
    /// <summary>
    /// Класс обработки данных.
    /// </summary>
    public class Core
    {
        private List<ICommand> Commands = new List<ICommand>()
        {
            new Bot.Commands.Start(),
            new Bot.Commands.WriteData(),
            new Bot.Commands.Home(),
            new Bot.Commands.Casino(),
            new Bot.Commands.Army(),
            new Bot.Commands.Buildings(),
            new Bot.Commands.Battle()
        };
        
        private ICommand Proccesing(string text)
        {
            foreach (var command in Commands)
            {
                if (command.Name.ToLower() == text) return command;
            }
            return null;
        }
        
        /// <summary>
        /// Выполняющий команды.
        /// </summary>
        /// <param name="Сообщение"></param>
        public void ExecutorCommand(LongPollVK.Models.AddNewMsg msg)
        {
            ICommand command = Proccesing(msg.Text.Split(' ')[0].ToLower());
            if (command != null)
            {
                object result = command.Execute(msg);
            
                if (command.Type == TypeResponse.Text)
                {
                    Api.MessageSend((string)result, msg.PeerId);
                }
                else if(command.Type == TypeResponse.Photo)
                {
                    //отправка фото
                }
                else if (command.Type == TypeResponse.TextAndPhoto)
                {
                    //отправка фото с текстом
                }
                else if (command.Type == TypeResponse.Console)
                {
                    Console.WriteLine((string)result);
                }
            }
            else
            {
                NoCommand.Execute(msg);
            }
            
        }
        
        /// <summary>
        /// Регистрация команд.
        /// </summary>
        /// <param name="Команда"></param>
        public void RegisterCommand(ICommand command)
        {
            if (Commands != null) Commands.Add(command);
            else Commands = new List<ICommand>() {command};
        }
        
        
        /// <summary>
        /// Обработка нового сообщения.
        /// </summary>
        /// <param name="message"></param>
        public static void NewMessage(LongPollVK.Models.AddNewMsg message)
        {
            Statistics.InMessage();
            Logger.WriteDebug($"({message.PeerId}) -> {message.Text}");
            var core = new Core();
            core.ExecutorCommand(message);
        }
    }
}
