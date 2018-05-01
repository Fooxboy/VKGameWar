﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public class Auth
    {
        public bool checkToken(string token) => Database.Public.CheckFromKey(token, "Tokens");

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
