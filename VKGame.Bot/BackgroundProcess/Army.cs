﻿using System.Threading;
using System;

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

            long userId = 0;
            int count = 0;
            Api.Resources resources = null;
            try
            {
                 userId = data.UserId;
                 count = data.Count;
                 resources = new Api.Resources(userId);
            }catch(Exception e)
            {
                Logger.WriteError($"{e.Message} \n {e.StackTrace}");

            }


            while (count > 0)
            {
                try
                {
                    if (count < 0) break;
                    Thread.Sleep(20000);
                    var soldiery = resources.Soldiery;
                    soldiery++;
                    resources.Soldiery = soldiery;
                    count -= 1;
                }catch(Exception e)
                {
                    Logger.WriteError($"{e.Message} \n {e.StackTrace}");
                }
                
            }
            Api.MessageSend("✔ Солдаты были обучены. Вы можете идти в бой!", userId);
        }


        /// <summary>
        /// Создание танков.
        /// </summary>
        public static void CreateTanks(object datas) 
        {
            var data = (Models.DataCreateSoldiery)datas;
            long userId=0;
            int count=0;
            Api.Resources resources = null;
            try
            {
                 userId = data.UserId;
                 count = data.Count;
                 resources = new Api.Resources(userId);
            }catch(Exception e)
            {
                Logger.WriteError($"{e.Message} \n {e.StackTrace}");

            }


            while (count > 0)
            {             
                try
                {
                    if (count < 0) break;
                    Thread.Sleep(60000);
                    var tanks = resources.Tanks;
                    tanks++;
                    resources.Tanks = tanks;
                    count -= 1;
                }catch(Exception e)
                {
                    Logger.WriteError($"{e.Message} \n {e.StackTrace}");

                }

            }

            Api.MessageSend("✔ Танки были сделаны. Вы можете идти в бой!", userId);
        }
    }
}