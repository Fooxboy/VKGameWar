using System.Threading;
using System;
using System.Linq;
using VKGame.Bot.Helpers;

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
            bool boost = false;
            Api.Resources resources = null;
            try
            {
                 userId = data.UserId;
                 count = data.Count;
                boost = data.Boost;
                 resources = new Api.Resources(userId);
            }catch(Exception e)
            {
                Logger.WriteError(e);
                Bot.Statistics.NewError();
            }
            var builds = new Api.Builds(userId);

            Bot.Statistics.CreateSol(count);
            if(boost)
                Api.Message.Send($"➡ Солдаты будут обучаться:  {(count * 20)/2} секунд", userId);
            else
            {
                Api.Message.Send($"➡ Солдаты будут обучаться:  {count * 20} секунд", userId);

            }

            var turn = Bot.Common.TurnCreateSoildery;

            while (count > 0)
            {
                try
                {
                    if (turn.All(u => u.Id != userId))
                        turn.Add(new Models.UserTurnCreate() { Id = userId, Count = count });
                    else turn.Find(u => u.Id == userId).Count = count;  
                    if (count < 0) break;

                    int time = 20000;

                    if (boost) time = time / 2;
                    
                    Thread.Sleep(time);
                    var soldiery = resources.Soldiery;
                    soldiery++;
                    resources.Soldiery = soldiery;
                    if (resources.Soldiery > Commands.Buildings.Api.MaxSoldiery(builds.Apartments)) resources.Soldiery = Commands.Buildings.Api.MaxSoldiery(builds.Apartments);
                    --count;
                }catch(Exception e)
                {
                    Logger.WriteError(e);
                    Bot.Statistics.NewError();
                }
                
            }
            var user = turn.Find(u => u.Id == userId);
            turn.Remove(user);
            Api.Message.Send($"✅ Солдаты были обучены. Вы можете идти в бой! ", userId);
        }


        /// <summary>
        /// Создание танков.
        /// </summary>
        public static void CreateTanks(object datas) 
        {
            var data = (Models.DataCreateSoldiery)datas;
            long userId=0;
            int count=0;
            bool boost = false;
            Api.Resources resources = null;
            try
            {
                 userId = data.UserId;
                 count = data.Count;
                boost = data.Boost;
                 resources = new Api.Resources(userId);
                
            }catch(Exception e)
            {
                Logger.WriteError(e);
                Bot.Statistics.NewError();
            }
            Bot.Statistics.CreateTanks(count);
            var builds = new Api.Builds(userId);

            if (boost)
            {
                count = count * 60000;
                count = count / 2;
                count = count / 1000;
                Api.Message.Send($"➡ Танки будут создаваться:  {count} секунд", userId);
            }    
            else 
                Api.Message.Send($"➡ Танки будут создаваться:  {count} минут", userId);

            var turn = Bot.Common.TurnCreateTanks;

            while (count > 0)
            {             
                try
                {
                    if (turn.All(u => u.Id != userId))
                        turn.Add(new Models.UserTurnCreate() { Id = userId, Count = count });
                    else turn.Find(u => u.Id == userId).Count = count;
                    if (count < 0) break;
                    var time = 60000;
                    if(boost) time = time / 2;
                    Thread.Sleep(60000);
                    var tanks = resources.Tanks;
                    tanks++;
                    resources.Tanks = tanks;
                    if (resources.Tanks > Commands.Buildings.Api.MaxTanks(builds.Hangars)) resources.Tanks = Commands.Buildings.Api.MaxTanks(builds.Hangars);
                    --count;
                }
                catch (Exception e)
                {
                    Logger.WriteError(e);
                    Bot.Statistics.NewError();
                }
            }

            var user = turn.Find(u => u.Id == userId);
            turn.Remove(user);
            Api.Message.Send("✅ Танки были сделаны. Вы можете идти в бой!", userId);
        }
    }
}