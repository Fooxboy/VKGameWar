using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Boxes : ICommand
    {
        public string Name => "Кейсы";
        public string Caption => "Здесь Вы можете посмотреть и управлять вашими кейсами!";
        public string Arguments => "(), (Вариант_выбора)";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>() {"купить", "открыть"};
        public Access Access => Access.User;


        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetBoxesText(msg);
            else
            {
                var type = typeof(Boxes);
                object obj = Activator.CreateInstance(type);
                var methods = type.GetMethods();

                foreach (var method in methods)
                {
                    var attributesCustom = Attribute.GetCustomAttributes(method);

                    foreach (var attribute in attributesCustom)
                    {
                        if (attribute.GetType() == typeof(Attributes.Trigger))
                        {
                            var myAtr = ((Attributes.Trigger)attribute);
                            if (myAtr.Name.ToLower() == messageArray[1].ToLower())
                            {
                                object result = method.Invoke(obj, new object[] { msg });
                                return (string)result;
                            }
                        }
                    }

                }
            }

            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }
       

        [Attributes.Trigger("купить")]
        public static string Buy(Models.Message msg)
        {
            var boxes = new Api.Boxes(msg.from_id);
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

                    var battleList = boxes.BattleBox;
                    Notifications.RemovePaymentCard(50, msg.from_id, "покупка кейсов");
                    battleList.Add(new Models.BattleBox());
                    boxes.BattleBox = battleList;
                    Statistics.BuyBox();

                    return "🎉 Вы купили битвенный кейс!";
                case "строительный":
                    var battleList1 = boxes.BuildBox;
                    Notifications.RemovePaymentCard(100, msg.from_id, "покупка кейсов");
                    battleList1.Add(new Models.BuildBox());
                    boxes.BuildBox = battleList1;
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
            var boxes = new Api.Boxes(msg.from_id);
            var resources = new Api.Resources(msg.from_id);
            switch(boxName.ToLower())
            {
                case "битвенный":
                    if (boxes.BattleBox.Count == 0) return "❌ У Вас нет таких боксов";
                    var box = boxes.BattleBox[0];
                    var food = box.Food;
                    var money = box.Money;
                    var soldiery = box.Soldiery;
                    var tanks = box.Tanks;
                    resources.Food = resources.Food + food;
                    resources.MoneyCard = resources.MoneyCard + money;
                    resources.Soldiery = resources.Soldiery + soldiery;
                    var boxesList = boxes.BattleBox;
                    boxesList.RemoveAt(0);
                    boxes.BattleBox = boxesList;
                    return $"✨ Вот, что Вам выпало из кейса: " +
                            $"\n 💳 Монеты: {money}" +
                            $"\n 🍕 Еда: {food}" +
                            $"\n 🧑 Солдат: {soldiery}" +
                            $"\n 💣 Танков: {tanks}" +
                            $"\n ✨ Поздравляем!";
                case "строительный":
                    if (boxes.BuildBox.Count == 0) return "❌ У Вас нет таких боксов";
                    var builds = new Api.Builds(msg.from_id);
                    var r = new Random();
                    var box1 = boxes.BuildBox[0];
                    var boxesList1 = boxes.BuildBox;
                    boxesList1.RemoveAt(1);
                    boxes.BuildBox = boxesList1;
                    var count = box1.Count;
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
                    return "❌ Неизвестный тип кейса!  Доступные типы кейсов: битвенный и строительный";
            }
        }

        private string GetBoxesText(Models.Message msg)
        {
            var boxes = new Api.Boxes(msg.from_id);
            return $"" +
                $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\n📦 Раздел для управления Вашими кейсами." +
                $"\n" +
                $"\nВАШИ КЕЙСЫ➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                $"\n⚔ БИТВЕННЫЙ: {boxes.BattleBox.Count}" +
                $"\n🏡 СТРОИТЕЛЬНЫЙ: {boxes.BuildBox.Count}" +
                $"\n" +
                $"\n❓ Для того, чтобы открыть определённый кейс напишите: Кейсы открыть название" +
                $"\n▶ Например: кейсы открыть битвенный" +
                $"\n😀 Подробное описание кейсов можете посмотреть в справке." +
                $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }
    }
}
