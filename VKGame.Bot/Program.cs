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
                    Logger.WriteDebug("Старт бота...");

                    var config = Config.Get();

                    Console.Title = $"War of the World  ver. {config.Version}";

                    Logger.WriteDebug("Создание экземпляра лонгпулла.");
                    var longpoll = new BotsLongPollVK();
                    Logger.WriteDebug("Создан.");

                    Logger.WriteDebug("Создание потока LongPoll.");
                    Thread threadLongPoll = new Thread(new ParameterizedThreadStart(longpoll.Start));
                    threadLongPoll.Name = "LongPoll";
                    threadLongPoll.Start(Common.GetToken());

                    /* Thread threadStatus = new Thread(BackgroundProcess.Common.UpdateStatus);
                     threadStatus.Name = "Status";
                     Logger.WriteDebug("Старт потока Status.");
                     threadStatus.Start();*/

                    /*
                    var stat = new Models.Statistics();
                    stat.AllMessages = 0;
                    stat.Battles = new Models.Statistics.BattlesModel { All = 0, Day = 0 };
                    stat.Boxs = new Models.Statistics.BoxsModel { BuyStoreAll = 0, BuyStoreDay = 0, WinBattleAll = 0, WinBattleDay = 0 };
                    stat.Competitions = new Models.Statistics.CompetitionsModel { All = 0, BattleCompetitionAll = 0, BattleCompetitionDay = 0, JoinPeopleAll = 0, JoinPeopleDay = 0 };
                    stat.CreateArmy = new Models.Statistics.CreateArmyModel { AllSol = 0, AllTanks = 0, DaySol = 0, DayTanks = 0 };
                    stat.CreateClans = new Models.Statistics.CreateClansModel { All = 0, Day = 0 };
                    stat.Errors = new Models.Statistics.ErrorsModel { All = 0, Day = 0 };
                    stat.GoHomeDay = 0;
                    stat.InMessageDay = 0;
                    stat.KreditsAll = 0;
                    stat.OutMessageDay = 0;
                    stat.PromocodesAll = 0;
                    stat.RefferalAll = 0;
                    stat.Registrations = new Models.Statistics.RegistrationsModel { All = 0, Day = 0 };
                    stat.WinBattleAll = 0;
                    stat.WinBattleDay = 0;
                    stat.WinCasino = new Models.Statistics.WinCasinoModel { All = 0, Day = 0 };

                    Bot.Statistics.SetStat(stat);
                   */


                    Thread threadCompetitions = new Thread(BackgroundProcess.Competitions.StartCompetition);
                    threadCompetitions.Name = "threadCompetitions";
                    threadCompetitions.Start();
                    Logger.WriteDebug("Старт потока threadCompetitions.");

                    Thread threadResetMembers = new Thread(BackgroundProcess.Common.ResetMembers);
                    threadResetMembers.Name = "threadResetMembers";
                    threadResetMembers.Start();
                    Logger.WriteDebug("Старт потока threadResetMembers.");

                  /*  Thread threadServer = new Thread(BackgroundProcess.Common.StartServer);
                    threadServer.Name = "threadServer";
                    threadServer.Start();
                    Logger.WriteDebug("Старт потока threadServer.");*/

                    Thread threadStatistics = new Thread(BackgroundProcess.Statistics.StartAdd);
                    threadStatistics.Name = "threadStatistics";
                    threadStatistics.Start();
                    Logger.WriteDebug("Старт потока threadStatistics.");

                    Thread threadCreditCheck = new Thread(BackgroundProcess.Bank.CheckCreditList);
                    Logger.WriteDebug($"Старт потока threadCreditCheck");
                    threadCreditCheck.Name = $"threadCreditCheck";
                    threadCreditCheck.Start();

                    Thread threadDailyBonus = new Thread(BackgroundProcess.Common.DailyBonus);
                    Logger.WriteDebug($"Старт потока threadDailyBonus");
                    threadDailyBonus.Name = $"threadDailyBonus";
                    threadDailyBonus.Start();

                    var listUser = Api.UserList.GetList();
                    foreach (var user in listUser.Users)
                    {
                        var registry = Api.Registry.GetRegistry(user);
                        //var userModel = Api.User.GetUser(user);
                        try
                        {
                            DateTime lastMessage;
                            try
                            {
                                lastMessage = DateTime.Parse(registry.LastMessage);

                            }catch(FormatException)
                            {
                                lastMessage = DateTime.Now;
                                registry.LastMessage = lastMessage.ToString();
                                Api.Registry.SetRegistry(registry);
                            }
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
                            Statistics.NewError();
                            Logger.WriteError(e);
                        }
                    }
                    longpoll.NewMesageEvent += Core.NewMessage;
                    longpoll.UserJoinEvent += Core.JoinInGroup;

                    var argumentsArg = Console.ReadLine();
                }catch(Exception e)
                {
                    Statistics.NewError();
                    Logger.WriteError(e);
                }
                
            }
           
        }
    }
}
