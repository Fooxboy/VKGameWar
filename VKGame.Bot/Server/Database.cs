using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VKGame.Bot.Server
{
    public class Database
    {

        public static string GetValue(long id, string table, string column)
        {
            var response = new Common.Response<string>();

            try
            {
                var db = new Bot.Database.Methods(table);
                response.Data = (string)db.GetFromId(id, column);
                response.Ok = true;
            }catch(Exception e)
            {
                Logger.WriteError($"Ошибка при отправке данных с сервера: {e.Message}" +
                    $"\n {e.Source}");
                response.Error = new Error() { Code = 5, Message = $"Внутренняя ошибка сервера: {e.Message}" };
                response.Ok = false;
            }
            return JsonConvert.SerializeObject(response);
        }

        public static string SetValue(long id, string table, string column, object value)
        {
            var response = new Common.Response<bool>();
            try
            {
                var db = new Bot.Database.Methods(table);
                db.Edit(id, column, value);
                response.Ok = true;
                response.Data = true;
            }catch(Exception e)
            {
                Logger.WriteError($"Ошибка при отправке данных с сервера: {e.Message}" +
                    $"\n {e.Source}");
                response.Error = new Error() { Code = 5, Message = $"Внутренняя ошибка сервера: {e.Message}" };
                response.Ok = false;
            }
            return JsonConvert.SerializeObject(response);
        }


        public class InSetValue
        {
            public long Id { get; set; }
            public string Table { get; set; }
            public string Column { get; set; }
            public object Value { get; set; }
        }

        public class InGetValue
        {
            public long Id { get; set; }
            public string Table { get; set; }
            public string Column { get; set; }
        }
    }
}
