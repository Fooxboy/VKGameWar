using System;
using System.IO;
using Newtonsoft.Json;

namespace VKGame.Bot
{
    /// <summary>
    /// Класс для работы со статистикой.
    /// </summary>
    public class Statistics
    {
        public static void SendMessage()
        {
            var stat = GetStat();
            stat.OutMessageDay += 1;
            SetStat(stat);
        }

        /*public static void SendPhoto()
        {
            var stat = GetStat();
            stat.OutMessageDay += 1;
            SetStat(stat);
        }*/

        public static void InMessage()
        {
            var stat = GetStat();
            stat.InMessageDay += 1;
            SetStat(stat);
        }

        public static void NewError()
        {
            try
            {
                var stat = GetStat();
                stat.Errors.Day += 1;
                SetStat(stat);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public static void NewRegistation()
        {
            var stat = GetStat();
            stat.Registrations.Day += 1;
            SetStat(stat);
        }

        public static void CreateBattle()
        {
            var stat = GetStat();
            stat.Battles.Day += 1;
            SetStat(stat);
        }

        public static void WinCasino(long count)
        {
            var stat = GetStat();
            stat.WinCasino.Day += count;
            SetStat(stat);
        }

        public static void CreateSol(long count)
        {
            var stat = GetStat();
            stat.CreateArmy.DaySol += count;
            SetStat(stat);
        }

        public static void CreateTanks(long count)
        {
            var stat = GetStat();
            stat.CreateArmy.DayTanks += count;
            SetStat(stat);
        }

        public static void BuyBox()
        {
            var stat = GetStat();
            stat.Boxs.BuyStoreDay += 1;
            SetStat(stat);
        }

        public static void WinBox()
        {
            var stat = GetStat();
            stat.Boxs.WinBattleDay += 1;
            SetStat(stat);
        }

        public static void CreateClan()
        {
            var stat = GetStat();
            stat.CreateClans.Day += 1;
            SetStat(stat);
        }

        public static void ActivatePromo()
        {
            var stat = GetStat();
            stat.PromocodesAll += 1;
            SetStat(stat);
        }

        public static void NewCredit()
        {
            var stat = GetStat();
            stat.KreditsAll += 1;
            SetStat(stat);
        } 

        public static void GoToHome()
        {
            var stat = GetStat();
            stat.GoHomeDay += 1;
            SetStat(stat);
        }

        public static void NewCompetition()
        {
            var stat = GetStat();
            stat.OutMessageDay += 1;
            SetStat(stat);
        }

        public static void JoinCompetition()
        {
            var stat = GetStat();
            stat.Competitions.JoinPeopleDay += 1;
            SetStat(stat);
        }

        public static void CreateCompetition()
        {
            var stat = GetStat();
            stat.Competitions.All += 1;
            SetStat(stat);
        }

        public static void BattleCompetition()
        {
            var stat = GetStat();
            stat.Competitions.BattleCompetitionDay += 1;
            SetStat(stat);
        }

        public static void NewReferral()
        {
            var stat = GetStat();
            stat.RefferalAll += 1;
            SetStat(stat);
        }

        public static void WinBattle()
        {
            var stat = GetStat();
            stat.WinBattleDay += 1;
            SetStat(stat);
        }

        public static void JoinBattle()
        {
            var stat = GetStat();
            stat.Battles.Day += 1;
            SetStat(stat);
        }

        public static void SetStat(Models.Statistics model)
        {
            var json = JsonConvert.SerializeObject(model);
            try
            {
                using (var writer = new StreamWriter(@"Files/Statistics.json", false, System.Text.Encoding.Default))
                {
                    writer.Write(json);
                }
            }catch(IOException)
            {
                System.Threading.Thread.Sleep(2000);
                using (var writer = new StreamWriter(@"Files/Statistics.json", false, System.Text.Encoding.Default))
                {
                    writer.Write(json);
                }
            }
           
        }

        public static Models.Statistics GetStat()
        {
            var json = String.Empty;

            try
            {
                using (var reader = new StreamReader(@"Files/Statistics.json"))
                {
                    json = reader.ReadToEnd();
                }
            }catch(IOException)
            {
                System.Threading.Thread.Sleep(2000);
                using (var reader = new StreamReader(@"Files/Statistics.json"))
                {
                    json = reader.ReadToEnd();
                }
            }
          
            return JsonConvert.DeserializeObject < Models.Statistics>(json);
        }
     }
}