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
        public List<string> Commands => new List<string>();
        public Access Access => Access.User;


        public object Execute(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');

            if (messageArray[1] == "ген") return Gen(msg.from_id, messageArray[2], messageArray[3]);

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

        public static string Gen(long userId, string price, string count)
        {
            var user = Api.User.GetUser(userId);
            if (user.Access < 4) return "Вам недоступна ента подкоманда.";

            var r = new Random();

            var promo = r.Next(10, 754358634);
            if(Api.Promocode.Check(promo))
            {
                promo = r.Next(10, 754358634);
                if (Api.Promocode.Check(promo))
                {
                    promo = r.Next(10, 754358634);
                }
            }

            Api.Promocode.Create(promo, Int32.Parse(count), Int32.Parse(price));

            return $"Вы успешно создали промокод {promo}";
        }
    }
}
