using System.Threading;

namespace VKGame.Bot.BackgroundProcess
{
    public class Buildings 
    {
        public static void AddingResources(object obj) 
        {
            long id = (long)obj;
            

            while(true) 
            {
                var resources = new Api.Resources(id);
                var builds = new Api.Builds(id);
                //Logger.WriteDebug("Выполнение фонового процесса дабовления.");
                var energy = resources.Energy;
                var eat = resources.Food;
                var water = resources.Water;

                //Пиздец говнокод, но почему-то по другому не работает :/
                if(energy < builds.WarehouseEnergy * 100)
                {
                    var temp = builds.PowerGenerators * 10;
                    energy = energy + temp;
                } 
                if(eat < builds.WarehouseEat * 100) 
                {
                    var temp = builds.Eatery * 5;
                    eat =  eat + temp;
                }

                if(water < builds.WarehouseWater * 100) 
                {
                    var temp = builds.WaterPressureStation * 10;
                    water = + temp;
                } 
                
                resources.Energy = energy;
                resources.Food = eat;
                resources.Water = water;
                Thread.Sleep(60000);
            }
        } 
    }
   
}