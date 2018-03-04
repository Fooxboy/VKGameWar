using System.IO;
using System.Threading;
using System;
using System.Drawing;

namespace VKGame.Bot.BackgroundProcess
{
    public class Common 
    {

        public static void DailyBonus() 
        {
            while(true) 
            {
                if(DateTime.Now.Hour == 8) 
                {
                    var listUsers = Api.UserList.GetList();
                    foreach(var userId in listUsers.Users) 
                    {
                        var user = Api.User.GetUser(userId);
                        var lastMessage = DateTime.Parse(user.LastMessage);
                        int day = lastMessage.Day;
                        int nowDay = 0;
                        if (lastMessage.Month == DateTime.Now.Month) nowDay = DateTime.Now.Day;
                        else nowDay = DateTime.Now.Day + 31;

                    }
                }
                
            }
        }
        public static void UpdateStatus() 
        {
            
            var common = new Bot.Common();
            var vk = common.GetMyVk();
            while(true) 
            {
                try 
                {
                    vk.Status.Set($"♻ Последнее обновление: {DateTime.Now}.", 161965172);
                    Thread.Sleep(10000);
                }catch 
                {
                    Thread.Sleep(60000);
                }
                
            }
        }

        public static void RebootBot() 
        {        
            if(DateTime.Now.Hour == 21)  
            {
                Thread.Sleep(5000);
                Logger.WriteError("ПЕРЕЗАГРУЗКА...");
                string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //это мы узнали полное имя запущенного приложения.
                //чтоб запустить его снова сделаем так
                System.Diagnostics.Process.Start(path);
                //далее чтоб закрыть это приложение сделаем так
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                //хотя думаю просто вернув return в функции Main() закроет приложение
            }
            Thread.Sleep(3600000);
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