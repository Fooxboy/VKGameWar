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
            Thread threadStatus = new Thread(BackgroundProcess.Common.UpdateStatus);
            threadStatus.Name = "Status";
            Logger.WriteWaring("Старт потока Status.");
            threadStatus.Start();
            Thread threadCreditCheck = new Thread(BackgroundProcess.Bank.CheckCreditList);
            Logger.WriteDebug($"Старт потока threadCreditCheck");
            threadCreditCheck.Name = $"threadCreditCheck";
            threadCreditCheck.Start();

          //  var threadReboot = new Thread(BackgroundProcess.Common.RebootBot);
          //  threadReboot.Name = "Reboot";
           // Logger.WriteWaring("Старт потока Reboot.");
           // threadReboot.Start();

            var listUser = Api.UserList.GetList();
            foreach(var user in listUser.Users) 
            {
                var userModel = Api.User.GetUser(user);
               // if(Int32.Parse(userModel.LastMessage) < DateTime.Now.Day-5) 
               //{
                    Thread threadAddingResource = new Thread(new ParameterizedThreadStart(BackgroundProcess.Buildings.AddingResources));
                    Logger.WriteDebug($"Старт потока AddResource {user}");
                    threadAddingResource.Name = $"AddResource_{user}";
                    threadAddingResource.Start(user);
               // }
            }
            try 
            {
                starter.AddNewMsgEvent += Core.NewMessage;
            }catch(Exception e) 
            {
                Logger.WriteError($"{e.Message} \n {e.StackTrace} \n{e.Source}");
            }
            
            var argumentsArg = Console.ReadLine();
        }
    }
}
