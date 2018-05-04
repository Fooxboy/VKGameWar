using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Friends: ICommand
    {
        public override string Name => "Друзья";
        public override string Arguments => "(), (Вариант_выбора)";
        public override string Caption => "Здесь Вы сможете управлять Вашими друзьями xD.";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() { };
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";


        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetFriendsText(msg);
            var type = typeof(Friends);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        [Attributes.Trigger("принять")]
        public string Join(Models.Message msg)
        {
            return null;
        }

        [Attributes.Trigger("отклонить")]
        public string Cancel(Models.Message msg)
        {
            return null;
        }


        [Attributes.Trigger("добавить")]
        public string Add(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            long friendTo = 0;

            try
            {
                friendTo = Int64.Parse(messageArray[2]);
            }catch(FormatException)
            {
                return "❌ Неверный Id пользователя.";
            }catch(IndexOutOfRangeException)
            {
                if (msg.fwd_messages.Count == 0)
                    return "❌ Вы не указали Id пользователя или не переслали сообщение с ним.";
                friendTo = msg.fwd_messages[0].user_id;
            }

            if (!Api.User.Check(friendTo))
                return "❌ Этот пользователь не зарегестирирвоан в нашей игре. Скорее напиши ему и пригласи его в игру!" +
                    " За это ты ещё и бонус получишь! Подробнее - Рефералы";

            //отправка заявки пользователю....

            return "✅ Заяка успешно отправлена! Если пользователь её приймёт, то Вы получите уведомление.";
        }

        [Attributes.Trigger("список")]
        public string List(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
            var friends = user.Friends;
            string friendsString = String.Empty;
            foreach(var friend in friends)
            {
                var friendUser = new Api.User(friend);
                friendsString += $"🏹 [id{friend}| {friendUser.Name}]" +
                    $"\n 🔝 Уровень: {friendUser.Level}" +
                    $"\n 🔫 Id клана: {friendUser.Clan}" +
                    $"\n 👀 Опыт: {friendUser.Experience}" +
                    $"\n ❌ Чтобы удалить его из друзей, напишите: друзья удалить {friend}" +
                    $"\n" +
                    $"\n";
            }
            return $"==========================================" +
                $"\n 😁 ВАШИ ДРУЗЬЯ:" +
                $"\n" +
                $"\n {friendsString}";
        }

        public static string GetFriendsText(Models.Message msg)
        {
            return null;
        }
    }
}
