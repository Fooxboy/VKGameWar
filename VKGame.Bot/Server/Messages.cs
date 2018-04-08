using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Linq;

namespace VKGame.Bot.Server
{
    public class Messages
    {
        public static string GetNow(ulong ts)
        {
            var response = new Common.Response<NowMessagesResponse>();

            try
            {

                if (ts != (ulong) Bot.Common.LastMessage)
                {
                }

                throw new Exception();

            }catch(Exception e)
            {
                Logger.WriteError(e);
                response.Ok = false;
                response.Error = new Error() { Code = 5, Message = $"Внутренняя ошибка сервера: {e.Message}" };
                return JsonConvert.SerializeObject(response);
            }
        }

        public static string SubscribeOnNewMessages()
        {
            var response = new Common.Response<ulong>();

            try
            {
                var ts = Bot.Common.LastMessage;
                var model = ts;
                response.Data = Convert.ToUInt64(model);
                response.Ok = true;
            }catch(Exception e)
            {
                Logger.WriteError(e);
                response.Ok = false;
                response.Error = new Error() { Code = 5, Message = $"Внутренняя ошибка сервера: {e.Message}" };
            }

            return JsonConvert.SerializeObject(response);
        }

        public class NowMessagesResponse
        {
            public List<MessageResponse> Updates { get; set; }
            public ulong Ts { get; set; }
        }

        public class MessageResponse: Models.MessageCache
        {

        }

        public class SubscribeOnNewMessageResponse
        {
            public ulong Ts { get; set; }

        }
    }
}
