using System.IO;
using System.Threading;
using System;
using System.Drawing;

namespace VKGame.Bot.BackgroundProcess
{
    public class Common 
    {
        public static void UpdateStatus() 
        {
            
            var common = new Bot.Common();
            var vk = common.GetMyVk();
            while(true) 
            {
                try 
                {
                    vk.Status.Set($"♻ Последнее обновление: {DateTime.Now}.");
                    Thread.Sleep(10000);
                }catch 
                {
                    Thread.Sleep(60000);
                }
                
            }
        }


        public static void UpdateHeaderGroup() 
        {
           //ы var common = new Bot.Common();
           // var vk = common.GetMyVk();
            //var image = "картинка";
            while(true) 
            {
                
            }
        }
    }
}