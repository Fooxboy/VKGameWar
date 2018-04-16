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
                    if(DateTime.Now.Hour == 23)
                    {
                 
                        var battlesAll = Api.Statistics.GetValue("BattlesAll");
                        var battlesDay = Api.Statistics.GetValue("BattlesDay");
                        Api.Statistics.SetValue("BattlesAll", battlesAll+battlesDay);
                        Api.Statistics.ResetValue("BattlesDay");
                        
                        
                        var boxsAllBuy = Api.Statistics.GetValue("BoxsBuyAll");
                        var boxsDayBuy = Api.Statistics.GetValue("BoxsBuyDay");
                        Api.Statistics.SetValue("BattlesAll", boxsAllBuy+boxsDayBuy);
                        Api.Statistics.ResetValue("BoxsBuyDay");
                        
                        var boxsAllWin = Api.Statistics.GetValue("BoxsWinAll");
                        var boxsDayWin = Api.Statistics.GetValue("BoxsWinDay");
                        Api.Statistics.SetValue("BoxsWinAll", boxsAllWin+boxsDayWin);
                        Api.Statistics.ResetValue("BoxsWinDay");
                        
                        var competitionAll = Api.Statistics.GetValue("CompetitionsBattleAll");
                        var competitionDay = Api.Statistics.GetValue("CompetitionsBattleDay");
                        Api.Statistics.SetValue("CompetitionsBattleAll", competitionAll+competitionDay);
                        Api.Statistics.ResetValue("CompetitionsBattleDay");
                        
                        var joinAll = Api.Statistics.GetValue("CompetitionsJoinAll");
                        var joinDay = Api.Statistics.GetValue("CompetitionsJoinDay");
                        Api.Statistics.SetValue("CompetitionsJoinAll", joinAll+joinDay);
                        Api.Statistics.ResetValue("CompetitionsJoinDay");

                        var allSol = Api.Statistics.GetValue("CreateArmySolAll");
                        var daySol = Api.Statistics.GetValue("CreateArmySolDay");
                        Api.Statistics.SetValue("CreateArmySolAll", allSol+daySol);
                        Api.Statistics.ResetValue("CreateArmySolDay");
                        
                        var allTank = Api.Statistics.GetValue("CreateArmyAllTanks");
                        var dayTank = Api.Statistics.GetValue("CreateArmyDayTanks");
                        Api.Statistics.SetValue("CreateArmyAllTanks", allTank+dayTank);
                        Api.Statistics.ResetValue("CreateArmyDayTanks");

                        var allclans = Api.Statistics.GetValue("CreateClansAll");
                        var dayclans = Api.Statistics.GetValue("CreateClansDay");
                        Api.Statistics.SetValue("CreateClansAll", allclans+dayclans);
                        Api.Statistics.ResetValue("CreateClansDay");
                        
                        var errorAll = Api.Statistics.GetValue("ErrorsAll");
                        var errorDay = Api.Statistics.GetValue("ErrorsDay");
                        Api.Statistics.SetValue("ErrorsAll", errorAll+errorDay);
                        Api.Statistics.ResetValue("ErrorsDay");

                        var regAll = Api.Statistics.GetValue("RegistrationsAll");
                        var regDay = Api.Statistics.GetValue("RegistrationsDay");
                        Api.Statistics.SetValue("RegistrationsAll", regAll+regDay);
                        Api.Statistics.ResetValue("RegistrationsDay");
                        
                        var winAll = Api.Statistics.GetValue("WinBattleAll");
                        var winDay = Api.Statistics.GetValue("WinBattleDay");
                        Api.Statistics.SetValue("WinBattleAll", winAll+winDay);
                        Api.Statistics.ResetValue("WinBattleDay");

                    }
                    Thread.Sleep(3600000);
                }catch(Exception e)
                {
                    Bot.Statistics.NewError();
                    Logger.WriteError(e);
                    Thread.Sleep(3600000);
                }
            }
        }
    }
}
