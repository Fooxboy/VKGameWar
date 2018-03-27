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
                
                try
                {
                    var bufferTime = 0;
                    var resources = new Api.Resources(id);
                    var builds = new Api.Builds(id);
                    var user = Api.User.GetUser(id);
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
                                Notifications.EnterPaymentCard(Convert.ToInt32(sumCash), user.Id, "Рефералы");
                            }
                            Api.Referrals.SetList(referrals, id); 
                        }
                    }

                    if (user.Experience >= user.Level * 100)
                    {    
                        ++user.Level;
                        Api.MessageSend($"🎉 Вы получили {user.Level} уровень! Давай ещё больше!", id);
                        Notifications.EnterPaymentCard(100, id, "новый уровень");
                    }
                    //само добавление ресурсов
                    var energy = resources.Energy;
                    var eat = resources.Food;
                    var water = resources.Water;

                    if ((resources.Soldiery > 1) || (resources.Soldiery != 0))
                    {
                        if ((eat < resources.Soldiery) && (bufferTime == 30))
                        {
                            Api.MessageSend("👨‍🍳 А Вам нечем кормить армию! Купите еды в магазине или 5 солдат каждые 30 минут будут умирать!", id);
                            resources.Soldiery = resources.Soldiery - 5;
                        }
                        else
                        {
                            if (eat > 0) eat = eat - resources.Soldiery;
                            else eat = 0;

                        }
                    }
                   
                    //Пиздец говнокод, но почему-то по другому не работает :/
                    if (energy < Commands.Buildings.Api.MaxEnergy(builds.WarehouseEnergy))
                    {
                        var temp =  Commands.Buildings.Api.MaxEnergyGen(builds.PowerGenerators);
                        if((energy + temp) >= Commands.Buildings.Api.MaxEnergy(builds.WarehouseEnergy))
                        {
                            energy = Commands.Buildings.Api.MaxEnergy(builds.WarehouseEnergy);
                        }
                        else
                        {
                            energy += temp;
                        }
                    }
                    if (eat < Commands.Buildings.Api.MaxFood(builds.WarehouseEat))
                    {
                        var temp = Commands.Buildings.Api.MaxFoodGen(builds.Eatery);
                        if((eat + temp) >= Commands.Buildings.Api.MaxFood(builds.WarehouseEat))
                        {
                            eat = builds.WarehouseEat * 100;
                        }else
                        {
                            eat = eat + temp;
                        }
                    }

                    if (water < Commands.Buildings.Api.MaxWater(builds.WarehouseWater))
                    {
                        var temp = Commands.Buildings.Api.MaxWaterGen(builds.WaterPressureStation);
                        if ((water + temp) >= Commands.Buildings.Api.MaxWater(builds.WarehouseWater))
                            water = Commands.Buildings.Api.MaxWater(builds.WarehouseWater);
                        else water += temp;
                    }

                    resources.Energy = energy;
                    resources.Food = eat;
                    resources.Water = water;
                    ++bufferTime;
                    Api.User.SetUser(user);
                }catch(Exception e)
                {
                    Bot.Statistics.NewError();
                    Logger.WriteError(e);
                }
                Thread.Sleep(60000);
            }
        } 
    }
   
}