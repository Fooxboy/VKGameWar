using VkNet.Model.RequestParams;
using System.Collections.Generic;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Команда, которая срабатывает, когда не единой команды не выполнено.
    /// </summary>
    public class NoCommand
    {
        public static void Execute(Models.Message msg)
        {
            try
            {
                var lastcommand = Common.LastCommand;
                var command = lastcommand[msg.from_id];
                var response = command.Execute(msg);
                Api.MessageSend((string)response, msg.from_id);
            }catch(KeyNotFoundException)
            {
                Api.MessageSend("Неизвестная команда!", msg.from_id);

            }
        }
    }
}
