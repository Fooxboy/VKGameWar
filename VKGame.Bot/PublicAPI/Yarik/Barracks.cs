﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Barracks
    {

        public static object GetCount(long user, int idArmy)
        {
            string army = idArmy.
                ToString().
                Replace("1", "One").
                Replace("2", "Two").
                Replace("3", "Three").
                Replace("4", "Four").
                Replace("5", "Five");

            var db = new Database.Public("Barracks");
            long count = 0;
            try
            {
                 count = (long)db.GetFromId(user, army);
            }catch
            {
                return new Models.Error() { Code = 15, Message = "Неизвестный тип армии." };
            }
            return count;
        }

        public static object SetCount(long user, int idArmy, int value)
        {
            var db = new Database.Public("Barracks");

            string army = idArmy.
                ToString().
                Replace("1", "One").
                Replace("2", "Two").
                Replace("3", "Three").
                Replace("4", "Four").
                Replace("5", "Five");

            db.EditFromId(user, army, value);
            return true;
        }

    }
}
