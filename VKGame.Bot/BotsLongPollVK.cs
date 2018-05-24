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
    /// <summary>
    /// Лонг пулл ботов ВКонтакте.
    /// </summary>
    public class BotsLongPollVK
    {
        //Основные переменные
        private string Server = String.Empty;
        private string Key = String.Empty;
        //161965172
        private string IdGroup = string.Empty;
        private long Ts = 0;
        private string Token = String.Empty;
        public delegate void NewMessageHeadler(Models.Message message);
        public event NewMessageHeadler NewMesageEvent;
        public delegate void UserJoinHeadler(Models.UserJoin model);
        public event UserJoinHeadler UserJoinEvent;
        public delegate void UserLeaveHeadler(Models.UserLeave model);
        public event UserLeaveHeadler UserLeaveEvent;

        /// <summary>
        /// Метод начала прослушивания новых событий
        /// </summary>
        /// <param name="token">Токен сообщества</param>
        public void Start(object token)
        {
            while (true)
            {
                try
                {
                    //Проверка на тест мод
                    if (!Common.IsTestingMode)
                        IdGroup = "161965172";
                    else
                        IdGroup = "166120379";

                    //проверка на наличие данных в переменных.
                    //Используется для того, чтобы каждый раз не получать новые
                    if (Token == String.Empty) Token = (string)token;
                    if (Server == String.Empty || Key == String.Empty || Ts == 0)
                    {
                        var modelKeyAndTs = GetKeyAndTs();
                        var response = modelKeyAndTs.response;
                        Key = response.key;
                        Ts = response.ts;
                        Server = response.server;
                    }

                    //Получение json результата от сервера ВКонтакте
                    var json = Request();

                    //если json пустой - ничего не делать
                    if(json != null)
                    {
                        //десериализация json....
                        Models.RootBotsLongPollVK responseLongPoll = null;
                        try
                        {
                            responseLongPoll = JsonConvert.DeserializeObject<Models.RootBotsLongPollVK>(json);
                            Ts = responseLongPoll.ts;
                        }
                        catch
                        {
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
                            //проверка типов полученных событй
                            var type = update.type;
                            if (type == "message_new")
                            {
                                //новое сообщение
                                var jobj = (JObject)update.@object;
                                var model = jobj.ToObject<Models.Message>();
                                model.from_id = model.user_id;
                                NewMesageEvent?.Invoke(model);
                            }
                            else if (type == "group_join")
                            {
                                //вступление в группу
                                var jobj = (JObject)update.@object;
                                var model = jobj.ToObject<Models.UserJoin>();
                                UserJoinEvent?.Invoke(model);
                            }
                            else if (type == "group_leave")
                            {
                                //уход из группы
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

        /// <summary>
        /// Выполняет запрос к серверу лонг пулла ВКонтакте
        /// </summary>
        /// <returns> json результат</returns>
        private string Request()
        {
            var url = $"{Server}?act=a_check&key={Key}&ts={Ts}&wait=20";
            var json = String.Empty;
            var request = HttpWebRequest.Create(url);
            request.Timeout = 30000;
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

        /// <summary>
        /// Метод получения Key и Ts
        /// </summary>
        /// <returns> модель TsAndKey </returns>
        private Models.TsAndKey.ResponseModel GetKeyAndTs()
        {
            var json = String.Empty;
            using(var web = new WebClient())
            {
                json = web.DownloadString($"https://api.vk.com/method/groups.getLongPollServer?group_id={IdGroup}&access_token={Token}&v=5.73");
            }
            var model = JsonConvert.DeserializeObject<Models.TsAndKey.ResponseModel>(json);
            return model;
        }
    }
}
