using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;

namespace VKGame.Bot
{
    public class BotsLongPollVK
    {
        private string Server = String.Empty;
        private string Key = String.Empty;
        private long Ts = 0;
        private string Token = String.Empty;


        public delegate void NewMessageHeadler(Models.Message message);
        public event NewMessageHeadler NewMesageEvent;

        public delegate void UserJoinHeadler(Models.UserJoin model);
        public event UserJoinHeadler UserJoinEvent;

        public delegate void UserLeaveHeadler(Models.UserLeave model);
        public event UserLeaveHeadler UserLeaveEvent;

        public void Start(object token)
        {
            while (true)
            {
                try
                {
                    if (Token == String.Empty) Token = (string)token;

                    if (Server == String.Empty || Key == String.Empty || Ts == 0)
                    {
                        //Logger.WriteDebug("Получение Key, Server, Ts...");
                        var modelKeyAndTs = GetKeyAndTs();
                        var response = modelKeyAndTs.response;
                        Key = response.key;
                        Ts = response.ts;
                        Server = response.server;
                    }

                    var json = Request();

                    if(json != null)
                    {
                        //Logger.WriteDebug(json);
                        Models.RootBotsLongPollVK responseLongPoll = null;
                        try
                        {
                            responseLongPoll = JsonConvert.DeserializeObject<Models.RootBotsLongPollVK>(json);
                            Ts = responseLongPoll.ts;
                        }
                        catch
                        {
                            //Logger.WriteDebug("Получение Key, Server, Ts...");
                            var modelKeyAndTs = GetKeyAndTs();
                            var response = modelKeyAndTs.response;
                            Key = response.key;
                            Ts = response.ts;
                            Server = response.server;
                            var newJson = Request();
                            responseLongPoll = JsonConvert.DeserializeObject<Models.RootBotsLongPollVK>(newJson);
                        }

                        if (responseLongPoll.updates == null)
                        {
                            //Logger.WriteDebug("Получение Key, Server, Ts...");
                            var modelKeyAndTs = GetKeyAndTs();
                            var response = modelKeyAndTs.response;
                            Key = response.key;
                            Ts = response.ts;
                            Server = response.server;
                            var newJson = Request();
                            responseLongPoll = JsonConvert.DeserializeObject<Models.RootBotsLongPollVK>(newJson);
                        }

                        var updates = responseLongPoll.updates;
                        foreach (var update in updates)
                        {
                            var type = update.type;
                            if (type == "message_new")
                            {
                                var jobj = (JObject)update.@object;
                                var model = jobj.ToObject<Models.Message>();
                                model.from_id = model.user_id;
                                NewMesageEvent?.Invoke(model);
                            }
                            else if (type == "group_join")
                            {
                                var jobj = (JObject)update.@object;
                                var model = jobj.ToObject<Models.UserJoin>();
                                UserJoinEvent?.Invoke(model);
                            }
                            else if (type == "group_leave")
                            {
                                var jobj = (JObject)update.@object;
                                var model = jobj.ToObject<Models.UserLeave>();
                                UserLeaveEvent?.Invoke(model);
                            }
                        }
                    }    
                }
                catch (Exception e)
                {
                    Statistics.NewError();
                    Logger.WriteError(e);
                }
            }    
        }

        private string Request()
        {
            var url = $"{Server}?act=a_check&key={Key}&ts={Ts}&wait=10";
            var json = String.Empty;
            var request = HttpWebRequest.Create(url);
            request.Timeout = 15000;
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                }
            }
            return json;
        }

        private Models.TsAndKey.ResponseModel GetKeyAndTs()
        {
            var json = String.Empty;
            using(var web = new WebClient())
            {
                json = web.DownloadString($"https://api.vk.com/method/groups.getLongPollServer?group_id=161965172&access_token={Token}&v=5.73");
            }

            var model = JsonConvert.DeserializeObject<Models.TsAndKey.ResponseModel>(json);
            return model;
        }
    }
}
