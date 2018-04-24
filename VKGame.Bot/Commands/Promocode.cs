using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Promocode:ICommand
    {
        public override string Name => "Промокод";
        public override string Caption =>"Раздел предназначен для работы с промокодами.";
        public override string Arguments => "(промокод)";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>();
        public override Access Access => Access.User;


        public object Execute(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');

            if (messageArray[1] == "ген") return Gen(msg.from_id, messageArray[2], messageArray[3]);

            string promocode = String.Empty;
            try
            {
                promocode = messageArray[1];
            }catch (IndexOutOfRangeException) 
            {
                return "❌ Вы  не указали промокод!";
            }
            if(!Api.Promocodes.Check(promocode)) return "❌ Такого промокода не существует.";
            var promo = new Api.Promocodes(promocode);
            if(promo.Count == 0) return "❌ Количество использований этого промокода уже изчерпано.";
            foreach(var userId in promo.Users) 
            {
                if(userId == msg.from_id) return "❌ Вы уже использовали данный промокод.";
            }
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
            var user = new Api.User(userId);
            if (user.Access < 4) return "Вам недоступна ента подкоманда.";

            byte[] hash = Encoding.ASCII.GetBytes($"{userId}_{price}_{count}{new Random().Next(1, 1000)}_{new Random().Next(5000, 33253)}_promo{DateTime.Now}");
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";
            foreach (var b in hashenc)
                result += b.ToString("x2");

            var promoId = result.ToUpper();

            Api.Promocodes.Create(promoId, count, price);
          
            return $"Вы успешно создали промокод: {promoId}";
        }
    }
}
