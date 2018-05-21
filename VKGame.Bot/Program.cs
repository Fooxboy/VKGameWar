﻿using System;
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
        /*---------------------------------------------------------------------------
        Запускается бот в тестовом режиме или в релизе.
        Если testmode = true, тогда бот запускается в  группе https://vk.com/waroftheworldtest
        Для тестирования функционала использовать в значении true
        ---------------------------------------------------------------------------*/
        public static bool TestMode = true; 

        static void Main(string[] args)
        {
            while(true)
            {
                try
                {
                    //логгирование и запуск потоков..
                    Logger.WriteDebug("Старт бота...");
                    const string Version = "1.5 beta";
                    Console.Title = $"War of the World  ver. {Version}";

                    Logger.WriteDebug("Создание экземпляра лонгпулла.");
                    var longpoll = new BotsLongPollVK();
                    Logger.WriteDebug("Создан.");

                    Logger.WriteDebug("Создание потока LongPoll.");
                    Thread threadLongPoll = new Thread(new ParameterizedThreadStart(longpoll.Start));
                    threadLongPoll.Name = "LongPoll";
                    Common.IsTestingMode = TestMode;
                    if(TestMode)
                    {
                        threadLongPoll.Start(Common.GetTestToken());
                    }else
                    {
                        threadLongPoll.Start(Common.GetToken());
                    }             

                    Thread threadCompetitions = new Thread(BackgroundProcess.Competitions.StartCompetition);
                    threadCompetitions.Name = "threadCompetitions";
                    threadCompetitions.Start();
                    Logger.WriteDebug("Старт потока threadCompetitions.");

                    Thread threadResetMembers = new Thread(BackgroundProcess.Common.ResetMembers);
                    threadResetMembers.Name = "threadResetMembers";
                    threadResetMembers.Start();
                    Logger.WriteDebug("Старт потока threadResetMembers.");

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

                    //чтение информации с реестра бота...
                    var registryBot = new RegistryBot();
                    //если бот был запущен после перезагрузки...
                    if(registryBot.RunForReboot)
                    {
                        Logger.WriteDebug("Запуск после перезагрузки...");
                        var turnTankJson = String.Empty;
                        Logger.WriteDebug("Востановление данных...");
                        using (var reader = new StreamReader(@"Files/Temp/tempTanks.json"))
                        {
                            turnTankJson = reader.ReadToEnd();
                        }
                        var turnSolJson = String.Empty;
                        using (var reader = new StreamReader(@"Files/Temp/tempSol.json"))
                        {
                            turnTankJson = reader.ReadToEnd();
                        }

                        Logger.WriteDebug("Удаление временных файлов...");
                        File.Delete(@"Files/Temp/tempTanks.json");
                        File.Delete(@"Files/Temp/tempSol.json");
                        var turnTank = JsonConvert.DeserializeObject<List<Models.UserTurnCreate>>(turnTankJson);
                        var turnSol = JsonConvert.DeserializeObject<List<Models.UserTurnCreate>>(turnSolJson);

                        Logger.WriteDebug("Запуск задач...");
                        foreach (var turnT in turnTank)
                            new Task(() => BackgroundProcess.Army.CreateTanks(new Models.DataCreateSoldiery() { UserId = turnT.Id, Count = Convert.ToInt32(turnT.Count) })).Start();

                        foreach (var turnS in turnSol)
                            new Task(() => BackgroundProcess.Army.CreateSoldiery(new Models.DataCreateSoldiery() { UserId = turnS.Id, Count = Convert.ToInt32(turnS.Count) })).Start();

                        if (registryBot.PlayInRulette)
                            new Task(BackgroundProcess.Casino.TimerTriggerRoulette).Start();

                        registryBot.RunForReboot = false;
                    }


                    Logger.WriteDebug("Получение всех пользователей...");
                    var listUser = Api.User.AllList;
                    foreach (var user in listUser)
                    {
                        //Запуск задач для каждого пользователя...
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

                    //подписка на евенты.
                    longpoll.NewMesageEvent += Core.NewMessage;
                    longpoll.UserJoinEvent += Core.JoinInGroup;
                    Process.GetCurrentProcess().Exited += Core.BotOffline;

                    Logger.WriteDebug("Конец инициализации...");
                    var argumentsArg = Console.ReadLine();
                }catch(Exception e)
                {
                    //обработка глобальных ошибок.
                    Statistics.NewError();
                    Logger.WriteError(e);
                }             
            }          
        }     
    }
}
