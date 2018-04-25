using System;
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
                var message = "У Вас нет доступа к этому методу.";
                Encoding srcEncodingFormat = Encoding.GetEncoding("Windows-1251");
                Encoding dstEncodingFormat = Encoding.UTF8;
                byte[] originalByteString = srcEncodingFormat.GetBytes(message);
                byte[] convertedByteString = Encoding.Convert(srcEncodingFormat,
                dstEncodingFormat, originalByteString);
                string finalString = dstEncodingFormat.GetString(convertedByteString);
                return Newtonsoft.Json.JsonConvert.SerializeObject(new RootResponse() { result = false, data = new Models.Error() { Code = 9, Message = finalString } });
            }
        }
    }

    public class RootResponse
    {
        public bool result { get; set; }
        public object data { get; set; }
    }
}
