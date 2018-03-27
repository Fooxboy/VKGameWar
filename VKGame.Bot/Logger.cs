using System;
using System.Data;

namespace VKGame.Bot
{
    /// <summary>
    /// Класс для работы с логами.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Вывод предупреждений на консоль.
        /// </summary>
        /// <param name="Сообщение"></param>
        public static void WriteWaring(object message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            var time = DateTime.Now;
            Console.WriteLine($"[{time}]=> {message}");
            Console.ResetColor();
        }

        public static void NewMessage(object message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            var time = DateTime.Now;
            Console.WriteLine($"[{time}]=> {message}");
            Console.ResetColor();
        }
        
        /// <summary>
        /// Вывод ошибок на консоль.
        /// </summary>
        /// <param name="Сообщение"></param>
        public static void WriteError(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var time = DateTime.Now;
            Console.WriteLine($"[{time}]=>" +
                $"\n >>ИСКЛЮЧЕНИЕ: {e.GetType().Name}" +
                $"\n >>СООБЩЕНИЕ: {e.Message}" +
                $"\n >>STACK TRACE: {e.StackTrace}");
            Console.ResetColor();
        }
        
        /// <summary>
        /// Дебаг-собщение
        /// </summary>
        /// <param name="Сообщение"></param>
        public static void WriteDebug(object message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            var time = DateTime.Now;
            Console.WriteLine($"[{time}]=> {message}");
            Console.ResetColor();
        }
    }
}