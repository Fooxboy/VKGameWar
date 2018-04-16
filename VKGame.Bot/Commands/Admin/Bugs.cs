using System;
using System.Collections.Generic;

namespace VKGame.Bot.Commands.Admin
{
    public class Bugs :ICommand
    {
        public string Name => "Баги";
        public string Caption => "аа";
        public string Arguments => "";
        public List<string> Commands => new List<string>();
        public TypeResponse Type => TypeResponse.Text;
        public Access Access => Access.Admin;

        public object Execute(Models.Message msg)
        {
            
            var messageArray = msg.body.Split(' ');

            if (messageArray[1] == "список") return listBugs();
            if(messageArray[1] == "инфо") return InfoBug(messageArray[2]);

            var idBug = messageArray[1];
            var status = messageArray[2];
            
            var bug = new Api.Bugs(Convert.ToInt64(idBug));

            bug.Status = Convert.ToInt64(status);
            Api.Message.Send($"✨ У Вашего бага с ID {idBug} изменился статус на {status}" +
                             $"\n❓ Возможные статусы: 0 - не прочитано, 1 - прочитано, 2 - работа над исправлением, 3 - исправлено в обновлении, 4 - исправлено, 5 - не баг, 6 - бесмысленное сообщение", bug.User);
            return $"Багу с ID {idBug} присвоен статус: {status}";
        }

        public static string listBugs()
        {         
            var bugs = Api.Bugs.AllList;
            string s = String.Empty;

            foreach (var bugId in bugs)
            {
                var bug = new Api.Bugs(bugId);
                s += $"\nID: {bug.Id}" +
                     $"\nТекст: {bug.Text}" +
                     $"\nСтатус: {bug.Status}" +
                     $"\nПользователь: *id{bug.User}" +
                     $"\nВремя: {bug.Time}" +
                     $"\n";
            }
            return s;
        }

        public static string InfoBug(string id)
        {
            var bug = new Api.Bugs(Convert.ToInt64(id));
           var s = $"\nID: {bug.Id}" +
                 $"\nТекст: {bug.Text}" +
                 $"\nСтатус: {bug.Status}" +
                 $"\nПользователь: *id{bug.User}" +
                 $"\nВремя: {bug.Time}" +
                 $"\n";

            return s;
        }
    }
}