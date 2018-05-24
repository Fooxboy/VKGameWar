using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using System.Net;
using System.Linq;
using Microsoft.Data.Sqlite;
using VKGame.Bot.Models;
using VKGame.Bot.Commands;

namespace VKGame.Bot
{
    /* =============================================================
     * Мусорка статитческих данных.
     * Сейчас ты видешь то, как НЕНУЖНО писать код.
     * Я не помню даже как получилась эта свалка
     * Но переписывать её было очень затратно
     * И она ростёт  и ростёт по сегодняшний день
     * Надеюсь, я скоро перепишу бота и избавлюсь от неё
     * А щас есть как есть.
     * ============================================================= */

    /// <summary>
    /// Глобальные методы.
    /// </summary>
    public class Common
    {      
        /// <summary>
        /// Токен группы
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            return "89d513f60f1f26a711a376720a9dba0149ab2a283941b83fc1753c0a9fd54b2350be6ada97a44ee083de1";
        }

        /// <summary>
        /// Токен тест группы
        /// </summary>
        /// <returns></returns>
        public static string GetTestToken()
        {
            return "a85cd63f0b1a3530214a918cde6f829a3315e475f9b9880cb6192ce75129a6ae9c398eeceb41a1c760d7b";
        }

        //Все доступные команды.
        public static readonly List<ICommand> Commands = new List<ICommand>()
        {
            new Start(),
            new WriteData(),
            new Home(),
            new Casino(),
            new Army(),
            new Buildings(),
            new Battle(),
            new Store(),
            new Promocode(),
            new Bank(),
            new Commands.Boxes(),
            new Commands.Quests(),
            new Commands.Referrals(),
            new Clans(),
            new Competitions(),
            new Commands.Database(),
            new ExecuteCode(),
            new Settings(),
            new Sections(),
            new Balance(),
            new Commands.Admin.News(),
            new Commands.Admin.NotifyAll(),
            new Commands.Admin.Reboot(),
            new Feedback(),
            new Commands.Bug(),
            new Commands.Admin.Stat(),
            new Commands.Admin.System(),
            new Skills(),
            new Commands.Admin.Bugs(),
            new Top(),
            new Commands.Admin.AccessCommand(),
            new Gifts(),
            new Help(),
            new Friends()
        };

        /// <summary>
        /// Локальный токен страницы
        /// </summary>
        public static string GetMyToken => "";


        /// <summary>
        /// Данные рулетки
        /// </summary>
        public static Roulette Roulette = new Roulette(){ Prices =  new List<RoulettePrices>(), Fund = 0};


        /// <summary>
        /// Публичное уведомление
        /// </summary>
        public static string Notification = null;


        /// <summary>
        /// Индентификатор команды
        /// </summary>
        public static long LastMessage = 0;

        /// <summary>
        /// Последние команды пользователей
        /// </summary>
        public static Dictionary<long, ICommand> LastCommand = new Dictionary<long, ICommand>();
        
        /// <summary>
        /// Создаваемые солдаты
        /// </summary>
        public static Dictionary<long, int> CountCreateArmySoldiery = new Dictionary<long, int>();

        /// <summary>
        /// Солдаваемы танки
        /// </summary>
        public static Dictionary<long, int> CountCreateArmyTanks = new Dictionary<long, int>();

        /// <summary>
        /// Тест мод
        /// </summary>
        public static bool IsTestingMode = false;

        /// <summary>
        /// Топ юзеры
        /// </summary>
        public static List<long> TopUsers = new List<long>();
       
        /// <summary>
        /// Очередб создаваемых солдат
        /// </summary>
        public static List<Models.UserTurnCreate> TurnCreateSoildery = new List<Models.UserTurnCreate>();

        /// <summary>
        /// Очередь создаваемых танков
        /// </summary>
        public static List<Models.UserTurnCreate> TurnCreateTanks = new List<Models.UserTurnCreate>();

        /// <summary>
        /// че г
        /// </summary>
        public static VkApi VkG = null;

        /// <summary>
        /// че м
        /// </summary>
        public static VkApi VkM = null;


        /// <summary>
        /// Похожие команды
        /// </summary>
        /// <param name="word"> Слово</param>
        /// <param name="commands"> Команды</param>
        /// <returns></returns>
        public static string SimilarWord(string word, List<string> commands)
        {
            Dictionary<string, int> commandTop = new Dictionary<string, int>();
            if (commands.Count == 0) return "В команде нет подкоманд.";
            foreach (var command in commands)
            {
                var charsCommand = command.ToCharArray();
                var charsWord = word.ToCharArray();
                int count = 0;
                foreach (var charCommand in charsCommand)
                {
                    foreach (var charWord in charsWord)
                    {
                        if (charCommand == charWord) ++count;
                    }
                }
                commandTop.Add(command, count);
            }
            string valueReturn = String.Empty;
            int lastValue = 0;
            foreach (var commandModel in commandTop)
            {
                if (commandModel.Value > lastValue)
                {
                    lastValue = commandModel.Value;
                    valueReturn = commandModel.Key;
                }
            }
            return valueReturn;
        }
        /// <summary>
        /// Получение рандомной помощи
        /// </summary>
        /// <returns>Рандомная помощь</returns>
        public static string GetRandomHelp() 
        {
            string[] ListHelp =
            {
                "Нет денег? Возьми кредит! Напиши: Банк кредит <сумма>",
                "Чем больше солдат у Вас в казарме, тем больше они кушают еды!",
                "Чтобы победить, нужно продумать каждый ход!",
                "Не с кем повоевать? Воюй с ботом! Напиши: Бой бот <сумма>",
                "Вы азартны? Попробуйте нашу рулетку! Напишите: Казино рулетка",
                "Приглашайте в игру рефералов! При регистрации пользователь должен указать Ваш id. Подробнее: Рефералы",
                "В соревнованиях можно хорошо заработать! Напиши: Соревнования",
                "Нашли баг? Срочно опишите его в разделе БАГ. Использование: БАГ (сообщение о баге)",
                "Хотите оставить отзыв? Напишите его! Отзыв (ваш отзыв)",
                "Надоело долго ждать обучения армии? Используйте усилители! напишите: Усилители",
                "Хочешь удивить своего друга или подругу? Подари подарок!",
                "Добавь своего друга в друзья в игре! Подробнее: друзья"
            };
            var r = new Random();
            var i = r.Next(0, (ListHelp.Length -1));
            return ListHelp[i];
        }

        /// <summary>
        /// Получение экземпляра ВК м
        /// </summary>
        /// <returns></returns>
        public static VkApi GetMyVk() 
        {
            if(Common.VkM == null)
            {
                var VkApi = new VkApi();

                string tokenMy = GetMyToken;
                VkApi.Authorize(new ApiAuthParams
                {
                    AccessToken = tokenMy,
                    UserId = 308764786
                });

                VkM = VkApi;
            }
            return VkM;
        }
        

        /// <summary>
        /// Получение экземпляра ВКонтакте для управления от имени группы
        /// </summary>
        /// <returns>Эклемпляр ВКонтакте</returns>
        public static VkApi GetVk()
        {
            if(VkG == null)
            {
                var token = string.Empty;
                if (IsTestingMode)
                    token = GetTestToken();
                else token = GetToken();
                var VkApi = new VkApi();
                VkApi.Authorize(new ApiAuthParams
                {
                    AccessToken = token,
                    UserId = 308764786
                });
                VkG = VkApi;
            }
            return VkG;
        }
    }
}
