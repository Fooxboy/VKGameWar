using System;
using System.Collections.Generic;
using System.Text;
using VKGame.Bot.Commands;
using System.Threading;

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
            new Bot.Commands.Battle(),
            new Bot.Commands.Store()
        };
        
        private ICommand Proccesing(string text)
        {
            try 
            {
                foreach (var command in Commands)
                {
                    if (command.Name.ToLower() == text) return command;
                }
                return null;
            }catch (Exception e) 
            {
                Logger.WriteError(e.Message);
                return null;
            }
            
        }
        
        /// <summary>
        /// Выполняющий команды.
        /// </summary>
        /// <param name="Сообщение"></param>
        public void ExecutorCommand(object msgObj)
        {
            try 
            {
                var msg = (LongPollVK.Models.AddNewMsg)msgObj;
                ICommand command = Proccesing(msg.Text.Split(' ')[0].ToLower());
                if (command != null)
                {

                    object result = command.Execute(msg);

                    if (command.Type == TypeResponse.Text)
                    {
                        Api.MessageSend((string)result, msg.PeerId);
                    }
                    else if (command.Type == TypeResponse.Photo)
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
            }catch(Exception e ) 
            {
                Logger.WriteError(e.Message);
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
            var user = Api.User.GetUser(message.PeerId);
            if(user != null) 
            {
                if (user.LastMessage != DateTime.Now.Day.ToString())
                {
                    user.LastMessage = DateTime.Now.Day.ToString();
                    Api.User.SetUser(user);
                }
            }
            
            Statistics.InMessage();
            Logger.WriteDebug($"({message.PeerId}) -> {message.Text}");
            var core = new Core();
            try 
            {
                var thread = new Thread(new ParameterizedThreadStart(core.ExecutorCommand));
                //Logger.WriteDebug("Старт потока ответа на сообщение.");
                thread.Start(message);
            }catch(Exception e) 
            {
                Logger.WriteError(e.Message);
                Api.MessageSend($"Включен режим отладки. ОШИБКА: \n {e.Message}", message.PeerId);
            }
            
        }
    }
}
