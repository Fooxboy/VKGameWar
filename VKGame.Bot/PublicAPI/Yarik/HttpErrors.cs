using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class HttpErrors
    {
        public static string E404
        {
            get
            {
               
                return JsonConvert.SerializeObject(new RootResponse() { result = false, data = new Models.Error() { Code = 404, Message = "No page found." } });
            }
        }

        public static string E500
        {
            get
            {
               
                return JsonConvert.SerializeObject(new RootResponse() { result = false, data = new Models.Error() { Code = 400, Message = "Server is not available" } });
            }
        }
    }
}
