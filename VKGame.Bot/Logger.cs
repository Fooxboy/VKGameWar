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
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                var time = DateTime.Now;
                Console.WriteLine($"({time}) [waring]=> {message}");
                Console.ResetColor();
            }catch(Exception e)
            {
                WriteError(e);
            }
            
        }

        public static void NewMessage(object message)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                var time = DateTime.Now;
                Console.WriteLine($"({time}) [message]=> {message}");
                Console.ResetColor();
            }catch(Exception e)
            {
                WriteError(e);
            }      
        }
        
        /// <summary>
        /// Вывод ошибок на консоль.
        /// </summary>
        /// <param name="Сообщение"></param>
        public static void WriteError(Exception e)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                var time = DateTime.Now;
                Console.WriteLine($"({time}) [error]=>" +
                    $"\n >>ИСКЛЮЧЕНИЕ: {e.GetType().Name}" +
                    $"\n >>СООБЩЕНИЕ: {e.Message}" +
                    $"\n >>STACK TRACE: {e.StackTrace}");
                Console.ResetColor();
                Api.Errors.Add(e);
            }catch(Exception)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    var time = DateTime.Now;
                    Console.WriteLine($"({time}) [error]=>" +
                        $"\n >>ИСКЛЮЧЕНИЕ: {e.GetType().Name}" +
                        $"\n >>СООБЩЕНИЕ: {e.Message}" +
                        $"\n >>STACK TRACE: {e.StackTrace}");
                    Console.ResetColor();
                }
                catch (Exception)
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        var time = DateTime.Now;
                        Console.WriteLine($"({time}) [error]=>" +
                            $"\n >>ИСКЛЮЧЕНИЕ: {e.GetType().Name}" +
                            $"\n >>СООБЩЕНИЕ: {e.Message}" +
                            $"\n >>STACK TRACE: {e.StackTrace}");
                        Console.ResetColor();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            

        }

        /// <summary>
        /// Дебаг-собщение
        /// </summary>
        /// <param name="Сообщение"></param>
        public static void WriteDebug(object message)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                var time = DateTime.Now;
                Console.WriteLine($"({time}) [debug]=> {message}");
                Console.ResetColor();
            }catch(Exception e)
            {
                WriteError(e);
            }          
        }
    }
}