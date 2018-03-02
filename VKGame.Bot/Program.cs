using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace VKGame.Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.WriteWaring("Старт бота...");
            Console.Title = "MyGameVK.";
            Logger.WriteWaring("Создание экземпляра лонгпулла.");
            var starter = new LongPollVK.StarterLongPoll();
            Logger.WriteWaring("Создан.");
            const string token = "Тут токен, который никому не нужен.ы";
            Logger.WriteWaring("Создание потока LongPoll.");
            Thread threadLongPoll = new Thread(new ParameterizedThreadStart(starter.Start));
            threadLongPoll.Name = "LongPoll";
            threadLongPoll.Start(token);
            Logger.WriteWaring("Поток запущен.");
            Thread threadStatus = new Thread(BackgroundProcess.Common.UpdateStatus);
            threadStatus.Name = "Status";
            Logger.WriteWaring("Старт потока статус.");
            threadStatus.Start();
            var listUser = Api.UserList.GetList();
            foreach(var user in listUser.Users) 
            {
                var userModel = Api.User.GetUser(user);
                if(Int32.Parse(userModel.LastMessage) < 5) 
                {
                    Thread threadAddingResource = new Thread(new ParameterizedThreadStart(BackgroundProcess.Buildings.AddingResources));
                    Logger.WriteDebug($"Старт потока AddResource {user}");
                    threadAddingResource.Name = $"AddResource {user}";
                    threadAddingResource.Start(user);
                }
            }
            try 
            {
                starter.AddNewMsgEvent += Core.NewMessage;
            }catch(Exception e) 
            {
                Logger.WriteError(e.Message);
            }
            
            var argumentsArg = Console.ReadLine();
        }
    }
}
