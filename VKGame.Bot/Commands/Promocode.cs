using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading;

namespace VKGame.Bot.Commands
{
    public class Promocode:ICommand
    {
        public string Name => "Промокод";
        public string Caption =>"Раздел предназначен для работы с промокодами.";
        public string Arguments => "(промокод)";
        public TypeResponse Type => TypeResponse.Text;
        public object Execute(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');
            long promocode = 0;
            try 
            {
                promocode = Int64.Parse(messageArray[1]);
            }catch (IndexOutOfRangeException) 
            {
                return "❌ Вы  не указали промокод!";
            }catch (FormatException) 
            {
                return "❌ Вы указали невалидный промокод";
            }
            if(!Api.Promocode.Check(promocode)) return "❌ Такого промокода не существует.";
            var promo = new Api.Promocode(promocode);
            if(promo.Count == 0) return "❌ Количество использований этого промокода уже изчерпано.";
            foreach(var userId in promo.Users) 
            {
                if(userId == msg.from_id) return "❌ Вы уже использовали данный промокод.";
            }
            var resources =  new Api.Resources(msg.from_id);
            Notifications.EnterPaymentCard(Convert.ToInt32(promo.MoneyCard), msg.from_id, "Активация промокода.");
            var usersPromo = promo.Users;
            usersPromo.Add(msg.from_id);
            promo.Users = usersPromo;
            promo.Count = promo.Count -1;
            Statistics.ActivatePromo();
            return $"✅ Вы успешно активировали промокод {promocode}.";
        }
    }
}
