using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace VKGame.Bot.Commands
{
    public class Clans : ICommand
    {
        public override string Name => "Клан";
        public override string Caption => "Здесь можно управлять своим кланом!";
        public override string Arguments => "(), (Вариант_выбора)";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() { "исключить", "создать", "запрос", "бой", "инфо", "распустить", "покинуть", "вступить"  };
        public override Access Access => Access.User;


        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetClansText(msg);
            var type = typeof(Clans);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        [Attributes.Trigger("исключить")]
        public static string RemoveMember(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            var user = new Api.User(msg.from_id);
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
            
            if(Api.User.Check(userId)) return $"❌ Этот пользователь не играет в эту игру вообще. Можешь ему рассказать о ней -  [id{userId}|тыкай]. :)";
            var userRemove = new Api.User(userId);
            var members = clan.Members;
            if(userRemove.Clan != clan.Id) return "❌ Такого пользователя нет в клане!";
            if (userRemove.Id == user.Id) return "❌ Вы не можете исключить самого себя!";
            members.Remove(userRemove.Id);
            userRemove.Clan = 0;
            clan.Members = members;
            Api.Message.Send($"❗ Вас исключили из клана {clan.Name}", userRemove.Id);
            return "✅ Вы исключили пользователя из клана!";
        }

        [Attributes.Trigger("создать")]
        public static string Create(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            var name = "";
            try
            {
                name = messageArray[2];
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали название клана! Пример: Клан создать любофф";
            }
            if (name.Length > 30 || name.Length < 2) return "❌ Название клана должно быть меньше 30 символов или больше двух.";
            var user = new Api.User(msg.from_id);
            var resources = new Api.Resources(user.Id);
            if (resources.MoneyCard < 1000) return $"❌ Для создания клана нужна сумма в размере 1000 💳 Ваш баланс: {resources.MoneyCard}";
            var idClan = Api.Clans.New(user.Id, name);
            Notifications.RemovePaymentCard(1000, user.Id, "создание клана");
            
            user.Clan = idClan;
            Statistics.CreateClan();
            return $"✅ Вы успешно создали клан {name}!";
        }

        [Attributes.Trigger("запрос")]
        public static string Request(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
            if (user.Clan == 0) return "❌ Вы не находитесь в каком-либо клане. Вы можете создать свой клан.";
            var clan = new Api.Clans(user.Clan);
            if (clan.Creator != user.Id) return "❌ Вы должны быть создателем клана!";
            var messageArray = msg.body.Split(' ');

            var access = "";
            try
            {
                access = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали аргумент. Доспутные аргументы: принять, отклонить";
            }

            var text = "лол";
            if (access.ToLower() == "принять")
            {
                //таTODO: допилить создание клановых боев.

            }
            else if (access.ToLower() == "отклонить")
            {

            }
            else return "❌ Вы указали неизвестный аргумент. Доспутные аргументы: принять, отклонить";
            return $"{text}";
        }

        [Attributes.Trigger("бой")]
        public static string Battle(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
            if (user.Clan == 0) return "❌ Вы не находитесь в каком-либо клане. Вы можете создать свой клан.";
            var clan = new Api.Clans(user.Clan);
            if (clan.Creator != user.Id) return "❌ Вы должны быть создателем клана!";
            long clanId = 0;
            var messageArray = msg.body.Split(' ');
            try
            {
                clanId = Int64.Parse(messageArray[2]);
            }catch(FormatException)
            {
                return "❌ Неверное Id клана!";
            }catch(IndexOutOfRangeException)
            {
                return "меню";
            }
            if (!Api.Clans.Check(clanId)) return "❌ Такого клана не существует!";

            long price = 0;

            try
            {
                price = Int64.Parse(messageArray[3]);
            }catch(FormatException)
            {
                return "❌ Неизвестная сумма...";
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали сумму. Пример: клан бой id сумма";
            }
          //  if (clan.War == true) return "❌ Вы и так учавствуете в войне!";
          //  if (clan.WarId != 0) return "❌ Вашему клану или Вы другому клану уже кинули запрос войны.";
          //  if (clan.Fund < price) return $"❌ В фонде вашего клана недостаточно денег. Фонд клана: {clan.Fund}💳";
            var clanWar = new Api.Clans(clanId);
            //Api.Message.Send($"😎 Привет-привет! Вам поступил запрос о создании войны от клана {clan.Name}! Победитель получает в фонд клана - {clan.Fund *2}💳" +
             //   $"\n❓ Чтобы принять запрос, напишите: Клан запрос принять" +
             //   $"\n❓ Чтобы отклонить запрос, напишите: Клан запрос отклонить", clanWar.Creator);
          //  clan.WarId = clanId;
          //  clanWar.WarId = clan.Id;
            return "✅ Основателю клана был отправлен запрос о начале войны. Если он согласится, то Вы и Ваши соклановцы получат уведомления об этом.";
        }

        [Attributes.Trigger("инфо")]
        public static string Info(Models.Message msg)
        {
                var messageArray = msg.body.Split(' ');
                var user = new Api.User(msg.from_id);
                if (user.Clan == 0) return "❌ Вы не находитесь в клане!";
                var clan = new Api.Clans(user.Clan);
                var members = clan.Members;
                var moders = clan.Moders;
                string MemberString = String.Empty;
                string modersString = String.Empty;
                
                foreach (var member in members)
                { 
                    var userMember = new Api.User(member);
                    MemberString += $"\n😎 [id{member}|{userMember.Name}]";
                }

                foreach (var moder in moders)
                {
                    var userModer = new Api.User(moder);
                    modersString += $"\n💣 [id{moder}|{userModer.Name}]";
                }
                          
                var infoClan = $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                             $"\n✅ Название: {clan.Name}" +
                             $"\n😀 Создатель: *id{clan.Creator}" +
                             $"\n" +
                             $"\n❗ Модераторы:" +
                             $"\n {modersString}" +
                             $"\n" +
                             $"\n🖐 Участники: " +
                             $"{MemberString}" +
                             $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
                return infoClan;
            
        }

        [Attributes.Trigger("распустить")]
        public static string Delete(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
            if (user.Clan == 0) return "❌ Вы не находитесь в каком-либо клане!";
            var clan = new Api.Clans(user.Clan);
            if (clan.Creator != user.Id) return "❌ Вы не являетесь создателем клана!";
            var members = clan.Members;
            foreach(var member in members)
            {
                var userMember = new Api.User(member);
                userMember.Clan = 0;
                if(member != clan.Creator)
                {
                    Api.Message.Send("🎊 Хо-хо-хо. Похоже Ваш создатель слился и распустил клан! Ищи теперь новый :)", member);
                    Thread.Sleep(500);
                }
            }
            Api.Clans.Delete(user.Clan);
            return "✅ Вы успешно распустили клан! Ваши соклановцы получили уведомление об этом!";
        }

        [Attributes.Trigger("покинуть")]
        public static string Leave(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
            if (user.Clan == 0) return "❌ Вы не находитесь в каком-либо клане!";
            var clan = new Api.Clans(user.Clan);
            if (user.Id == clan.Creator) return "❌ Вы не можете покинуть свой же клан. Распустите его!";
            var members = clan.Members;
            members.Remove(user.Id);
            var membersStr = "";
            foreach (var memeber in members) membersStr += $"{memeber},";
            clan.Members = members;
            user.Clan = 0;
            Api.Message.Send($"✨ Ваш клан покинул [{user.Id}|{user.Name}]", clan.Creator);
            return $"✅ Вы покинули клан {clan.Name}";

        }


        [Attributes.Trigger("вступить")]
        public static string Join(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            long id = 0;
            try
            {
                id = Int64.Parse(messageArray[2]);
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали id клана. Напрмиер: клан вступить 666";
            }catch(FormatException)
            {
                return "❌ Вы ввели неверный id клана.";
            }
            if (!Api.Clans.Check(id)) return "❌ Клана с таким id несуществует!";
            var user = new Api.User(msg.from_id);
            if (user.Clan != 0) return "❌ Вы уже находитесь в клане! Для начала покинте его! Напишите: Клан покинуть";
            var clan = new Api.Clans(id);
            List<long> members = clan.Members;
            members.Add(user.Id);
            user.Clan = id;
            string membersStr = "";
            foreach (var member in members) membersStr += $"{member},";
            clan.Members = members;
            Api.Message.Send($"🎉 Хей-хей, у тебя в клане новый участник: [{user.Id}|{user.Name}] | ID: {user.Id}", clan.Creator);
            return $"✅ Вы вступили в клан {clan.Name}!";
                
        }

        private string GetClansText(Models.Message msg)
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
