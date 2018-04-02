using JetBrains.Annotations;
using VkNet.Model.RequestParams;
using System.Collections.Generic;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Класс для работы с вводимыми данными.
    /// </summary>
    public class WriteData:ICommand
    {
        public string Name => "!";
        public string Caption => "Используется для ввода данных. Например, при смени имени.";
        public string Arguments => "(данные)";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>();
        public Access Access => Access.User;


        [NotNull]
        public object Execute(Models.Message msg)
        {
            string text = "";
            string[] arrayText = msg.body.Split(' ');
            for (int i = 1; arrayText.Length > i; i++) text += $"{arrayText[i]} ";
            
            //var common = new Common();

            var vk = Common.GetVk();
            var historyMessage = vk.Messages.GetHistory(new MessagesGetHistoryParams()
            {
                Count = 10,
                PeerId = msg.from_id
            });

            foreach (var message in historyMessage.Messages)
            {
                var messageCommand = message.Body.Split(' ')[0].ToLower();
                Logger.WriteDebug(messageCommand);
                if (messageCommand == "старт") return Start.SetNick(msg, text);
            }

            return "❌" + " Не найдена команда, для которой нужны данные.";
        }
    }
}