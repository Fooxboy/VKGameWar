using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using System.Net;
using System.Linq;

namespace VKGame.Bot
{
    /// <summary>
    /// Глобальные методы.
    /// </summary>
    public class Common
    { 
        
        public static string GetToken()
        {
            return "89d513f60f1f26a711a376720a9dba0149ab2a283941b83fc1753c0a9fd54b2350be6ada97a44ee083de1";
        }

        public static string Notification = null;

        public static long LastMessage = 0;

        public static Dictionary<long, ICommand> LastCommand = new Dictionary<long, ICommand>();

        public static List<Models.UserTurnCreate> TurnCreateSoildery = new List<Models.UserTurnCreate>();
        public static List<Models.UserTurnCreate> TurnCreateTanks = new List<Models.UserTurnCreate>();

        public static VkApi VkG = null;
        public static VkApi VkM = null;


        public static string SimilarWord(string word, List<string> commands)
        {
            Dictionary<string, int> commandTop = new Dictionary<string, int>();
            if (commands.Count == 0) return "В команде нет подкоманд.";
            foreach(var command in commands)
            {
                var charsCommand = command.ToCharArray();
                var charsWord = word.ToCharArray();
                int count = 0;
                foreach(var charCommand in charsCommand)
                {
                    foreach(var charWord in charsWord)
                    {
                        if (charCommand == charWord)
                        {
                            count = count + 1;
                        }
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
                "Нашли баг? Срочно опишите его в разделе БАГ. Использование: БАГ (сообщение о баге)"
            };
            var r = new Random();
            var i = r.Next(0, (ListHelp.Length -1));
            return ListHelp[i];
        }

        public static VkApi GetMyVk() 
        {
            if(Common.VkM == null)
            {
                var VkApi = new VkApi();

                string tokenMy = "ff9843ac13585ffc0ee35fd9ccbeae504187bf07e5e9b2a47490f2e9c1fdfd6c0664aeb4b3c2016878cb5";
                VkApi.Authorize(new ApiAuthParams
                {
                    AccessToken = tokenMy,
                    UserId = 308764786
                });

                VkM = VkApi;
            }
            return VkM;
        }
        
        public static VkApi GetVk()
        {
            if(VkG == null)
            {   
                var token = GetToken();
                var VkApi = new VkApi();
                VkApi.Authorize(new ApiAuthParams
                {
                    AccessToken = token,
                    UserId = 308764786,
                    Settings = VkNet.Enums.Filters.Settings.All
                });

                VkG = VkApi;
            }
            return VkG;
        }
    }
}
