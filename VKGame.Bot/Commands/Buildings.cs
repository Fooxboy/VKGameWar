using System;
using System.Collections.Generic;

namespace VKGame.Bot.Commands 
{
    public class Buildings : ICommand
    {
        public string Name => "постройки";
        public string Arguments => "(), (Вариант_выбора)";
        public string Caption => "Раздел для управления Вашими зданиями.";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {

            var messageArray = msg.Text.Split(' ');
            if (messageArray.Length == 1)
                return GetBuildingsText(msg, $"Время последнего обновления: {DateTime.Now}");
            else
            {
                var type = typeof(Buildings);
                object obj = Activator.CreateInstance(type);
                var methods = type.GetMethods();

                foreach (var method in methods)
                { 
                    var attributesCustom = Attribute.GetCustomAttributes(method);

                    foreach (var attribute in attributesCustom)
                    {
                        if (attribute.GetType() == typeof(Attributes.Trigger))
                        {

                            var myAtr = ((Attributes.Trigger) attribute);

                            if (myAtr.Name == messageArray[1])
                            {

                                object result = method.Invoke(obj, new object[] {msg});
                                return (string) result;
                            }
                        }
                    }
                    
                }
            }
            return "❌ Неизвестная подкоманда.";
        }

        private string GetBuildingsText(LongPollVK.Models.AddNewMsg msg, string notify) 
        {
            var user = Api.User.GetUser(msg.PeerId);
            var builds = new Api.Builds(msg.PeerId);        
            return $"‼{notify}"+
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n⚠В этом разделе Вы можете наблюдать какие здания у Вас есть. Сможете их купить или продать."+
                   $"\n"+
                   $"\nВАШИ ЗДАНИЯ➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n🏡 Жилые дома: {builds.Apartments}. Может поселится {builds.Apartments * 10} солдат."+
                   $"\n⚡ Электростанции: {builds.PowerGenerators}. Генерируется {builds.PowerGenerators * 10} ⚡ в минуту."+
                   $"\n💦 Водонапорные башни: {builds.WaterPressureStation}. Генерируется {builds.WaterPressureStation* 10} 💧 в минуту."+
                   $"\n🍔 Закусочные: {builds.Eatery}. Готовится {builds.Eatery * 5} 🍕 в минуту."+
                   $"\n"+
                   $"\n🔋 Энергетические батареи: {builds.WarehouseEnergy}"+
                   $"\n🌊 Бочки с водой: {builds.WarehouseWater}"+
                   $"\n🍬 Холодильники: {builds.WarehouseEat}"+
                   $"\n💣 Ангары: {builds.WarehouseEat}"+
                   $"\n"+
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💵 Вы можете покупать новые постройки. Для этого напишите: {Name} купить название_постройки"+
                   $"\n➡ Цены на постройки Вы можете узнать, написав: {Name} цены"+
                   $"\n"+
                   $"\n⚒ Вы можете продавать старые постройки. Например, Вам не хватает денег. Для продажи напишите: {Name} продать название_постройки"+
                   $"\n➡ Цены на продажу постройки Вы можете узнать, написав: {Name} цены";                             
                          
        }

        [Attributes.Trigger("цены")]
        public static string Prices(LongPollVK.Models.AddNewMsg msg) 
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
            
            return result;
        }

        [Attributes.Trigger("купить")]
        public static string BuyBuilds(LongPollVK.Models.AddNewMsg msg) 
        {
            var user = Api.User.GetUser(msg.PeerId);
            var builds = new Api.Builds(msg.PeerId);
            var messageArray = msg.Text.Split(' ');
            
            Dictionary<string,int> buildsList = new Dictionary<string, int>();
            buildsList.Add("Жилой", 400);
            buildsList.Add("Электростанцию", 550);
            buildsList.Add("Водонапорную", 550);
            buildsList.Add("Закусочную", 500);
            buildsList.Add("Энергетическиую", 500);
            buildsList.Add("Бочку", 400);
            buildsList.Add("Холодильник", 400);
            buildsList.Add("Ангар", 400);
            

            var build = messageArray[2];
            foreach(var buildL in buildsList) 
            {
                if(buildL.Key.ToLower() == build.ToLower()) 
                {
                    if(!Notifications.RemovePaymentCard(buildL.Value, msg.PeerId, "Покупка построек")) return "❌ На Вашем счету нет нужной суммы.";
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
            return "❌ Неизвестное название постройки!";
        }

        [Attributes.Trigger("продать")]
        public static string SellOfBuilds(LongPollVK.Models.AddNewMsg msg) 
        {

            var builds = new Api.Builds(msg.PeerId);
            var messageArray = msg.Text.Split(' ');
            
            Dictionary<string,int> buildsList = new Dictionary<string, int>();
            buildsList.Add("Жилой", 300);
            buildsList.Add("Электростанцию", 400);
            buildsList.Add("Водонапорную", 400);
            buildsList.Add("Закусочную", 400);
            buildsList.Add("Энергетическиую", 400);
            buildsList.Add("Бочку", 300);
            buildsList.Add("Холодильник", 300);
            buildsList.Add("Ангар", 300);


            var build = messageArray[2];

            foreach(var buildL in buildsList) 
            {
                Notifications.EnterPaymentCard(buildL.Value,msg.PeerId, "Продажа построек");
                return "✅ Вы успешно продали постройку!";
            }

            return "❌ Неизвестное название постройки! ";
        } 
    }
}