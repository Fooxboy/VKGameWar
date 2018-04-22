﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VKGame.Bot.PublicAPI.Models;
using System.Security.Cryptography;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Battle
    {
        public static bool Check(string battleId)
        {
            return Database.Public.CheckFromKey("Id", battleId, "Battles");
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
