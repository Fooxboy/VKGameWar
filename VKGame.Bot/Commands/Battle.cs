using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace VKGame.Bot.Commands
{
    public class Battle : ICommand 
    {
        public string Name => "Бой";
        public string Arguments => "(), (вариант_выбора)";
        public string Caption => "Раздел предназначен для проведения боёв.";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>() { "бот", "атака", "вступить", "покинуть", "мой", "создать", "список" };

        public object Execute(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');
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
                            if (myAtr.Name.ToLower() == messageArray[1].ToLower())
                            {
                                object result = method.Invoke(obj, new object[] { msg });
                                return (string)result;
                            }
                        }
                    }
                }
            }
            var word = Common.SimilarWord(messageArray[0], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        public static class Api
        {
            public static long HpUser(long userId, Models.User user = null, Bot.Api.Builds builds = null )
            {
                if (builds == null) builds = new Bot.Api.Builds(userId);
                if (user == null) user = VKGame.Bot.Api.User.GetUser(userId);
                return (builds.Apartments * 10) + (builds.Eatery * 10) + (builds.Hangars * 50) +
                (builds.Mine * 10) + (builds.PowerGenerators * 50) + (builds.WarehouseEat * 30) +
                (builds.WarehouseEnergy * 20) + (builds.WarehouseWater * 10) +
                (builds.WaterPressureStation * 50) + (user.Level * 100);
            }

            public static bool JoinBattle(long battleId, long userId, Models.User user = null, Bot.Api.Battles battle = null)
            {
                if (user == null) user = Bot.Api.User.GetUser(userId);

                var userHp = Api.HpUser(userId, user);
                if (battle == null) battle = new Bot.Api.Battles(battleId);
                battle.UserTwo = userId;
                battle.HpTwo = userHp;
                battle.IsStart = true;
               
                Statistics.JoinBattle();

                return true;
            }

            public static bool RemoveListBattle(long battleId)
            {
                var listBattle = Bot.Api.Battles.GetListBattles();
                listBattle.Battles.Remove(battleId);
                Bot.Api.Battles.SetListBattles(listBattle);
                return true;
            }

            public static bool AddToListBattles(long battleId)
            {
                var ListBattles = VKGame.Bot.Api.Battles.GetListBattles();
                if (ListBattles.Battles == null) ListBattles.Battles = new System.Collections.Generic.List<long>();
                ListBattles.Battles.Add(battleId);
                VKGame.Bot.Api.Battles.SetListBattles(ListBattles);
                return true;
            }

            public static long CreateBattle(long userCreate, string body, int price, Models.User user= null)
            {
                if (user == null) user = VKGame.Bot.Api.User.GetUser(userCreate);
                long userHp = Api.HpUser(userCreate,user);
                var builds = new Bot.Api.Builds(userCreate);
                var battleId = VKGame.Bot.Api.Battles.NewBattle(userCreate, body, userHp, price);
               
                Notifications.RemovePaymentCard(price, userCreate, "Создание битвы");
                return battleId;
            }
        }  

        [Attributes.Trigger("бот")]
        public static string BotPlay(Models.Message msg)
        {
            //создание битвы
            var messageArray = msg.body.Split(' ');
            int price = 0;
            try
            {
                price = Convert.ToInt32(messageArray[2]);
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не уазали сумму!";
            }catch(FormatException)
            {
                return "❌ Вы указали неверную сумму";
            }
            var user = Bot.Api.User.GetUser(msg.from_id);
            var registry = Bot.Api.Registry.GetRegistry(user.Id);
            var resources = new Bot.Api.Resources(user.Id);
            if (resources.MoneyCard < price) return $"❌ Вы указали сумму больше, чем у вас есть! Ваш баланс: {resources.MoneyCard}";
            ++registry.CountCreateBattles;
            if(resources.MoneyCard < price)
            if (user.IdBattle != 0) return $"❌ Вы уже находитесь в другой битве. Id битвы, в которой Вы находитесь: {user.IdBattle}. Чтобы покинуть ту битву, напишите: бой покинуть";
            var idBattle = Api.CreateBattle(user.Id, "Бой с ботом", price, user);
            user.IdBattle = idBattle;
            Bot.Api.User.SetUser(user);
            Bot.Api.Registry.SetRegistry(registry);

            //Бот присоединяется к битве
            var battle = new Bot.Api.Battles(idBattle);
            battle.UserTwo = 16101;
            battle.HpTwo = Api.HpUser(user.Id, user);
            battle.IsStart = true;
            return "✅ Вы успешно создали новую битву! Вы атакуете первый! " +
                "\n❓ Как атаковать: бой атака кол-во тип_войска" +
                "\n❗ Пример: бой атака 10 солдат" +
                "\n❗ Доступные типы войск: солдат и танков";
        }

        private static void BotAttack(long BattleId)
        {
            var battle = new Bot.Api.Battles(BattleId);
            long[] HpArray =
            {
                40, 50,60, 80, 90, 100,140, 160, 200,210
            };
            var r = new Random();
            long Hp = HpArray[r.Next(0, HpArray.Length - 1)];

            var userHp = battle.HpOne;

            userHp = userHp - Hp;
            battle.HpOne = userHp;
            battle.UserCourse = battle.UserOne;
            Bot.Api.MessageSend($"🤔 Вам нанесли {Hp} урона! Срочно отвечайте!" +
                $"\n❓ Как атаковать: бой атака кол-во тип_войска" +
                $"\n❗ Пример: бой атака 10 солдат" +
                $"\n❗ Доступные типы войск: солдат и танков", battle.UserOne);
            if(userHp < 0 || userHp == 0)
            {
                var user = Bot.Api.User.GetUser(battle.UserOne);
                user.IdBattle = 0;
                Bot.Api.User.SetUser(user);
                Bot.Api.MessageSend("😥 :( Вы проиграли! Попробуйте ещё раз!", battle.UserOne);
            }
        } 
        

        [Attributes.Trigger("атака")]
        public static string Attack(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');
            var user = Bot.Api.User.GetUser(msg.from_id);
            var resources = new Bot.Api.Resources(user.Id);
            
            if(user.IdBattle == 0 ) return "❌ Вы не участвуете ни в какой битве. Вы можете присоединиться к уже существующей: бой список  или вы можете создать свою: бой создать название цена";
            var battle = new Bot.Api.Battles(user.IdBattle);
            if(battle.UserCourse != user.Id) return "❌ Сейчас ход Вашего противника.";
            int countArmy = 0;
            try 
            {
                countArmy = Int32.Parse(messageArray[2]);
            }catch (FormatException) 
            {
                return "❌ Вы указали неверное число.";
            }catch(IndexOutOfRangeException) 
            {
                return "❌ Вы не указали число!";
            }

            string type = "";
            try {
                type = messageArray[3].ToLower();
            }catch(IndexOutOfRangeException) 
            {
                return "❌ Вы не указали тип войска. Доступные типы войск: содалдат и  танков";
            }
            var r = new Random();
             
            int countHP = 0;
            if(type == "солдат") 
            {
                if(countArmy > resources.Soldiery) return $"❌ У Вас недостаточно солдат. У Вас осталось солдат: {resources.Soldiery}. Вы можете прямо сейчас обучить новых!";

                countHP = countArmy * 10;
                if (countHP > 300) return "Вы не можете наносить больше 300 урона за раз!";
                var food = resources.Food;
                var soldiery = resources.Soldiery;
                resources.Soldiery = soldiery - countArmy;
                var countFoodForSoldiery = countArmy * 5;
                if (countFoodForSoldiery > food) return $"❌ У Вас недостаточно еды, чтобы прокормить армию! Ваши запасы: {food}. Необходимо еды: {countFoodForSoldiery}. Вы можете купить еды в магазине или подождать пока она появится сама.";
                food -= countFoodForSoldiery;
                resources.Food = food;
            }else if( type == "танков") 
            {
                if(countArmy > resources.Tanks) return $"❌ У Вас недостаточно танков. У Вас осталось танков: {resources.Tanks}. Вы можете прямо сейчас создать новых!";
                countHP = countArmy * 100;
                if (countHP > 500) return "Вы не можете наносить больше 500 урона за раз!";
                var tanks = resources.Tanks;
                var water = resources.Water;
                resources.Tanks = tanks - countArmy;
                var countWaterForTanks = countArmy * 5;
                if (countWaterForTanks > resources.Water) return $"❌ ОГО! У Вас кончилась вода! Осталось: {resources.Water}. Необходимо: {countWaterForTanks} А как танки будут ездить без топлива-то??? Вы можете купить воды в магазине или подождать пока она появится сама.";
                water = water - countWaterForTanks;
                resources.Water = water;

            }
            else return "❌ Вы ввели неизвестный тип армии. Доступные типы армии: танков и солдат";

            long enemy = 0;
            long hpEnemy = 0;
            long hpMy = 0;
            long enemyNumber = 0;
            if (user.Id == battle.UserOne)
            {
                enemy = battle.UserTwo;
                hpEnemy = battle.HpTwo;
                hpMy = battle.HpOne;
                enemyNumber = 2;
            }
            else if (user.Id == battle.HpTwo)
            {
                enemy = battle.UserOne;
                hpEnemy = battle.HpOne;
                hpMy = battle.HpTwo;
                enemyNumber = 1;
            }
                hpEnemy -= countHP;
                if (hpEnemy < 0 || hpEnemy == 0)
                {
                    
                    if (user.Competition == 0)
                        Notifications.EnterPaymentCard(Convert.ToInt32(battle.Price * 2), user.Id, "победа в битве");
                    user.IdBattle = 0;
                    var registry = Bot.Api.Registry.GetRegistry(user.Id);
                    ++registry.CountWinBattles;
                    ++registry.CountBattles;
                    user.Experience = user.Experience + 10;
                    Bot.Api.Registry.SetRegistry(registry);
                    Bot.Api.User.SetUser(user);
                    if (enemy != 16101)
                    {
                    var refistryTwo = Bot.Api.Registry.GetRegistry(enemy);
                        var userTwo = Bot.Api.User.GetUser(enemy);
                        userTwo.IdBattle = 0;
                        ++refistryTwo.CountBattles;
                        Bot.Api.User.SetUser(userTwo);
                        Bot.Api.Registry.SetRegistry(refistryTwo);
                    }
                    int shance = r.Next(1, 4);
                    Quests.WinBattle(user.Id);
                    string WinText = "✨🎉 Поздравляю! Вы победили! Вы уничтожили противника! За это вы получаете фонд битвы!";
                    if (shance == 4)
                    {
                        Statistics.WinBox();
                        WinText += "\n 🎈 Вам выпал битвенный кейс!";
                        var boxes = new Bot.Api.Boxes(user.Id);
                        var battleList = boxes.BattleBox;
                        battleList.Add(new Models.BattleBox());
                        boxes.BattleBox = battleList;
                    }
                    Statistics.WinBattle();
                    if (battle.UserTwo != 16101) Bot.Api.MessageSend("❌ПОРАЖЕНИЕ! ВАС УНИЧТОЖИЛИ! В следующем бою Вам повезёт больше!", battle.UserTwo);
                    if (user.Competition != 0)
                    {
                        Competitions.EndBattle(user.Id, battle.UserTwo, user.Competition, battle.Id);
                    }
                    return WinText;
                }
                else 
                {
                    if(enemyNumber ==1)
                {
                    battle.HpOne = hpEnemy;
                }else if(enemyNumber == 2)
                {
                    battle.HpTwo = hpEnemy;
                }
                   
                    battle.UserCourse = battle.UserTwo;
                    if (battle.UserTwo == 16101)
                    {
                        BotAttack(battle.Id);
                    }else
                    {
                        Bot.Api.MessageSend($"❌ Вам нанесли {countHP} урона! Осталось хп: {battle.HpTwo}. Теперь Ваша очередь атаковать!" +
                            $"\n❓ Как атаковать: бой атака кол-во тип_войска" +
                            $"\n❗ Пример: бой атака 10 солдат" +
                            $"\n❗ Доступные типы войск: солдат и танков", enemy);
                    }
                    return $"✅ Вы нанесли {countHP} урона противнику! Остаток хп противника: {battle.HpTwo}.";
                } 
        }
        private string GetBattleText(Models.Message msg) 
        {
            var user = Bot.Api.User.GetUser(msg.from_id);
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n✋ Привет, {user.Name}!"+
                   $"\n" +
                   $"\n⚔ Чтобы начать бой - посмотри список боёв! Напиши: бой список" +
                   $"\n🛡 Не хочешь вступать в бой? Создай свой! Напиши: бой создать сумма название" +
                   $"\n😎 Нет подходящей битвы? Поиграй с ботом! Напиши: бой бот сумма"+
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";

        }

        [Attributes.Trigger("вступить")]
        public static string JoinToBattle(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');
            var idBattle = 0;
            try {
                 idBattle = Convert.ToInt32(messageArray[2]);
            }catch(FormatException) {
                return "❌ Вы указали неверный ID";
            }catch(IndexOutOfRangeException) 
            {
                return "❌ Вы не указали ID";
            }
            if(!Bot.Api.Battles.Check(idBattle)) return "❌ Такой битвы не существует";
            var battle = new Bot.Api.Battles(idBattle);
            var user = Bot.Api.User.GetUser(msg.from_id);
            var resources = new Bot.Api.Resources(user.Id);
            var builds = new Bot.Api.Builds(user.Id);
            if(battle.Creator == user.Id) return "❌ Вы не можете вступить в свою же битву.";
            if(user.IdBattle != 0) return $"❌ Вы уже находитесь в другой битве c ID - {user.IdBattle}";
            
                if(battle.IsStart) return "❌ Эта битва уже началась. Вы не можете в неё вступить.";
                if(battle.Id == Convert.ToInt64(idBattle)) 
                {
                    if(resources.MoneyCard < battle.Price) return $"❌ На Вашем банковском счету недостаточно монет. Ваш баланс: {resources.MoneyCard}";
                    Notifications.RemovePaymentCard(Convert.ToInt32(battle.Price), user.Id, "битвы");
                    Api.JoinBattle(battle.Id, user.Id, user, battle);
                    user.IdBattle = battle.Id;
                    Api.RemoveListBattle(battle.Id);
                    if(!Bot.Api.User.SetUser(user)) return "❌ Произошла ошибка при добавлении в БД. Вы не присоеденились к битве. Попробуйте позже";
                    Bot.Api.MessageSend("‼ К Вам в битву вступили! Вы атакуете первый! " +
                    "\n❓ Как атаковать: бой атака кол-во тип_войска" +
                    "\n❗ Пример: бой атака 10 солдат" +
                    "\n❗ Доступные типы войск: солдат и танков", battle.Creator);
                return "✅ Вы успешно вступили в эту битву! Враг атакует первый.";
                }
            return "❌ Вы указали неверный ID";
        }

        [Attributes.Trigger("покинуть")]
        public static string Leave(Models.Message msg) 
        {
            var user = Bot.Api.User.GetUser(msg.from_id);
            if(user.IdBattle == 0) return "Вы не находитесь ни в какой битве.";
            var battle = new Bot.Api.Battles(user.IdBattle);
            user.IdBattle = 0;
            Bot.Api.User.SetUser(user);
            
            long userwo = 0 ;
            if(battle.UserOne == user.Id) userwo = battle.UserTwo;
            else if(battle.UserTwo == user.Id) userwo = battle.UserOne;
            
            if(userwo != 16101)
            {
                if (userwo != 0)
                {
                    var userTwo = Bot.Api.User.GetUser(userwo);
                    userTwo.IdBattle = 0;
                    Bot.Api.User.SetUser(userTwo);
                    Notifications.EnterPaymentCard(System.Convert.ToInt32(battle.Price * 2), userwo, "Победа в битве");
                    Bot.Api.MessageSend("Ваш противник трусливо сбежал! Вы получаете весь фонд!", userwo);
                }
            } 
            
            return "✅ Вы успешно покинули битву :( Плохо так делать!";
        }

        [Attributes.Trigger("мой")]
        public static string MyBattle(Models.Message msg) 
        {
            var user = Bot.Api.User.GetUser(msg.from_id);
            if(user.IdBattle == 0) return "❌ Вы не находитесь ни в какой битве.";
            var battle = new Bot.Api.Battles(user.IdBattle);
            Models.User userCreator = null;

            if (battle.Creator != user.Id)
            {
                userCreator = Bot.Api.User.GetUser(battle.Creator);
            }else
            {
                userCreator = user;
            }

            string result = $"⚔ Битва №{battle.Id}.\n ➡ Название: {battle.Body}." +
                          $"\n😀 Создатель: [id{userCreator.Id}|{userCreator.Name}]" +
                          $"\n🔝 Уровень противника: {userCreator.Level}" +
                          $"\n🛡 HP противника: {battle.HpOne}" +
                          $"\n💰 Ставка: {battle.Price}";

            return result;
        }

        [Attributes.Trigger("создать")]
        public static string CreateBattle(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');
            Statistics.CreateBattle();

            var price = 0;
            try {
                price = Convert.ToInt32(messageArray[2]);
            } catch(FormatException) {
                return "❌ Вы ввели неверную сумму.";
            } catch(IndexOutOfRangeException) 
            {
                return "❌ Вы не указали цену.";
            }
            string body = "";
            try {
                for (int i = 3; messageArray.Length > i; i++)
                {
                    body += $"{messageArray[i]} ";
                }
            }catch(IndexOutOfRangeException) 
            {
                body = "Бой на смерть";
            }
            var user = VKGame.Bot.Api.User.GetUser(msg.from_id);
            var resources = new Bot.Api.Resources(user.Id);
            if (user.IdBattle != 0) return $"❌ Вы уже находитесь в другой битве c ID - {user.IdBattle}.";
            if (resources.MoneyCard < price) return $"❌ Вы ставите денег больше, чем у Вас на балансе. Ваш баланс: {resources.MoneyCard} 💳 Вы можете получить деньги, например, в казино";
            var battleId = Api.CreateBattle(user.Id, body, price, user);
            var registry = Bot.Api.Registry.GetRegistry(user.Id);
            ++registry.CountCreateBattles;
            user.IdBattle = battleId;
            Api.AddToListBattles(battleId);
            Bot.Api.Registry.SetRegistry(registry);
            if (!VKGame.Bot.Api.User.SetUser(user)) return "Ошибка при добавлении пользователя в БД. Но битва создалась наверное. Но Вы не вошли в битву.";
            return "✅ Вы успешно создали новую битву! Теперь осталось подождать противника.";
        }

        [Attributes.Trigger("список")]
        public static string ListBattles(Models.Message msg) 
        {
            string json =  "";
            using(var reader = new StreamReader(@"Files/Battles.json")) 
            {
                json = reader.ReadToEnd();
            }
            var model = JsonConvert.DeserializeObject<Models.ActiveBattles>(json);      
            string result = "CПИСОК БИТВ➖➖➖➖➖➖➖➖➖➖➖➖➖\n";
            if(model.Battles.Count == 0) return "😒 Список битв пуст. Создайте первую битву!";
            foreach(var battleId in model.Battles) 
            {
                var battle = new Bot.Api.Battles(battleId);
                var user = VKGame.Bot.Api.User.GetUser(battle.Creator);
                result +=   $"⚔ Битва №{battle.Id}.\n ➡ Название: {battle.Body}."+
                          $"\n🧑 Создатель: [id{user.Id}|{user.Name}]"+
                          $"\n🔝 Уровень противника: {user.Level}"+
                          $"\n🛡 HP противника: {battle.HpOne}"+
                          $"\n💰 Ставка: {battle.Price}"+
                          $"\n💥 Чтобы вступить в эту битву, напишите: бой вступить {battle.Id}"+
                          $"\n"+
                          $"\n";                   
            }
            result+= "\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
            return result;
        }
    }
}