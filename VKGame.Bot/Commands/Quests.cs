using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Quests :ICommand
    {
        public string Name => "Квесты";
        public string Caption => "Здесь можно получить специальные задания!";
        public string Arguments => "(), (Вариант выбора)";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            return null;
        }

        public static string Start(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            var choise = "";
            try
            {
                choise = messageArray[2];
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали номер квеста!";
            }

            return null;
        }

        private string GetQuestsText(LongPollVK.Models.AddNewMsg msg)
        {
       
            string text = $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                        $"\n🔎 КВЕСТЫ" +
                        $"\n❓ Здесь хранятся специальные задания, за которые можно получить неплохую сумму!" +
                        $"\n" +
                        $"\nСПИСОК КВЕСТОВ➖➖➖➖➖➖➖➖➖➖" +
                        $"\n▶ 1 - Накопить 10 побед за день." +
                        $"\n▶❓ Для того, чтобы начать выполнять, напишите: Квест старт 1." +
                        $"\n▶💰 Вознаграждение: ";
            return text;
        }
    }
}
