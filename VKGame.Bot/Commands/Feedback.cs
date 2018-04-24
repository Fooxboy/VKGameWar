using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Feedback :ICommand
    {
        public override string Name => "Отзыв";
        public override string Caption => "Раздел для написании отзывов и предложений по боту.";
        public override string Arguments => "(Ваш отзыв или предложение)";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>();
        public override Access Access => Access.User;

        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');

            if (messageArray.Length == 1) return NoFeedBack();

            if (messageArray[1] == "+список")
            {
                var user = new Api.User(msg.from_id);
                if (user.Access < 3) return "Вам недоступна команда просмотра отзывов.";
                var feedbacks = Api.Feedbacks.Alllist;
                //int count = Int32.Parse(messageArray[2]);
                
                string feedbackText = String.Empty;

                foreach (var feedbackId in feedbacks)
                {
                    var feedback = new Api.Feedbacks(feedbackId);
                    feedbackText += $"🆔 *id{feedback.User}" +
                                    $"\n⏰ {feedback.Time}" +
                                    $"\n🔥 {feedback.Text}" +
                                    $"\n" +
                                    $"\n";
                }

               
                return feedbackText;
            }
            else
            {
                string feedbackText = String.Empty;
                for(int i =1; i < messageArray.Length; i++)
                {
                    feedbackText += $"{messageArray[i]} ";
                }

                var id = Api.Feedbacks.Add(feedbackText, msg.from_id);

                //TODO: заменить
                Api.Message.Send($"🎈 Добавлен новый отзыв - ID = {id}!", 308764786);

                return "😎 Ваш отзыв будет обязательно прочтён!";
            }
        }

        private string NoFeedBack()
        {
            return "😀 Здесь можно написать свой отызыв о боте и он будет отправлен администрации! Использование: Отзыв <Ваш любой отзыв>";

        } 
    }
}
