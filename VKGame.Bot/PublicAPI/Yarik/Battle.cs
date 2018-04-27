using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VKGame.Bot.PublicAPI.Models;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Battle
    {
        public static bool Check(string battleId)
        {
            return Database.Public.CheckFromKey("Id", battleId, "Battles");
        }


        public static object Lose(long userId, string clan)
        {
            var attempts = (int)Users.GetAttempts(userId);
            Users.SetAttempts(userId, --attempts);
            return true;
        }


        public static object Attack(long userId, string clan, IArmy army, int count)
        {
            if (!((bool)Clans.GetIsStartBattle(clan)))
                return new Error() { Code = 18, Message = "Клан не находится в битве." };

            var userBattleObject = Users.GetBattleId(userId);
            if (userBattleObject is IError) return (IError)userBattleObject;
            var battleId = (string)userBattleObject;
            if(battleId== "0")
                return new Error() { Code = 16, Message = "Пользователь не участвует в битве." };
            var countForUserObject =  Army.GetCountForType(userId, army.Type);
            if (countForUserObject is IError) return (IError)countForUserObject;
            var countForUser = (int)countForUserObject;
            if(count > countForUser)
                return new Error() { Code = 17, Message = "У Пользователя недостаточно армии." };
            var enemyHp = (int)GetHpEnemyLs(battleId);
            var damage = (int)Army.GetSummaryDamage(userId, army);

            var r = new Random();

            var mainChance = r.Next(1, 4);

            //чел промазол.
            if(mainChance == 2)
            {
                return 2;
            }

            //чел попал
            var chanceAttackToBarracks = r.Next(1, 5);
            var enemy = (long)GetEnemyLs(battleId);

            var chanseLoseWhereUpLevel = 2;
            if((int)Users.GetProtection(enemy) < (int)Users.GetProtection(userId))
            {
                chanseLoseWhereUpLevel = r.Next(1, 5);
            }          

            //уровень защиты игрока.
            if (chanceAttackToBarracks == 1) return new Models.Attack() { Status = 2};
    
            //атака на казармы
            if(chanceAttackToBarracks == 3)
            {
                var choiseCasarm = r.Next(1,6);
                var countB = (int)Barracks.GetCount(userId, choiseCasarm);
                countB -= 1;
                Barracks.SetCount(userId, choiseCasarm,  countB);

                return new Attack() { Status = 3, IdBarrack = choiseCasarm };
            }

            enemyHp -= damage;
            if(enemyHp <= 0)
            {
                //пользователь вин.
                var countAttemps = (int)Users.GetAttempts(userId);
                Users.SetAttempts(userId, --countAttemps);

                int money = 0;
                money += 10000;
                money += (int)Army.GetPrice(userId);
                money += 100 * (int)Clans.Level(clan);

                var moneys = (int)Users.Money(userId);
                moneys += money;
                Users.SetMoney(userId, moneys);
                var found = (int)Clans.GetFound(clan);
                found += money;
                Clans.SetFound(clan,found);
                
                return new Attack() { Status = 4, Money = money, HpEnemy = enemyHp };
            }

            SetHpEnemyLs(battleId, enemyHp);

            return new Attack() { Status = 1, Damage = damage, HpEnemy = enemyHp };
        }

        public static object GetEnemyLs(string battleIdLs)
        {
            var db = new Database.Public("BattlesOneOnOne");
            var value = (long)db.GetFromKey("Id", battleIdLs, "Enemy");
            return value;
        }

        public static object GetHpEnemyLs(string battleIdLs)
        {
            var db = new Database.Public("BattlesOneOnOne");
            var value = (long)db.GetFromKey("Id", battleIdLs, "HpEnemy");
            return value;
        }

        public static object SetHpEnemyLs(string battleIdLs, int value)
        {
            var db = new Database.Public("BattlesOneOnOne");
            db.EditFromKey("Id", battleIdLs, "HpEnemy", value);
            return true ;
        }

        public static object GetClanOne(string battleId)
        {
            if (!Check(battleId))
                return new Error() { Code = 6, Message = "Не найдена битва с таким ID." };
            var db = new Database.Public("Battles");
            return db.GetFromKey("Id", battleId, "ClanOne");
        }

        public static object GetClanTwo(string battleId)
        {
            if (!Check(battleId))
                return new Error() { Code = 6, Message = "Не найдена битва с таким ID." };
            var db = new Database.Public("Battles");
            return db.GetFromKey("Id", battleId, "ClanTwo");
        }

        public static object GetClanEnemy(string battleId, string clan)
        {
            if (!Check(battleId))
                return new Error() { Code = 6, Message = "Не найдена битва с таким ID." };
            var db = new Database.Public("Battles");
            if (clan == (string)db.GetFromKey("Id", battleId, "ClanOne"))
                return db.GetFromKey("Id", battleId, "ClanTwo");
            else if (clan == (string)db.GetFromKey("Id", battleId, "ClanTwo"))
                return db.GetFromKey("Id", battleId, "ClanOne");
            else return new Error() { Code = 7, Message = "Клан не учавствует в этой битве." };
        }

        public static object Create(string clanId)
        {
            //проверка на валидность.
            if (!Clans.Check(clanId))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };
            //поиск противников..
            var clanEnemy = SearchClans(clanId);
            if (clanEnemy is null) return new CreateResponse() { Status = 2};

            //Устанавливаем значение 0, чтобы эти кланы не выводились при поиске.
            Clans.SetIsSearchBattle(clanId, 0);
            Clans.SetIsSearchBattle(clanEnemy, 0);

            //Устанавливаем значения isStartBattle

            //Генерируем публичный ID битвы для обоих кланов.
            byte[] hash = Encoding.ASCII.GetBytes($"{clanId}vs{clanEnemy}_in_{DateTime.Now}");
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";
            foreach (var b in hashenc)
                result += b.ToString("x2");

            string battleId = result.ToUpper();

            //Устанавливаем кланам публичный ID битвы.
            Clans.SetBattleId(clanId, battleId);
            Clans.SetBattleId(clanEnemy, battleId);


            //Создаём бой.
            var fields = new List<string> {"Id", "ClanOne", "ClanTwo" };
            var values = new List<string> { battleId, clanId, clanEnemy };
            Database.Public.Add(fields, values, "Battles");

            return new CreateResponse() { Status = 1, Id = clanEnemy, Name = (string)Clans.GetName(clanEnemy)};
        }

        public static object CheckMemberChoise(string battleId, long userId)
        {
            if (!Check(battleId))
                return new Error() { Code = 6, Message = "Не найдена битва с таким ID." };
            if (!Users.Check(userId))
                return new Error() { Code = 12, Message = "Этот пользователь не зарегестирован." };
            var members = (List<long>)GetMembersChoise(battleId);
            return members.Any(u => u == userId);
        }


        public static object Choised(string clan, string battleId, long user, int choise)
        {
            if (!Check(battleId))
                return new Error() { Code = 6, Message = "Не найдена битва с таким ID." };
            if (!Users.Check(user))
                return new Error() { Code = 12, Message = "Этот пользователь не зарегестирован." };
            if (!Clans.Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var choiseds = (ChoiseMembers)GetChoise(clan, battleId);
            var value = choiseds.List.FindAll(u => u.Number == choise).FirstOrDefault();
            if(value == null) return 
                    new Error() { Code = 14, Message = "Нет игрока с таким номером выбора." };
            AddMembersChoise(battleId, value.User.Id);
            var battleIdLs = CreateBattleLs(user, value.User.Id);
            Users.SetBattleId(user, battleIdLs);
            var army = (Models.Count)Army.GetCount(user);
            int price = 0;
            foreach(var ar in army.List)
            {
                var units = (Models.Units)Users.GetArmy(user);
                 var unit = units.Army[ar.Id -1];
                price += ar.Count * unit.Price;
            }
            Army.SetPrice(user, price);

            return true;
        }


        public static string CreateBattleLs(long userId, long enemy)
        {
            byte[] hash = Encoding.ASCII.GetBytes($"Battle_{userId}From{enemy}, {DateTime.Now.ToString()}");
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";
            foreach (var b in hashenc)
                result += b.ToString("x2");

            var battleId = result.ToUpper();

            var hpEnemy = GetUserHp(enemy);
            var fields = new List<string>() { "Id", "Creator", "Enemy", "HpEnemy"};
            var values = new List<string>() { battleId, userId.ToString(), enemy.ToString(), hpEnemy.ToString()};
            Database.Public.Add(fields, values, "BattlesOneOnOne");
            return battleId;
        }

        public static object GetUserHp(long userId)
        {
            var one = (int)Barracks.GetCount(userId, 1);
            var two = (int)Barracks.GetCount(userId, 2);
            var three = (int)Barracks.GetCount(userId, 3);
            var four = (int)Barracks.GetCount(userId, 4);
            var five = (int)Barracks.GetCount(userId, 5);

            var sum = (one + two + three + four + four) * 100;
            return sum;

        }

        public static object GetChoise(string clan, string battleId)
        {
            if (!Battle.Check(battleId))
                return new Error() { Code = 6, Message = "Не найдена битва с таким ID." };

            if (!Clans.Check(clan))
                return new Error()
                {
                    Code = 4,
                    Message = "Клан с таким ID не зарегестирован."
                };

            var clanEnemy = Battle.GetClanEnemy(battleId, clan);
            if (clanEnemy is IError) return clanEnemy;
            var members = Clans.GetMembers((string)clanEnemy);
            if (members is IError) return members;
            var memberss = (List<long>)members;
            var model = new ChoiseMembers()
            {
                @List = new List<ChoiseMember>()
            };
            for (int i = 0; memberss.Count > i; i++)
            {
                if(!(bool)CheckMemberChoise(battleId, memberss[i]))
                {
                    model.List.Add(new ChoiseMember()
                    {
                        Number = i,
                        User = new User()
                        {
                            Id = memberss[i],
                            Money = (int)Users.Money(memberss[i]),
                            Units = (Models.Units)Users.GetArmy(memberss[i])
                        }
                    });
                }      
            }
            return model;
        }

        public static bool CheckMemberChoiseBool(string battleId, long userId)
        {
            var members = (List<long>)GetMembersChoise(battleId);
            return members.Any(u => u == userId);
        }

        public static object AddMembersChoise(string battleId, long userId)
        {
            if (!Check(battleId))
                return new Error() { Code = 6, Message = "Не найдена битва с таким ID." };
            if(!Users.Check(userId))
                return new Error() { Code = 12, Message = "Этот пользователь не зарегестирован." };

            var db = new Database.Public("Battles");

            var members = (List<long>)GetMembersChoise(battleId);

            members.Add(userId);

            var json = JsonConvert.SerializeObject(members);

            db.EditFromKey("Id", battleId, "UsersLose", json);
            return true;
        }

        public static object GetMembersChoise(string battleId)
        {
            if (!Check(battleId))
                return new Error() { Code = 6, Message = "Не найдена битва с таким ID." };

            var db = new Database.Public("Battles");
            var value = (string)db.GetFromKey("Id", battleId, "UsersLose");
            if (value == "") return new List<long> { };

            var obj = JsonConvert.DeserializeObject<List<long>>(value);
            return obj;
        }

        //поиск по уровню.
        private static string SearchForLevels(int level, string clanId, bool plus)
        {
            //Получение id кланов.
            var clansIds = Clans.GetClansIsSearchBattle();

            if (clansIds.Count == 0)
                return null;
            //создание объектов clan.
            var clans = new List<Clan>();
            foreach (var clanIdAll in clansIds)
                clans.Add(new Clan(clanIdAll));
            var clan = new Clan(clanId);

            //first step: поиск кланов с одним и тем же уровнем.
            var firstStep = clans.OrderBy(u => u.Level == level);
            if (firstStep.Count() != 0)
            {
                //поиск кланов с одним и тем же кол-вом участников.
                firstStep = firstStep.OrderBy(u => u.CountMembers == clan.CountMembers);
                if (firstStep.Count() != 0) return firstStep.FirstOrDefault().Id;
                else
                {
                    //увеличиваем выборку, если таких кланов нет.
                    firstStep = firstStep.OrderBy(u => u.CountMembers >= clan.CountMembers && !(u.CountMembers > u.CountMembers * 1.5));
                    if (firstStep.Count() != 0)
                    {
                        //Сортируем по возрастанию - первое значение - самое маленькое количество участников.
                        firstStep = firstStep.OrderBy(u => u.CountMembers);
                        if (firstStep.Count() != 0) return firstStep.FirstOrDefault().Id;
                        else
                        {
                           //Если таких кланов нет - сортируем по уровню.
                            if(plus)
                            {
                                //больше
                                if (level - clan.Level < 2)
                                    return SearchForLevels(++level, clanId, true);
                                else return null;
                            }else
                            {
                                //меньше
                                if (clan.Level - level < 2)
                                    return SearchForLevels(--level, clanId, false);
                                else return null;
                            }
                        }
                    }
                    else
                    {
                        //уменьшаем подборку если таких кланов нет.
                        firstStep = firstStep.OrderBy(u => u.CountMembers <= clan.CountMembers && !(u.CountMembers < u.CountMembers / 1.5));

                        if (firstStep.Count() != 0)
                        {
                            //Сортируем по убыванию - первое значение - самое большое количество участников.
                            firstStep = firstStep.OrderByDescending(u => u.CountMembers);
                            if (firstStep.Count() != 0) return firstStep.FirstOrDefault().Id;
                            else
                            {
                                //Если таких кланов нет - сортируем по уровню.
                                if (plus)
                                {
                                    //больше
                                    if (level - clan.Level < 2)
                                        return SearchForLevels(++level, clanId, true);
                                    else return null;
                                }
                                else
                                {
                                    //меньше
                                    if (clan.Level - level < 2)
                                        return SearchForLevels(--level, clanId, false);
                                    else return null;
                                }
                            }
                        }
                        else
                            return null;
                    }
                }
            } else
            {
                //Если таких кланов нет - сортируем по уровню.
                if (plus)
                {
                    //больше
                    if (level - clan.Level < 2)
                        return SearchForLevels(++level, clanId, true);
                    else return null;
                }
                else
                {
                    //меньше
                    if (clan.Level - level < 2)
                        return SearchForLevels(--level, clanId, false);
                    else return null;
                }
            }
        }
        //Поиск наиболее подходящего клана.
        public static string SearchClans(string clanId)
        {
            var clan = new Clan(clanId);
            var result = SearchForLevels(clan.Level, clanId, true);
            return result;
        }

    }
}
