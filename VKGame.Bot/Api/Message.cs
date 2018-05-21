using System;
using VkNet.Model.RequestParams;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Раздел с сообщенияеми
    /// </summary>
    public static class Message
    {
        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="peerId">Пир ид</param>
        /// <returns></returns>
        public static long Send(string text, long peerId)
        {		
            try
            {
                if (text == null) text = "❌ Команда ничего не вернула. Попробуйте ещё раз.";
                var vk = Common.GetVk();
                var idMessage = vk.Messages.Send(new MessagesSendParams()
                {
                    UserId = peerId,
                    Message = text
                });
                return idMessage;
            }catch(Exception e)
            {
                Logger.WriteError(e);
                Bot.Statistics.NewError();
                return 0;
            }
           
        }
        
        /// <summary>
        /// Редактировать сообщение
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="messageId">ид сообщения</param>
        /// <param name="peerId">пир ид</param>
        /// <returns></returns>
        public static bool Edit(string text, long messageId, long peerId)
        {
            var vk = Common.GetVk();

            try
            {
                var result = vk.Messages.Edit(new MessageEditParams
                {
                    Message = text,
                    PeerId = peerId,
                    MessageId = messageId,
                    KeepForwardMessages = true,
                    KeepSnippets = true
                });
                return result;
            }
            catch(Exception e)
            {
                Logger.WriteError(e);
                Bot.Statistics.NewError();
                return false;
            }         
        }
    }
}