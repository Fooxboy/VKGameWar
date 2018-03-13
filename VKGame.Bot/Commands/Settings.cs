using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Settings: ICommand
    {
        public string Name => "Настройки";
        public string Arguments => "(), (Вариант_выбора)";
        public string Caption => "Здесь можно настроить все под себя!";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            return "Настроки пока что недоступны. :(";
        }
    }
}
