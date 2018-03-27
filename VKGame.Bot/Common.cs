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
        
        public static string GetToken()
        {
            return "fdbb5fb61db9939adc73759a114ed7b45853e5f171cca4b619e3b44452beef3ace1dbc2467c5e805ac240";
        }

        public static long LastMessage = 0;

        public static Dictionary<long, ICommand> LastCommand = new Dictionary<long, ICommand>();

        public static VkApi VkG = null;
        public static VkApi VkM = null;


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

        public static VkApi GetMyVk() 
        {
            if(Common.VkM == null)
            {
                var VkApi = new VkApi();

                string tokenMy = "dc9cb591241f4ac1e81415fd4c98cd396891ec690f4aa9798a846c5e8e39c04196e972dc7d2214859b2e3";
                VkApi.Authorize(new ApiAuthParams
                {
                    AccessToken = tokenMy
                });

                VkM = VkApi;
            }
            return VkM;
        }
        
        public static VkApi GetVk()
        {
            if(VkG == null)
            {
                var token = GetToken();
                var VkApi = new VkApi();
                VkApi.Authorize(new ApiAuthParams
                {
                    AccessToken = token
                });

                VkG = VkApi;
            }

            return VkG;
        }
    }
}
