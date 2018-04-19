using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Registration
    {
        public static object AllServies(long userId)
        {
            return null;
        }

        public static object Army(long userId)
        {
            if (Yarik.Army.Check(userId)) return new Models.Error() { Code = 3, Message = "Пользователь уже зарегестрирован." };

            Database.Public.Add(new List<string>() { "Id" }, new List<string>() { userId.ToString() }, "Army");
            return true;
        }
    }
}
