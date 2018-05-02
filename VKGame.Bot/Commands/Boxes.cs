using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Boxes : ICommand
    {
        public override string Name => "Кейсы";
        public override string Caption => "Здесь Вы можете посмотреть и управлять вашими кейсами!";
        public override string Arguments => "(), (Вариант_выбора)";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>() {"купить", "открыть"};
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";


        public override object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetBoxesText(msg);
            
            var type = typeof(Boxes);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }
       

        [Attributes.Trigger("купить")]
        public static string Buy(Models.Message msg)
        {
            var boxes = new Api.Boxs(msg.from_id);
            var messageArray = msg.body.Split(' ');
            string boxName = "";
            try
            {
                boxName = messageArray[2];
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали название кейса!";
            }
            switch (boxName.ToLower())
            {
                case "битвенный":

                    Notifications.RemovePaymentCard(50, msg.from_id, "покупка кейсов");
                    boxes.Battle = boxes.Battle + 1;
                    Statistics.BuyBox();

                    return "🎉 Вы купили битвенный кейс!";
                case "строительный":
                    var battleList1 = boxes.Build;
                    Notifications.RemovePaymentCard(100, msg.from_id, "покупка кейсов");
                    boxes.Build = boxes.Build + 1;
                    Statistics.BuyBox();

                    return "🎉 Вы купили строительный кейс!";
                default:
                    return "❌ Неизвестный тип кейса! Доступные типы кейсов: битвенный и строительный";
            }
        }

        [Attributes.Trigger("открыть")]
        public static string Onen(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            string boxName = "";
            try
            {
                boxName = messageArray[2];
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали название кейса!";
            }
            var boxes = new Api.Boxs(msg.from_id);
            var resources = new Api.Resources(msg.from_id);
            switch(boxName.ToLower())
            {
                case "битвенный":
                    if (boxes.Battle == 0) return "❌ У Вас нет таких боксов";
                    var box = new Models.Boxes.Battle();
                    var food = box.Food;
                    var money = box.Money;
                    var soldiery = box.Soldiery;
                    var tanks = box.Tanks;
                    resources.Food = resources.Food + food;
                    resources.MoneyCard = resources.MoneyCard + money;
                    resources.Soldiery = resources.Soldiery + soldiery;
                    boxes.Battle = boxes.Battle - 1;
                    return $"✨ Вот, что Вам выпало из кейса: " +
                            $"\n 💳 Монеты: {money}" +
                            $"\n 🍕 Еда: {food}" +
                            $"\n 🧑 Солдат: {soldiery}" +
                            $"\n 💣 Танков: {tanks}" +
                            $"\n ✨ Поздравляем!";
                case "строительный":
                    if (boxes.Build == 0) return "❌ У Вас нет таких боксов";
                    var builds = new Api.Builds(msg.from_id);
                    var r = new Random();
                    var box1 = new Models.Boxes.Build();
                    boxes.Build = boxes.Build - 1;
                    var count = box1.Count;

                    if (count == 0) return "😢 Ой, а Вам попался пустой кейс! :(";
                    var rand = r.Next(1, 9);
                    if(rand == 1)
                    {
                        builds.Apartments = builds.Apartments + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🏡 Жилые дома: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 2)
                    {
                        builds.Eatery = builds.Eatery + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🍔 Закусочные: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 3)
                    {
                        builds.Hangars = builds.Hangars + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 💣 Ангары: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 4)
                    {
                        builds.Mine = builds.Mine + count;
                        return $"✨ Вот, что Вам выпало из кейса:" +
                            $"\n ==ПУСТ== ";
                    }else if(rand == 5)
                    {
                        builds.PowerGenerators = builds.PowerGenerators + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n ⚡ Электростанции: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 6)
                    {
                        builds.WarehouseEat = builds.WarehouseEat + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🍬 Холодильники: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 7)
                    {
                        builds.WarehouseEnergy = builds.WarehouseEnergy + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🔋 Энергетические батареи: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 8)
                    {
                        builds.WarehouseWater = builds.WarehouseWater + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🌊 Бочки с водой: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 9)
                    {
                        builds.WaterPressureStation = builds.WaterPressureStation + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 💦 Водонапорные башни: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else
                    {
                        return "❌ Возникла ошибка, которая не может возникнуть.";
                    }
                default:
                    return "❌ Неизвестный тип кейса! Доступные типы кейсов: битвенный и строительный";
            }
        }

        private string GetBoxesText(Models.Message msg)
        {
            var boxes = new Api.Boxs(msg.from_id);
            return $"" +
                $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\n📦 Раздел для управления Вашими кейсами." +
                $"\n" +
                $"\nВАШИ КЕЙСЫ➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\n⚔ БИТВЕННЫЙ: {boxes.Battle}" +
                $"\n🏡 СТРОИТЕЛЬНЫЙ: {boxes.Build}" +
                $"\n😎 VIP: {boxes.Vip} " +
                $"\n" +
                $"\n❓ Для того, чтобы открыть определённый кейс напишите: Кейсы открыть название" +
                $"\n▶ Например: кейсы открыть битвенный" +
                $"\n😀 Подробное описание кейсов можете посмотреть в справке." +
                $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }
    }
}
