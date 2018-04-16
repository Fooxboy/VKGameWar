using System.Collections.Generic;
using VKGame.Bot.Models;

namespace VKGame.Bot.Commands
{
    public class Top :ICommand
    {
        public string Name => "Топ";
        public string Arguments => "(вариант выбора)";
        public string Caption => "Зал славы. Здесь можно узнать лучших игроков и лучшии кланы в игре.";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>() {"игроков", "кланов"};
        public Access Access => Access.User;
        public object Execute(Message msg)
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

        private string GetTopText()
        {       
            return "😎 Здесь находится зал славы лучших игроков игры и лучших кланов." +
                   "\n" +
                   "\n❓ Чтобы получить топ 10 лучших игроков, напишите: топ игроков" +
                   "\n❓ Чтобы получить топ 10 лучших кланов, напишите: топ кланов";
        }
    }
}