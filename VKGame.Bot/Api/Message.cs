using System;
using VkNet.Model.RequestParams;

namespace VKGame.Bot.Api
{
    public static class Message
    {
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