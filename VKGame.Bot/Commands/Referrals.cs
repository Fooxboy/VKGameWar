using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Referrals :ICommand
    {
        public string Name => "рефералы";
        public string Caption => "Здесь можно посмотреть кто у Вас реферал и сколько вы получаете с него прибыли.";
        public string Arguments => "()";
        public TypeResponse Type => TypeResponse.Text;
        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            return GetReferralsText(msg.PeerId);
        }

        private string GetReferralsText(long id)
        {
            var referrals = Api.Referrals.GetList(id);
            string referralsStr = "";
            foreach(var refer in referrals.ReferralsList)
            {
                referralsStr += $"😀 Имя: {refer.Name}" +
                    $"\n🆗 Дата регистрации реферала: {refer.DateRegistration}" +
                    $"\n💳 Прибыль с реферала: {refer.FarmMoney}" +
                    $"\n";
            }
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\n ВАШИ РЕФЕРАЛЫ:" +
                $"\n" +
                $"{referralsStr}" +
                $"\n" +
                $"\n😀 Всего рефералов: {referrals.ReferralsList.Count}" +
                $"\n💳 Всего получено прибыли: {referrals.SumCash}" +
                $"\n" +
                $"\n ❓ Для того, чтобы кто-то стал Вашим рефералом, нужно, чтобы он при регистарации написал:" +
                $"\n ▶ Старт {id}" +
                $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }
    }
}
