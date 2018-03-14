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
        //string Token = null;
        VkApi VkApi = null;

        /// <summary>
        /// Получение токена
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            return "fdbb5fb61db9939adc73759a114ed7b45853e5f171cca4b619e3b44452beef3ace1dbc2467c5e805ac240";
        }

        public static ulong LastMessage = 0;

        public static string GetRandomHelp() 
        {
            string[] ListHelp =  
            {
                "Тут",
                ""
            };
            var r = new Random();
            var i = r.Next(0, (ListHelp.Length -1));
            return ListHelp[i];
        }

        public VkApi GetMyVk() 
        {
            var VkApi = new VkApi();
           
            string tokenMy  = "dc9cb591241f4ac1e81415fd4c98cd396891ec690f4aa9798a846c5e8e39c04196e972dc7d2214859b2e3";
            VkApi.Authorize(new ApiAuthParams
            {
                AccessToken = tokenMy
            });
            return VkApi;
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
