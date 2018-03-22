﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VKGame.Bot.Commands
{
    public class Quests :ICommand
    {
        public string Name => "Квесты";
        public string Caption => "Здесь можно получить специальные задания!";
        public string Arguments => "(), (Вариант выбора)";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            if (messageArray.Length == 1)
                return GetQuestsText(msg);
            else
            {
                var type = typeof(Quests);
                object obj = Activator.CreateInstance(type);
                var methods = type.GetMethods();

                foreach (var method in methods)
                {
                    var attributesCustom = Attribute.GetCustomAttributes(method);

                    foreach (var attribute in attributesCustom)
                    {
                        if (attribute.GetType() == typeof(Attributes.Trigger))
                        {
                            var myAtr = ((Attributes.Trigger)attribute);

                            if (myAtr.Name == messageArray[1])
                            {
                                object result = method.Invoke(obj, new object[] { msg });
                                return (string)result;
                            }
                        }
                    }
                }
                return "❌ Неизвестная подкоманда.";
            }
        }

        [Attributes.Trigger("старт")]
        public static string Start(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            long choise = 0;
            try
            {
                choise = Int64.Parse(messageArray[2]);
            } catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали номер квеста! Чтобы посмотреть список: Квесты";
            } catch (FormatException)
            {
                return "❌ Вы указали неверный ID квеста! Чтобы посмотреть список: Квесты";
            }

            if (Api.Quests.Check(choise)) return "❌ Вы указали неверный ID квеста! Чтобы посмотреть список: Квесты";
            var quest = new Api.Quests(choise);
            var members = quest.Users;
            if (members.List.Any(u => u.Id == msg.PeerId)) return "❌ Вы и так учавствуете в квесте!";
            if (!quest.IsOnline) return "❌ Квест уже закончился :( Чтобы посмотреть список: Квесты ";

            var user = Api.User.GetUser(msg.PeerId);

            if (user.Quest != 0) return $"❌ Вы и так учавствуете в квесте № {user.Quest}! Если хоитите покинуть квест - Квесты покинуть";

            Models.Quests.User member = null;
            var membersList = quest.Users.List;
            var membersWhere = membersList.Where(u => u.Id == msg.PeerId);
            foreach (Models.Quests.User memberfor in membersWhere)
            {
                member = memberfor;
            }

            if(member == null)
            {
                member = new Models.Quests.User()
                {
                    Id = msg.PeerId,
                    Progress = 0,
                    Status = 1
                };

                members.List.Add(member);
            }else
            {
                if (member.Status == 3) return "❌ Вы уже выполнили этот квест!";
                member.Status = 1;
            }
           
            quest.Users = members;
            

            return "✅ Вы успешно начали выполнять квест! Чтобы узнать прогресс, напишите: квесты прогресс";
        }

        public static void WinBattle(long id)
        {
            var user = Api.User.GetUser(id);

            var quest = new Api.Quests(user.Quest);
            if (user.Quest == 1)
            {
                
                Models.Quests.User member = null;
                var membersList = quest.Users.List;
                var membersWhere = membersList.Where(u => u.Id == id);
                foreach (Models.Quests.User memberfor in membersWhere)
                {
                    member = memberfor;
                }

                if(member != null)
                {
                    if(member.Status == 1)
                    {
                        member.Progress += 1; 
                    }
                }
               
                if(member.Progress > 10 || member.Progress == 10)
                {
                    member.Status = 3;
                    user.Quest = 0;

                    Api.MessageSend($"💥 Вы выполнили квест! Вы получаете {quest.Price}", user.Id);
                    Notifications.EnterPaymentCard(Convert.ToInt32(quest.Price), user.Id, "выполнение квеста");
                }
                membersList.Remove(member);
                membersList.Add(member);
                Api.User.SetUser(user);
            }
        }

        public static void JoinBattle(long id)
        {

        }

        public static void GoToHome(long id)
        {
            var user = Api.User.GetUser(id);

            var quest = new Api.Quests(user.Quest);
            if (user.Quest == 3)
            {

                Models.Quests.User member = null;
                var membersList = quest.Users.List;
                var membersWhere = membersList.Where(u => u.Id == id);
                foreach (Models.Quests.User memberfor in membersWhere)
                {
                    member = memberfor;
                }

                if (member != null)
                {
                    if (member.Status == 1)
                    {
                        member.Progress += 1;
                    }
                }

                if (member.Progress > 5 || member.Progress == 5)
                {
                    member.Status = 3;
                    user.Quest = 0;

                    Api.MessageSend($"💥 Вы выполнили квест! Вы получаете {quest.Price}", user.Id);
                    Notifications.EnterPaymentCard(Convert.ToInt32(quest.Price), user.Id, "выполнение квеста");
                }
                membersList.Remove(member);
                membersList.Add(member);
                Api.User.SetUser(user);
            }
        }

        [Attributes.Trigger("Покинуть")]
        public static string Leave(LongPollVK.Models.AddNewMsg msg)
        {
            return "возможность покинуть квест пока что недоступна :/";
        }

        [Attributes.Trigger("прогресс")]
        public static string Progress(LongPollVK.Models.AddNewMsg msg)
        {
            var user = Api.User.GetUser(msg.PeerId);
            if (user.Quest == 0) return "❌ Вы у не учавствуете ни в каком квесте! Посмотрите список квестов, написав: квесты";
            var quest = new Api.Quests(user.Quest);

            Models.Quests.User member = null;
            var membersWhere = quest.Users.List.Where(u => u.Id == msg.PeerId);
            foreach(Models.Quests.User memberfor in membersWhere)
            {
                member = memberfor;
            }
            if (member == null) return "❌ Вас нет в списке участников! Скорее, произошла ошибка, напишите [fooxboy|разработчику] об ошибке.";

            string info = $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\nИНФОРМАЦИЯ О ВАШЕМ КВЕСТЕ, КОТОРЫЙ ВЫПОЛНЯЕТСЯ СЕЙЧАС:" +
                          $"\n" +
                          $"\n➡ Название: {quest.Name}" +
                          $"\n➡ Id: {quest.Id}" +
                          $"\n➡ Прогресс: {member.Progress}" +
                          $"\n➡ Статус: {member.Status}" +
                          $"\n❓ 1-Ещё выполняется, 3- закончено выполнение." +
                          $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";

            return info;
        }

        private string GetQuestsText(LongPollVK.Models.AddNewMsg msg)
        {
       
            string text = $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                        $"\n🔎 КВЕСТЫ" +
                        $"\n❓ Здесь хранятся специальные задания, за которые можно получить неплохую сумму!" +
                        $"\n" +
                        $"\nСПИСОК КВЕСТОВ➖➖➖➖➖➖➖➖➖➖" +
                        $"\n➡ 1 - Победить в 10 битвах до 23:00(мск)" +
                        $"\n➡❓ Для того, чтобы начать выполнять, напишите: Квест старт 1." +
                        $"\n➡💰 Вознаграждение: price" +
                        $"\n➡❓ Квест можно выполнять ежедневно, начиная с 23:02 по мск" +
                        $"\n" +
                        $"\n➡ 2 - Поучаствовать в 15 битвах до 23:00(мск)" +
                        $"\n➡❓ Для того, чтобы начать выполнять, напишите: Квест старт 2." +
                        $"\n➡💰 Вознаграждение: price" +
                        $"\n➡❓ Квест можно выполнять ежедневно, начиная с 23:02 по мск" +
                        $"\n" +
                        $"\n➡ 3 - (only for testers) Написать домой 5 раз" +
                        $"\n➡❓ Для того, чтобы начать выполнять, напишите: Квест старт 3." +
                        $"\n➡💰 Вознаграждение: price";
            return text;
        }
    }
}
