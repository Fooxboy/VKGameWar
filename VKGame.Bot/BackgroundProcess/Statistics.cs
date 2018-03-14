using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace VKGame.Bot.BackgroundProcess
{
    public class Statistics
    {
        public static void StartAdd()
        {
            while(true)
            {
                try
                {
                    if(DateTime.Now.Hour == 1)
                    {
                        var stat = Bot.Statistics.GetStat();
                        stat.Battles.All += stat.Battles.Day;
                        stat.Battles.Day = 0;

                        stat.Boxs.BuyStoreAll += stat.Boxs.BuyStoreDay;
                        stat.Boxs.BuyStoreDay = 0;

                        stat.Boxs.WinBattleAll += stat.Boxs.WinBattleDay;
                        stat.Boxs.WinBattleDay = 0;

                        stat.Competitions.BattleCompetitionAll += stat.Competitions.BattleCompetitionDay;
                        stat.Competitions.BattleCompetitionDay = 0;

                        stat.Competitions.JoinPeopleAll += stat.Competitions.JoinPeopleDay;
                        stat.Competitions.JoinPeopleDay = 0;

                        stat.CreateArmy.AllSol += stat.CreateArmy.DaySol;
                        stat.CreateArmy.DaySol = 0;

                        stat.CreateArmy.AllTanks += stat.CreateArmy.DayTanks;
                        stat.CreateArmy.DayTanks = 0;

                        stat.CreateClans.All += stat.CreateClans.Day;
                        stat.CreateClans.Day = 0;

                        stat.Errors.All += stat.Errors.Day;
                        stat.Errors.Day = 0;

                        stat.Registrations.All += stat.Registrations.Day;
                        stat.Registrations.Day = 0;

                        stat.WinBattleAll += stat.WinBattleDay;
                        stat.WinBattleDay = 0;

                        Bot.Statistics.SetStat(stat);
                    }

                    Thread.Sleep(3600000);
                    

                }catch(Exception e)
                {
                    Bot.Statistics.NewError();
                    Logger.WriteError($"{e.Message} \n {e.StackTrace}");
                    Thread.Sleep(3600000);

                }
            }
        }
    }
}
