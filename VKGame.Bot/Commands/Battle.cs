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

        public object Execute(LongPollVK.Models.AddNewMsg msg) 
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
            return "❌ Неизвестная подкоманда.";
        }

        [Attributes.Trigger("бот")]
        public static string Bot(LongPollVK.Models.AddNewMsg msg)
        {
            //создание битвы
            var messageArray = msg.Text.Split(' ');
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
            var resources = new Api.Resources(msg.PeerId);
            if (resources.MoneyCard < price) return $"❌ Вы указали сумму больше, чем у вас есть! Ваш баланс: {resources.MoneyCard}";
            var user = Api.User.GetUser(msg.PeerId);
            user.CountCreateBattles = user.CountCreateBattles + 1;
            if(resources.MoneyCard < price)
            if (user.IdBattle != 0) return $"❌ Вы уже находитесь в другой битве. Id битвы, в которой Вы находитесь: {user.IdBattle}. Чтобы покинуть ту битву, напишите: бой покинуть";
            long userHp = 0;
            var builds = new Api.Builds(msg.PeerId);
            userHp = (builds.Apartments * 10) + (builds.Eatery * 10) + (builds.Hangars * 50) +
            (builds.Mine * 10) + (builds.PowerGenerators * 50) + (builds.WarehouseEat * 30) +
            (builds.WarehouseEnergy * 20) + (builds.WarehouseWater * 10) +
            (builds.WaterPressureStation * 50) + (user.Level * 100);
            var battleId = Api.Battles.NewBattle(msg.PeerId, "Битва с ботом", userHp, price);
            Notifications.RemovePaymentCard(price, msg.PeerId, "битва с ботом");
            user.IdBattle = battleId;
            Api.User.SetUser(user);

            //Бот присоединяется к битве
            var battle = new Api.Battles(battleId)
            {
                UserTwo = 16101,
                HpTwo = userHp,
                IsStart = true
            };
            return "✅ Вы успешно создали новую битву! Вы атакуете первый! " +
                "\n❓ Как атаковать: бой атака кол-во тип_войска" +
                "\n❗ Пример: бой атака 10 солдат" +
                "\n❗ Доступные типы войск: солдат и танков";
        }

        private static void BotAttack(long BattleId)
        {
            var battle = new Api.Battles(BattleId);
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
            Api.MessageSend($"🤔 Вам нанесли {Hp} урона! Срочно отвечайте!" +
                $"\n❓ Как атаковать: бой атака кол-во тип_войска" +
                $"\n❗ Пример: бой атака 10 солдат" +
                $"\n❗ Доступные типы войск: солдат и танков", battle.UserOne);
            if(userHp < 0 || userHp == 0)
            {
                var user = Api.User.GetUser(battle.UserOne);
                user.IdBattle = 0;
                Api.User.SetUser(user);
                Api.MessageSend("😥 :( Вы проиграли! Попробуйте ещё раз!", battle.UserOne);
            }
        } 
        

        [Attributes.Trigger("атака")]
        public static string Attack(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');
            var resources = new Api.Resources(msg.PeerId);
            var user = Api.User.GetUser(msg.PeerId);
            if(user.IdBattle == 0 ) return "❌ Вы не участвуете ни в какой битве. Вы можете присоединиться к уже существующей: бой список  или вы можете создать свою: бой создать название цена";
            var battle = new Api.Battles(user.IdBattle);
            if(battle.UserCourse != msg.PeerId) return "❌ Сейчас ход Вашего противника.";
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
                resources.Soldiery = soldiery- countArmy;
                var countFoodForSoldiery = countArmy * 5;
                if (countFoodForSoldiery > food) return $"❌ У Вас недостаточно еды, чтобы прокормить армию! Ваши запасы: {food}. Необходимо еды: {countFoodForSoldiery}. Вы можете купить еды в магазине или подождать пока она появится сама.";
                food = food - countFoodForSoldiery;
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
            if(msg.PeerId == battle.UserOne) 
            {
                var hpUser = battle.HpTwo;
                hpUser -= countHP;
                if(hpUser < 0 || hpUser == 0) 
                {
                    var listBattles = Api.Battles.GetListBattles();
                    listBattles.Battles.Remove(battle.Id);
                    Api.Battles.SetListBattles(listBattles);
                    if(user.Competition == 0)
                        Notifications.EnterPaymentCard(Convert.ToInt32(battle.Price * 2), msg.PeerId, "победа в битве");
                    user.IdBattle = 0;
                    user.CountWinBattles = user.CountWinBattles +1;
                    user.CountBattles = user.CountBattles + 1;
                    user.Experience = user.Experience + 10;
                    Api.User.SetUser(user);
                    if(battle.UserTwo !=16101)
                    {
                        var userTwo = Api.User.GetUser(battle.UserTwo);
                        userTwo.IdBattle = 0;
                        userTwo.CountBattles = userTwo.CountBattles + 1;
                        Api.User.SetUser(userTwo);
                    }
                    int shance = r.Next(1, 4);
                    Quests.WinBattle(user.Id);
                    string WinText = "✨🎉 Поздравляю! Вы победили! Вы уничтожили противника! За это вы получаете фонд битвы!";
                    if (shance== 4)
                    {
                        Statistics.WinBox();
                        WinText += "\n 🎈 Вам выпал битвенный кейс!";
                        var boxes = new Api.Boxes(msg.PeerId);
                        var battleList = boxes.BattleBox;
                        battleList.Add(new Models.BattleBox());
                        boxes.BattleBox = battleList;
                    }
                    Statistics.WinBattle();
                    if(battle.UserTwo != 16101) Api.MessageSend("❌ПОРАЖЕНИЕ! ВАС УНИЧТОЖИЛИ! В следующем бою Вам повезёт больше!", battle.UserTwo);
                    if(user.Competition != 0)
                    {
                        Competitions.EndBattle(msg.PeerId, battle.UserTwo, user.Competition, battle.Id);
                    }
                    return WinText;
                }else 
                {
                    battle.HpTwo = hpUser;
                    battle.UserCourse = battle.UserTwo;
                    if (battle.UserTwo == 16101)
                    {
                        BotAttack(battle.Id);
                    }else
                    {
                        Api.MessageSend($"❌ Вам нанесли {countHP} урона! Осталось хп: {battle.HpTwo}. Теперь Ваша очередь атаковать!" +
                            $"\n❓ Как атаковать: бой атака кол-во тип_войска" +
                            $"\n❗ Пример: бой атака 10 солдат" +
                            $"\n❗ Доступные типы войск: солдат и танков", battle.UserTwo);
                    }
                    return $"✅ Вы нанесли {countHP} урона противнику! Остаток хп противника: {battle.HpTwo}.";
                } 
            }else if(msg.PeerId == battle.UserTwo) 
            {
                var hpUser = battle.HpOne;
                hpUser -= countHP;
                if (hpUser < 0 || hpUser == 0)
                {
                    var listBattles = Api.Battles.GetListBattles();
                    listBattles.Battles.Remove(battle.Id);
                    Api.Battles.SetListBattles(listBattles);
                    if(user.Competition == 0)
                        Notifications.EnterPaymentCard(Convert.ToInt32(battle.Price * 2), msg.PeerId, "победа в битве");
                    user.CountWinBattles = user.CountWinBattles + 1;
                    user.CountBattles = user.CountBattles + 1;

                    user.IdBattle = 0;
                    user.Experience = user.Experience + 10;
                    Api.User.SetUser(user);
                    var userTwo = Api.User.GetUser(battle.UserOne);
                    userTwo.IdBattle = 0;
                    userTwo.CountBattles = userTwo.CountBattles + 1;
                    Api.User.SetUser(userTwo);
                    int shance = r.Next(1, 5);
                    Quests.WinBattle(user.Id);

                    string WinText = "✨🎉 Поздравляю! Вы победили! Вы уничтожили противника! За это вы получаете фонд битвы!";
                    if (shance == 4)
                    {
                        Statistics.WinBox();

                        WinText += "\n 🎈 Вам выпал битвенный кейс!";
                        var boxes = new Api.Boxes(msg.PeerId);
                        var battleList = boxes.BattleBox;
                        battleList.Add(new Models.BattleBox());
                        boxes.BattleBox = battleList;
                    }
                    Api.MessageSend("❌ ПОРАЖЕНИЕ! ВАС УНИЧТОЖИЛИ!  В следующем бою Вам повезёт больше!", battle.UserOne);
                    if (user.Competition != 0)
                    {
                        Competitions.EndBattle(msg.PeerId, battle.UserOne, user.Competition, battle.Id);
                    }
                    return WinText;
                }else 
                {
                    battle.UserCourse = battle.UserOne;
                    battle.HpOne = hpUser;
                    Api.MessageSend($"❌ Вам нанесли {countHP} урона! Осталось хп: {battle.HpOne} Теперь Ваша очередь атаковать!" +
                        $"\n❓ Как атаковать: бой атака кол-во тип_войска" +
                        $"\n❗ Пример: бой атака 10 солдат" +
                        $"\n❗ Доступные типы войск: солдат и танков", battle.UserOne);
                    return $" Вы нанесли {countHP} урона противнику! Остаток хп противника: {battle.HpOne}.";           
                } 
            }
            return "❌ Неизвестный пользователь. (ОШИБКА ЕРРОР ПИЗДА ВСЕМУ МИРУ)";
        }


        private string GetBattleText(LongPollVK.Models.AddNewMsg msg) 
        {
            var user = Api.User.GetUser(msg.PeerId);
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n✋ Привет, {user.Name}!"+
                   $"\n" +
                   $"\n⚔ Чтобы начать бой - посмотри список боёв! Напиши: бой список" +
                   $"\n🛡 Не хочешь вступать в бой? Создай свой! Напиши: бой создать сумма название" +
                   $"\n😎 Нет подходящей битвы? Поиграй с ботом! Напиши: бой бот сумма"+
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";

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
            }catch(IndexOutOfRangeException) 
            {
                return "❌ Вы не указали ID";
            }
            if(!Api.Battles.Check(idBattle)) return "❌ Такой битвы не существует";
            var battle = new Api.Battles(idBattle);
            var user = Api.User.GetUser(msg.PeerId);
            var resources = new Api.Resources(msg.PeerId);
            var builds = new Api.Builds(msg.PeerId);
            if(battle.Creator == msg.PeerId) return "❌ Вы не можете вступить в свою же битву.";
            if(user.IdBattle != 0) return $"❌ Вы уже находитесь в другой битве c ID - {user.IdBattle}";
            
                if(battle.IsStart) return "❌ Эта битва уже началась. Вы не можете в неё вступить.";
                if(battle.Id == Convert.ToInt64(idBattle)) 
                {
                    if(resources.MoneyCard < battle.Price) return $"❌ На Вашем банковском счету недостаточно монет. Ваш баланс: {resources.MoneyCard}";
                    Notifications.RemovePaymentCard(Convert.ToInt32(battle.Price), msg.PeerId, "битвы");
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
                    var listBattle = Api.Battles.GetListBattles();
                    listBattle.Battles.Remove(battle.Id);
                    Api.Battles.SetListBattles(listBattle);
                    Statistics.JoinBattle();
                    Api.MessageSend("‼ К Вам в битву вступили! Вы атакуете первый! " +
                        "\n❓ Как атаковать: бой атака кол-во тип_войска" +
                        "\n❗ Пример: бой атака 10 солдат" +
                        "\n❗ Доступные типы войск: солдат и танков", battle.Creator);
                    return "✅ Вы успешно вступили в эту битву! Враг атакует первый.";
                }
            

            return "❌ Вы указали неверный ID";
        }

        [Attributes.Trigger("покинуть")]
        public static string Leave(LongPollVK.Models.AddNewMsg msg) 
        {
            var user = Api.User.GetUser(msg.PeerId);
            if(user.IdBattle == 0) return "Вы не находитесь ни в какой битве.";
            var battle = new Api.Battles(user.IdBattle);
            var listBattles = Api.Battles.GetListBattles();
            listBattles.Battles.Remove(battle.Id);
            Api.Battles.SetListBattles(listBattles);
            user.IdBattle = 0;
            Api.User.SetUser(user);
            
            long userwo = 0 ;
            if(battle.UserOne == msg.PeerId) userwo = battle.UserTwo;
            else if(battle.UserTwo == msg.PeerId) userwo = battle.UserOne;
            
            if(userwo != 16101)
            {
                if (userwo != 0)
                {
                    var userTwo = Api.User.GetUser(userwo);
                    userTwo.IdBattle = 0;
                    Api.User.SetUser(userTwo);
                    if (battle.UserTwo != 16101)
                    {
                        Notifications.EnterPaymentCard(System.Convert.ToInt32(battle.Price * 2), userwo, "Победа в битве");
                        Api.MessageSend("Ваш противник трусливо сбежал! Вы получаете весь фонд!", userwo);
                    }
                    else
                    {


                    }
                }
            } 
            
            return "✅ Вы успешно покинули битву :( Плохо так делать!";
        }

        [Attributes.Trigger("мой")]
        public static string MyBattle(LongPollVK.Models.AddNewMsg msg) 
        {
            var user = Api.User.GetUser(msg.PeerId);
            if(user.IdBattle == 0) return "❌ Вы не находитесь ни в какой битве.";
            var battle = new Api.Battles(user.IdBattle);
            Models.User userCreator = null;

            if (battle.Creator != user.Id)
            {
                userCreator = Api.User.GetUser(battle.Creator);
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
        public static string CreateBattle(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');
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

            var resources = new Api.Resources(msg.PeerId);
            var user = Api.User.GetUser(msg.PeerId);
            user.CountCreateBattles = user.CountCreateBattles +1;
            if (user.IdBattle != 0) return $"❌ Вы уже находитесь в другой битве c ID - {user.IdBattle}.";
            if (resources.MoneyCard < price) return $"❌ Вы ставите денег больше, чем у Вас на балансе. Ваш баланс: {resources.MoneyCard} 💳 Вы можете получить деньги, например, в казино";
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
            Notifications.RemovePaymentCard(price, msg.PeerId, "Создание битвы");
            user.IdBattle = battleId;
            if(!Api.User.SetUser(user)) return "Ошибка при добавлении пользователя в БД. Но битва создалась наверное. Но Вы не вошли в битву.";
            return "✅ Вы успешно создали новую битву! Теперь осталось подождать противника.";
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
            if(model.Battles.Count == 0) return "😒 Список битв пуст. Создайте первую битву!";
            foreach(var battleId in model.Battles) 
            {
                var battle = new Api.Battles(battleId);
                var user = Api.User.GetUser(battle.Creator);
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