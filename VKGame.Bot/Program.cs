using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


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
                    //var config = Config.Get();
                    const double Version = 1.4;
                    Console.Title = $"War of the World  ver. {Version}";

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

                    var registryBot = new RegistryBot();
                    if(registryBot.RunForReboot)
                    {
                        var turnTankJson = String.Empty;
                        using(var reader = new StreamReader(@"Files/Temp/tempTanks.json"))
                        {
                            turnTankJson = reader.ReadToEnd();
                        }
                        var turnSolJson = String.Empty;
                        using (var reader = new StreamReader(@"Files/Temp/tempSol.json"))
                        {
                            turnTankJson = reader.ReadToEnd();
                        }

                        File.Delete(@"Files/Temp/tempTanks.json");
                        File.Delete(@"Files/Temp/tempSol.json");
                        var turnTank = JsonConvert.DeserializeObject<List<Models.UserTurnCreate>>(turnTankJson);
                        var turnSol = JsonConvert.DeserializeObject<List<Models.UserTurnCreate>>(turnSolJson);

                        foreach(var turnT in turnTank)
                            new Task(() => BackgroundProcess.Army.CreateTanks(new Models.DataCreateSoldiery() { UserId = turnT.Id, Count = Convert.ToInt32(turnT.Count) })).Start();

                        foreach (var turnS in turnSol)
                            new Task(() => BackgroundProcess.Army.CreateSoldiery(new Models.DataCreateSoldiery() { UserId = turnS.Id, Count = Convert.ToInt32(turnS.Count) })).Start();

                        if (registryBot.PlayInRulette)
                            new Task(BackgroundProcess.Casino.TimerTriggerRoulette).Start();

                        registryBot.RunForReboot = false;
                    }

                    var listUser = Api.User.AllList;
                    foreach (var user in listUser)
                    {
                        var registry = new Api.Registry(user);
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
                            }
                            int day = lastMessage.Day;
                            int nowDay = 0;
                            if (lastMessage.Month == DateTime.Now.Month) nowDay = DateTime.Now.Day;
                            else nowDay = DateTime.Now.Day + 31;
                            if (nowDay - day < 2)
                            {
                                Thread threadAddingResource = new Thread(new ParameterizedThreadStart(BackgroundProcess.Buildings.AddingResources));
                                Logger.WriteDebug($"Старт потока AddResource_{user}");
                                threadAddingResource.Name = $"AddResource_{user}";
                                threadAddingResource.Start(user);
                            }else
                            {
                                registry.StartThread = false;
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
                    Process.GetCurrentProcess().Exited += Core.BotOffline;

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
