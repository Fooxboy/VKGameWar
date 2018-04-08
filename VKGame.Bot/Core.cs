using System;
using System.Collections.Generic;
using System.Text;
using VKGame.Bot.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace VKGame.Bot
{
    /// <summary>
    /// Класс обработки данных.
    /// </summary>
    public class Core
    {
        public static List<ICommand> Commands = new List<ICommand>()
        {
            new Start(),
            new WriteData(),
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
            new Commands.Database(),
            new ExecuteCode(),
            new Settings(),
            new Sections(),
            new Balance(),
            new Commands.Admin.News(),
            new Commands.Admin.NotifyAll(),
            new Commands.Admin.Reboot(),
            new Feedback(),
            new Bug(),
            new Commands.Admin.Stat(),
            new Commands.Admin.System(),
            new Skills()
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
                    var lastCommands = Common.LastCommand;
                    try
                    {
                        var buffer = lastCommands[msg.from_id];
                        lastCommands[msg.from_id] = command;
                    }catch(KeyNotFoundException)
                    {
                        lastCommands.Add(msg.from_id, command);
                    }

                    var user = new Api.User(msg.from_id);

                    object result;

                    if((int)user.Access < (int)command.Access)
                    {
                        result = "❌ У Вас нет прав для выполнения команды!";
                    }else result = command.Execute(msg);

                    if (command.Type == TypeResponse.Text)
                    {

                        Api.Message.Send((string)result, msg.from_id);
                        /* string wait = "🔁 Подождите. Команда выполняется 🔁";
                         var messageId = Api.MessageSend(wait, msg.from_id);
                         if(messageId != 0)
                         {
                             string text = (string)result;
                             var resultEdit = Api.MessageEdit(text, messageId, msg.from_id);
                             if(!resultEdit)
                             {
                                Api.MessageSend("❌ Ошибка. Команда не смогла быть выполнена.", msg.from_id);
                             }
                         }*/
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
                            Api.Message.Send($"🎈 ОШИБКА: {e.InnerException.Message}" +
                            $"\n 🎉 Исключение: {e.InnerException.GetType().Name}" +
                            $"\n 🎠 StackTrace: {e.InnerException.StackTrace}", msg.from_id);

                            Statistics.NewError();
                        }
                        else
                        {
                            Api.Message.Send($"🎈  ОШИБКА: {e.Message}" +
                             $"\n 🎉  Исключение: {e.GetType().Name}" +
                            $"\n 🎠  StackTrace: {e.StackTrace}", msg.from_id);
                        }
                    }
                    else
                    {
                        Statistics.NewError();

                        Api.Message.Send("😘 Что-то пошло не так. Попробуй-те ещё раз. Если будет опять эта надпись, то, скорее всего это не сейчас работает.", msg.from_id);
                        Logger.WriteError(e.InnerException);
                    }
                }catch(Exception e2)
                {
                    Statistics.NewError();

                    Api.Message.Send($"🎈 ОШИБКА: \n{e2.Message}" +
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

        public static void LeaveInGroup(Models.UserLeave userId)
        {
            if (Api.User.Check(userId.user_id))
            {
                var user = new Api.User(userId.user_id);
                var registry = new Api.Registry(userId.user_id);
                registry.IsLeaveIsGroup = true;
                Api.Message.Send("😭 Постооой... Ну куда же ты??? Что тебе не понравилось? Ботом можно пользоваться даже, когда ты не подписан на группу, но все же..." +
                    "\n ❓ Хочешь написать положительный или отрицательный отзыв? Напиши: Отзыв <текст> ", user.Id);
            }
        }

        private static void JoinInGroupHealder(Models.UserJoin userId)
        {
            var user = new Api.User(userId.user_id);
            if (Api.User.Check(userId.user_id))
            {
                var registry = new Api.Registry(user.Id);
                if (!registry.IsBonusInGroupJoin)
                {
                    Api.Message.Send("❤ Спасибо, что подписался на группу! Здесь будут публиковаться разные новости и промо акции!" +
                        "\n ➡ Вот тебе бонус за подписку в размере 100 💳", user.Id);
                    Notifications.EnterPaymentCard(100, user.Id, "бонус за подписку");
                    registry.IsBonusInGroupJoin = true;
                }
                else
                {
                    Api.Message.Send("❤ Спасибо, что подписался на группу!", user.Id);
                }
            }
        }

        public static void JoinInGroup(Models.UserJoin userId) => new Task(() => JoinInGroupHealder(userId));

        private static void BotOfflineHeadler(object sender, EventArgs e)
        {
            Console.WriteLine("Бот выключен.");
        }

        public static void BotOffline(object sender, EventArgs e) => new Task(() => BotOfflineHeadler(sender, e)).Start();


        private static void NewMessageHalder(Models.Message message)
        {
            try
            {
                Common.LastMessage = message.id;
                
                
                Api.CacheMessages.AddMessage(message.id, DateTime.Now.ToString(), 1, 1, message.from_id, message.body, message.from_id);
                if (Api.User.Check(message.from_id))
                {
                    var user = new Api.User(message.from_id);
                    var registry = new Api.Registry(user.Id);
                    if (DateTime.Parse(registry.LastMessage).Day != DateTime.Now.Day)
                    {
                        registry.LastMessage = DateTime.Now.ToString();
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
                        Api.Message.Send($"💙 Вы ещё не зарегистрированны в нашей игре! Напишите: старт", message.from_id);
                    }
                }
                Statistics.SendMessage();
                Statistics.InMessage();
            }
            catch (Exception e)
            {
                Statistics.NewError();
                Logger.WriteError(e);
            }
        }

        /// <summary>
        /// Обработка нового сообщения.
        /// </summary>
        /// <param name="message"></param>
        public static void NewMessage(Models.Message message) => new Task(() => NewMessageHalder(message)).Start();
    }
}
