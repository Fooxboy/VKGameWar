using System.Threading;
using System;
using System.Threading.Tasks;
using System.Timers;

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
                    var user = new Api.User(id);
                    if (bufferTime > 60 || bufferTime == 60) bufferTime = 0;
                  
                    //Получение деняк с рефералов
                    if ((DateTime.Now.Day == 15) && (DateTime.Now.Hour == 8))
                    {
                        bufferTime = 0;
                        var referrals = new Api.Referrals(id);
                        if(referrals.MonthCash != DateTime.Now.Month)
                        {
                            int countReferrals = referrals.RefList.Count;
                            long sumCash = 0;
                            foreach (var referral in referrals.RefList)
                            {
                                var userRef = new Api.User(referral);
                                long cashRef = 100 * userRef.Level;
                                resources.MoneyCard = resources.MoneyCard + cashRef;
                                sumCash += cashRef;
                            }

                            referrals.SumCash += sumCash;
                            referrals.MonthCash = DateTime.Now.Month;
                            if(countReferrals ==0)
                            {
                                Api.Message.Send($"🎉 Если бы у Вас были рефералы, вы бы получили за них бонус! Но у Вас их нет! Скорее приводите своих друзей  в игру! Для того, чтобы пользователь стал Вашим рефером, при регистрации он должен написать: Старт {id} ", id);

                            }else
                            {
                                Api.Message.Send($"🎉 Вы получили с {countReferrals} рефералов: {sumCash} 💳", id);
                                Notifications.EnterPaymentCard(Convert.ToInt32(sumCash), user.Id, "Рефералы");
                            }
                        }
                    }

                    if (user.Experience >= user.Level * 100)
                    {    
                        ++user.Level;
                        Api.Message.Send($"🎉 Вы получили {user.Level} уровень! Давай ещё больше!", id);
                        Notifications.EnterPaymentCard(100, id, "новый уровень");
                    }
                    //само добавление ресурсов
                    var energy = resources.Energy;
                    var eat = resources.Food;
                    var water = resources.Water;
                    var registry = new Api.Registry(user.Id);
                    var config = new Api.ConfigBoosters(user.Id);


                    if ((resources.Soldiery > 1) || (resources.Soldiery != 0))
                    {
                        if ((eat < resources.Soldiery) && (bufferTime == 30))
                        {
                            Api.Message.Send("👨‍🍳 А Вам нечем кормить армию! Купите еды в магазине или 5 солдат каждые 30 минут будут умирать!", id);
                            resources.Soldiery = resources.Soldiery - 5;
                        }
                        else
                        {
                            if (eat > 0) eat = eat - resources.Soldiery;
                            else eat = 0;

                        }
                    }
                   
                    var boosters = new Bot.Api.Boosters(user.Id);
                    if (boosters.CreateWater != 0 || boosters.CreateFood != 0)
                    {
                        if (!registry.ShowNotifyBoostResources)
                        {
                            Api.Message.Send(
                                "✨ У Вас есть усилители получения ресурсов! Активируйте их в разделе: Усилители ",
                                user.Id);
                        }
                    }
                    
                    //Пиздец говнокод, но почему-то по другому не работает :/
                    if (energy < Commands.Buildings.Api.MaxEnergy(builds.WarehouseEnergy))
                    {
                        var temp =  Commands.Buildings.Api.MaxEnergyGen(builds.PowerGenerators);

                        if (energy < 0) energy = 0;

                        if((energy + temp) >= Commands.Buildings.Api.MaxEnergy(builds.WarehouseEnergy))
                        {
                            energy = Commands.Buildings.Api.MaxEnergy(builds.WarehouseEnergy);
                        }
                        else
                        {
                            energy += temp;
                        }

                        if (energy > Commands.Buildings.Api.MaxEnergy(builds.WarehouseEnergy))
                            energy = Commands.Buildings.Api.MaxEnergy(builds.WarehouseEnergy);
                    }

                    if (eat < 0) eat = 0;
                    
                    if (eat < Commands.Buildings.Api.MaxFood(builds.WarehouseEat))
                    {
                        bool boost = false;
                        var temp = Commands.Buildings.Api.MaxFoodGen(builds.Eatery);
                        if((eat + temp) >= Commands.Buildings.Api.MaxFood(builds.WarehouseEat))
                        {       
                            eat = builds.WarehouseEat * 100;
                        }else
                        {
                            if (!registry.ActivedBoostFood)
                            {
                                if (boosters.CreateFood != 0)
                                {
                                    if(config.CreateFood == 1) new Task(() => StartFoodTimer(user.Id)).Start();
                                    boost = true;
                                }
                            }
                            else
                            {
                                if(config.CreateFood == 1) new Task(() => StartFoodTimer(user.Id)).Start();
                                boost = true;
                            }

                            if (boost) temp *= 2;
                            if (eat > Commands.Buildings.Api.MaxFood(builds.WarehouseEat))
                                eat = Commands.Buildings.Api.MaxFood(builds.WarehouseEat);
                            else
                                eat = eat + temp;
                            
                        }
                    }

                    if (eat > Commands.Buildings.Api.MaxFood(builds.WarehouseEat))
                        eat = Commands.Buildings.Api.MaxFood(builds.WarehouseEat);

                    if (water < 0) water = 0;

                    if (water < Commands.Buildings.Api.MaxWater(builds.WarehouseWater))
                    {
                        var temp = Commands.Buildings.Api.MaxWaterGen(builds.WaterPressureStation);
                        if ((water + temp) >= Commands.Buildings.Api.MaxWater(builds.WarehouseWater))
                            water = Commands.Buildings.Api.MaxWater(builds.WarehouseWater);
                        else
                        {
                            var boost = false;
                            if (!registry.ActivedBoostWater)
                            {
                                if (boosters.CreateWater != 0)
                                {
                                    if (config.CreateWater == 1)
                                    {
                                        new Task(() => StartWaterTimer(user.Id)).Start();
                                        boost = true;
                                    }
                                    
                                }
                            }
                            else
                            {
                                if (config.CreateWater == 1)
                                {
                                    new Task(() => StartWaterTimer(user.Id)).Start();
                                    boost = true;
                                }
                                
                            }

                            if (boost) temp *= 2;
                            if (water > Commands.Buildings.Api.MaxWater(builds.WarehouseWater))
                                water = Commands.Buildings.Api.MaxWater(builds.WarehouseWater);
                            else
                                water = water + temp;

                        }
                    }

                    if (water > Commands.Buildings.Api.MaxWater(builds.WarehouseWater))
                        water = Commands.Buildings.Api.MaxWater(builds.WarehouseWater);

                    resources.Energy = energy;
                    Thread.Sleep(333);
                    resources.Food = eat;
                    Thread.Sleep(333);
                    resources.Water = water;
                    Thread.Sleep(333);

                    bufferTime += 2;
                }catch(Exception e)
                {
                    Bot.Statistics.NewError();
                    Logger.WriteError(e);
                }
                Thread.Sleep(60000);
            }
        }

        public static void StartFoodTimer(long userId)
        {
            var registry = new Api.Registry(userId);
            registry.ActivedBoostFood = false;
            var bosters = new Api.Boosters(userId);
            bosters.CreateFood -= 1;
        }
        
        public static void StartWaterTimer(long userId)
        {
            var registry = new Api.Registry(userId);
            registry.ActivedBoostWater = false;
            var bosters = new Api.Boosters(userId);
            bosters.CreateFood -= 1;
        }
    }
   
}