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
            new Bot.Commands.Store(),
            new Bot.Commands.Promocode(),
            new Bot.Commands.Bank(),
            new Bot.Commands.Boxes(),
            new Bot.Commands.Quests(),
            new Bot.Commands.Referrals(),
            new Bot.Commands.Clans(),
            new Bot.Commands.Competitions(),
            new Bot.Commands.Database(),
            new Bot.Commands.ExecuteCode(),
            new Bot.Commands.Settings()
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
                Logger.WriteError($"{e.Message} \n {e.StackTrace} \n{e.Source}");
                Statistics.NewError();
                return null;
            }
            
        }
        
        /// <summary>
        /// Выполняющий команды.
        /// </summary>
        /// <param name="Сообщение"></param>
        public void ExecutorCommand(object msgObj)
        {
            var msg = (LongPollVK.Models.AddNewMsg)msgObj;
            try
            {
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
            }
            catch (Exception e)
            {
                Statistics.NewError();

                try
                {
                    var config = Config.Get();
                    if (config.IsDebug)
                    {
                        Statistics.NewError();

                        if (e.InnerException != null)
                        {
                            Api.MessageSend($"ОШИБКА: {e.InnerException.Message}" +
                            $"\n Исключение: {e.InnerException.GetType().Name}" +
                            $"\n StackTrace: {e.InnerException.StackTrace}", msg.PeerId);

                            Statistics.NewError();
                        }
                        else
                        {
                            Api.MessageSend($"ОШИБКА: {e.Message}" +
                             $"\n Исключение: {e.GetType().Name}" +
                            $"\n StackTrace: {e.StackTrace}", msg.PeerId);
                        }

                    }
                    else
                    {
                        Statistics.NewError();

                        Api.MessageSend("😘 Что-то пошло не так. Попробуй-те ещё раз. Если будет опять эта надпись, то, скорее всего это не сейчас работает.", msg.PeerId);
                        Logger.WriteError($"ОШИБКА: \n{e.InnerException.Message}" +
                        $"\n Исключение: {e.InnerException.GetType().Name}" +
                        $"\n Стек: {e.InnerException.StackTrace}");
                    }
                }catch(Exception e2)
                {
                    Statistics.NewError();

                    Api.MessageSend($"ОШИБКА: \n{e2.Message}" +
                            $"\n Исключение: {e2.GetType().Name}" +
                           $"\n StackTrace: {e2.StackTrace}", msg.PeerId);
                }
                
                
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
            try
            {
                Common.LastMessage = message.MessageId;
                var messagesCache = Api.CacheMessages.GetList();
                if (messagesCache == null) messagesCache = new Models.MessagesCache() { Message = new List<Models.MessageCache>() };
                if (messagesCache.Message == null) messagesCache.Message = new List<Models.MessageCache>();
                messagesCache.Message.Add(new Models.MessageCache { Text = message.Text, Id = message.MessageId, PeerId = message.PeerId, Time = message.Time });
                Api.CacheMessages.SetList(messagesCache);
                var user = Api.User.GetUser(message.PeerId);
                if (user != null)
                {
                    if (DateTime.Parse(user.LastMessage).Day != DateTime.Now.Day)
                    {
                        user.LastMessage = DateTime.Now.ToString();
                        Api.User.SetUser(user);
                    }
                    Logger.WriteDebug($"({message.PeerId}) -> {message.Text}");
                    var core = new Core();
                    try
                    {
                        var thread = new Thread(new ParameterizedThreadStart(core.ExecutorCommand));
                        thread.Start(message);
                    }
                    catch (Exception e)
                    {
                        Statistics.NewError();

                        Logger.WriteError($"{e.Message} \n {e.StackTrace} \n{e.Source}");
                    }
                }
                else
                {
                    var command = message.Text.Split(' ')[0].ToLower();
                    if (command == "старт")
                    {
                        Logger.WriteDebug($"({message.PeerId}) -> {message.Text}");
                        var core = new Core();
                        try
                        {
                            var thread = new Thread(new ParameterizedThreadStart(core.ExecutorCommand));
                            thread.Start(message);
                        }
                        catch (Exception e)
                        {
                            Statistics.NewError();

                            Logger.WriteError($"{e.Message} \n {e.StackTrace} \n{e.Source}");
                        }
                    }
                    else
                    {
                        Api.MessageSend($"Вы ещё не зарегистрированны в нашей игре! Напишите: старт", message.PeerId);
                    }

                }

                Statistics.SendMessage();
                Statistics.InMessage();
            }catch(Exception e)
            {
                Statistics.NewError();

                Logger.WriteError($"{e.Message} \n {e.StackTrace} \n{e.Source}");
            }          
        }
    }
}
