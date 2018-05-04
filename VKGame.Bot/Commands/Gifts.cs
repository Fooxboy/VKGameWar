﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VKGame.Bot.Commands
{
    public class Gifts :ICommand
    {
        public override string Name => "Подарки";
        public override string Arguments => "(), (вариант_выбора)";
        public override string Caption => "Раздел предназначен для управления Вашими подарками.";
        public override TypeResponse Type => TypeResponse.Text;
        public override string HelpUrl => "сслыка недоступна";

        public override List<string> Commands =>
            new List<string>() { "список" , "открыть", "отправить" };

        public override Access Access => Access.User;

        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetGiftsText();
            var result = Helpers.Command.CheckMethods(typeof(Battle), messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        [Attributes.Trigger("открыть")]
        public static string OpenBox(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            var user = new Api.User(msg.from_id);
            var gifts = user.Gifts;
            if (gifts.Count == 0) return "❌ У Вас нет подарков.";

            long id = 0;
            try
            {
                id = Int64.Parse(messageArray[2]);
                
            }catch(FormatException) { return "❌ Вы указали неверный ID подарка."; }
            catch(IndexOutOfRangeException)
            {
                foreach(var giftId in gifts)
                {
                    var giftA = new Api.Gifts(giftId);
                    if (!giftA.IsOpen)
                        id = giftId;
                    break;
                }
            }
            if (id == 0) return "❌ У Вас нет нераспакованых подарков!";
            if (!Api.Gifts.Check(id)) return "❌ Такого подарка не существует!";

            var gift = new Api.Gifts(id);
            if (gift.To != user.Id) return "❌ этот подарок не для Вас!";
            if (gift.IsOpen) return "❌ Вы этот подарок уже открыли!";
            Notifications.EnterPaymentCard(Convert.ToInt32(gift.Price), user.Id, "Открытие подарка");
            gift.IsOpen = true;

            return $"🎁 Вы успешно открыли подарок! И получили: {gift.Price} монет!";
        }

        [Attributes.Trigger("отправить")]
        public static string Send(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');

            long userTo;
            int price;

            try
            {
                userTo = Int64.Parse(messageArray[2]);
            }catch(FormatException)
            {
                return "❌ Неверный id пользователя.";
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали id пользователя. Например, подарки отправить id_пользователя сумма";
            }

            try
            {
                price = Int32.Parse(messageArray[2]);
            }
            catch (FormatException)
            {
                return "❌ Неверная сумма";
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не сумму. Например, подарки отправить id_пользователя сумма";
            }

            if (!Api.User.Check(userTo))
                return "❌ Пользователь, id которого Вы указали, не зарегестрирован. Пригласи его в игру и получай с него монеты!" +
                    "\n Подробнее в разделе Рефералы";
            if (price > 500)
                return "❌ Нельзя подарить больше 500 монет!";

            if (!Notifications.RemovePaymentCard(price, msg.from_id, "Отправка подарка"))
                return "❌ На Вашем счету недостаточно монет.";

            //добавление подарка пользователю.
            var userToObject = new Api.User(userTo);
            var gifts = userToObject.Gifts;
            var giftId = Api.Gifts.New(msg.from_id, userTo, price);
            gifts.Add(giftId);
            userToObject.Gifts = gifts;
            new Task(() => Api.Message.Send($"🎁 Опа! А Вам подарок! Чтобы открыть его, напишите: Подарки открыть {giftId}", userTo)).Start();
            return "🎁 Пользователь получил Ваш подарок!";
        }

        [Attributes.Trigger("список")]
        public static string ListBoxs(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
            var gifts = user.Gifts;
            string strGift = string.Empty;
            if (gifts.Count == 0) strGift = "😒 Похоже, у Вас нет подарков.";
            foreach(var giftId in gifts)
            {
                var gift = new Api.Gifts(giftId);
                string price = string.Empty;
                if(gift.IsOpen)
                {
                    price = $"👀 Открыт: Да" +
                        $"\n 🎁 Внутри: {gift.Price} монет";
                }else
                {
                    price = $"👀 Открыт: Нет";
                }
                strGift += $"➡ ID: {gift.Id}" +
                        $"\n ✌ От кого: *id{gift.From}" +
                        $"\n {price}" +
                        $"\n";
            }

            return $"🎁 ВАШИ ПОДАРКИ: " +
                $"\n {strGift}" +
                $"\n ❓ Чтобы открыть определённый подарок, напишите: подарки открыть (ID)";

        }

        public static string GetGiftsText()
        {
            return "🎁 ЗДЕСЬ НАХОДЯТСЯ ВСЕ ВАШИ ПОДАРКИ 🎁" +
                "\n" +
                "\n 😉 Для того, чтобы открыть последний полученный подарок: подарки открыть" +
                "\n ❓ Для того, чтобы откырыть определённый полученный подарок: подарки открыть (id)" +
                "\n 🎂 Список ваших подарков: подарки список";
        }
    }
}
