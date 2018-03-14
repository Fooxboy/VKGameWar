using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class Statistics
    {
        /// <summary>
        /// Принято сообщений за день.
        /// </summary>
        public long InMessageDay { get; set; }

        /// <summary>
        /// Отправлено сообщений за день.
        /// </summary>
        public long OutMessageDay { get; set; }

        /// <summary>
        /// Всего сообщений
        /// </summary>
        public long AllMessages { get; set; }

        /// <summary>
        /// Ошибки
        /// </summary>
        public ErrorsModel Errors { get; set; }

        /// <summary>
        /// Бои
        /// </summary>
        public  BattlesModel Battles {get;set;}

        /// <summary>
        /// Регистрации
        /// </summary>
        public RegistrationsModel Registrations { get; set; }

        /// <summary>
        /// Выграли в казино монет.
        /// </summary>
        public WinCasinoModel WinCasino { get; set; }

        /// <summary>
        /// Создано армии.
        /// </summary>
        public CreateArmyModel CreateArmy { get; set; }

        /// <summary>
        /// Получено боксов
        /// </summary>
        public BoxsModel Boxs { get; set; }

        /// <summary>
        /// Создано кланов.
        /// </summary>
        public CreateClansModel CreateClans { get; set; }

        /// <summary>
        /// Активировано промокодов.
        /// </summary>
        public long PromocodesAll { get; set; }

        /// <summary>
        /// Сумма, на которую взяли кредитов.
        /// </summary>
        public long KreditsAll { get; set; }

        /// <summary>
        /// Сегодня переходили на домашний екран.
        /// </summary>
        public long GoHomeDay { get; set; }

        /// <summary>
        /// Соревнования
        /// </summary>
        public CompetitionsModel Competitions { get; set; }

        /// <summary>
        /// Пользователи, которые присоединились по реферальной ссылке.
        /// </summary>
        public long RefferalAll { get; set; }

        /// <summary>
        /// Выграли монет в битвах всего
        /// </summary>
        public long WinBattleAll { get; set; }

        /// <summary>
        /// Выграли монет в битвах сегодня
        /// </summary>
        public long WinBattleDay { get; set; }




        public class CompetitionsModel
        {
            public long JoinPeopleAll { get; set; }
            public long JoinPeopleDay { get; set; }

            public long All { get; set; }

            public long BattleCompetitionAll { get; set; }
            public long BattleCompetitionDay { get; set; }
        }

        public class CreateClansModel
        {
            public long All { get; set; }
            public long Day { get; set; }
        }
        public class BoxsModel
        {
            public long BuyStoreAll { get; set; }
            public long BuyStoreDay { get; set; }
            
            public long WinBattleAll { get; set; }
            public long WinBattleDay { get; set; }
        }

        public class CreateArmyModel
        {
            public long AllTanks { get; set; }
            public long DayTanks { get; set; }

            public long AllSol { get; set; }
            public long DaySol { get; set; }
        }

        public class WinCasinoModel
        {
            public long All { get; set; }
            public long Day { get; set; }
        }

        public class RegistrationsModel
        {
            /// <summary>
            /// Всего
            /// </summary>
            public long All { get; set; }
            /// <summary>
            /// За день
            /// </summary>
            public long Day { get; set; }
        }

        public class BattlesModel
        {
            /// <summary>
            /// Всего
            /// </summary>
            public long All { get; set; }
            /// <summary>
            /// За день
            /// </summary>
            public long Day { get; set; }
        }

        public class ErrorsModel
        {
            /// <summary>
            /// всего
            /// </summary>
            public long All { get; set; }
            /// <summary>
            /// За день
            /// </summary>
            public long Day { get; set; }
        }
    }
}
