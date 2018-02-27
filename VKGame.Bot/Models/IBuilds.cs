using System;

namespace VKGame.Bot.Models
{
    public interface IBuilds
    {
        /// <summary>
        /// ид
        /// </summary>
        long Id {get;}
        /// <summary>
        /// Жилые квартиры.
        /// </summary>
        long Apartments {get;set;}
        /// <summary>
        /// Генераторы электроэнергии.
        /// </summary>
        long PowerGenerators {get;set;}
        /// <summary>
        /// Шахты добычи нефти.
        /// </summary>
        long Mine {get;set;}
        /// <summary>
        /// Водонапорные башни.
        /// </summary>
         long WaterPressureStation {get;set;}
       /// <summary>
        /// Закусочные.
        /// </summary>
        long Eatery {get;set;}
         /// <summary>
        /// Хранилище энергии
        /// </summary>

        long WarehouseEnergy {get;set;}
         /// <summary>
        /// Хранилище воды
        /// </summary>

        long WarehouseWater {get;set;}
         /// <summary>
        /// Хранилиже еды
        /// </summary>

        long WarehouseEat {get;set;}
         /// <summary>
        /// Ангары
        /// </summary>
        long Hangars {get;set;}
    }
}