﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class HttpErrors
    {
        public static string E404 => JsonConvert.SerializeObject(new RootResponse() { result = false, data = new Models.Error() { Code = 404, Message = " Не найдена запрошеная страница." } });

        public static string E500 => JsonConvert.SerializeObject(new RootResponse() { result = false, data = new Models.Error() { Code = 400, Message = "От сервера не поступило ответа." } });
    }
}
