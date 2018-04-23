using System;
using System.Collections.Generic;
using System.Text;
using VKGame.Bot.Helpers;
using System.Diagnostics;

namespace VKGame.Bot.Commands.Admin
{
    public class System : ICommand
    {
        public override string Name => "Система";
        public override string Caption => "аа";
        public override string Arguments => "";
        public override List<string> Commands => new List<string>();
        public override TypeResponse Type => TypeResponse.Text;
        public override Access Access => Access.Admin;

        public override object Execute(Models.Message msg)
        {

            string text = $"==ИНФОРМАЦИЯ==" +
                        $"\n🛡 ОС: {SystemInfo.OS}" +
                        $"\n🎈 Имя компьютера: {SystemInfo.NameComputer}" +
                        $"\n💣 Количество ядер процессора: {SystemInfo.CountCoreProcessor}" +
                       // $"\n😨 Оперативная память(SystemPageSize): {SystemInfo.RAM/1024} ГБ." +
                        $"\n🔥 Оперативная память процесса: {SystemInfo.RAMProcess/1024/1024} МБ." +
                        $"\n✔ UpdateSystemTime: {SystemInfo.UpdateSystemTime}" +
                        $"\n⚔ Количество работающих потоков: {SystemInfo.Threads.Count}";
            return text;
        }
    }
}
