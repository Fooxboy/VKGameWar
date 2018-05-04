using System;
using System.Collections.Generic;
using VKGame.Bot.Models;

namespace VKGame.Bot.Commands
{
    public class Skills: ICommand
    {
        public override string Name => "Навыки";
        public override string Arguments => "(), (Вариант_выбора)";
        public override string Caption => "Здесь Вы сможете улучшть свои навыки и посмотреть их список.";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() {"улучшить"};
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";


        public override object Execute(Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetSkilsText(msg.from_id);          
            var type = typeof(Skills);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }
        
        [Attributes.Trigger("улучшить")]
        public static string UpgrateSkill(Message msg)
        {
            var messageArray = msg.body.Split(' ');

            var typeSkill = String.Empty;

            try
            {
                typeSkill = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали какой навык Вы хотите улучшить! Доступные навыки: Удача";
            }
            
            
            var skills = new Api.Skills(msg.from_id);
            if (typeSkill.ToLower() == "удача")
            {

                if (!Notifications.RemovePaymentCard(200, msg.from_id, "улучшение навыка"))
                    return "❌ У Вас недостаточно монет на балансе. Необходимо: 200 💳";
                skills.Fortuna = ++skills.Fortuna;
            }else if (typeSkill == "лохъ")
            {
                
            }
            else
            {
                return "❌ Вы указали несуществующий навык. Доступные значения: удача ";
            }
            
            return "✅ Вы успешно улучшили свой навык! ";
        }

        private static string GetSkilsText(long userId)
        {
            var skills = new Api.Skills(userId);
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                   $"\n ВАШИ НАВЫКИ" +
                   $"\n" +
                   $"\n🍀 Удача - {skills.Fortuna} " +
                   $"\n" +
                   $"\n" +
                   $"\n❓ Чтобы улучшить наши навыки напишите: навыки улучшить название_навыка" +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }    
    }
}