using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class LongPoll
    {
        public ulong Ts = 0;

        public delegate void NewMessageDelegate(Models.Message message);
        public event NewMessageDelegate NewMessageEvent;

        public delegate void ErrorDelegate(Exception e);
        public event ErrorDelegate ErrorEvent;

        public delegate void DebugDelegate(string text);
        public event DebugDelegate DebugEvent;

        public void Start()
        {
            while(true)
            {
                try
                {
                    DebugEvent?.Invoke("Выполнение кода лонг пулла..");
                    if (Ts == 0)
                    {
                        DebugEvent?.Invoke("ts = 0..");

                        var modelSub = Api.Messages.SubscribeOnNewMessages();
                        if (modelSub.Ok == false)
                        {
                            throw new Exception($"Код: {modelSub.Error.Code} Сообщение: {modelSub.Error.Message}");
                        }

                        DebugEvent?.Invoke("Присваивание тс..");

                        var ts = modelSub.Data;
                        Ts = ts;
                        DebugEvent?.Invoke($"ts = {Ts}..");

                    }

                    DebugEvent?.Invoke("Получение сообщений..");

                    var messageUpdates = Api.Messages.GetNow(Ts);
                    if(messageUpdates.Ok == false)
                    {
                        DebugEvent?.Invoke("Ошибка..");

                        throw new Exception($"Код: {messageUpdates.Error.Code} Сообщение: {messageUpdates.Error.Message}");
                    }

                    var messages = messageUpdates.Data.Updates;
                    DebugEvent?.Invoke("Пересчёт сообщений..");

                    if (messages.Count != 0)
                    {
                        foreach(var message in messages)
                        {
                            NewMessageEvent?.Invoke(message);
                        }

                    }else
                    {
                        DebugEvent?.Invoke("Новых Сообщений 0..");

                    }

                    DebugEvent?.Invoke("Записывание тс..");

                    Ts = messageUpdates.Data.Ts;

                }
                catch(Exception e)
                {
                    ErrorEvent?.Invoke(e);
                }
            }
        }
    }
}
