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
            while(true)
            {
                try
                {
                    Logger.WriteWaring("Старт бота...");
                    Console.Title = "MyGameVK";
                    Logger.WriteDebug("Создание экземпляра лонгпулла.");
                    var starter = new LongPollVK.StarterLongPoll();
                    Logger.WriteWaring("Создан.");
                    const string token = "Тут токен, который никому не нужен.ы";
                    Logger.WriteDebug("Создание потока LongPoll.");
                    Thread threadLongPoll = new Thread(new ParameterizedThreadStart(starter.Start));
                    threadLongPoll.Name = "LongPoll";
                    threadLongPoll.Start(token);
                    Thread threadStatus = new Thread(BackgroundProcess.Common.UpdateStatus);
                    threadStatus.Name = "Status";
                    Logger.WriteDebug("Старт потока Status.");
                    threadStatus.Start();
                    Thread threadCreditCheck = new Thread(BackgroundProcess.Bank.CheckCreditList);
                    Logger.WriteDebug($"Старт потока threadCreditCheck");
                    threadCreditCheck.Name = $"threadCreditCheck";
                    threadCreditCheck.Start();
                    Thread threadDailyBonus = new Thread(BackgroundProcess.Common.DailyBonus);
                    Logger.WriteDebug($"Старт потока threadDailyBonus");
                    threadDailyBonus.Name = $"threadDailyBonus";
                    threadDailyBonus.Start();

                    //  var threadReboot = new Thread(BackgroundProcess.Common.RebootBot);
                    //  threadReboot.Name = "Reboot";
                    // Logger.WriteWaring("Старт потока Reboot.");
                    // threadReboot.Start();

                    var listUser = Api.UserList.GetList();
                    foreach (var user in listUser.Users)
                    {
                        var userModel = Api.User.GetUser(user);
                        try
                        {
                            var lastMessage = DateTime.Parse(userModel.LastMessage);
                            int day = lastMessage.Day;
                            int nowDay = 0;
                            if (lastMessage.Month == DateTime.Now.Month) nowDay = DateTime.Now.Day;
                            else nowDay = DateTime.Now.Day + 31;
                            if (DateTime.Now.Day - day < 5)
                            {
                                Thread threadAddingResource = new Thread(new ParameterizedThreadStart(BackgroundProcess.Buildings.AddingResources));
                                Logger.WriteDebug($"Старт потока AddResource_{user}");
                                threadAddingResource.Name = $"AddResource_{user}";
                                threadAddingResource.Start(user);
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.WriteError($"{e.Message} \n {e.StackTrace} \n{e.Source}");
                        }

                    }
                    starter.AddNewMsgEvent += Core.NewMessage;

                    var argumentsArg = Console.ReadLine();
                }catch(Exception e)
                {
                    Logger.WriteError($"{e.Message} \n {e.StackTrace} \n{e.Source}");
                }
                
            }
           
        }
    }
}
