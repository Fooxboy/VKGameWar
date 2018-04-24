using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VKGame.Bot.Api;

namespace VKGame.Bot.Commands
{
    public class Bug: ICommand
    {
        public override string Name => "Баг";
        public override string Caption => "Раздел предназначен для уведомлении администрации о багах в игре.";
        public override string Arguments => "(Описание бага)";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>();
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";

        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');

            if (messageArray.Length == 1) return NoTextBug();

            if(messageArray[1] == "список")
            {
                var userBugs = Api.Bugs.BugsFromUser(msg.from_id);

                string bugsStr = String.Empty;
                if (userBugs.Count == 0) bugsStr = "🤔 Похоже, Вы ещё не отправляли ни одного бага.";
                else
                {
                    foreach(var bugId in userBugs)
                    {
                        var bug = new Api.Bugs(bugId);
                        bugsStr += $"🆔 Id - {bug.Id}" +
                                $"\n ⏰ Время отправки: {bug.Time}" +
                                $"\n ✨ Сообщение {bug.Text}" +
                                $"\n 🎈 Статус: {bug.Status}" +
                                $"\n ❓ Возможные статусы: 0 - не прочитано, 1 - прочитано, 2 - работа над исправлением, 3 - исправлено в обновлении, 4 - исправлено, 5 - не баг, 6 - бесмысленное сообщение" +
                                $"\n" +
                                $"\n" +
                                $"";
                    }
                }
                return bugsStr;
            }else
            {
                var strBug = String.Empty;

                for (int i = 1; i < messageArray.Length; i++)
                {
                    strBug += $"{messageArray[i]} ";
                }

                var randomId = Bugs.Add(strBug, msg.from_id);
                Api.Message.Send($"🎈 Новое сообщение о баге! ID - {randomId}", 308764786);

                return "🎈 Вы сообщили о баге! Если сообщение о баге ложное - вы получите выговор, если нет, то получите бонус в виде монет. Список багов, которые вы отправили: баг список ";
            }            
        }

        private static string NoTextBug()
        {
            return "🎗🎁 Здесь Вы можете сообщить о баге администрации проекта. Описывайте подробно как и что. Что вы делали, чтобы получить баг. " +
                "\n Для того, чтобы сообщнить достаточно написать: Баг <описание бага>";
        }
    }
}
