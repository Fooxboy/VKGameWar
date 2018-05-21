using System;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Статистика
    /// </summary>
    public class Statistics
    {
        public static void SetValue(string key, long count=1)
        {
            var db = new Database.Stat("Statistics");
            var lastValue = System.Int64.Parse((string) db.GetFromKey(key));
            lastValue += count;
            db.EditFromKey(key, lastValue);
        }

        public static long GetValue(string key)
        {
            var db = new Database.Stat("Statistics");
            return (long)db.GetFromKey(key);
        }

        public static void ResetValue(string key)
        {
            var db = new Database.Stat("Statistics");
            db.EditFromKey(key, 0);
        }

        /// <summary>
        /// Получение всей статистики
        /// </summary>
        /// <returns> Объект статитстики</returns>
        public static Models.Statistics GetAll()
        {
            var db = new Database.Stat("Statistics");
            var model = new Models.Statistics()
            {
                InMessageDay = Int64.Parse( (string) db.GetFromKey("InMessageDay")),
                OutMessageDay = Int64.Parse((string) db.GetFromKey("OutMessageDay")),
                AllMessages = Int64.Parse((string) db.GetFromKey("AllMessages")),
                Errors =  new Models.Statistics.ErrorsModel()
                {
                    All = Int64.Parse((string)db.GetFromKey("ErrorsAll")),
                    Day = Int64.Parse((string)db.GetFromKey("ErrorsDay"))
                },
                Battles = new Models.Statistics.BattlesModel()
                {
                    All = Int64.Parse((string)db.GetFromKey("BattlesAll")),
                    Day = Int64.Parse((string)db.GetFromKey("BattlesDay"))
                },
                Registrations = new Models.Statistics.RegistrationsModel()
                {
                    All = Int64.Parse((string)db.GetFromKey("RegistrationsAll")),
                    Day = Int64.Parse((string)db.GetFromKey("RegistrationsDay"))
                },
                WinCasino = new Models.Statistics.WinCasinoModel()
                {
                    All = Int64.Parse((string)db.GetFromKey("WinCasinoAll")),
                    Day = Int64.Parse((string)db.GetFromKey("WinCasinoDay"))
                },
                CreateArmy = new Models.Statistics.CreateArmyModel()
                {
                    AllTanks  = Int64.Parse((string)db.GetFromKey("CreateArmyAllTanks")),
                    AllSol = Int64.Parse((string)db.GetFromKey("CreateArmySolAll")),
                    DaySol = Int64.Parse((string)db.GetFromKey("CreateArmySolDay")),
                    DayTanks = Int64.Parse((string)db.GetFromKey("CreateArmyDayTanks"))
                },
                Boxs = new Models.Statistics.BoxsModel()
                {
                    BuyStoreAll = Int64.Parse((string)db.GetFromKey("BoxsBuyAll")),
                    BuyStoreDay = Int64.Parse((string)db.GetFromKey("BoxsBuyDay")),
                    WinBattleAll = Int64.Parse((string)db.GetFromKey("BoxsWinAll")),
                    WinBattleDay = Int64.Parse((string)db.GetFromKey("BoxsWinDay"))
                },
                Competitions = new Models.Statistics.CompetitionsModel()
                {
                    All= Int64.Parse((string)db.GetFromKey("CompetitionsAll")),
                    BattleCompetitionAll = Int64.Parse((string)db.GetFromKey("CompetitionsBattleAll")) ,
                    BattleCompetitionDay = Int64.Parse((string)db.GetFromKey("CompetitionsBattleDay")),
                    JoinPeopleAll = Int64.Parse((string)db.GetFromKey("CompetitionsJoinAll")),
                    JoinPeopleDay = Int64.Parse((string)db.GetFromKey("CompetitionsJoinDay")) 
                },
                CreateClans = new Models.Statistics.CreateClansModel()
                {
                    All= Int64.Parse((string)db.GetFromKey("CreateClansAll")),
                    Day= Int64.Parse((string)db.GetFromKey("CreateClansDay"))
                },
                PromocodesAll = Int64.Parse((string)db.GetFromKey("PromocodesAll")),
                KreditsAll = Int64.Parse((string)db.GetFromKey("KreditsAll")),
                RefferalAll = Int64.Parse((string)db.GetFromKey("RefferalAll")),
                WinBattleAll = Int64.Parse((string)db.GetFromKey("WinBattleAll")),
                WinBattleDay = Int64.Parse((string)db.GetFromKey("WinBattleDay"))             
            };

            return model;
        }
    }
}