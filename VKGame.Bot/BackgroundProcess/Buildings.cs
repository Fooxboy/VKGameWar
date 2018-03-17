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
        /// Добавление ресурсов и прочее.
        /// </summary>
        /// <param name="obj">Id</param>
        public static void AddingResources(object obj) 
        {
            long id = (long)obj;   

            while(true) 
            {
                var bufferTime = 0;
                try
                {
                    var resources = new Api.Resources(id);
                    var builds = new Api.Builds(id);

                    if (bufferTime > 60 || bufferTime == 60) bufferTime = 0;
                  
                    //Получение деняк с рефералов
                    if ((DateTime.Now.Day == 15) && (bufferTime == 30))
                    {
                        bufferTime = 0;
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
                            if(countReferrals ==0)
                            {
                                Api.MessageSend($"🎉 Если бы у Вас были рефералы, вы бы получили за них бонус! Но у Вас их нет! Скорее приводите своих друзей  в игру! Для того, чтобы пользователь стал Вашим рефером, при регистрации он должен написать: Старт {id} ", id);

                            }else
                            {
                                Api.MessageSend($"🎉 Вы получили с {countReferrals} рефералов: {sumCash} 💳", id);

                            }
                            Api.Referrals.SetList(referrals, id); 
                        }
                    }
                    
                    //само добавление ресурсов
                    var energy = resources.Energy;
                    var eat = resources.Food;
                    var water = resources.Water;

                    if (eat < resources.Soldiery&& bufferTime == 30)
                    {
                        Api.MessageSend("А Вам нечем кормить армию! Купите еды в магазине или 5 солдат каждые 30 минут будут умирать!", id);
                        resources.Soldiery = resources.Soldiery - 5;
                    }
                    else
                    {
                        if (eat > 0) eat = eat - resources.Soldiery;
                        else eat = 0;
                        
                    }
                    //Пиздец говнокод, но почему-то по другому не работает :/
                    if (energy < builds.WarehouseEnergy * 100)
                    {
                        var temp = builds.PowerGenerators * 10;
                        if((energy + temp) > builds.WarehouseEnergy * 100)
                        {
                            energy = builds.WarehouseEnergy * 100;
                        }else
                        {
                            energy = energy + temp;

                        }
                    }
                    if (eat < builds.WarehouseEat * 100)
                    {
                        var temp = builds.Eatery * 10;
                        if((eat + temp) > builds.WarehouseEat * 100)
                        {
                            eat = builds.WarehouseEat * 100;
                        }else
                        {
                            eat = eat + temp;
                        }
                    }

                    if (water < builds.WarehouseWater * 100)
                    {
                        var temp = builds.WaterPressureStation * 10;
                        if((water + temp) > builds.WarehouseWater * 100)
                        {
                            water = builds.WarehouseWater * 100;
                        }else
                        {
                            water = water + temp;
                        }
                    }

                    resources.Energy = energy;
                    resources.Food = eat;
                    resources.Water = water;
                    ++bufferTime;
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