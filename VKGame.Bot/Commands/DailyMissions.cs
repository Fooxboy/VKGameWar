using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class DailyMissions: ICommand
    {
        public override string Name => "Задания";
        public override string Caption => "В Этом разеделе находятся доступные Вам задания.";
        public override string Arguments => "(), (вариант_выбора)";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() {};
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";


        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1) return GetMissionsText(msg);
            var result = Helpers.Command.CheckMethods(typeof(Army), messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        public string GetMissionsText(Models.Message msg)
        {
            return null;
        }
    }
}
