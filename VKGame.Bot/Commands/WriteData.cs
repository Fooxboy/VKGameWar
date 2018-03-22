using JetBrains.Annotations;
using VkNet.Model.RequestParams;

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

        [NotNull]
        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            string text = "";
            string[] arrayText = msg.Text.Split(' ');
            for (int i = 1; arrayText.Length > i; i++) text += $"{arrayText[i]} ";
            
            var common = new Common();

            var vk = common.GetVk();
            var historyMessage = vk.Messages.GetHistory(new MessagesGetHistoryParams()
            {
                Count = 10,
                PeerId = msg.PeerId
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