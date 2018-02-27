using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace VKGame.Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.WriteWaring("Старт бота...");
            Console.Title = "MyGameVK.";
            Logger.WriteWaring("Создание экземпляра лонгпулла.");
            var starter = new LongPollVK.StarterLongPoll();
            Logger.WriteWaring("Создан.");
            const string token = "типа че за хуйня";
            Logger.WriteWaring("Создание потока LongPoll.");
            Thread threadLongPoll = new Thread(new ParameterizedThreadStart(starter.Start));
            threadLongPoll.Name = "LongPoll";
            threadLongPoll.Start(token);
            Logger.WriteWaring("Поток запущен.");
            starter.AddNewMsgEvent += Core.NewMessage;
            var argumentsArg = Console.ReadLine();
        }
    }
}
