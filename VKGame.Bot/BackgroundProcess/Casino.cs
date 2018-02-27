using System.IO;
using System.Threading;

namespace VKGame.Bot.BackgroundProcess
{
    /// <summary>
    /// Фоновые процессы для Казино.
    /// </summary>
    public class Casino
    {
        /// <summary>
        /// Вызывает метод конца игры.
        /// </summary>
        public static void TimerTriggerEndGame()
        {
            while (true)
            {
                if(File.Exists(@"Files\Tickets.json")) 
                    new Timer(new TimerCallback(Commands.Casino.EndGameTicket), null, 0, 300000);      
                else Thread.Sleep(60000);   
            }
        }
    }
}