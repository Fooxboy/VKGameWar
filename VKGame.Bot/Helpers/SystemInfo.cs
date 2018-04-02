using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;


namespace VKGame.Bot.Helpers
{
    public class SystemInfo
    {
        public static OperatingSystem OS => Environment.OSVersion;
        public static string NameComputer => Environment.MachineName;
        public static int CountCoreProcessor => Environment.ProcessorCount;
        public static string StackTrace => Environment.StackTrace;
        public static int RAM => Environment.SystemPageSize;
        public static int UpdateSystemTime => Environment.TickCount;
        public static long RAMProcess => Environment.WorkingSet;
        public static ProcessThreadCollection Threads => Process.GetCurrentProcess().Threads;
        public static TimeSpan CPUProcess => Process.GetCurrentProcess().TotalProcessorTime;
    }
}
