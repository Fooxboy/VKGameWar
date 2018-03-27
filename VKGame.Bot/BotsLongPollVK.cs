using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                        Logger.WriteDebug("Получение Key, Server, Ts...");
                        var modelKeyAndTs = GetKeyAndTs();
                        var response = modelKeyAndTs.response;
                        Key = response.key;
                        Ts = response.ts;
                        Server = response.server;
                    }

                    var json = Request();
                    Logger.WriteDebug(json);
                    Models.RootBotsLongPollVK responseLongPoll = null;
                    try
                    {
                        responseLongPoll = JsonConvert.DeserializeObject<Models.RootBotsLongPollVK>(json);
                        Ts = responseLongPoll.ts;
                    }
                    catch
                    {
                        Logger.WriteDebug("Получение Key, Server, Ts...");
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
                        Logger.WriteDebug("Получение Key, Server, Ts...");
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
                            var model = (Models.UserJoin)update.@object;
                            UserJoinEvent?.Invoke(model);
                        }
                        else if (type == "group_leave")
                        {
                            var model = (Models.UserLeave)update.@object;
                            UserLeaveEvent?.Invoke(model);
                        }
                        else
                        {

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
            try
            {
                var url = $"{Server}?act=a_check&key={Key}&ts={Ts}&wait=25";
                var json = String.Empty;
                using (var web = new WebClient())
                {
                    json = web.DownloadString(url);
                }
                return json;
            }
            catch(WebException e)
            {
                Logger.WriteError(e);
                Statistics.NewError();
                Thread.Sleep(5000);
                 return Request();
            }       
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
