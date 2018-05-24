using System;
using System.Collections.Generic;

namespace VKGame.Bot.Commands 
{
    public class Buildings : ICommand
    {
        public override string Name => "Постройки";
        public override string Arguments => "(), (Вариант_выбора)";
        public override string Caption => "Раздел для управления Вашими зданиями.";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string> {"цены", "купить", "продать" };
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";


        public override object Execute(Models.Message msg)
        {
            var notify = String.Empty;
            if (Common.Notification == null) notify = $"Последнее обновление: {DateTime.Now}";
            else notify = Common.Notification;

            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetBuildingsText(msg, notify);
            
            var type = typeof(Buildings);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        public static class Api
        {
            public static long MaxTanks(long countHangars) => countHangars * 5;

            public static long MaxSoldiery(long coutApartaments) => coutApartaments * 10;

            public static long MaxEnergyGen(long enGenerators) => enGenerators * 10;

            public static long MaxWaterGen(long watGenerators) => watGenerators * 10;

            public static long MaxFoodGen(long foodGenerators) => foodGenerators * 10;

            public static long MaxEnergy(long battary) => battary * 100;

            public static long MaxWater(long boxWater) => boxWater * 100;

            public static long MaxFood(long foodWare) => foodWare * 100;

        }

        private string GetBuildingsText(Models.Message msg, string notify) 
        {
            var user = new Bot.Api.User(msg.from_id);
            var builds = new Bot.Api.Builds(user.Id);
            var levels = new Bot.Api.Levels(user.Id);
            return $"‼{notify}"+
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n⚠В этом разделе Вы можете наблюдать какие здания у Вас есть. Сможете их купить или продать."+
                   $"\n"+
                   $"\nВАШИ ЗДАНИЯ➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n🏡 Жилые дома: {builds.Apartments}. Уровень: {levels.Apartments}. Может поселится {Api.MaxSoldiery(builds.Apartments)} солдат."+
                   $"\n⚡ Электростанции: {builds.PowerGenerators}. Уровень: {levels.PowerGenerators}. Генерируется {Api.MaxEnergyGen(builds.PowerGenerators)} ⚡ в минуту."+
                   $"\n💦 Водонапорные башни: {builds.WaterPressureStation}. Уровень: {levels.WaterPressureStation}. Генерируется {Api.MaxWaterGen(builds.WaterPressureStation)} 💧 в минуту."+
                   $"\n🍔 Закусочные: {builds.Eatery}. Уровень: {levels.Eatery}. Готовится {Api.MaxFoodGen(builds.Eatery)} 🍕 в минуту."+
                   $"\n"+
                   $"\n🔋 Энергетические батареи: {builds.WarehouseEnergy}. Уровень: {levels.WarehouseEnergy}. Вмещается: {Api.MaxEnergy(builds.WarehouseEnergy)} ⚡" +
                   $"\n🌊 Бочки с водой: {builds.WarehouseWater}. Уровень: {levels.WarehouseWater}. Вмещается: {Api.MaxWater(builds.WarehouseWater)} 💧" +
                   $"\n🍬 Холодильники: {builds.WarehouseEat}. Уровень: {levels.WarehouseEat}. Вмещается: {Api.MaxFood(builds.WarehouseEat)} 🍕" +
                   $"\n💣 Ангары: {builds.Hangars}. Уровень: {levels.Hangars} Вмещается: {Api.MaxTanks(builds.Hangars)} танков"+
                   $"\n"+
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💵 Вы можете покупать новые постройки. Для этого напишите: {Name} купить название_постройки"+
                   $"\n➡ Цены на постройки Вы можете узнать, написав: {Name} цены"+
                   $"\n"+
                   $"\n⚒ Вы можете продавать старые постройки. Например, Вам не хватает денег. Для продажи напишите: {Name} продать название_постройки"+
                   $"\n➡ Цены на продажу постройки Вы можете узнать, написав: {Name} цены" +
                   $"" +
                   $"\n🔝 Вы можете улучшать постройки. Для этого напишите: {Name} улучшить название_постройки" +
                   $"\n➡ Цены на продажу постройки Вы можете узнать, написав: {Name} цены ";                             
                          
        }

        [Attributes.Trigger("цены")]
        public static string Prices(Models.Message msg) 
        {
            Dictionary<string,int> buildsList = new Dictionary<string, int>();
            buildsList.Add("Жилой дом", 400);
            buildsList.Add("Электростанция", 550);
            buildsList.Add("Водонапорная башня", 550);
            buildsList.Add("Закусочная", 500);
            buildsList.Add("Энергетическая батарея", 500);
            buildsList.Add("Бочка с водой", 400);
            buildsList.Add("Холодильник", 400);
            buildsList.Add("Ангар", 500);
            

            string result = "ЦЕНЫ ДЛЯ ПОКУПКИ➖➖➖➖➖➖➖➖➖➖➖\n";
            foreach(var build in buildsList) 
            {
                result += $"🔸 {build.Key} стоимость: {build.Value} 💳\n";
            }
            result += "➡ Чтобы купить здание, напишите: постройки купить название_постройки\n ⚠ Название должно быть  в ВИНИТЕЛЬНОМ падеже (вижу что?), например: постройки купить Электростанцию\n";
            result += "\n";
            Dictionary<string,int> buildsListSellOf = new Dictionary<string, int>();
            buildsListSellOf.Add("Жилой дом", 300);
            buildsListSellOf.Add("Электростанция", 400);
            buildsListSellOf.Add("Водонапорная башня", 400);
            buildsListSellOf.Add("Закусочная", 400);
            buildsListSellOf.Add("Энергетическая батарея", 400);
            buildsListSellOf.Add("Бочка с водой", 300);
            buildsListSellOf.Add("Холодильник", 300);
            buildsListSellOf.Add("Ангар", 300);
            

            result += "ЦЕНЫ ДЛЯ ПРОДАЖИ➖➖➖➖➖➖➖➖➖➖➖\n";
            foreach(var build in buildsListSellOf) 
            {
                result += $"🔸 {build.Key} стоимость: {build.Value} 💳\n";
            }
            result += "➡ Чтобы продать здание, напишите: постройки купить название_постройки\n ⚠ Название должно быть в ВИНИТЕЛЬНОМ падеже (вижу что?), например: постройки продать Электростанцию\n";

            Dictionary<string,int> UpgratePrices = new Dictionary<string, int>();
            UpgratePrices.Add("Жилой", 200);
            UpgratePrices.Add("Электростанцию", 300);
            UpgratePrices.Add("Водонапорную", 300);
            UpgratePrices.Add("Закусочную", 300);
            UpgratePrices.Add("Энергетическиую", 200);
            UpgratePrices.Add("Бочку", 200);
            UpgratePrices.Add("Холодильник", 300);
            UpgratePrices.Add("Ангар", 400);
            
            result += "ЦЕНЫ ДЛЯ УЛУЧШЕНИЯ➖➖➖➖➖➖➖➖➖➖➖\n";
            
            foreach(var build in UpgratePrices) 
            {
                result += $"🔸 {build.Key} стоимость: {build.Value} 💳\n";
            }
            result += "➡ Чтобы улучшить здание, напишите: постройки улучшить название_постройки" +
                      "\n ⚠ Название должно быть в ВИНИТЕЛЬНОМ падеже (вижу что?), например: постройки улучшить Электростанцию\n";

            return result;
        }
        
        [Attributes.Trigger("улучшить")]
        public static string UpgrateBuilds(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');

            var build = string.Empty;

            try
            {
                build =  messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return
                    "❌ Вы не указали название постройки! Доступные названия: Жилой дом, Электростанцию, Водонапорную башню, Закусочную, Энергетическиую, Бочку, Холодильник, Ангар";
            }
            
            var builds = new Bot.Api.Levels(msg.from_id);
            
            Dictionary<string,int> buildsList = new Dictionary<string, int>();
            buildsList.Add("Жилой", 200);
            buildsList.Add("Электростанцию", 300);
            buildsList.Add("Водонапорную", 300);
            buildsList.Add("Закусочную", 300);
            buildsList.Add("Энергетическиую", 200);
            buildsList.Add("Бочку", 200);
            buildsList.Add("Холодильник", 300);
            buildsList.Add("Ангар", 400);
            
            foreach(var buildL in buildsList) 
            {
                if(buildL.Key.ToLower() == build.ToLower()) 
                {
                    if(!Notifications.RemovePaymentCard(buildL.Value, msg.from_id, "Покупка построек")) return "❌ На Вашем счету нет нужной суммы.";
                    if(buildL.Key.ToLower() == "жилой")
                    {
                        var liveBuild = builds.Apartments;
                        liveBuild =liveBuild + 1;
                        builds.Apartments = liveBuild;
                    }else if(buildL.Key.ToLower() == "электростанцию") 
                    {
                        var liveBuild = builds.PowerGenerators;
                        liveBuild = liveBuild + 1;

                        builds.PowerGenerators = liveBuild;
                    }else if(buildL.Key.ToLower() == "водонапорную") 
                    {
                        var liveBuild = builds.WaterPressureStation;
                        liveBuild= liveBuild +1;
                        builds.WaterPressureStation = liveBuild;
                    }else if(buildL.Key.ToLower() == "закусочную") 
                    {
                        var liveBuild = builds.Eatery;
                        liveBuild = liveBuild + 1;

                        builds.Eatery = liveBuild;
                    }else if(buildL.Key.ToLower() == "энергетическиую") 
                    {
                        var liveBuild = builds.WarehouseEnergy;
                        liveBuild = liveBuild + 1;

                        builds.WarehouseEnergy = liveBuild;
                    }else if(buildL.Key.ToLower() == "бочку") 
                    {
                        var liveBuild = builds.WarehouseWater;
                        liveBuild = liveBuild + 1;

                        builds.WarehouseWater = liveBuild;
                    }else if(buildL.Key.ToLower() == "холодильник") 
                    {
                        var liveBuild = builds.WarehouseEat;
                        liveBuild = liveBuild + 1;

                        builds.WarehouseEat = liveBuild;
                    }else if(buildL.Key.ToLower() == "ангар") {
                        var liveBuild = builds.Hangars;
                        liveBuild = liveBuild + 1;

                        builds.Hangars = liveBuild;
                    }
                    else return "❌ Не удалось улучшить здание. Попробуйте позже.";
                   
                    return "✅ Вы успешно улучшили постройку!";
                }           
            }
            return "❌ Неизвестное название постройки! Доступные названия: Жилой дом, Электростанцию, Водонапорную башню, Закусочную, Энергетическиую, Бочку, Холодильник, Ангар";
        }

        [Attributes.Trigger("купить")]
        public static string BuyBuilds(Models.Message msg) 
        {
            var user = new Bot.Api.User(msg.from_id);
            var builds = new Bot.Api.Builds(user.Id);
            var messageArray = msg.body.Split(' ');
            
            Dictionary<string,int> buildsList = new Dictionary<string, int>();
            buildsList.Add("Жилой", 400);
            buildsList.Add("Электростанцию", 550);
            buildsList.Add("Водонапорную", 550);
            buildsList.Add("Закусочную", 500);
            buildsList.Add("Энергетическиую", 500);
            buildsList.Add("Бочку", 400);
            buildsList.Add("Холодильник", 400);
            buildsList.Add("Ангар", 400);


            var build = string.Empty;

            try
            {
                build =  messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return
                    "❌ Вы не указали название постройки! Доступные названия: Жилой дом, Электростанцию, Водонапорную башню, Закусочную, Энергетическиую, Бочку, Холодильник, Ангар";
            }
            
            foreach(var buildL in buildsList) 
            {
                if(buildL.Key.ToLower() == build.ToLower()) 
                {
                    if(!Notifications.RemovePaymentCard(buildL.Value, user.Id, "Покупка построек")) return "❌ На Вашем счету нет нужной суммы.";
                    if(buildL.Key.ToLower() == "жилой")
                    {
                        var liveBuild = builds.Apartments;
                        liveBuild =liveBuild + 1;
                        builds.Apartments = liveBuild;
                    }else if(buildL.Key.ToLower() == "электростанцию") 
                    {
                        var liveBuild = builds.PowerGenerators;
                        liveBuild = liveBuild + 1;

                        builds.PowerGenerators = liveBuild;
                    }else if(buildL.Key.ToLower() == "водонапорную") 
                    {
                        var liveBuild = builds.WaterPressureStation;
                        liveBuild= liveBuild +1;
                        builds.WaterPressureStation = liveBuild;
                    }else if(buildL.Key.ToLower() == "закусочную") 
                    {
                        var liveBuild = builds.Eatery;
                        liveBuild = liveBuild + 1;

                        builds.Eatery = liveBuild;
                    }else if(buildL.Key.ToLower() == "энергетическиую") 
                    {
                        var liveBuild = builds.WarehouseEnergy;
                        liveBuild = liveBuild + 1;

                        builds.WarehouseEnergy = liveBuild;
                    }else if(buildL.Key.ToLower() == "бочку") 
                    {
                        var liveBuild = builds.WarehouseWater;
                        liveBuild = liveBuild + 1;

                        builds.WarehouseWater = liveBuild;
                    }else if(buildL.Key.ToLower() == "холодильник") 
                    {
                        var liveBuild = builds.WarehouseEat;
                        liveBuild = liveBuild + 1;

                        builds.WarehouseEat = liveBuild;
                    }else if(buildL.Key.ToLower() == "ангар") {
                        var liveBuild = builds.Hangars;
                        liveBuild = liveBuild + 1;

                        builds.Hangars = liveBuild;
                    }
                    else return "❌ Не удалось купить здание. Попробуйте позже.";
                   
                    return "✅ Вы успешно купили постройку!";
                }
            }
            return "❌ Неизвестное название постройки! Доступные названия: Жилой дом, Электростанцию, Водонапорную башню, Закусочную, Энергетическиую, Бочку, Холодильник, Ангар";
        }

        [Attributes.Trigger("продать")]
        public static string SellOfBuilds(Models.Message msg) 
        {

            var builds = new Bot.Api.Builds(msg.from_id);
            var messageArray = msg.body.Split(' ');
            
            Dictionary<string,int> buildsList = new Dictionary<string, int>();
            buildsList.Add("Жилой", 300);
            buildsList.Add("Электростанцию", 400);
            buildsList.Add("Водонапорную", 400);
            buildsList.Add("Закусочную", 400);
            buildsList.Add("Энергетическиую", 400);
            buildsList.Add("Бочку", 300);
            buildsList.Add("Холодильник", 300);
            buildsList.Add("Ангар", 300);


            var build = String.Empty;

            try
            {
                build = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали название постройки! Доступные названия: Жилой дом, Электростанцию, Водонапорную башню, Закусочную, Энергетическиую, Бочку, Холодильник, Ангар";

            }

            foreach(var buildL in buildsList) 
            {
                Notifications.EnterPaymentCard(buildL.Value,msg.from_id,"Продажа построек");
                return "✅ Вы успешно продали постройку!";
            }

            return "❌ Неизвестное название постройки! Доступные названия: Жилой дом, Электростанцию, Водонапорную башню, Закусочную, Энергетическиую, Бочку, Холодильник, Ангар";
        } 
    }
}