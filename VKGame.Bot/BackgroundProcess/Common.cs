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
                try
                {
                    Thread.Sleep(3600000);
                    if (DateTime.Now.Hour == 8)
                    {
                        var listUsers = Api.UserList.GetList();
                        foreach (var userId in listUsers.Users)
                        {
                            var user = Api.User.GetUser(userId);
                            var lastMessage = DateTime.Parse(user.LastMessage);
                            int day = lastMessage.Day;
                            int nowDay = 0;
                            if (lastMessage.Month == DateTime.Now.Month) nowDay = DateTime.Now.Day;
                            else nowDay = DateTime.Now.Day + 31;
                            if (DateTime.Now.Day - day < 2)
                            {
                                Api.MessageSend("🎉 Ежедневный бонус! Спасибо, что Вы играете каждый день! Вот Ваш маленький бонус сегодня! 500 монет!", userId);
                                Notifications.EnterPaymentCard(500, userId, "ежедненый бонус");
                            }
                            else
                            {
                                if ((DateTime.Now.Day - day > 5) && (DateTime.Now.Day - day < 20))
                                {
                                    Api.MessageSend("🎈 Привет! Я вижу, что ты давно не играл! Ваша армия Вас ждёт! НАЧИНАЙ ИГРАТЬ :)", userId);
                                }

                            }

                        }
                    }
                }catch(Exception e)
                {
                    Logger.WriteError($"{e.Message} \n {e.StackTrace}");

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