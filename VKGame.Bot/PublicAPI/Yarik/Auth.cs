using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public class Auth
    {
        public bool checkToken(string token)
        {
           var result = Database.Public.CheckFromKey(token, "Tokens");
            if(result)
            {
                var db = new Database.Public("Tokens");
                Console.WriteLine($"Сделал запрос {db.GetFromKey(token)}");
            }
            return result;
        }

        public void AccessToken(string token)
        {

        }

        public string NoAccess
        {
            get
            {    
                return Newtonsoft.Json.JsonConvert.SerializeObject(new RootResponse() { result = false, data = new Models.Error() { Code = 9, Message = "Can not access method." } });
            }
        }
    }

    public class RootResponse
    {
        public bool result { get; set; }
        public object data { get; set; }
    }
}
