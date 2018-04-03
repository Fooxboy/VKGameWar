using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VKGame.Bot.Commands
{
    public class Bug: ICommand
    {
        public string Name => "Баг";
        public string Caption => "Раздел предназначен для уведомлении администрации о багах в игре.";
        public string Arguments => "(Описание бага)";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>();
        public Access Access => Access.User;

        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');

            if (messageArray.Length == 1) return NoTextBug();

            if(messageArray[1] == "список")
            {
                var bugs = Api.Bug.GetBugs().bugs;
                var userBugs = bugs.Where(b => b.User == msg.from_id);

                string bugsStr = String.Empty;
                if (userBugs.Count() == 0) bugsStr = "🤔 Похоже, Вы ещё не отправляли ни одного бага.";
                else
                {
                    foreach(var bug in userBugs)
                    {
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

                var bugs = Api.Bug.GetBugs();
                var randomId = strBug.GetHashCode();
                var bugModel = new Models.Bug() { Id = randomId, Status = 0, Text = strBug, Time = DateTime.Now.ToString(), User = msg.from_id };
                bugs.bugs.Add(bugModel);
                Api.Bug.SetBug(bugs);
                Api.MessageSend($"🎈 Новое сообщение о баге! ID - {randomId}", 308764786);

                return "🎈 Вы сообщили о баге! Если сообщение о баге ложное - вы получите выговор, если нет, то получите бонус в виде монет. Список багов, которые вы отправили: баг список ";
            }            
        }

        public static string NoTextBug()
        {
            return "🎗🎁 Здесь Вы можете сообщить о баге администрации проекта. Описывайте подробно как и что. Что вы делали, чтобы получить баг. " +
                "\n Для того, чтобы сообщнить достаточно написать: Баг <описание бага>";
        }
    }
}
