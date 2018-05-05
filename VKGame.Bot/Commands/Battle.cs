using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using VKGame.Bot.Api;

namespace VKGame.Bot.Commands
{
    public class Battle : ICommand
    {
        public override string Name => "Бой";
        public override string Arguments => "(), (вариант_выбора)";
        public override string Caption => "Раздел предназначен для проведения боёв.";
        public override TypeResponse Type => TypeResponse.Text;
        public override string HelpUrl => "сслыка недоступна";

        public override List<string>  Commands =>
            new List<string>() {"бот", "атака", "вступить", "покинуть", "мой", "создать", "список"};

        public override Access Access => Access.User;


        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetBattleText();
            var result = Helpers.Command.CheckMethods(typeof(Battle), messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        public static class API
        {
            public static long HpUser(long userId)
            {
                var builds = new Api.Builds(userId);
                var levels = new Api.Levels(userId);
                var user = new Api.User(userId);
                var hp = (builds.Apartments * 10) + (builds.Eatery * 10) +
                         (builds.Hangars * 30) + (builds.PowerGenerators * 30) +
                         (builds.WarehouseEat * 30) + (builds.WarehouseEnergy * 30) +
                         (builds.WaterPressureStation * 30) + (user.Level * 100) +
                         (levels.Apartments * 10) + (levels.Eatery * 10) +
                         (levels.Hangars * 10) + (levels.PowerGenerators * 10) +
                         (levels.Soldiery * 100) + (levels.Tanks * 100) +
                         (levels.WarehouseEat * 10) + (levels.WarehouseEnergy * 100) +
                         (levels.WarehouseWater * 10) + (levels.WaterPressureStation * 10);
                return hp;
            }

            public static void JoinToBattle(long userId, long idBattle)
            {
                Statistics.JoinBattle();
                var battle = new Api.Battles(idBattle);
                if(battle.Type == 1)
                {
                    var user = new User(userId);
                    var registry = new Api.Registry(userId);
                    user.BattleId = idBattle;
                    registry.CountBattles += 1;
                    battle.AddMember(userId, API.HpUser(userId));
                    battle.Found *= 2;
                    battle.IsStart = true;
                }else if(battle.Type == 2)
                {
                    var user = new User(userId);
                    var registry = new Api.Registry(userId);
                    user.BattleId = idBattle;
                    registry.CountBattles += 1;
                    battle.AddMember(userId, API.HpUser(userId));
                    battle.Found *= 2;
                    battle.IsStart = true;
                }
                else if(battle.Type == 3)
                {
                    battle.AddMember(0, API.HpUser(userId));
                    battle.Found *= 2;
                    battle.IsStart = true;
                }
                
            }
            public const long SoldieryHp = 100;           
            public const long TanksHp = 200;
            public const long OneLvlSoldiery = 50;
            public const long OneLevlTanks = 50;
            public const long ResourceSoldiery = 5;
            public const long ResourceTanks = 5;
            public static void EndBattle(long battleId, long winner)
            {
                var battle = new Api.Battles(battleId);
                if(battle.Type != 3)
                {
                    foreach (var member in battle.Members)
                    {
                        var user = new User(member.Key);
                        user.BattleId = 0;
                        if (member.Key == winner)
                        {
                            user.Experience += 10;
                            var registry = new Api.Registry(member.Key);
                            registry.CountWinBattles += 1;
                        }
                    }
                }
                
                battle.IsActive = false;
                new Task(() =>
                    Api.Message.Send($"💣 ПОБЕДА! Вы победили в этом бою! Вы получаете фонд битвы: {battle.Found} 💳", winner)).Start();
                Notifications.EnterPaymentCard(Convert.ToInt32(battle.Found), winner, "победа в битве");
            }
            
            public static long Create(long userId, string nameBattle, long price)
            {
                var user = new User(userId);
                var registry = new Registry(userId);
                registry.CountBattles += 1;
                registry.CountCreateBattles += 1;
                var id = Battles.Create(nameBattle, userId, price, HpUser(userId));
                user.BattleId = id;
                return id;
            }

            public static void BotAttack(long userId)
            {
                var user = new User(userId);
                var battleId = user.Id;
                var battle = new Battles(battleId);
                var choise = new Random().Next(1, 4);
                if (choise == 1)
                {
                    var typeBuilds = string.Empty;
                    var buildsVrag = new Builds(userId);
                    var lol = new Random().Next(1, 9);
                    if (lol == 1)
                    {
                        buildsVrag.Apartments -= 1;
                        typeBuilds = "Жилой дом";
                    }
                    else if (lol == 2)
                    {
                        buildsVrag.Eatery -= 1;
                        typeBuilds = "Столовую";
                    }
                    else if (lol == 3)
                    {
                        buildsVrag.Hangars -= 1;
                        typeBuilds = "Ангар";
                    }
                    else if (lol == 4)
                    {
                        buildsVrag.PowerGenerators -= 1;
                        typeBuilds = "Энергостаницию";
                    }
                    else if (lol == 5)
                    {
                        buildsVrag.WarehouseEat -= 1;
                        typeBuilds = "Холодильник";
                    }
                    else if (lol == 6)
                    {
                        buildsVrag.WarehouseEnergy -= 1;
                        typeBuilds = "Батарею";
                    }
                    else if (lol == 7)
                    {
                        buildsVrag.WarehouseWater -= 1;
                        typeBuilds = "Бочку с водой";
                    }
                    else if (lol == 8)
                    {
                        buildsVrag.WaterPressureStation -= 1;
                        typeBuilds = "Водонапорную башню";
                    }

                    new Task(() => Api.Message.Send($"💣 Упс! Вам бот нанёс урон по заданию! И Вы потеряли {typeBuilds}." +
                                                    "\n❓ Если у Вас количество зданий - 0 или меньше, то скорее купите новые!" +
                                                    "\n❗ Скорее отвечайте: бой атака <количество> <тип войска>", user.Id)).Start();
                }else if(choise ==2)
                {
                    new Task(() =>
                  Api.Message.Send($"🤣 Ха-ха-ха!Бот не смог в Вас попасть! Так держать!" +
                                   $"\n ❗ Скорее отвечайте: бой атака <количество> <тип войска>", userId)).Start();               
                }else if(choise == 3)
                {
                    int count = new Random().Next(5, 16);
                    var levels = new Levels(user.Id);
                    long coutHpArmy = 0;
                    coutHpArmy += count * API.SoldieryHp;
                    coutHpArmy += levels.Soldiery * API.OneLvlSoldiery;
                    var members = battle.Members;
                    var hp = members[userId] - coutHpArmy;
                    battle.SetHp(userId, hp);
                    if(hp <0 || hp == 0)
                    {
                        new Task(() =>
                        Message.Send("😭 Вас разграмили! Вы проиграли! В следующей битве повезёт больше!", userId)).Start();
                        user.BattleId = 0;
                    }else
                    {
                        new Task(() => Api.Message.Send($"💣 ВАМ НАНЕСЁН УДАР! Урон: {coutHpArmy}" +
                                               $"\n ❤ Ваше здоровье: [{hp}/{API.HpUser(userId)}]" +
                                               $"\n ❗ Скорее отвечайте: бой атака <количество> <тип войска>", userId)).Start();
                    }               
                }
            }

            public static long ChoiseUser(long user1, long user2)
            {
                var skills1 = new Api.Skills(user1);
                var skills2 = new Api.Skills(user2);
                var r = new Random();
                if (skills1.Fortuna == skills2.Fortuna)
                {
                    int choise = r.Next(1, 3);
                    if (choise == 1) return user1;
                    else if (choise == 2) return user2;
                }     
                if (skills1.Fortuna > skills2.Fortuna)
                {
                    var choise = r.Next(1, 4);
                    if (choise == 2) return user2;
                    else return user1;
                }
                else
                {
                    var choise = r.Next(1, 4);
                    if (choise == 2) return user1;
                    else return user2;
                }
            } 
        }

        [Attributes.Trigger("бот")]
        public static string BattleBot(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            var user = new User(msg.from_id);
            if (user.BattleId != 0)
                return "❌ Вы не можете начать новую битву, потому что Вы уже учавствуте в другой битве.";
            int price;
            try
            {
                price = Int32.Parse(messageArray[2]);
            }catch(FormatException) { return "❌ Сумма должна быть числом!"; }
            catch(IndexOutOfRangeException) { return "❌ Вы не указали сумму. Пример: Бой бот 1234"; }
            if (!Notifications.RemovePaymentCard(price, user.Id, "бой с ботом"))
                return $"❌ Извините, у Вас нет необходиой суммы на банковском счету. Необходимо: {price}";
            var battleId = API.Create(user.Id, $"Bot vs {user.Name}", price);

            var battle = new Battles(battleId);
            battle.Type = 3;
            API.JoinToBattle(0, battle.Id);
            battle.UserAttack = user.Id;
            return $"🏹 Вы успешно создали новую битву с ботом! Атака происходит так же, как и при обычном бое.";
        }

        
        [Attributes.Trigger("атака")]
        public static string AttackToBattle(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            var typeArmy = String.Empty;
            long count = 0;
            try
            {
                count = Int64.Parse(messageArray[2]);
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали количество армии. Пример: атака <количество> <тип армии> ";
            }
            catch (FormatException)
            {
                return "❌ Количество армии должно быть числовым значением.";
            }
            try
            {
                typeArmy = messageArray[3];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали тип армии. Доступные типы: Танков, Солдат";
            }         
            var user = new Api.User(msg.from_id);
            if (user.BattleId == 0) return "❌ Вы не учавствуете ни в какой битве.";      
            var battle = new Api.Battles(user.BattleId);
            var skills = new Api.Skills(user.Id);
            var resources = new Api.Resources(msg.from_id);
            long countHpArmy = 0;
            long countResoures = 0;
            string typeResourses = String.Empty;
            var levels = new Levels(msg.from_id);
            if (typeArmy.ToLower() == "солдат")
            {
                if (resources.Soldiery < count) return "❌ У Вас нет необходимого количества солдат.";
                countHpArmy += count * API.SoldieryHp;
                countHpArmy += levels.Soldiery * API.OneLvlSoldiery;
                countResoures += count * API.ResourceSoldiery;
                typeResourses = "🍕";
                if (resources.Food < countResoures)
                    return
                        "❌ У Вас нехватает еды, чтобы прокормить армию. Купите еды или подождите пока она сама пополниться";
                resources.Food -= countResoures;

            }
            else if (typeArmy.ToLower() == "танков")
            {
                if (resources.Tanks < count) return "❌ У Вас нет необходимого количества танков.";
                countHpArmy += count * API.TanksHp;
                countHpArmy += levels.Tanks * API.OneLevlTanks;
                countResoures += count * API.ResourceTanks;
                typeResourses = "💧";
                if (resources.Water < countResoures)
                    return
                        "❌ У Вас нехватает воды, чтобы завести танки. Купите воду в магазие или подождите пока она сама пополниться";
                resources.Water -= countResoures;
            }
            else return "❌ Вы ввели неизвестный тип армии. Доступные: Солдат, танков";
            var members = battle.Members;
            if (battle.UserAttack != msg.from_id)
                return "❌ Сейчас очередь атаки Вашего врага. Подождите его хода.";
            if (!battle.IsStart)
                return "❌ Игра ещё не началась. Вы не можете атаковать, пока игра не начнётся.";
            long vrag = 0;
            foreach (var member in members)
            {
                if (member.Key != msg.from_id) vrag = member.Key;
            }
            User userVrag = null;
            Api.Skills skillsVrag = null ;
            if(battle.Type == 1)
            {
                 userVrag = new Api.User(vrag);
                 skillsVrag = new Api.Skills(vrag);
            }else if(battle.Type == 2)
            {
                userVrag = new User(vrag);
                skillsVrag = new Api.Skills(vrag);
            }else if(battle.Type == 3)
            {
                userVrag = new Api.User(msg.from_id);
                skillsVrag = new Api.Skills(msg.from_id);
            }else
            {
                userVrag = new Api.User(msg.from_id);
                skillsVrag = new Api.Skills(msg.from_id);
            }
            var resourcesStr = $"❗ На атаку вы потратили: {countResoures} {typeResourses}";
            var myHp = $"❤ Ваше здоровье: [{members[msg.from_id]}/{API.HpUser(msg.from_id)}]";          
            var r = new Random();
            int chaise = r.Next(1, 4);         
            if (skills.Fortuna > skillsVrag.Fortuna)
            {
                if (chaise == 2)
                {
                    chaise = r.Next(1, 4);
                    if (chaise == 2)
                    {
                        if (user.Level < userVrag.Level)
                        {
                            chaise = r.Next(1, 3);
                        }
                    }else if (chaise == 1)
                    {
                        if (user.Level < userVrag.Level)
                        {
                            var lol = r.Next(1, 5);
                            if (lol == 4) chaise = 3;
                        }
                    }
                }else if (chaise == 1)
                {
                    var lol = r.Next(1, 6);
                    if (lol == 3) chaise = 3;
                    else
                    {
                        if (user.Level < userVrag.Level)
                        {
                            var kek = r.Next(1, 5);
                            if (kek == 4) chaise = 3;
                        }
                    }
                }
            }else if (skills.Fortuna == skillsVrag.Fortuna)
            {
                chaise = r.Next(1, 4);
            }else if (skills.Fortuna < skillsVrag.Fortuna)
            {
                var loh = r.Next(1, 5);
                if (loh == 1) chaise = 1;
                if (loh == 2) chaise = 2;
                if (loh == 3) chaise = 2;
                if (loh == 4) chaise = 3;
            }
            
            if (chaise == 1)
            {
                string typeBuilds = String.Empty;              
                var kek = r.Next(1, 4);
                if (kek == 2)
                {
                    if(battle.Type == 1)
                    {
                        var buildsVrag = new Builds(vrag);
                        var lol = r.Next(1, 9);
                        if (lol == 1)
                        {
                            buildsVrag.Apartments -= 1;
                            typeBuilds = "Жилой дом";
                        }
                        else if (lol == 2)
                        {
                            buildsVrag.Eatery -= 1;
                            typeBuilds = "Столовую";
                        }
                        else if (lol == 3)
                        {
                            buildsVrag.Hangars -= 1;
                            typeBuilds = "Ангар";
                        }
                        else if (lol == 4)
                        {
                            buildsVrag.PowerGenerators -= 1;
                            typeBuilds = "Энергостаницию";
                        }
                        else if (lol == 5)
                        {
                            buildsVrag.WarehouseEat -= 1;
                            typeBuilds = "Холодильник";
                        }
                        else if (lol == 6)
                        {
                            buildsVrag.WarehouseEnergy -= 1;
                            typeBuilds = "Батарею";
                        }
                        else if (lol == 7)
                        {
                            buildsVrag.WarehouseWater -= 1;
                            typeBuilds = "Бочку с водой";
                        }
                        else if (lol == 8)
                        {
                            buildsVrag.WaterPressureStation -= 1;
                            typeBuilds = "Водонапорную башню";
                        }

                        new Task(() => Api.Message.Send($"💣 Упс! Вам противник нанёс урон по заданию! И Вы потеряли {typeBuilds}." +
                                                        "\n❓ Если у Вас количество зданий - 0 или меньше, то скорее купите новые!" +
                                                        "\n❗ Скорее отвечайте: бой атака <количество> <тип войска>", vrag)).Start();
                    }else if(battle.Type == 2)
                    {
                        var buildsVrag = new Builds(vrag);
                        var lol = r.Next(1, 9);
                        if (lol == 1)
                        {
                            buildsVrag.Apartments -= 1;
                            typeBuilds = "Жилой дом";
                        }
                        else if (lol == 2)
                        {
                            buildsVrag.Eatery -= 1;
                            typeBuilds = "Столовую";
                        }
                        else if (lol == 3)
                        {
                            buildsVrag.Hangars -= 1;
                            typeBuilds = "Ангар";
                        }
                        else if (lol == 4)
                        {
                            buildsVrag.PowerGenerators -= 1;
                            typeBuilds = "Энергостаницию";
                        }
                        else if (lol == 5)
                        {
                            buildsVrag.WarehouseEat -= 1;
                            typeBuilds = "Холодильник";
                        }
                        else if (lol == 6)
                        {
                            buildsVrag.WarehouseEnergy -= 1;
                            typeBuilds = "Батарею";
                        }
                        else if (lol == 7)
                        {
                            buildsVrag.WarehouseWater -= 1;
                            typeBuilds = "Бочку с водой";
                        }
                        else if (lol == 8)
                        {
                            buildsVrag.WaterPressureStation -= 1;
                            typeBuilds = "Водонапорную башню";
                        }

                        new Task(() => Api.Message.Send($"💣 Упс! Вам противник нанёс урон по заданию! И Вы потеряли {typeBuilds}." +
                                                        "\n❓ Если у Вас количество зданий - 0 или меньше, то скорее купите новые!" +
                                                        "\n❗ Скорее отвечайте: бой атака <количество> <тип войска>", vrag)).Start();
                    }else if(battle.Type == 3)
                    {
                    }                
                    if(battle.Type == 1 || battle.Type == 2)
                    {
                        battle.UserAttack = vrag;
                        return $"👍 Отличный удар! Вы нанесли критический урон и забрали у врага {typeBuilds}! Кажется, кому-то скоро нужно покупать новое здание:)" +
                          $"\n {resourcesStr}" +
                          $"\n {myHp}";
                    }else
                    {
                        API.BotAttack(user.Id);
                        return $"👍 Отличный удар! Вы нанесли критический урон и забрали у бота одно здание!" +
                            $"\n {resourcesStr}" +
                            $"\n {myHp}";
                    }               
                }               
                if(kek == 1 || kek ==3)
                {
                    if(battle.Type ==1 || battle.Type ==2)
                    {
                        new Task(() =>
                       Message.Send($"💣 Воу-Воу! Противник Вам нанёс урон по зданию! Здоровье зданий уменьшелось!" +
                                        $"\n❗ Скорее отвечайте: бой атака <количество> <тип войска>", vrag)).Start();
                        battle.UserAttack = vrag;
                        return $"⚔ Ну почти.. Ты попал противнику в здание и нанёс по нем урон!" +
                               $"\n{resourcesStr}" +
                               $"\n {myHp}";
                    }else
                    {
                        return $"⚔ Ну почти.. Ты попал боту в здание и нанёс по нем урон!" +
                               $"\n{resourcesStr}" +
                               $"\n {myHp}";
                    }
                }
            }
            if (chaise == 2)
            {
                if(battle.Type == 1 || battle.Type == 2)
                {
                    new Task(() =>
                   Api.Message.Send($"🤣 Ха-ха-ха! Ваш враг не смог в Вас попасть! Так держать!" +
                                    $"\n ❗ Скорее отвечайте: бой атака <количество> <тип войска>", vrag)).Start();
                    battle.UserAttack = vrag;
                    return $"😢 Нуу было близко! Вы промазали! Попробуйте в следующий раз." +
                           $"\n {resourcesStr}" +
                           $"\n {myHp}";
                }else
                {
                    API.BotAttack(user.Id);
                    return $"😢 Нуу было близко! Вы промазали! Попробуйте в следующий раз." +
                          $"\n {resourcesStr}" +
                          $"\n {myHp}";
                }        
            }
            if (chaise == 3)
            {
                var hpVrag = members[vrag] - countHpArmy;
                if (hpVrag < 0 || hpVrag == 0)
                {
                    if (battle.Type == 1)
                    {
                        var registryVrag = new Registry(vrag);
                        registryVrag.CountLoserBattle += 1;
                        new Task(() =>
                        Api.Message.Send("😭 Вас разграмили! Вы проиграли! В следующей битве повезёт больше!", vrag)).Start();
                        API.EndBattle(battle.Id, msg.from_id);
                        return resourcesStr;
                    }
                    else if (battle.Type == 2)
                    {
                        var registryVrag = new Registry(vrag);
                        registryVrag.CountLoserBattle += 1;
                    }
                    else if (battle.Type == 3)
                    {
                        API.EndBattle(battle.Id, msg.from_id);
                    }
                }
                battle.SetHp(vrag, hpVrag);
                if(battle.Type == 3)
                {
                    API.BotAttack(user.Id);
                    return $"✨ Прямо  в яблочко! Вы нанесли противнику {countHpArmy} урона!" +
                       $"\n {resourcesStr}" +
                       $"\n {myHp}";
                }
                new Task(() => Api.Message.Send($"💣 ВАМ НАНЕСЁН УДАР! Урон: {countHpArmy}" +
                                                $"\n ❤ Ваше здоровье: [{hpVrag}/{API.HpUser(vrag)}]" +
                                                $"\n ❗ Скорее отвечайте: бой атака <количество> <тип войска>", vrag)).Start();
                battle.UserAttack = vrag;
                return $"✨ Прямо  в яблочко! Вы нанесли противнику {countHpArmy} урона!" +
                       $"\n {resourcesStr}" +
                       $"\n {myHp}";
            }
            return "❌ Произошла неизвестная ошибка, при выполнении. Статус неизвестен.";
        }
        
        [Attributes.Trigger("покинуть")]
        public static string LeaveToBattle(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
            if (user.BattleId == 0) return "❌ Вы не учавствуете в какой-либо битве.";
            var battle = new Api.Battles(user.BattleId);
            if (!battle.IsStart)
            {
                battle.IsActive = false;
                user.BattleId = 0;
                return "✅ Вы успешно покинули бой.";
            }
            var members = battle.Members;
            long winner = 0;    
            foreach (var member in members)
            {
                if (member.Key != msg.from_id) winner = member.Key;
            }      
            new Task(()=> Api.Message.Send("😎  Ваш противник покинул бой! Вы получаете весь фонд битвы.", winner)).Start();        
            API.EndBattle(user.BattleId, winner);             
            return "😢 Вы успешно покинули бой. Ваш противник получил уведомление об этом.";
        }

        [Attributes.Trigger("мой")]
        public static string MyBattle(Models.Message msg)
        {
            var userO = new Api.User(msg.from_id);
            if (userO.BattleId == 0) return "❌ Вы не находитесь ни в какой битве!";
            var battle = new Api.Battles(userO.BattleId);         
            var membersString = String.Empty;
            foreach (var member in battle.Members)
            {
                var user = new Api.User(member.Key);
                membersString += $"\n 😀 Имя: [id{user.Id}|{user.Name}]" +
                                 $"\n 🔝 Уровень: {user.Level}" +
                                 $"\n 💯 Рейтинг: user.top" +
                                 $"\n";
            }
            var battleString = $"\n ⚔ БИТВА №{battle.Id}" +
                          $"\n ➡ Название: {battle.Name}" +
                          $"\n 💳 Фонд: {battle.Found}" +
                          $"\n 😎 Участники:" +
                          $"{membersString}" +
                          $"\n";
            return battleString;
        }

        [Attributes.Trigger("вступить")]
        public static string JoinToBattle(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            long battleId = 0;
            try
            {
                battleId = Convert.ToInt64(messageArray[2]);
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали Id битвы. Пример: вступить 235";
            }
            catch (FormatException)
            {
                return "❌ Вы указали неверный Id битвы. Id должен быть чилом.";
            }
            var user = new Api.User(msg.from_id);
            if (user.BattleId != 0) return "❌ Вы и так учавствуете в другой битве!";
            if (!Api.Battles.Check(battleId))
                return "❌ Такой битвы несуществует! Чтобы узнать список доступных битв, напишите: список";
            var battle = new Api.Battles(battleId);
            if (!battle.IsActive) return "❌ Эта битва уже закончилась! Вы не можете вступить в неё. Посмотрите другие битвы, напишите: список";
            if (battle.IsStart) return "❌ Эта битва уже началась! Вы не можете вступить в неё! Посмотрите другие битвы, напишите: список";
            if (battle.Creator == msg.from_id) return "❌ Вы не можете вступить в свою же битву!";
            if (!Notifications.RemovePaymentCard(Convert.ToInt32(battle.Found), msg.from_id, "вступление в битву"))
                return $"❌ У Вас не хватает монет на балансе. Необходимо: {battle.Found} ";        
            API.JoinToBattle(msg.from_id, battleId);        
            new Task(()=>Api.Message.Send($"⚔ К Вам в битву вступил противник!" +
                                          $"\n ❤ ВАШЕ ЗДОРОВЬЕ: {API.HpUser(battle.Creator)}" +
                                          $"\n 🖤 ЗДОРОВЬЕ ПРОТИВНИКА: {API.HpUser(msg.from_id)}", battle.Creator)).Start();      
            new Task(()=> ChoisidBattleStart(new List<long>() {msg.from_id, battle.Creator}, battle.Id)).Start();
            return $"✅ Вы успешно вступили в битву. Сейчас произойдёт рандомный выбор игрока, который первый атакует." +
                   $"\n ❤ ВАШЕ ЗДОРОВЬЕ: {API.HpUser(msg.from_id)}" +
                   $"\n 🖤 ЗДОРОВЬЕ ПРОТИВНИКА: {API.HpUser(battle.Creator)}";
        }

        private static void ChoisidBattleStart(List<long> users, long battleId)
        {
            var user = API.ChoiseUser(users[0], users[1]);
            var battle = new Api.Battles(battleId);
            battle.UserAttack = user;
            Api.Message.Send("💣 Вы атакуете первым! Скорее! Бой атака <количество> <тип армии> ", user);
        }
        
        [Attributes.Trigger("создать")]
        public static string CreateBattle(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            long price = 0;
            try
            {
                price = Convert.ToInt64(messageArray[2]);
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не цену битвы! Пример: бой создать 100 попробуй меня победить.";
            }catch(FormatException)
            {
                return "❌ Вы указали неверную цену! Цена должна быть ЧИСЛОМ!";
            }

            string nameBattle = String.Empty;
            try
            {
                for (var i = 3; i < messageArray.Length; i++) nameBattle += $"{messageArray[i]} ";
            }
            catch (IndexOutOfRangeException)
            {
                nameBattle = "Битва без названия";
            }
            if (nameBattle == String.Empty) nameBattle = "Битва без названия";
            if (!Notifications.RemovePaymentCard(Convert.ToInt32(price), msg.from_id, "создание битвы"))
                return $"❌ У Вас на балансе нет необходимой суммы: {price} 💳 ";
            var battleId = API.Create(msg.from_id, nameBattle, price);          
            return $"✅ Битва успешна создана. Id битвы -- {battleId}";
        }


        [Attributes.Trigger("список")]
        public static string ActiveListBattles(Models.Message msg)
        {
            var battlesStr = String.Empty;
            var battles = Api.Battles.GetActivesAll;
            foreach (var battleId in battles)
            {
                var battle = new Bot.Api.Battles(battleId);

                var membersString = String.Empty;
                foreach (var member in battle.Members)
                {
                    var user = new Api.User(member.Key);
                    membersString += $"\n 😀 Имя: [id{user.Id}|{user.Name}]" +
                                     $"\n 🔝 Уровень: {user.Level}" +
                                     $"\n ❤ Здоровье: {member.Value}" +
                                     $"\n 💯 Рейтинг: user.top" +
                                     $"\n";
                }
                battlesStr += $"\n ⚔ БИТВА №{battle.Id}" +
                              $"\n ➡ Название: {battle.Name}" +
                              $"\n 💳 Цена: {battle.Found}" +
                              $"\n 😎 Участники:" +
                              $"{membersString}" +
                              $"\n ❓ Чтобы вступить в бой, напишите: Бой вступить {battle.Id}" +
                              $"\n";
            }
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n СПИСОК АКТИВНЫХ БОЁВ" +
                   $"\n" +
                   $"\n{battlesStr}" +
                   $"\n" +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }


        private string GetBattleText()
        {
            return "➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                   "\n ⚔ Раздел битв." +
                   "\n" +
                   "\n❓ Здесь можно создать битву или вступить в битву, которая уже создана." +
                   "\n " +
                   "\n ⚔ Бой список -- выводит список доступных битв." +
                   "\n ✨ Бой создать (цена) (название) -- создание собственной битвы." +
                   "\n 😎 Бой вступить (ID) -- вступить в уже созданную битву." +
                   "\n 🔫 Бой бот (цена) -- создаёт битву с ботом. ";
        }    
    }
}