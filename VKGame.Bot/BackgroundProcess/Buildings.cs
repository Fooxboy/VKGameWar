using System.Threading;

namespace VKGame.Bot.BackgroundProcess
{
    public class Buildings 
    {
        public static void AddingResources(object obj) 
        {
            long id = 12345;
            var resources = new Api.Resources(id);
            var builds = new Api.Builds(id);

            while(true) 
            {
                var energy = resources.Energy;
                var eat = resources.Food;
                var water = resources.Water;

                energy =+ (builds.PowerGenerators * 10);
                eat =+ (builds.Eatery *5);
                water =+ (builds.WaterPressureStation *10);

                resources.Energy = energy;
                resources.Food = eat;
                resources.Water = water;
                Thread.Sleep(60000);
            }
        } 
    }
   
}