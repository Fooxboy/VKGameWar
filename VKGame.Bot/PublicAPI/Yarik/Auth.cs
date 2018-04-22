using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public class Auth
    {
        public bool checkToken(string token) => Database.Stat.CheckFromKey(token, "Tokens");

        public void AccessToken(string token)
        {

        }

        public string NoAccess => Newtonsoft.Json.JsonConvert.SerializeObject(new Models.Error() { Code = 9, Message = "У Вас нет доступа к этому методу." });
    }
}
