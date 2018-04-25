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
                var msg = "Не найдена запрошеная страница.";
                Encoding srcEncodingFormat = Encoding.GetEncoding("Windows-1251");
                Encoding dstEncodingFormat = Encoding.UTF8;
                byte[] originalByteString = srcEncodingFormat.GetBytes(msg);
                byte[] convertedByteString = Encoding.Convert(srcEncodingFormat,
                dstEncodingFormat, originalByteString);
                string finalString = dstEncodingFormat.GetString(convertedByteString);
                return JsonConvert.SerializeObject(new RootResponse() { result = false, data = new Models.Error() { Code = 404, Message = finalString } });
            }
        }

        public static string E500
        {
            get
            {
                var msg = "От сервера не поступило ответа.";
                Encoding srcEncodingFormat = Encoding.GetEncoding("Windows-1251");
                Encoding dstEncodingFormat = Encoding.UTF8;
                byte[] originalByteString = srcEncodingFormat.GetBytes(msg);
                byte[] convertedByteString = Encoding.Convert(srcEncodingFormat,
                dstEncodingFormat, originalByteString);
                string finalString = dstEncodingFormat.GetString(convertedByteString);


                return JsonConvert.SerializeObject(new RootResponse() { result = false, data = new Models.Error() { Code = 400, Message =  finalString} });
            }
        }
    }
}
