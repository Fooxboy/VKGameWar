using System.Threading;
using System;

namespace VKGame.Bot.BackgroundProcess
{
    /// <summary>
    /// Фоновые процессы построек и не только!
    /// </summary>
    public class Buildings 
    {
        /// <summary>
        /// Добавление ресурсов
        /// </summary>
        /// <param name="obj">Id</param>
        public static void AddingResources(object obj) 
        {
            long id = (long)obj;   

            while(true) 
            {
                try
                {
                    var resources = new Api.Resources(id);
                    var builds = new Api.Builds(id);
                    if (DateTime.Now.Day == 15)
                    {
                        var referrals = Api.Referrals.GetList(id);
                        if(referrals.MouthCash != DateTime.Now.Month)
                        {
                            int countReferrals = referrals.ReferralsList.Count;
                            long sumCash = 0;
                            foreach (var referral in referrals.ReferralsList)
                            {
                                referrals.ReferralsList.Remove(referral);
                                var userRef = Api.User.GetUser(referral.Id);
                                long cashRef = 100 * userRef.Level;
                                resources.MoneyCard = resources.MoneyCard + cashRef;
                                sumCash += cashRef;
                                referral.FarmMoney += cashRef;
                                referrals.ReferralsList.Add(referral);
                            }

                            referrals.SumCash += sumCash;
                            referrals.MouthCash = DateTime.Now.Month;
                            Api.MessageSend($"🎉 Вы получили с {countReferrals} рефералов: {sumCash} 💳", id);
                            Api.Referrals.SetList(referrals, id); 
                        }
                    }
                    
                    var energy = resources.Energy;
                    var eat = resources.Food;
                    var water = resources.Water;

                    if (eat < resources.Soldiery)
                    {
                        // Api.MessageSend("А Вам нечем кормить армию! Купите еды в магазине или солдаты будут умирать!", id);
                        //ы  resources.Soldiery = resources.Soldiery - 1;
                    }
                    else
                    {
                        eat = eat - resources.Soldiery;

                    }
                    //Пиздец говнокод, но почему-то по другому не работает :/
                    if (energy < builds.WarehouseEnergy * 100)
                    {
                        var temp = builds.PowerGenerators * 10;
                        energy = energy + temp;
                    }
                    if (eat < builds.WarehouseEat * 100)
                    {
                        var temp = builds.Eatery * 5;
                        eat = eat + temp;
                    }

                    if (water < builds.WarehouseWater * 100)
                    {
                        var temp = builds.WaterPressureStation * 10;
                        water = water + temp;
                    }

                    resources.Energy = energy;
                    resources.Food = eat;
                    resources.Water = water;
                    Thread.Sleep(60000);
                }catch(Exception e)
                {
                    Bot.Statistics.NewError();
                    Logger.WriteError($"{e.Message} \n {e.StackTrace}");

                }

            }
        } 
    }
   
}