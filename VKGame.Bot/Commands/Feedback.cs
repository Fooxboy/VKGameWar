using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Feedback :ICommand
    {
        public string Name => "Отзыв";
        public string Caption => "Раздел для написании отзывов и предложений по боту.";
        public string Arguments => "(Ваш отзыв или предложение)";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>();
        public Access Access => Access.User;

        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray[1] == "+список")
            {
                var user = Api.User.GetUser(msg.from_id);
                if (user.Access < 3) return "Вам недоступна команда просмотра отзывов.";
                var feedbacks = Api.Feedback.GetFeedback();
                int count = Int32.Parse(messageArray[2]);

                string feedbackText = String.Empty;

                for (int i = 0; i < feedbacks.Feedback.Count; i++)
                {
                    feedbackText += $"🆔 *id{feedbacks.Feedback[i].UserId}" +
                        $"\n⏰ {feedbacks.Feedback[i].Time}" +
                        $"\n🔥 {feedbacks.Feedback[i].Text}" +
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

                var feedbackModel = new Models.FeedBack() { Time = DateTime.Now.ToString(), Text = feedbackText, UserId = msg.from_id };
                var feedbacks = Api.Feedback.GetFeedback();
                feedbacks.Feedback.Add(feedbackModel);
                Api.Feedback.SetFeedback(feedbacks);

                //TODO: заменить
                Api.MessageSend("🎈 Добавлен новый отзыв!", 308764786);

                return "😎 Ваш отзыв будет обязательно прочтён!";
            }
        }
    }
}
