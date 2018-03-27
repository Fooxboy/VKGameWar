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
                if (ts == Convert.ToUInt64(Bot.Common.LastMessage)) Thread.Sleep(10000);
                var modelMessages = new NowMessagesResponse { Updates = new List<MessageResponse>() };
                var listMessages = Api.CacheMessages.GetList();
                var messageNotWatch = listMessages.Message.Where(message => Convert.ToUInt64(message.Id) > ts);
                foreach (Models.MessageCache message in messageNotWatch)
                {
                    var modelMessage = new MessageResponse
                    {
                        Id = message.Id,
                        Text = message.Text,
                        Time = message.Time,
                        PeerId = message.PeerId
                    };
                    modelMessages.Updates.Add(modelMessage);
                    ts = Convert.ToUInt64(message.Id);
                }

                modelMessages.Ts = ts;
                response.Data = modelMessages;
                response.Ok = true;
                return JsonConvert.SerializeObject(response);

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
