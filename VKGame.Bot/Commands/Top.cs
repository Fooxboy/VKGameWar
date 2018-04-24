using System.Collections.Generic;
using VKGame.Bot.Models;

namespace VKGame.Bot.Commands
{
    public class Top :ICommand
    {
        public override string Name => "Топ";
        public override string Arguments => "(вариант выбора)";
        public override string Caption => "Зал славы. Здесь можно узнать лучших игроков и лучшии кланы в игре.";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() {"игроков", "кланов"};
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";

        public override object Execute(Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetTopText();
            var result = Helpers.Command.CheckMethods(typeof(Top), messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        [Attributes.Trigger("игроков")]
        public static string TopUsers(Models.Message msg)
        {
            var tops = new Api.Tops();
            var topsUser = tops.Users;
            string usersTopString = string.Empty;

            foreach(var userId in topsUser)
            {
                var user = new Api.User(userId);
                var registry = new Api.Registry(userId);

                usersTopString += $"😀 [id{user.Id}|{user.Name}]" +
                                 $"\n 🔝 Уровень: {user.Level} [{user.Experience}/ {user.Level *100}]" +
                                 $"\n 🏹 Выграл боёв: {registry.CountWinBattles}" +
                                 $"\n 🚬 Проиграл боёв: {registry.CountLoserBattle}" +
                                 $"\n 🔫 Всего боёв: {registry.CountBattles}" +
                                 $"\n ✔ Дата регистрации: {registry.DateReg}" +
                                 $"\n";

            }
            if (topsUser.Count == 0) usersTopString = "😎 Лучших пользователей пока что нет.";

            return $"\n 👍 СПИСОК ЛУЧШИХ ИГРОКОВ" +
                $"\n" +
                $"\n 🔄 Последнее обновление: {tops.DateUpdate}" +
                $"\n" +
                $"\n{usersTopString}" +
                $"\n" +
                $"\n ❤ Хочешь попасть сюда? Скорее играй и выиигрывай!";
        }

        [Attributes.Trigger("кланов")]
        public static string TopClans(Models.Message msg)
        {
            return "❌ Топ кланов пока что недоступен.";
        }

        private string GetTopText()
        {       
            return "😎 Здесь находится зал славы лучших игроков игры и лучших кланов." +
                   "\n" +
                   "\n❓ Чтобы получить топ 10 лучших игроков, напишите: топ игроков" +
                   "\n❓ Чтобы получить топ 10 лучших кланов, напишите: топ кланов";
        }
    }
}