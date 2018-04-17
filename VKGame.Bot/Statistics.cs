using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace VKGame.Bot
{
    /// <summary>
    /// Класс для работы со статистикой.
    /// </summary>
    public class Statistics
    {
        public static void SendMessage()
        {
            Api.Statistics.SetValue("OutMessageDay");
        }
        public static void InMessage()
        {
            Api.Statistics.SetValue("InMessageDay");

        }

        public static void NewError()
        {
            try
            {
                 Api.Statistics.SetValue("ErrorsDay");

            }catch(Exception e)
            {
                Logger.WriteError(e);
            }
        }

        public static void NewRegistation()
        {
            Api.Statistics.SetValue("RegistrationsDay");

        }

        public static void CreateBattle()
        {
            Api.Statistics.SetValue("BattlesDay");
        }

        public static void WinCasino(long count)
        {
            Api.Statistics.SetValue("WinCasinoDay", count);
        }

        public static void CreateSol(long count)
        {
            Api.Statistics.SetValue("CreateArmySolDay", count);
        }

        public static void CreateTanks(long count)
        {
            Api.Statistics.SetValue("CreateArmyDayTanks", count);
        }

        public static void BuyBox()
        {
            Api.Statistics.SetValue("BoxsBuyDay");
        }

        public static void WinBox()
        {
            Api.Statistics.SetValue("BoxsWinDay");
        }

        public static void CreateClan()
        {
            Api.Statistics.SetValue("CreateClansDay");
        }

        public static void ActivatePromo()
        {
            Api.Statistics.SetValue("PromocodesAll");
        }

        public static void NewCredit()
        {
            Api.Statistics.SetValue("KreditsAll");
        }

        public static void GoToHome()
        {
            Api.Statistics.SetValue("GoHomeDay");
        }

        public static void NewCompetition()
        {
            Api.Statistics.SetValue("CompetitionsBattleDay");
        }

        public static void JoinCompetition()
        {
            Api.Statistics.SetValue("CompetitionsJoinDay");
        }

        public static void CreateCompetition()
        {
            Api.Statistics.SetValue("CompetitionsAll");
        }

        public static void BattleCompetition()
        {
            Api.Statistics.SetValue("CompetitionsBattleDay");
        }

        public static void NewReferral()
        {
            Api.Statistics.SetValue("RefferalAll");
        }

        public static void WinBattle()
        {
            Api.Statistics.SetValue("WinBattleDay");
        }

        public static void JoinBattle()
        {
           // Api.Statistics.SetValue("");

        }

        public static Models.Statistics GetAll => Api.Statistics.GetAll();
     }
}