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
            new Bot.Commands.Settings(),
            new Bot.Commands.Sections(),
            new Bot.Commands.Balance()
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
                Logger.WriteError(e);
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
            var msg = (Models.Message)msgObj;
            try
            {
                ICommand command = Proccesing(msg.body.Split(' ')[0].ToLower());
                if (command != null)
                {

                    object result = command.Execute(msg);

                    if (command.Type == TypeResponse.Text)
                    {
                        Api.MessageSend((string)result, msg.from_id);
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
                            Api.MessageSend($"🎈 ОШИБКА: {e.InnerException.Message}" +
                            $"\n 🎉 Исключение: {e.InnerException.GetType().Name}" +
                            $"\n 🎠 StackTrace: {e.InnerException.StackTrace}", msg.from_id);

                            Statistics.NewError();
                        }
                        else
                        {
                            Api.MessageSend($"🎈  ОШИБКА: {e.Message}" +
                             $"\n 🎈  Исключение: {e.GetType().Name}" +
                            $"\n 🎈  StackTrace: {e.StackTrace}", msg.from_id);
                        }

                    }
                    else
                    {
                        Statistics.NewError();

                        Api.MessageSend("😘 Что-то пошло не так. Попробуй-те ещё раз. Если будет опять эта надпись, то, скорее всего это не сейчас работает.", msg.from_id);
                        Logger.WriteError(e.InnerException);
                    }
                }catch(Exception e2)
                {
                    Statistics.NewError();

                    Api.MessageSend($"🎈 ОШИБКА: \n{e2.Message}" +
                            $"\n 🎉 Исключение: {e2.GetType().Name}" +
                           $"\n 🎠 StackTrace: {e2.StackTrace}", msg.from_id);
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
        public static void NewMessage(Models.Message message)
        {
            try
            {
                Logger.WriteDebug("Обработка сообщения...");
                Common.LastMessage = message.id;
                var messagesCache = Api.CacheMessages.GetList();
                if (messagesCache == null) messagesCache = new Models.MessagesCache() { Message = new List<Models.MessageCache>() };
                if (messagesCache.Message == null) messagesCache.Message = new List<Models.MessageCache>();
                messagesCache.Message.Add(new Models.MessageCache { Text = message.body, Id = message.id, PeerId = message.from_id, Time = message.date.ToString() });
                Api.CacheMessages.SetList(messagesCache);
                var user = Api.User.GetUser(message.from_id);
                if (user != null)
                {
                    if (DateTime.Parse(user.LastMessage).Day != DateTime.Now.Day)
                    {
                        user.LastMessage = DateTime.Now.ToString();
                        Api.User.SetUser(user);
                    }
                    Logger.NewMessage($"({message.from_id}) -> {message.body}");
                    var core = new Core();
                    try
                    {
                        var thread = new Thread(new ParameterizedThreadStart(core.ExecutorCommand));
                        thread.Start(message);
                    }
                    catch (Exception e)
                    {
                        Statistics.NewError();

                        Logger.WriteError(e);
                    }
                }
                else
                {
                    var command = message.body.Split(' ')[0].ToLower();
                    if (command == "старт")
                    {
                        Logger.NewMessage($"({message.from_id}) -> {message.body}");
                        var core = new Core();
                        try
                        {
                            var thread = new Thread(new ParameterizedThreadStart(core.ExecutorCommand));
                            thread.Start(message);
                        }
                        catch (Exception e)
                        {
                            Statistics.NewError();

                            Logger.WriteError(e);
                        }
                    }
                    else
                    {
                        Api.MessageSend($"Вы ещё не зарегистрированны в нашей игре! Напишите: старт", message.from_id);
                    }
                }
                Statistics.SendMessage();
                Statistics.InMessage();
            }catch(Exception e)
            {
                Statistics.NewError();
                Logger.WriteError(e);
            }          
        }
    }
}
