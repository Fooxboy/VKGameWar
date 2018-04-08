using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VKGame.Bot.Models;

namespace VKGame.Bot.Commands
{
    public class Quests :ICommand
    {
        public string Name => "Квесты";
        public string Caption => "Здесь можно получить специальные задания!";
        public string Arguments => "(), (Вариант выбора)";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>() {"старт", "покинуть", "прогресс" };
        public Access Access => Access.User;


        public object Execute(Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetQuestsText(msg);
            
            var type = typeof(Quests);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        [Attributes.Trigger("старт")]
        public static string Start(Message msg)
        {
            var messageArray = msg.body.Split(' ');
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

            if (!Api.Quests.Check(choise)) return "❌ Вы указали несуществующий ID квеста! Чтобы посмотреть список: Квесты";
            var quest = new Api.Quests(choise);
            var members = quest.Users;
            if (members.List.Any(u => u.Id == msg.from_id)) return "❌ Вы и так учавствуете в квесте!";
            if (!quest.IsOnline) return "❌ Квест уже закончился :( Чтобы посмотреть список: Квесты ";

            var user = new Api.User(msg.from_id);

            if (user.Quest != 0) return $"❌ Вы и так учавствуете в квесте № {user.Quest}! Если хоитите покинуть квест - Квесты покинуть";

            Models.Quests.User member = null;
            var membersList = quest.Users.List;
            var membersWhere = membersList.Where(u => u.Id == user.Id);
            foreach (Models.Quests.User memberfor in membersWhere)
            {
                member = memberfor;
            }

            if(member == null)
            {
                member = new Models.Quests.User()
                {
                    Id = user.Id,
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
            user.Quest = choise;

            return "✅ Вы успешно начали выполнять квест! Чтобы узнать прогресс, напишите: квесты прогресс";
        }

        public static void WinBattle(long id)
        {
            var user = new Api.User(id);
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

                if (member != null)
                {
                    membersList.Remove(member);

                    if (member.Status == 1)
                    {
                        member.Progress += 1;
                    }

                    if (member.Progress > 5 || member.Progress == 5)
                    {
                        member.Status = 3;
                        user.Quest = 0;
                        user.Level += 10;

                        Api.Message.Send($"💥 Вы выполнили квест! Вы получаете {quest.Price}", user.Id);
                        Notifications.EnterPaymentCard(Convert.ToInt32(quest.Price), user.Id, "выполнение квеста");
                    }
                    membersList.Add(member);

                    var model = new Models.Quests.Users();
                    model.List = membersList;
                    quest.Users = model;
                }
            }
        }

        public static void JoinBattle(long id)
        {
            var user = new Api.User(id);
            var quest = new Api.Quests(user.Quest);
            if (user.Quest == 2)
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
                    membersList.Remove(member);

                    if (member.Status == 1)
                    {
                        member.Progress += 1;
                    }

                    if (member.Progress > 5 || member.Progress == 5)
                    {
                        member.Status = 3;
                        user.Quest = 0;
                        user.Level += 10;

                        Api.Message.Send($"💥 Вы выполнили квест! Вы получаете {quest.Price}", user.Id);
                        Notifications.EnterPaymentCard(Convert.ToInt32(quest.Price), user.Id, "выполнение квеста");
                    }
                    membersList.Add(member);

                    var model = new Models.Quests.Users();
                    model.List = membersList;
                    quest.Users = model;
                }
            }
        }

        public static void GoToHome(long id)
        {
            var user = new Api.User(id);
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
                    membersList.Remove(member);

                    if (member.Status == 1)
                    {
                        member.Progress += 1;
                    }

                    if (member.Progress > 5 || member.Progress == 5)
                    {
                        member.Status = 3;
                        user.Quest = 0;
                        user.Level += 10;


                        Api.Message.Send($"💥 Вы выполнили квест! Вы получаете {quest.Price}", user.Id);
                        Notifications.EnterPaymentCard(Convert.ToInt32(quest.Price), user.Id, "выполнение квеста");
                    }
                    membersList.Add(member);

                    var model = new Models.Quests.Users();
                    model.List = membersList;
                    quest.Users = model;
                }
            }
        }

        [Attributes.Trigger("покинуть")]
        public static string Leave(Message msg)
        {
            var user = new Api.User(msg.from_id);
            if (user.Quest == 0) return "❌ Вы у не учавствуете ни в каком квесте! Посмотрите список квестов, написав: квесты";
            var quest = new Api.Quests(user.Quest);

            user.Quest = 0;

            Models.Quests.User member = null;
            var membersWhere = quest.Users.List.Where(u => u.Id == user.Id);
            foreach (Models.Quests.User memberfor in membersWhere)
            {
                member = memberfor;
            }
            if (member == null) return "❌ Вас нет в списке участников! Скорее, произошла ошибка, напишите [fooxboy|разработчику] об ошибке.";

            var listMembers = quest.Users.List;
            listMembers.Remove(member);
            var model = new Models.Quests.Users();
            model.List = listMembers;
            quest.Users = model;
            return "✅ Вы покинули квест! ";
        }

        [Attributes.Trigger("прогресс")]
        public static string Progress(Message msg)
        {
            var user = new Api.User(msg.from_id);
            if (user.Quest == 0) return "❌ Вы у не учавствуете ни в каком квесте! Посмотрите список квестов, написав: квесты";
            var quest = new Api.Quests(user.Quest);

            Models.Quests.User member = null;
            var membersWhere = quest.Users.List.Where(u => u.Id == user.Id);
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

        private string GetQuestsText(Message msg)
        {
       
            string text = $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                        $"\n🔎 КВЕСТЫ" +
                        $"\n❓ Здесь хранятся специальные задания, за которые можно получить неплохую сумму!" +
                        $"\n" +
                        $"\nСПИСОК КВЕСТОВ➖➖➖➖➖➖➖➖➖➖" +
                        $"\n➡ 1 - Победить в 10 битвах до 23:00(мск)" +
                        $"\n➡❓ Для того, чтобы начать выполнять, напишите: Квест старт 1." +
                        $"\n➡💰 Вознаграждение: 200" +
                        $"\n➡❓ Квест можно выполнять ежедневно, начиная с 23:02 по мск" +
                        $"\n" +
                        $"\n➡ 2 - Поучаствовать в 15 битвах до 23:00(мск)" +
                        $"\n➡❓ Для того, чтобы начать выполнять, напишите: Квест старт 2." +
                        $"\n➡💰 Вознаграждение: 250" +
                        $"\n➡❓ Квест можно выполнять ежедневно, начиная с 23:02 по мск" +
                        $"\n" +
                        $"\n➡ 3 - (only for testers) Написать домой 5 раз" +
                        $"\n➡❓ Для того, чтобы начать выполнять, напишите: Квест старт 3." +
                        $"\n➡💰 Вознаграждение: 300";
            return text;
        }
    }
}
