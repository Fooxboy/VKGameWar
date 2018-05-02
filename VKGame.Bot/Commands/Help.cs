using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Help : ICommand
    {
        public override string Name => "Помощь";
        public override string Arguments => "()";
        public override string Caption => "Здесь можно узнать, где и как получить помощь об разделах.";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() { "купить" };
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";

        public override object Execute(Models.Message msg)
        {
            string text = "😁 Привет! Сейчас я тебе помогу..." +
                "\n❓ Чтобы узнать список всех доступных разделах, напиши: Разделы." +
                "\n❓ Чтобы получить помощь об выбранном разделе, напиши: <название_раздела> помощь" +
                "\n👀 Если возникли ещё какие-либо вопросы, поищи ответы на них в нашей группе ВКонтакте :)";
            return text;
        }
    }
}
