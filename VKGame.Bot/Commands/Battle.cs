using System;
using Newtonsoft.Json;
using System.IO;

namespace VKGame.Bot.Commands
{
    public class Battle : ICommand 
    {
        public string Name => "бой";
        public string Arguments => "(), (вариант_выбора)";
        public string Caption => "Раздел предназначен для проведения боёв.";
        public TypeResponse Type => TypeResponse.Text;

        public object  Execute(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');
            if (messageArray.Length == 1)
                return GetBattleText(msg);
            else
            {
                var type = typeof(Battle);
                object obj = Activator.CreateInstance(type);
                var methods = type.GetMethods();

                foreach (var method in methods)
                {
                    var attributesCustom = Attribute.GetCustomAttributes(method);

                    foreach (var attribute in attributesCustom)
                    {
                        if (attribute.GetType() == typeof(Attributes.Trigger))
                        {

                            var myAtr = ((Attributes.Trigger)attribute);

                            if (myAtr.Name == messageArray[1])
                            {

                                object result = method.Invoke(obj, new object[] { msg });
                                return (string)result;
                            }
                        }
                    }

                }
            }
            return "Неизвестная подкоманда.";
        }

        [Attributes.Trigger("атака")]
        public static string Attack(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');
            var resources = new Api.Resources(msg.PeerId);
            var user = Api.User.GetUser(msg.PeerId);
            if(user.IdBattle == 0 ) return "❌ Вы не участвуете ни в какой битве.";
            var battle = new Api.Battles(user.IdBattle);
            if(battle.UserCourse != msg.PeerId) return "❌ Сейчас ход Вашего противника.";
            int countArmy = 0;
            try 
            {
                countArmy = Int32.Parse(messageArray[1]);
            }catch (FormatException) 
            {
                return "❌ Вы указали неверное число.";
            }
            var type = messageArray[2].ToLower();
            int countHP = 0;
            if(type == "солдат") 
            {
                if(countArmy > resources.Soldiery) return "❌ У Вас недостаточно солдат.";
                countHP = countArmy * 50;
                var soldiery = resources.Soldiery;
                resources.Soldiery = soldiery- countArmy;
            }else if( type == "танков") 
            {
                if(countArmy > resources.Tanks) return "❌ У Вас недостаточно солдат.";
                countHP = countArmy * 100;
                var tanks = resources.Tanks;
                resources.Tanks = tanks - countArmy;
            }else return "❌ Вы ввели неизвестный тип армии.";
            if(msg.PeerId == battle.UserOne) 
            {
                var hpUser = battle.HpTwo;
                hpUser -= countHP;
                if(hpUser < 0 || hpUser == 0) 
                {
                    //endGame
                    return "✅ Вы уничтожили противника!";
                }else 
                {
                    battle.HpTwo = hpUser;
                    battle.UserCourse = battle.UserTwo;
                    return $"✅ Вы нанесли {countHP} урона противнику!";
                } 
            }else if(msg.PeerId == battle.UserTwo) 
            {
                var hpUser = battle.HpOne;
                hpUser -= countHP;
                if (hpUser < 0 || hpUser == 0)
                {
                    //endGame
                    return "✅ Вы уничтожили противника!";
                }else 
                {
                    battle.UserCourse = battle.HpOne;
                    battle.HpOne = hpUser;
                    return $"✅ Вы нанесли {countHP} урона противнику!";           
                } 
            }
            return "❌ Неизвестный пользователь.";
        }


        private string GetBattleText(LongPollVK.Models.AddNewMsg msg) 
        {
            return "Лобби.";
        }

