using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace VKGame.Bot.Server
{
    public class Auth
    {
        public static string DecodeToken(string token)
        {
            string[] array = token.Split('_');
            return array[0];
        }

        public static string GetTokenServer(string Name, string Password)
        {
            var users = Common.GetUsers().Users;
            var user = users.Where(u => u.Name == Name);
            bool isPass = false;
            var response = new Common.Response<ResponseAuth>();
            foreach(Connection connect in user)
            {
                if (connect.Password == Password) isPass = true;
            }

            if (!isPass)
            {
                var error = new Error() { Code = 1, Message = "Логин или пароль неверный!" };
                response.Ok = false;
                response.Error = error;
                return JsonConvert.SerializeObject(response);
            }

            //create token
            string token = $"{Name}_{DateTime.Now.Hour}";

            var auth = new ResponseAuth() { Token = token };
            response.Data = auth;
            response.Ok = true;
            return JsonConvert.SerializeObject(response);
        }

        public class ResponseAuth
        {
            public string Token { get; set; }
        }

        public class InAuth
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }
    }
}
