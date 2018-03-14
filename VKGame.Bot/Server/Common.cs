
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace VKGame.Bot.Server
{
    public class Common
    {
        public static Connection.User GetUsers()
        {
            string json = String.Empty;
            using(var reader = new StreamReader(@"Server/Users.json"))
            {
                json = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<Connection.User>(json);
        }

        public class Response<T>
        {
            public bool Ok { get; set; }
            public T Data { get; set; }
            public Error Error { get; set; }
        }
    }
}