        [Attributes.Trigger("вступить")]
        public static string JoinToBattle(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');
            var idBattle = 0;
            try {
                 idBattle = Convert.ToInt32(messageArray[2]);
            }catch(FormatException) {
                return "❌ Вы указали неверный ID";
            }
            if(!Api.Battles.Check(idBattle)) return "❌ Такой битвы не существует";
            var battle = new Api.Battles(idBattle);
            var user = Api.User.GetUser(msg.PeerId);
            var resources = new Api.Resources(msg.PeerId);
            var builds = new Api.Builds(msg.PeerId);
            if(user.IdBattle != 0) return "❌ Вы уже находитесь в другой битве.";
            
                if(battle.IsStart) return "❌ Эта битва уже началась. Вы не можете в неё вступить.";
                if(battle.Id == Convert.ToInt64(idBattle)) 
                {
                    if(resources.MoneyCard < battle.Price) return "❌ На Вашем банковском счету недостаточно монет.";
                    Notifications.RemovePaymentCard(Convert.ToInt32(battle.Price), msg.PeerId, "битвы");
                    //battles.Battles.Remove(battle);
                    user.IdBattle = battle.Id;
                    if(!Api.User.SetUser(user)) return "❌ Произошла ошибка при добавлении в БД. Вы не присоеденились к битве. Попробуйте позже";
                    var userHp = (builds.Apartments * 10) + (builds.Eatery * 10) + 
                    (builds.Hangars * 50) + (builds.Mine * 10) + (builds.PowerGenerators * 50) + 
                    (builds.WarehouseEat * 30) + (builds.WarehouseEnergy * 20) + 
                    (builds.WarehouseWater * 10) + (builds.WaterPressureStation * 50) +
                    (user.Level * 100);
                    battle.UserTwo = msg.PeerId;
                    battle.HpTwo = userHp;
                    battle.IsStart = true;
                   // battles.Battles.Add(battle);
                  //  Api.Battles.Set(battles);
                    Api.MessageSend("‼ К Вам в битву вступили! Вы атакуете первый!", battle.Creator);
                    return "✅ Вы успешно вступили в эту битву! Враг атакует первый.";
                }
            

            return "❌ Вы указали неверный ID";
        }

        [Attributes.Trigger("создать")]
        public static string CreateBattle(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');

            var price = 0;
            try {
                price = Convert.ToInt32(messageArray[2]);
            } catch(FormatException) {
                return "❌ Вы ввели неверную сумму.";
            }
            string body = "";
            for(int i=3; messageArray.Length > i; i++) 
            {
                body += $"{messageArray[i]} ";
            }      

            var resources = new Api.Resources(msg.PeerId);
            var user = Api.User.GetUser(msg.PeerId);
            if (user.IdBattle != 0) return "❌ Вы уже находитесь в другой битве.";
            if(resources.MoneyCard < price) return "❌ Вы ставите денег больше, чем у Вас на балансе.";
            //var battlesList = Api.Battles.Get();
            //var battle = new Models.Battle();
            long userHp = 0;
            var builds = new Api.Builds(msg.PeerId);
            userHp = (builds.Apartments * 10) + (builds.Eatery * 10) + (builds.Hangars * 50) +
            (builds.Mine * 10) + (builds.PowerGenerators * 50) + (builds.WarehouseEat * 30) +
            (builds.WarehouseEnergy * 20) + (builds.WarehouseWater * 10) +
            (builds.WaterPressureStation * 50) + (user.Level * 100);
            var battleId = Api.Battles.NewBattle(msg.PeerId, body, userHp, price);
            var ListBattles = Api.Battles.GetListBattles();
            if(ListBattles.Battles == null) ListBattles.Battles = new System.Collections.Generic.List<long>();
            ListBattles.Battles.Add(battleId);
            Api.Battles.SetListBattles(ListBattles);
            Notifications.RemovePaymentCard(price, msg.PeerId, "Содание битвы");
           // battlesList.Battles.Add(battle);
            user.IdBattle = battleId;
            if(!Api.User.SetUser(user)) return "Ошибка при добавлении пользователя в БД. Но битва создалась наверное. Вы не вошли в битву. И не пытайтесь.";
            return "✅ Вы успешно создали новую битву! Теперь осталось пододать противника.";
        }

        [Attributes.Trigger("список")]
        public static string ListBattles(LongPollVK.Models.AddNewMsg msg) 
        {
            string json =  "";
            using(var reader = new StreamReader(@"Files/Battles.json")) 
            {
                json = reader.ReadToEnd();
            }
            var model = JsonConvert.DeserializeObject<Models.ActiveBattles>(json);

            string result = "CПИСОК БИТВ➖➖➖➖➖➖➖➖➖➖➖➖➖\n";
            if(model.Battles.Count == 0) return "Список битв пуст. Создайте первую битву!";
            foreach(var battleId in model.Battles) 
            {
                var battle = new Api.Battles(battleId);
                var user = Api.User.GetUser(battle.Creator);
                result +=   $"⚔ Битва №{battle.Id}.\n Название: {battle.Body}."+
                          $"\n🧑 Создатель: {user.Name}"+
                          $"\n🔝 Уровень противника: {user.Level}"+
                          $"\n🛡 HP противника: {battle.HpOne}"+
                          $"\n💰 Ставка: {battle.Price}"+
                          $"\n💥 Чтобы вступить в эту битву, напишите: бой вступить {battle.Id}"+
                          $"\n";
                          
            }
            result+= "➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
            return result;
        }
    }
}