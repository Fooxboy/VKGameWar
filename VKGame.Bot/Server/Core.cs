using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VKGame.Bot.Server
{
    public class Core
    {
        public static string Processing(string json)
        {
            InData data;
            try
            {
                data = JsonConvert.DeserializeObject<InData>(json);
            }catch
            {
                var error1 = new Error { Code = 2, Message = "Неверные данные." };
                var response1 = new Common.Response<string>() { Error = error1, Ok = false };
                return JsonConvert.SerializeObject(response1);
            }

            if(data.Data == null || data.Method == null)
            {
                var error1 = new Error { Code = 3, Message = "Вы не указали метод или данные." };
                var response1 = new Common.Response<string>() { Error = error1, Ok = false };
                return JsonConvert.SerializeObject(response1);
            }

            var methodName = data.Method;

            var type = typeof(Core);
            object obj = Activator.CreateInstance(type);
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                var attributesCustom = Attribute.GetCustomAttributes(method);
                foreach (var attribute in attributesCustom)
                {
                    if (attribute.GetType() == typeof(AttributeMethod))
                    {
                        var myAtr = ((AttributeMethod)attribute);
                        if (myAtr.Name == methodName)
                        {
                            object result = method.Invoke(obj, new object[] {data.Data});
                            return (string)result;
                        }
                    }
                }
            }

            var error = new Error { Code = 4, Message = "Вы указали неизвестный метод." };
            var response = new Common.Response<string>() { Error = error, Ok = false };
            return JsonConvert.SerializeObject(response);

        }

        [AttributeMethod("Auth.GetToken")]
        public static string AuthGetTokenServer(object dataObj)
        {
            var data = (Auth.InAuth)dataObj;
            return Auth.GetTokenServer(data.Name, data.Password);
        }

        [AttributeMethod("Messages.GetNow")]
        public static string MessagesGetNow(object dataObj)
        {
            var data = (ulong)dataObj;
            return Messages.GetNow(data);
        }

        [AttributeMethod("Messages.SubscribeOnNewMessages")]
        public static string MessagesSubscribeOnNewMessages(object dataObj)
        {
            return Messages.SubscribeOnNewMessages();
        }

        [AttributeMethod("Database.GetValue")]
        public static string DatabaseGetValue(object dataObj)
        {
            var data = (Database.InGetValue)dataObj;
            return Database.GetValue(data.Id, data.Table, data.Column);
        }

        [AttributeMethod("Database.SetValue")]
        public static string DatabaseSetValue(object dataObj)
        {
            var data = (Database.InSetValue)dataObj;
            return Database.SetValue(data.Id, data.Table, data.Column, data.Value);
        }

        [AttributeMethod("Stat.All")]
        public static string StatAll(object dataObj)
        {
            return Statistics.GetAll();
        }
        public class InData
        {
            public string Method { get; set; }
            public object Data { get; set; }
            public string Token { get; set; }
        }

    }
}
