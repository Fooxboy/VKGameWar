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

        //метод выполняющий поиск нужной команды.
        private ICommand Proccesing(string text)
        {
            try 
            {
                //перебор команд
                foreach (var command in Common.Commands)
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
                //получение команды после поиска.
                ICommand command = Proccesing(msg.body.Split(' ')[0].ToLower());
                if (command != null)
                {

                    var lastCommands = Common.LastCommand;
                    try
                    {
                        var buffer = lastCommands[msg.from_id];
                        lastCommands[msg.from_id] = command;
                    }
                    catch (KeyNotFoundException)
                    {

                        lastCommands.Add(msg.from_id, command);
                    }

                    //создание объектов пользования и проверка его прав.
                    var user = new Api.User(msg.from_id);
                    object result;
                    if((int)user.Access < (int)command.Access)
                    {
                        result = "❌ У Вас нет прав для выполнения команды!";
                    }else result = command.Execute(msg);

                    if (command.Type == TypeResponse.Text)
                    {
                        //отправка сообщений

                        //Api.Message.Send((string)result, msg.from_id);
                        string wait = "🔁 Подождите. Команда выполняется 🔁";
                         var messageId = Api.Message.Send(wait, msg.from_id);
                         if(messageId != 0)
                         {
                             string text = (string)result;
                             var resultEdit = Api.Message.Edit(text, messageId, msg.from_id);
                             if(!resultEdit)
                             {
                                Api.Message.Send("❌ Ошибка. Команда не смогла быть выполнена.", msg.from_id);
                                
                             }
                         }
                         //Обнуляем данные, чтобы сбощик мусора собрал их
                        command = null;
                        //Запускаем вручную сборку мусора
                        GC.Collect();
                        
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
                    NoCommand.Execute(msg);
            }
            catch (Exception e)
            {
                //Вывод ошибок при отправке или обработке команд или сообщений.
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
                    e = null;
                    //Запускаем вручную сборку мусора
                    GC.Collect();
                }
                catch(Exception e2)
                {
                    //обработка ошибок, если произошла ошибка.
                    Statistics.NewError();
                    Api.Message.Send($"🎈 ОШИБКА: \n{e2.Message}" +
                            $"\n 🎉 Исключение: {e2.GetType().Name}" +
                           $"\n 🎠 StackTrace: {e2.StackTrace}", msg.from_id);
                    e = null;
                    //Запускаем вручную сборку мусора
                    GC.Collect();
                }          
            }      
        }

        //обработчик события "уход из группы"
        public static void LeaveInGroup(Models.UserLeave userId)
        {
            Logger.WriteWaring($"Группу покинул пользователь {userId.user_id}");
            if (Api.User.Check(userId.user_id))
            {
                var user = new Api.User(userId.user_id);
                var registry = new Api.Registry(userId.user_id);
                registry.IsLeaveIsGroup = true;
                Api.Message.Send("😭 Постооой... Ну куда же ты??? Что тебе не понравилось? Ботом можно пользоваться даже, когда ты не подписан на группу, но все же..." +
                    "\n ❓ Хочешь написать положительный или отрицательный отзыв? Напиши: Отзыв <текст> ", user.Id);
            }
        }

        //Обработчик события "приход в группу"
        private static void JoinInGroupHealder(Models.UserJoin userId)
        {
            Logger.WriteWaring($"В группу вступил новый участник {userId.user_id}");
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

        //метод вызывающий обработчик события
        public static void JoinInGroup(Models.UserJoin userId) => new Task(() => JoinInGroupHealder(userId));

        private static void BotOfflineHeadler(object sender, EventArgs e)
        {
            Console.WriteLine("Бот выключен.");
        }

        public static void BotOffline(object sender, EventArgs e) => new Task(() => BotOfflineHeadler(sender, e)).Start();


        //обработчик прихода нового сообщения.
        private static void NewMessageHalder(Models.Message message)
        {
            try
            {
                //запись последнего id  
                Common.LastMessage = message.id;
                        
                //добавление в кэш сообщений
                Api.CacheMessages.AddMessage(message.id, 
                    DateTime.Now.ToString(), 
                    1, 1, message.from_id,
                    message.body, 
                    message.from_id);

                //проверка на существование пользователя в бд.
                if (Api.User.Check(message.from_id))
                {
                    //создание объектов пользователя
                    var user = new Api.User(message.from_id);
                    var registry = new Api.Registry(user.Id);
                    //указание даты последнего сообщения пользователя
                    if (DateTime.Parse(registry.LastMessage).Day != DateTime.Now.Day)
                    {
                        registry.LastMessage = DateTime.Now.ToString();
                    }
                    //вывод лога
                    Logger.NewMessage($"({message.from_id}) -> {message.body}");
                    //запуск потока добавление ресурсов, если он не запущен.
                    if(!registry.StartThread) new Thread(new ParameterizedThreadStart(BackgroundProcess.Buildings.AddingResources)).
                        Start(message.from_id);
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
                    //если пользователь не зарегестрирован.
                    var command = message.body.Split(' ')[0].ToLower();
                    //если команда старт........
                    if (command.ToLower() == "старт")
                    {
                        var lastCommands = Common.LastCommand;
                        try
                        {
                            var buffer = lastCommands[message.from_id];
                            lastCommands[message.from_id] = new Start();
                        }
                        catch (KeyNotFoundException)
                        {

                            lastCommands.Add(message.from_id, new Start());
                        }

                        Logger.NewMessage($"({message.from_id}) -> {message.body}");
                        try
                        {
                            Api.Message.Send((string)Common.Commands[0].Execute(message), message.from_id);
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
