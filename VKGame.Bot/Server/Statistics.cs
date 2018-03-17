using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VKGame.Bot.Server
{
    public class Statistics
    {
        public static string GetAll()
        {
            var response = new Common.Response<Models.Statistics>();

            try
            {
                var stat = Bot.Statistics.GetStat();
                response.Ok = true;
                response.Data = stat;
            }catch(Exception e)
            {
                response.Ok = false;
                response.Error = new Error() { Code = 5, Message = $"Внутренняя ошибка сервера: {e.Message}" };
            }

            return JsonConvert.SerializeObject(response);

        }
    }
}
