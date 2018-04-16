namespace VKGame.Bot.Api
{
    public class Statistics
    {
        public static void SetValue(string key, long count=1)
        {
            var db = new Database.Stat("Statistics");
            var lastValue = (long) db.GetFromKey(key);
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

        public static Models.Statistics GetAll()
        {
            var db = new Database.Stat("Statistics");
            var model = new Models.Statistics()
            {
                InMessageDay = (long) db.GetFromKey("InMessageDay"),
                OutMessageDay = (long) db.GetFromKey("OutMessageDay"),
                AllMessages =  (long) db.GetFromKey("AllMessages"),
                Errors =  new Models.Statistics.ErrorsModel()
                {
                    All = (long)db.GetFromKey("ErrorsAll"),
                    Day =  (long)db.GetFromKey("ErrorsDay")
                },
                Battles = new Models.Statistics.BattlesModel()
                {
                    All = (long)db.GetFromKey("BattlesAll"),
                    Day = (long)db.GetFromKey("BattlesDay")
                },
                Registrations = new Models.Statistics.RegistrationsModel()
                {
                    All =(long)db.GetFromKey("RegistrationsAll"),
                    Day = (long)db.GetFromKey("RegistrationsDay")
                },
                WinCasino = new Models.Statistics.WinCasinoModel()
                {
                    All = (long)db.GetFromKey("WinCasinoAll"),
                    Day = (long)db.GetFromKey("WinCasinoDay")
                },
                CreateArmy = new Models.Statistics.CreateArmyModel()
                {
                    AllTanks  = (long)db.GetFromKey("CreateArmyAllTanks"),
                    AllSol = (long)db.GetFromKey("CreateArmySolAll"),
                    DaySol = (long)db.GetFromKey("CreateArmySolDay"),
                    DayTanks = (long)db.GetFromKey("CreateArmyDayTanks")
                },
                Boxs = new Models.Statistics.BoxsModel()
                {
                    BuyStoreAll = (long)db.GetFromKey("BoxsBuyAll"),
                    BuyStoreDay = (long)db.GetFromKey("BoxsBuyDay"),
                    WinBattleAll = (long)db.GetFromKey("BoxsWinAll"),
                    WinBattleDay = (long)db.GetFromKey("BoxsWinDay")
                },
                Competitions = new Models.Statistics.CompetitionsModel()
                {
                    All=(long)db.GetFromKey("CompetitionsAll"),
                    BattleCompetitionAll =(long)db.GetFromKey("CompetitionsBattleAll") ,
                    BattleCompetitionDay = (long)db.GetFromKey("CompetitionsBattleDay"),
                    JoinPeopleAll = (long)db.GetFromKey("CompetitionsJoinAll"),
                    JoinPeopleDay =(long)db.GetFromKey("CompetitionsJoinDay") 
                },
                CreateClans = new Models.Statistics.CreateClansModel()
                {
                    All=(long)db.GetFromKey("CreateClansAll"),
                    Day=(long)db.GetFromKey("CreateClansDay")
                },
                PromocodesAll = (long)db.GetFromKey("PromocodesAll"),
                KreditsAll = (long)db.GetFromKey("KreditsAll"),
                RefferalAll = (long)db.GetFromKey("RefferalAll"),
                WinBattleAll = (long)db.GetFromKey("WinBattleAll"),
                WinBattleDay = (long)db.GetFromKey("WinBattleDay")             
            };

            return model;
        }
    }
}