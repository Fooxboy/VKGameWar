using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VKGame.Bot
{
    public class LongPollVK
    {
        #region Модели
        public class Models
        {
            /// <summary>
            /// Главная модель longPoll.
            /// </summary>
            public struct Root
            {
                /// <summary>
                /// тс
                /// </summary>
                public string Ts { get; set; }
                /// <summary>
                /// Массив с обновлениями.
                /// </summary>
                public List<List<object>> Updates { get; set; }
            }
            /// <summary>
            /// Класс для получения Сервера лонг пулла.
            /// </summary>
            public struct LongPollServer
            {
                /// <summary>
                /// Ответ от вк
                /// </summary>
                public struct response
                {
                    public ulong ts { get; set; }
                    public string key { get; set; }
                    public string pts { get; set; }
                    public string Server { get; set; }
                }
            }

            /// <summary>
            /// Модель обработки ошибки.
            /// </summary>
            public struct Error
            {
                /// <summary>
                /// Код ошибки
                /// </summary>
                public int failed { get; set; }
                /// <summary>
                /// тс
                /// </summary>
                public ulong ts { get; set; }
                /// <summary>
                /// Минимальная версия
                /// </summary>
                public int min_version { get; set; }
                /// <summary>
                /// Максимальная версия
                /// </summary>
                public int max_version { get; set; }
            }

            /// <summary>
            /// Класс, от которого наследуются сообщения с доп. полями.
            /// </summary>
            public class IExtraFields
            {
                /// <summary>
                /// Индефикатор беседы
                /// </summary>
                public long PeerId { get; set; }
                /// <summary>
                /// Время
                /// </summary>
                public string Time { get; set; }
                /// <summary>
                /// Текст
                /// </summary>
                public string Text { get; set; }
                /// <summary>
                /// Вложения
                /// </summary>
                public Attach Attach { get; set; }
                /// <summary>
                /// Рандом ид
                /// </summary>
                public long RandomId { get; set; }
                /// <summary>
                /// Тип входящего сообщения.
                /// </summary>
                public TypeMsg Type { get; set; }
                /// <summary>
                /// Id юсера от которого поступило сообщение.
                /// </summary>
                public long From { get; set; }
            }

            /// <summary>
            /// Тип сообщения.
            /// </summary>
            public enum TypeMsg
            {
                /// <summary>
                /// Личный диалог.
                /// </summary>
                Dialog = 1,
                /// <summary>
                /// Группавая беседа.
                /// </summary>
                Chat = 2
            }

            /// <summary>
            /// Замена флагов сообщения (FLAGS:=$flags). 
            /// </summary>
            public class ReplaceMsgFlag : IExtraFields
            {
                /// <summary>
                /// Индефикатор сообщения
                /// </summary>
                public ulong MessageId { get; set; }
                /// <summary>
                /// Флаги
                /// </summary>
                public long Flags { get; set; }
            }

            /// <summary>
            /// Установка флагов сообщения (FLAGS|=$mask). 
            /// </summary>
            public class InstallMsgFlag : IExtraFields
            {
                /// <summary>
                /// Индефикатор сообщения
                /// </summary>
                public ulong MessageId { get; set; }
                /// <summary>
                /// Маска
                /// </summary>
                public long Mask { get; set; }
            }

            /// <summary>
            /// Сброс флагов сообщения (FLAGS&=~$mask). 
            /// </summary>
            public class ResetMsgFlag : IExtraFields
            {
                /// <summary>
                /// Индефикатор сообщения
                /// </summary>
                public ulong MessageId { get; set; }
                /// <summary>
                /// Маска
                /// </summary>
                public long Mask { get; set; }
            }

            /// <summary>
            /// Добавление нового сообщения. 
            /// </summary>
            public class AddNewMsg : IExtraFields
            {
                /// <summary>
                /// Индефикатор сообщения
                /// </summary>
                public ulong MessageId { get; set; }
                /// <summary>
                /// Флаги
                /// </summary>
                public long Flags { get; set; }

            }

            /// <summary>
            /// Редактирование сообщения. 
            /// </summary>
            public struct EditMsg
            {
                /// <summary>
                /// Индефикатор сообщения
                /// </summary>
                public ulong MessageId { get; set; }
                /// <summary>
                /// Маска
                /// </summary>
                public long Mask { get; set; }
                /// <summary>
                /// Индефикатор беседы
                /// </summary>
                public long PeerId { get; set; }
                /// <summary>
                /// Время
                /// </summary>
                public string Time { get; set; }
                /// <summary>
                /// Изменённый текст.
                /// </summary>
                public string NewText { get; set; }
                /// <summary>
                /// Вложения
                /// </summary>
                public Attach Attach { get; set; }
            }

            /// <summary>
            /// Прочтение всех входящих сообщений в $peer_id, пришедших до сообщения с $local_id. 
            /// </summary>
            public struct ReadAllInMsg
            {
                /// <summary>
                /// Индефикатор беседы
                /// </summary>
                public long PeerId { get; set; }
                /// <summary>
                /// Id сообщения до которого выполнись определённое действие.
                /// </summary>
                public long LocalId { get; set; }
            }

            /// <summary>
            /// Прочтение всех исходящих сообщений в $peer_id, пришедших до сообщения с $local_id. 
            /// </summary>
            public struct ReadAllOutMsg
            {
                /// <summary>
                /// /// <summary>
                /// Индефикатор беседы
                /// </summary>
                /// </summary>
                public long PeerId { get; set; }
                /// <summary>
                /// /// <summary>
                /// Id сообщения до которого выполнись определённое действие.
                /// </summary>
                /// </summary>
                public long LocalId { get; set; }
            }

            /// <summary>
            /// Друг $user_id стал онлайн. $extra не равен 0, если в mode был передан флаг 64. В младшем байте (остаток от деления на 256) числа extra лежит идентификатор платформы (см. 7. Платформы). $timestamp — время последнего действия пользователя $user_id на сайте.
            /// </summary>
            public struct FriendOnline
            {
                /// <summary>
                /// Id пользователя
                /// </summary>
                public long UserId { get; set; }
                /// <summary>
                /// Платформа с которой он зашёл
                /// </summary>
                public long Extra { get; set; }
                /// <summary>
                /// Время
                /// </summary>
                public string Time { get; set; }
            }

            /// <summary>
            /// Друг $user_id стал оффлайн ($flags равен 0, если пользователь покинул сайт и 1, если оффлайн по таймауту ) . $timestamp — время последнего действия пользователя $user_id на сайте. 
            /// </summary>
            public struct FriendOffline
            {
                /// <summary>
                /// Id пользователя
                /// </summary>
                public long UserId { get; set; }
                /// <summary>
                /// Флаги сообщений
                /// </summary>
                public long Flags { get; set; }
                /// <summary>
                /// Время
                /// </summary>
                public string Time { get; set; }
            }

            /// <summary>
            /// Сброс флагов диалога $peer_id. Соответствует операции (PEER_FLAGS &= ~$flags). Только для диалогов сообществ. 
            /// </summary>
            public struct ResetDialogFlag
            {
                /// <summary>
                /// Id диалога.
                /// </summary>
                public long PeerId { get; set; }
                /// <summary>
                /// Маска
                /// </summary>
                public long Mask { get; set; }
            }

            /// <summary>
            /// Замена флагов диалога $peer_id. Соответствует операции (PEER_FLAGS:= $flags). Только для диалогов сообществ. 
            /// </summary>
            public struct ReplaceDialogFlag
            {
                /// <summary>
                /// Id диалога
                /// </summary>
                public long PeerId { get; set; }
                /// <summary>
                /// Флаги сообщений
                /// </summary>
                public long Flags { get; set; }
            }

            /// <summary>
            /// Установка флагов диалога $peer_id. Соответствует операции (PEER_FLAGS|= $flags). Только для диалогов сообществ. 
            /// </summary>
            public struct InstallDialogFlag
            {
                /// <summary>
                /// Id диалога
                /// </summary>
                public long PeerId { get; set; }
                /// <summary>
                /// Маска
                /// </summary>
                public long Mask { get; set; }
            }

            /// <summary>
            /// Удаление всех сообщений в диалоге $peer_id с идентификаторами вплоть до $local_id. 
            /// </summary>
            public struct DeletingsAllMsg
            {
                /// <summary>
                /// Id диалога
                /// </summary>
                public long PeerId { get; set; }
                /// <summary>
                /// Id сообщения до которого выполнись определённое действие.
                /// </summary>
                public long LocalId { get; set; }
            }

            /// <summary>
            /// Один из параметров (состав, тема) беседы $chat_id были изменены. $self — 1 или 0 (вызваны ли изменения самим пользователем). 
            /// </summary>
            public struct OneParamChanged
            {
                /// <summary>
                /// Id чата
                /// </summary>
                public long ChatId { get; set; }
                /// <summary>
                /// Вызваны ли изменения самим пользователем
                /// </summary>
                public long Self { get; set; }
            }

            /// <summary>
            /// Пользователь $user_id набирает текст в диалоге. Событие приходит раз в ~5 секунд при наборе текста. $flags = 1. 
            /// </summary>
            public struct UserTypingInDialog
            {
                /// <summary>
                /// Id пользователя
                /// </summary>
                public long UserId { get; set; }
                /// <summary>
                /// Флаги
                /// </summary>
                public long Flags { get; set; }
            }

            /// <summary>
            /// Пользователь $user_id набирает текст в беседе $chat_id. 
            /// </summary>
            public struct UserTypingInChat
            {
                /// <summary>
                /// Id пользователя
                /// </summary>
                public long UserId { get; set; }
                /// <summary>
                /// Id чата
                /// </summary>
                public long ChatId { get; set; }
            }

            /// <summary>
            /// Пользователь $user_id совершил звонок с идентификатором $call_id. 
            /// </summary>
            public struct UserCompletedCall
            {
                /// <summary>
                /// Id пользователя
                /// </summary>
                public long UserId { get; set; }
                /// <summary>
                /// Id звонка
                /// </summary>
                public long CallId { get; set; }
            }

            /// <summary>
            /// Счетчик в левом меню стал равен $count.
            /// </summary>
            public struct UnReadMessageCounter
            {
                /// <summary>
                /// Количество
                /// </summary>
                public long Count { get; set; }
            }

            /// <summary>
            /// Изменились настройки оповещений. $peer_id — идентификатор чата/собеседника, '$sound — 1/0, включены/выключены звуковые оповещения, $disabled_until — выключение оповещений на необходимый срок (-1: навсегда, ''0 
            /// </summary>
            public struct NotificationSettingsChanged
            {
                /// <summary>
                /// Id диалога
                /// </summary>
                public long PeerId { get; set; }
                /// <summary>
                /// Звук
                /// </summary>
                public int Sound { get; set; }
                /// <summary>
                /// Отключено навсегда
                /// </summary>
                public int DisabledUntil { get; set; }
            }

            /// <summary>
            /// Вложения
            /// </summary>
            public class Attach
            {
                /// <summary>
                /// Тип вложения 1
                /// </summary>
                public string attach1_type { get; set; }
                /// <summary>
                /// Тип вложения 2
                /// </summary>
                public string attach2_type { get; set; }
                /// <summary>
                /// Тип вложения 3
                /// </summary>
                public string attach3_type { get; set; }
                /// <summary>
                /// Тип вложения 4
                /// </summary>
                public string attach4_type { get; set; }
                /// <summary>
                /// Тип вложения 5
                /// </summary>
                public string attach5_type { get; set; }
                /// <summary>
                /// Тип вложения 6
                /// </summary>
                public string attach6_type { get; set; }
                /// <summary>
                /// Тип вложения 7
                /// </summary>
                public string attach7_type { get; set; }
                /// <summary>
                /// Тип вложения 8
                /// </summary>
                public string attach8_type { get; set; }
                /// <summary>
                /// Тип вложения 9
                /// </summary>
                public string attach9_type { get; set; }
                /// <summary>
                /// Тип вложения 10
                /// </summary>
                public string attach10_type { get; set; }
                /// <summary>
                /// Вложение 1
                /// </summary>
                public string attach1 { get; set; }
                /// <summary>
                /// Вложение 2
                /// </summary>
                public string attach2 { get; set; }
                /// <summary>
                /// Вложение 3
                /// </summary>
                public string attach3 { get; set; }
                /// <summary>
                /// Вложение 4
                /// </summary>
                public string attach4 { get; set; }
                /// <summary>
                /// Вложение 5
                /// </summary>
                public string attach5 { get; set; }
                /// <summary>
                /// Вложение 6
                /// </summary>
                public string attach6 { get; set; }
                /// <summary>
                /// Вложение 7
                /// </summary>
                public string attach7 { get; set; }
                /// <summary>
                /// Вложение 8
                /// </summary>
                public string attach8 { get; set; }
                /// <summary>
                /// Вложение 9
                /// </summary>
                public string attach9 { get; set; }
                /// <summary>
                /// Вложение 10
                /// </summary>
                public string attach10 { get; set; }
                /// <summary>
                /// Пересланные сообщения
                /// </summary>
                public string Fwr { get; set; }
                /// <summary>
                /// Id пользователя от которого поступило сообщение (только для групповых бесед)
                /// </summary>
                public string From { get; set; }
                /// <summary>
                /// Геотег
                /// </summary>
                public string Geo { get; set; }
                /// <summary>
                /// Гео провайдер
                /// </summary>
                public string GeoProvider { get; set; }
                /// <summary>
                /// Заголовок
                /// </summary>
                public string Title { get; set; }
                /// <summary>
                /// Вложение продукт 1
                /// </summary>
                public string attach1_product_id { get; set; }
                /// <summary>
                /// Вложение фото
                /// </summary>
                public string attach1_photo { get; set; }
                /// <summary>
                /// Вложение заголовок
                /// </summary>
                public string attach1_title { get; set; }
                /// <summary>
                /// Вложение ???
                /// </summary>
                public string attach1_desc { get; set; }
                /// <summary>
                /// Вложение юрл
                /// </summary>
                public string attach1_url { get; set; }
                //public string attach1_url {get;set;}
                /// <summary>
                /// Есть ли в сообщении емоджи
                /// </summary>
                public string Emoji { get; set; }
                /// <summary>
                /// идентификатор администратора или редактора, отправившего сообщение. Возвращается для сообщений, отправленных от имени сообщества (только для администраторов сообщества). 
                /// </summary>
                public string FromAdmin { get; set; }
                /// <summary>
                /// название сервисного действия с мультидиалогом. 
                /// </summary>
                public string SourceAct { get; set; }
                /// <summary>
                /// идентификатор пользователя, к которому относится сервисное действие
                /// </summary>
                public string SourseMid { get; set; }
            }
        }
        #endregion

        #region Ядро
        /// <summary>
        /// Ядро ЛонгПулла.
        /// </summary>
        public class Core
        {
            private string _token = null;

            public string Token
            {
                get => _token;
                set => _token = value;
            }

            /// <summary>
            /// Получение json ответа от сервера ВКонтакте.
            /// </summary>
            /// <param name="server">url сервера</param>
            /// <param name="key">значение параметра key</param>
            /// <param name="ts">значение параметра ts</param>
            /// <returns>Возращает json ответ сервера ВКонтакте.</returns>
            public string GetJson(string server, string key, ulong ts)
            {
                string json = null;
                using (var client = new WebClient())
                {
                    string url = $"https://{server}?act=a_check&key={key}&ts={ts}&wait=25&mode=2&version=2";
                    try 
                    {
                        json = client.DownloadString(url);
                    }catch(Exception e) 
                    {
                        Logger.WriteError($"{e.Message} \n {e.StackTrace}");
                        Statistics.NewError();
                        json = "[61,123456,1]";
                    }
                }
                return json;
            }

            /// <summary>
            /// Получение Ts, Server, Key.
            /// </summary>
            /// <returns>Объект с етими данными.</returns>
            public VkNet.Model.LongPollServerResponse GetTsAndServer()
            {
                var commondata = new Common();
                var vk = commondata.GetVk();
                var result = vk.Messages.GetLongPollServer();
                return result;
            }
        }
        #endregion

        #region Стартер
        /// <summary>
        /// Стартер лонг пулла.
        /// </summary>
        public class StarterLongPoll
        {
            public delegate void ReplaceMsgFlagDelegate([NotNull] Models.ReplaceMsgFlag message);
            /// <summary>
            /// Замена флагов сообщения (FLAGS:=$flags). 
            /// </summary>
            public event ReplaceMsgFlagDelegate ReplaceMsgFlagEvent;

            public delegate void InstallMsgFlagDelegate(Models.InstallMsgFlag message);
            /// <summary>
            /// Установка флагов сообщения (FLAGS|=$mask). 
            /// </summary>
            public event InstallMsgFlagDelegate InstallMsgFlagEvent;

            public delegate void ResetMsgFlagDelegate(Models.ResetMsgFlag message);
            /// <summary>
            /// Сброс флагов сообщения (FLAGS&=~$mask). 
            /// </summary>
            public event ResetMsgFlagDelegate ResetMsgFlagEvent;

            public delegate void AddNewMsgDelegate(Models.AddNewMsg message);
            /// <summary>
            /// Добавление нового сообщения. 
            /// </summary>
            public event AddNewMsgDelegate AddNewMsgEvent;

            public delegate void EditMsgDelegate(Models.EditMsg message);
            /// <summary>
            /// Редактирование сообщения. 
            /// </summary>
            public event EditMsgDelegate EditMsgEvent;

            public delegate void ReadAllInMsgDelegate(Models.ReadAllInMsg message);
            /// <summary>
            /// Прочтение всех входящих сообщений в $peer_id, пришедших до сообщения с $local_id. 
            /// </summary>
            public event ReadAllInMsgDelegate ReadAllInMsgEvent;

            public delegate void ReadAllOutMsgDelegate(Models.ReadAllOutMsg message);
            /// <summary>
            /// Прочтение всех исходящих сообщений в $peer_id, пришедших до сообщения с $local_id. 
            /// </summary>
            public event ReadAllOutMsgDelegate ReadAllOutMsgEvent;

            public delegate void FriendOnlineDelegate(Models.FriendOnline message);
            /// <summary>
            /// Друг $user_id стал онлайн. $extra не равен 0, если в mode был передан флаг 64. В младшем байте (остаток от деления на 256) числа extra лежит идентификатор платформы (см. 7. Платформы). $timestamp — время последнего действия пользователя $user_id на сайте.
            /// </summary>
            public event FriendOnlineDelegate FriendOnlineEvent;

            public delegate void FriendOfflineDelegate(Models.FriendOffline message);
            /// <summary>
            /// Друг $user_id стал оффлайн ($flags равен 0, если пользователь покинул сайт и 1, если оффлайн по таймауту ) . $timestamp — время последнего действия пользователя $user_id на сайте.
            /// </summary>
            public event FriendOfflineDelegate FriendOfflineEvent;

            public delegate void ResetDialogFlagDelegate(Models.ResetDialogFlag message);
            /// <summary>
            /// Сброс флагов диалога $peer_id. Соответствует операции (PEER_FLAGS &= ~$flags). Только для диалогов сообществ. 
            /// </summary>
            public event ResetDialogFlagDelegate ResetDialogFlagEvent;

            public delegate void ReplaceDialogFlagDelegate(Models.ReplaceDialogFlag message);
            /// <summary>
            /// Замена флагов диалога $peer_id. Соответствует операции (PEER_FLAGS:= $flags). Только для диалогов сообществ. 
            /// </summary>
            public event ReplaceDialogFlagDelegate ReplaceDialogFlagEvent;

            public delegate void InstallDialogFlagDelegate(Models.InstallDialogFlag message);
            /// <summary>
            /// Установка флагов диалога $peer_id. Соответствует операции (PEER_FLAGS|= $flags). Только для диалогов сообществ. 
            /// </summary>
            public event InstallDialogFlagDelegate InstallDialogFlagEvent;

            public delegate void DeletingsAllMsgDelegate(Models.DeletingsAllMsg message);
            /// <summary>
            /// Удаление всех сообщений в диалоге $peer_id с идентификаторами вплоть до $local_id. 
            /// </summary>
            public event DeletingsAllMsgDelegate DeletingsAllMsgEvent;

            public delegate void OneParamChangedDelegate(Models.OneParamChanged message);
            /// <summary>
            /// Один из параметров (состав, тема) беседы $chat_id были изменены. $self — 1 или 0 (вызваны ли изменения самим пользователем). 
            /// </summary>
            public event OneParamChangedDelegate OneParamChangedEvent;

            public delegate void UserTypingInDialogDelegate(Models.UserTypingInDialog message);
            /// <summary>
            /// Пользователь $user_id набирает текст в диалоге. Событие приходит раз в ~5 секунд при наборе текста. $flags = 1
            /// </summary>
            public event UserTypingInDialogDelegate UserTypingInDialogEvent;

            public delegate void UserTypingInChatDelegate(Models.UserTypingInChat message);
            /// <summary>
            /// Пользователь $user_id набирает текст в беседе $chat_id. 
            /// </summary>
            public event UserTypingInChatDelegate UserTypingInChatEvent;

            public delegate void UserCompletedCallDelegate(Models.UserCompletedCall message);
            /// <summary>
            /// Пользователь $user_id совершил звонок с идентификатором $call_id. 
            /// </summary>
            public event UserCompletedCallDelegate UserCompletedCallEvent;

            public delegate void UnReadMessageCounterDelegate(Models.UnReadMessageCounter message);
            /// <summary>
            /// Счетчик в левом меню стал равен $count. 
            /// </summary>
            public event UnReadMessageCounterDelegate UnReadMessageCounterEvent;

            public delegate void NotificationSettingsChangedDelegate(Models.NotificationSettingsChanged message);
            /// <summary>
            /// Изменились настройки оповещений. $peer_id — идентификатор чата/собеседника, '$sound — 1/0, включены/выключены звуковые оповещения, $disabled_until — выключение оповещений на необходимый срок (-1: навсегда, ''0 
            /// </summary>
            public event NotificationSettingsChangedDelegate NotificationSettingsChangedEvent;

            public delegate void NothingHappenedDelegate();
            /// <summary>
            /// Ничего не произошло.
            /// </summary>
            public event NothingHappenedDelegate NothingHappenedEvent;

            bool valueRepeat = true;
            private string Server = null;
            private ulong Ts = 0;
            private string Key = null;
            private ulong? Pts = null;
            /// <summary>
            /// Старт longPoll.
            /// </summary>
            /// <param name="objToken"></param>
            public void Start(object objToken)
            {             
                var common = new Common();
                string token = common.GetToken();
                var core = new Core();
                core.Token = token;
                while (valueRepeat)
                {
                    try 
                    {
                        if (Server == null || Key == null)
                        {
                        var model = core.GetTsAndServer();
                        Server = model.Server;
                        Ts = model.Ts;
                        Key = model.Key;
                        Pts = model.Pts;
                        }
                        var json = core.GetJson(Server, Key, Ts);
                        var rootLongPoll = JsonConvert.DeserializeObject<Models.Root>(json);

                        //Обработка ошибок лонг пулла.
                        if (rootLongPoll.Ts == null)
                        {
                            var errorLongPoll = JsonConvert.DeserializeObject<Models.Error>(json);
                            if (errorLongPoll.failed == 2 || errorLongPoll.failed == 3)
                            {
                                var model = core.GetTsAndServer();
                                Server = model.Server;
                                Ts = model.Ts;
                                Key = model.Key;
                                Pts = model.Pts;
                            }
                            //else if (errorLongPoll.failed == 2) Ts = errorLongPoll.ts;
                            else if (errorLongPoll.failed == 4) throw new Exception("Мин и макс версия.");
                        }

                        //заного задаём значение переменной.
                        rootLongPoll = JsonConvert.DeserializeObject<Models.Root>(json);
                        Ts = UInt64.Parse(rootLongPoll.Ts);
                        var updates = rootLongPoll.Updates;

                        //Проверяем кол-во обновлений.
                        if (updates.Count != 0)
                        {
                            //перебор всех обновлений
                            foreach (var update in updates)
                            {
                                //код обновления
                                long code = (long)update[0];

                                if (code == 1) //Замена флагов сообщения (FLAGS:=$flags). 
                                {
                                    var model = new Models.ReplaceMsgFlag();
                                    model.MessageId = System.Convert.ToUInt64((long)update[1]);
                                    model.Flags = (long)update[2];
                                    model.PeerId = (long)update[3];
                                    model.Time = Convert.ToString((long)update[4]);
                                    model.Text = (string)update[5];
                                    var type_attach = (JObject)update[6];
                                    model.Attach = type_attach.ToObject<Models.Attach>();
                                    if (model.Attach.From == null)
                                    {
                                        model.From = model.PeerId;
                                        model.Type = Models.TypeMsg.Dialog;
                                    }
                                    else
                                    {
                                        model.From = Int64.Parse(model.Attach.From);
                                        model.Type = Models.TypeMsg.Chat;
                                    }

                                ReplaceMsgFlagEvent?.Invoke(model);
                            }
                            else if (code == 2) //Установка флагов сообщения (FLAGS|=$mask). 
                            {
                                var model = new Models.InstallMsgFlag();
                                model.MessageId = Convert.ToUInt64((long) update[1]);
                                model.Mask = (long)update[2];
                                model.PeerId = (long)update[3];
                                model.Time = Convert.ToString((long)update[4]);
                                model.Text = (string)update[5];
                                var type_attach = (JObject)update[6];
                                model.Attach = type_attach.ToObject<Models.Attach>();
                                if (model.Attach.From == null)
                                {
                                    model.From = model.PeerId;
                                    model.Type = Models.TypeMsg.Dialog;
                                }
                                else
                                {
                                    model.From = Int64.Parse(model.Attach.From);
                                    model.Type = Models.TypeMsg.Chat;
                                }
                                InstallMsgFlagEvent?.Invoke(model);
                            }
                            else if (code == 3) //Сброс флагов сообщения (FLAGS&=~$mask). 
                            {
                                var model = new Models.ResetMsgFlag();
                                model.MessageId = Convert.ToUInt64((long) update[1]);
                                model.Mask = (long)update[2];
                                model.PeerId = (long)update[3];
                                model.Time = Convert.ToString((long)update[4]);
                                model.Text = (string)update[5];
                                var type_attach = (JObject)update[6];
                                model.Attach = type_attach.ToObject<Models.Attach>();
                                if (model.Attach.From == null)
                                {
                                    model.From = model.PeerId;
                                    model.Type = Models.TypeMsg.Dialog;
                                }
                                else
                                {
                                    model.From = Int64.Parse(model.Attach.From);
                                    model.Type = Models.TypeMsg.Chat;
                                }
                                ResetMsgFlagEvent?.Invoke(model);
                            }
                            else if (code == 4) //Добавление нового сообщения. 
                            {
                                var model = new Models.AddNewMsg();
                                model.MessageId = Convert.ToUInt64((long) update[1]);
                                model.Flags = (long)update[2];
                                model.PeerId = (long)update[3];
                                model.Time = Convert.ToString((long)update[4]);
                                model.Text = (string)update[5];
                                var type_attach = (JObject)update[6];
                                if(type_attach != null)
                                {
                                    model.Attach = type_attach.ToObject<Models.Attach>();
                                    if (model.Attach.From == null)
                                    {
                                        model.From = model.PeerId;
                                        model.Type = Models.TypeMsg.Dialog;
                                    }
                                    else
                                    {
                                        model.From = Int64.Parse(model.Attach.From);
                                        model.Type = Models.TypeMsg.Chat;
                                    }
                                } 
                                AddNewMsgEvent?.Invoke(model);
                            }
                            else if (code == 5) //Редактирование сообщения. 
                            {
                                var model = new Models.EditMsg();
                                model.MessageId = Convert.ToUInt64((long) update[1]);
                                model.Mask = (long)update[2];
                                model.PeerId = (long)update[3];
                                model.Time = Convert.ToString((long)update[4]);
                                model.NewText = (string)update[5];
                                var type_attach = (JObject)update[6];
                                model.Attach = type_attach.ToObject<Models.Attach>();
                                EditMsgEvent?.Invoke(model);
                            }
                            else if (code == 6) //Прочтение всех входящих сообщений в $peer_id, пришедших до сообщения с $local_id. 
                            {
                                var model = new Models.ReadAllInMsg();
                                model.PeerId = (long)update[1];
                                model.LocalId = (long)update[2];
                                ReadAllInMsgEvent?.Invoke(model);
                            }
                            else if (code == 7) //Прочтение всех исходящих сообщений в $peer_id, пришедших до сообщения с $local_id.
                            {
                                var model = new Models.ReadAllOutMsg();
                                model.PeerId = (long)update[1];
                                model.LocalId = (long)update[2];
                                ReadAllOutMsgEvent?.Invoke(model);
                            }
                            else if (code == 8) //Друг $user_id стал онлайн. $extra не равен 0, если в mode был передан флаг 64. В младшем байте (остаток от деления на 256) числа extra лежит идентификатор платформы (см. 7. Платформы). $timestamp — время последнего действия пользователя $user_id на сайте. 
                            {
                                var model = new Models.FriendOnline();
                                model.UserId = (long)update[1];
                                model.Extra = (long)update[2];
                                model.Time = Convert.ToString((long)update[3]);
                                FriendOnlineEvent?.Invoke(model);

                            }
                            else if (code == 9) //Друг $user_id стал оффлайн ($flags равен 0, если пользователь покинул сайт и 1, если оффлайн по таймауту ) . $timestamp — время последнего действия пользователя $user_id на сайте. 
                            {
                                var model = new Models.FriendOffline();
                                model.UserId = (long)update[1];
                                model.Time = Convert.ToString((long)update[3]);
                                FriendOfflineEvent?.Invoke(model);
                            }
                            else if (code == 10) //Сброс флагов диалога $peer_id. Соответствует операции (PEER_FLAGS &= ~$flags). Только для диалогов сообществ.
                            {
                                var model = new Models.ResetDialogFlag();
                                model.PeerId = (long)update[1];
                                model.Mask = (long)update[2];
                                ResetDialogFlagEvent?.Invoke(model);

                            }
                            else if (code == 11) //Замена флагов диалога $peer_id. Соответствует операции (PEER_FLAGS:= $flags). Только для диалогов сообществ. 
                            {
                                var model = new Models.ReplaceDialogFlag();
                                model.PeerId = (long)update[1];
                                model.Flags = (long)update[2];
                                ReplaceDialogFlagEvent?.Invoke(model);
                            }
                            else if (code == 12) //Установка флагов диалога $peer_id. Соответствует операции (PEER_FLAGS|= $flags). Только для диалогов сообществ. 
                            {
                                var model = new Models.InstallDialogFlag();
                                model.PeerId = (long)update[1];
                                model.Mask = (long)update[2];
                                InstallDialogFlagEvent?.Invoke(model);
                            }
                            else if (code == 13) //Удаление всех сообщений в диалоге $peer_id с идентификаторами вплоть до $local_id.
                            {
                                var model = new Models.DeletingsAllMsg();
                                model.PeerId = (long)update[1];
                                model.LocalId = (long)update[2];
                                DeletingsAllMsgEvent?.Invoke(model);
                            }
                            else if (code == 14) //Восстановление недавно удаленных сообщений в диалоге $peer_id с идентификаторами вплоть до $local_id.
                            {
                                //TODO: сделать модель
                            }
                            else if (code == 51) //Один из параметров (состав, тема) беседы $chat_id были изменены. $self — 1 или 0 (вызваны ли изменения самим пользователем). 
                            {
                                var model = new Models.OneParamChanged();
                                model.ChatId = (long)update[1];
                                model.Self = (long)update[2];
                                OneParamChangedEvent?.Invoke(model);
                            }
                            else if (code == 61) //Пользователь $user_id набирает текст в диалоге. Событие приходит раз в ~5 секунд при наборе текста. $flags = 1. 
                            {
                                var model = new Models.UserTypingInDialog();
                                model.UserId = (long)update[1];
                                model.Flags = (long)update[2];
                                UserTypingInDialogEvent?.Invoke(model);
                            }
                            else if (code == 62) //Пользователь $user_id набирает текст в беседе $chat_id. 
                            {
                                var model = new Models.UserTypingInChat();
                                model.UserId = (long)update[1];
                                model.ChatId = (long)update[2];
                                UserTypingInChatEvent?.Invoke(model);
                            }
                            else if (code == 70) //Пользователь $user_id совершил звонок с идентификатором $call_id. 
                            {
                                var model = new Models.UserCompletedCall();
                                model.UserId = (long)update[1];
                                model.CallId = (long)update[2];
                                UserCompletedCallEvent?.Invoke(model);
                            }
                            else if (code == 80) //Счетчик в левом меню стал равен $count.
                            {
                                var model = new Models.UnReadMessageCounter();
                                model.Count = (long)update[1];
                                UnReadMessageCounterEvent?.Invoke(model);
                            }
                            else if (code == 114) //зменились настройки оповещений. $peer_id — идентификатор чата/собеседника, '$sound — 1/0, включены/выключены звуковые оповещения, $disabled_until — выключение оповещений на необходимый срок (-1: навсегда, ''0 
                            {
                                var model = new Models.NotificationSettingsChanged();
                                model.PeerId = (long)update[1];
                                model.Sound = Convert.ToInt32((long)update[2]);
                                model.DisabledUntil = Convert.ToInt32((long)update[3]);
                                NotificationSettingsChangedEvent?.Invoke(model);
                            }
                            else //неизвестный код обновления
                            {
                                NothingHappenedEvent?.Invoke();
                            }
                        }
                    }
                    else NothingHappenedEvent?.Invoke();
                    }catch (Exception e)
                    {
                        Bot.Statistics.NewError();
                        Logger.WriteError($"{e.Message} \n {e.StackTrace}");
                    }
                    
                }
            }
        }
        #endregion
    }
}
