using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Referrals :ICommand
    {
        public override string Name => "Рефералы";
        public override string Caption => "Здесь можно посмотреть кто у Вас реферал и сколько вы получаете с него прибыли.";
        public override string Arguments => "()";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>();
        public override Access Access => Access.User;


        public override object Execute(Models.Message msg)
        {
            return GetReferralsText(msg.from_id);
        }

        private string GetReferralsText(long id)
        {
            var referrals = new Api.Referrals(id).RefList;
            string referralsStr = "";
            foreach(var refer in referrals)
            {
                var user = new Api.User(refer);
                referralsStr += $"😀 Имя: {user.Name}" +
                    $"\n🆗 Дата регистрации реферала: time" +
                    $"\n💳 Прибыль с реферала: farm" +
                    $"\n";
            }
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\n ВАШИ РЕФЕРАЛЫ:" +
                $"\n" +
                $"{referralsStr}" +
                $"\n" +
                $"\n😀 Всего рефералов: {referrals.Count}" +
                $"\n💳 Всего получено прибыли: {new Api.Referrals(id).SumCash}" +
                $"\n" +
                $"\n ❓ Для того, чтобы кто-то стал Вашим рефералом, нужно, чтобы он при регистарации написал:" +
                $"\n ➡ Старт {id}" +
                $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }
    }
}
