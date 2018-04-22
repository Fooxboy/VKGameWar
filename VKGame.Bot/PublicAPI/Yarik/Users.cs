using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Users
    {
        public static object Money(long userId)
        {
            var db = new Database.Public("Users");
            return (long)db.GetFromId(userId, "Money");
        }
    }
}
