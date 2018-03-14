using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Clans : ICommand
    {
        public string Name => "клан";
        public string Caption => "Здесь можно управлять своим кланом!";
        public string Arguments => "(), (Вариант_выбора)";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            if (messageArray.Length == 1)
                return GetClansText(msg);
            else
            {
                var type = typeof(Clans);
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

        [Attributes.Trigger("оп")]
        public static string Exceptions(LongPollVK.Models.AddNewMsg msg)
        {
            throw new Exception("Тестовое исключение.");
        }

        [Attributes.Trigger("исключить")]
        public static string RemoveMember(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            var user = Api.User.GetUser(msg.PeerId);
            if (user.Clan == 0) return "❌ Вы не находитесь в каком-либо клане!";
            var clan = new Api.Clans(user.Clan);
            if (clan.Creator != user.Id) return "❌ Вы не являетесь создателем этого клана, чтобы исключать людей!";
            long userId = 0;
            try
            {
                userId = Int64.Parse(messageArray[2]);
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали id пользователя!";
            }catch(FormatException)
            {
                return "❌ Вы указали неверное id пользователя!";
            }
            var userRemove = Api.User.GetUser(userId);
            if (userRemove == null) return "❌ Этот пользователь не играет в эту игру вообще. Можешь ему рассказать о ней :)";
            bool isUser = false;
            var members = clan.Members;
            if (!isUser) return "❌ Такого пользователя нет в клане!";
            if (userRemove.Id == user.Id) return "❌ Вы не можете исключить самого себя!";
            members.Remove(userRemove.Id);
            userRemove.Clan = 0;
            Api.User.SetUser(userRemove);
            clan.Members = members;
            Api.MessageSend($"❗ Вас исключили из клана {clan.Name}", userRemove.Id);
            return "✅ Вы исключили пользователя из клана!";
        }

        [Attributes.Trigger("создать")]
        public static string Create(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            var name = "";
            try
            {
                name = messageArray[2];
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали название клана! Пример: Клан создать любофф";
            }
            if (name.Length > 30 || name.Length < 2) return "❌ Название клана должно быть меньше 30 символов или больше двух.";
            var resources = new Api.Resources(msg.PeerId);
            if (resources.MoneyCard < 1000) return "❌ Для создания клана нужна сумма в размере 1000 💳";
            var idClan = Api.Clans.New(msg.PeerId, name);
            Notifications.RemovePaymentCard(1000, msg.PeerId, "создание клана");
            var user = Api.User.GetUser(msg.PeerId);
            user.Clan = idClan;
            Api.User.SetUser(user);
            Statistics.CreateClan();
            return $"✅ Вы успешно создали клан {name}!";
        }

        [Attributes.Trigger("инфо")]
        public static string Info(LongPollVK.Models.AddNewMsg msg)
        {
            try
            {
                var messageArray = msg.Text.Split(' ');
                var user = Api.User.GetUser(msg.PeerId);
                if (user.Clan == 0) return "❌ Вы не находитесь в клане!";
                var clan = new Api.Clans(user.Clan);
                var members = clan.Members;
                string MemberString = "";

                foreach (var member in members)
                { 
                    var userMember = Api.User.GetUser(member);
                    MemberString += $"\n😎 [id{member}|{userMember.Name}]";
                }
                var infoClan = $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                             $"\n✅ Название: {clan.Name}" +
                             $"\n😀 Создатель: {clan.Creator}" +
                             $"\n" +
                             $"\n🖐 Участники: " +
                             $"{MemberString}" +
                             $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
                return infoClan;
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}\n {e.StackTrace}");
                return "Ты пидорас";
            }
            


        }

        [Attributes.Trigger("распустить")]
        public static string Delete(LongPollVK.Models.AddNewMsg msg)
        {
            var user = Api.User.GetUser(msg.PeerId);
            if (user.Clan == 0) return "❌ Вы не находитесь в каком-либо клане!";
            var clan = new Api.Clans(user.Clan);
            if (clan.Creator != msg.PeerId) return "❌ Вы не являетесь создателем клана!";
            var members = clan.Members;
            foreach(var member in members)
            {
                var userMember = Api.User.GetUser(member);
                userMember.Clan = 0;
                Api.User.SetUser(userMember);
                if(member != clan.Creator)
                {
                    Api.MessageSend("🎊 Хо-хо-хо. Похоже Ваш создатель слился и распустил клан! Ищи теперь новый :)", member);
                }
            }
            Api.Clans.Delete(user.Clan);
            return "✅ Вы успешно распустили клан! Ваши соклановцы получили уведомление об этом!";
        }

        [Attributes.Trigger("покинуть")]
        public static string Leave(LongPollVK.Models.AddNewMsg msg)
        {
            var user = Api.User.GetUser(msg.PeerId);
            if (user.Clan == 0) return "❌ Вы не находитесь в каком-либо клане!";
            var clan = new Api.Clans(user.Clan);
            if (msg.PeerId == clan.Creator) return "❌ Вы не можете покинуть свой же клан. Распустите его!";
            var members = clan.Members;
            members.Remove(msg.PeerId);
            var membersStr = "";
            foreach (var memeber in members) membersStr += $"{memeber},";
            clan.Members = members;
            user.Clan = 0;
            Api.User.SetUser(user);
            Api.MessageSend($"✨ Ваш клан покинул [{user.Id}|{user.Name}]", clan.Creator);
            return $"✅ Вы покинули клан {clan.Name}";

        }


        [Attributes.Trigger("вступить")]
        public static string Join(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            long id = 0;
            try
            {
                id = Int64.Parse(messageArray[2]);
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали id клана. Напрмиер: клан вступить 229";
            }catch(FormatException)
            {
                return "❌ Вы ввели неверный id клана.";
            }
            if (!Api.Clans.Check(id)) return "❌ Клана с таким id несуществует!";
            var user = Api.User.GetUser(msg.PeerId);
            if (user.Clan != 0) return "❌ Вы уже находитесь в клане! Для начала покинте его! Клан покинуть";
            var clan = new Api.Clans(id);
            List<long> members = clan.Members;
            members.Add(msg.PeerId);
            user.Clan = id;
            string membersStr = "";
            foreach (var member in members) membersStr += $"{member},";
            clan.Members = members;
            Api.User.SetUser(user);
            Api.MessageSend($"🎉 Хей-хей, у тебя в клане новый участник: [{user.Id}|{user.Name}] | ID: {user.Id}", clan.Creator);
            return $"✅ Вы вступили в клан {clan.Name}!";
                
        }

        private string GetClansText(LongPollVK.Models.AddNewMsg msg)
        {
            return "❓ Краткая помощь по разделу кланов:" +
                 "\n❗ Информация о клане: Клан инфо" +
                 "\n✅ Вступить в клан: Клан вступить id" +
                 "\n😎 Создать свой клан: Клан создать название" +
                 "\n⚠ Для ухода из клана: Клан покинуть" +
                 "\n🤔 Чтобы распустить свой клан: Клан распустить";
        }
    }
}
