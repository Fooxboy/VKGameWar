using System.Threading;

namespace VKGame.Bot.BackgroundProcess
{
    /// <summary>
    /// Фоновые процессы раздела армия.
    /// </summary>
    public class Army
    {
        /// <summary>
        /// Обучение солдат.
        /// </summary>
        /// <param name="ид пользователя"></param>
        public static void CreateSoldiery(object objectData)
        {
            var data = (Models.DataCreateSoldiery)objectData;
            Logger.WriteDebug(data.Count);
            var userId = data.UserId;
            var count = data.Count;
            var resources = new Api.Resources(userId);

            while (count > 0)
            {
                if (count < 0) break;       
                Thread.Sleep(20000);
                var soldiery = resources.Soldiery;
                soldiery++;
                resources.Soldiery = soldiery;
                count -= 1;
            }
            Api.MessageSend("✔ Солдаты были обучены. Вы можете идти в бой!", userId);
        }


        /// <summary>
        /// Создание танков.
        /// </summary>
        public static void CreateTanks(object datas) 
        {
            Logger.WriteDebug("create");
            
            var data = (Models.DataCreateSoldiery) datas;
            var userId = data.UserId;
            var count = data.Count;
            var resources = new Api.Resources(userId);

            while (count > 0)
            {                
                if (count < 0) break;
                Thread.Sleep(60000);
                var tanks = resources.Tanks;
                tanks++;
                resources.Tanks = tanks;
                count -= 1;
            }

            Api.MessageSend("✔ Танки были сделаны. Вы можете идти в бой!", userId);
        }
    }
}