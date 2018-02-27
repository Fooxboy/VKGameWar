using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using System.Net;

namespace VKGame.Bot
{
    /// <summary>
    /// Глобальные методы.
    /// </summary>
    public class Common
    {
        string Token = null;
        VkApi VkApi = null;

        /// <summary>
        /// Получение токена
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            if (Token != null) return Token;
            else
            {
                using (var client = new WebClient())
                {
                    var result = client.DownloadString("http://fooxboy.esy.es/Token.txt");
                    Token = result;
                    //Console.WriteLine(Token);
                };

                if (Token != null)
                {
                    return Token;
                }
                else throw new ArgumentNullException();
            }  
        }   
        
        public VkApi GetVk()
        {
            if (VkApi != null) return VkApi;
            else
            {
                var token = GetToken();
                var VkApi = new VkApi();
                VkApi.Authorize(new ApiAuthParams
                {
                    AccessToken = token
                });
                return VkApi;
            }
        }
    }
}
