using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VKGame.Bot.PublicAPI.Models;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Battle
    {
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
            if (clanEnemy is null) return new CreateResponse() { Status = 2, Id = null };

            Clans.SetIsSearchBattle(clanId, 0);

        }

        //поиск по уровню.
        private static string SearchForLevels(int level, string clanId, bool plus)
        {
            //Получение id кланов.
            var clansIds = Clans.GetClansIsSearchBattle();

            if (clansIds.Count == 0)
                return null;
            //создание объектов clan.
            var clans = new List<Models.Clan>();
            foreach (var clanIdAll in clansIds)
                clans.Add(new Models.Clan(clanIdAll));
            var clan = new Models.Clan(clanId);

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
            var clan = new Models.Clan(clanId);
            var result = SearchForLevels(clan.Level, clanId, true);

            if (result is null) return null;
            else return result;
        }

    }
}
