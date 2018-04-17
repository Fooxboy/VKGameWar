using System.IO;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VKGame.Bot.BackgroundProcess
{
    public class Common 
    {
        public static void DailyBonus() 
        {
            while(true) 
            {
                Thread.Sleep(3600000);
                try
                {
                    if (DateTime.Now.Hour == 8)
                    {
                        var listUsers = Api.User.AllList;
                        Dictionary<long, long> usersTop = new Dictionary<long, long>();
                        foreach (var userId in listUsers)
                        {
                            
                            var registry = new Api.Registry(userId);
                            usersTop.Add(userId, registry.CountWinBattles);
                            var lastMessage = DateTime.Parse(registry.LastMessage);
                            int day = lastMessage.Day;
                            int nowDay = 0;
                            if (lastMessage.Month == DateTime.Now.Month) nowDay = DateTime.Now.Day;
                            else nowDay = DateTime.Now.Day + 31;
                            if (nowDay - day < 2)
                            {
                                Api.Message.Send("🎉 Ежедневный бонус! Спасибо, что Вы играли вчера! Вот Ваш маленький бонус сегодня! 300 монет!", userId);
                                Notifications.EnterPaymentCard(300, userId, "ежедненый бонус");
                            }
                            else
                            {
                                if ((nowDay - day > 5) && (nowDay - day < 20))
                                {
                                    Api.Message.Send("🎈 Привет! Я вижу, что ты давно не играл! Ваша армия Вас ждёт! НАЧИНАЙ ИГРАТЬ :)", userId);
                                }
                            }
                        }

                        var userTopAll = usersTop.OrderByDescending(u => u.Value);
                        var i = 0;
                        var listAh = new List<long>();
                        foreach(var topUser in userTopAll)
                        {
                            if (i == 10) break;
                            listAh.Add(topUser.Key);
                            i++;
                        }

                        var tops = new Api.Tops();
                        tops.Users = listAh;
                        tops.DateUpdate = DateTime.Now.ToString();
                    }
                }catch(Exception e)
                {
                    Bot.Statistics.NewError();
                    Logger.WriteError(e);
                }
            }
        }
    
        public static void StartServer()
        {
            try
            {
                Server.Start.Listen();

            }catch(Exception e)
            {
                Bot.Statistics.NewError();
                Logger.WriteError(e);
            }
        }

        public static void ResetMembers()
        {
            while(true)
            {
                try
                {
                    if(DateTime.Now.Hour == 23 || DateTime.Now.Hour == 12)
                    {
                        try
                        {
                            Api.CacheMessages.ResetCache();
                        }
                        catch (Exception e)
                        {
                            Bot.Statistics.NewError();
                            Logger.WriteError(e);
                        }
                    }
                    if (DateTime.Now.Hour == 23)
                    { 
                        //обуление пользователей в квесте 1
                        var quest = new Api.Quests(1);
                        var members = quest.Users.List;
                        if(members != null)
                        {
                            if (members.Count != 0)
                            {
                                foreach (var member in members)
                                {
                                    var user = new Api.User(member.Id);
                                    user.Quest = 0;
                                }
                                members = new System.Collections.Generic.List<Models.Quests.User>();
                            }
                        }
                       
                        quest.Users = new Models.Quests.Users() { List = members };

                        //Обнуление пользователй в квесте 2
                        var quest2 = new Api.Quests(2);
                        var members2 = quest.Users.List;
                        if(members2 != null)
                        {
                            if (members2.Count != 0)
                            {
                                foreach (var member2 in members2)
                                {
                                    var user2 = new Api.User(member2.Id);
                                    user2.Quest = 0;
                                }
                                members2 = new System.Collections.Generic.List<Models.Quests.User>();
                            }
                        }           
                        quest2.Users = new Models.Quests.Users() { List = members2 };
                    }
                }catch(Exception e)
                {
                    Bot.Statistics.NewError();
                    Logger.WriteError(e);
                }
            }

        }

        public static void RebootBot() 
        {        
            if(DateTime.Now.Hour == 21)  
            {
                Thread.Sleep(5000);
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
    }
}