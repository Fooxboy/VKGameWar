using VkNet.Model.RequestParams;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Команда, которая срабатывает, когда не единой команды не выполнено.
    /// </summary>
    public class NoCommand
    {
        public static void Execute(LongPollVK.Models.AddNewMsg msg)
        {
            //Нихуя не делать.
        }
    }
}
