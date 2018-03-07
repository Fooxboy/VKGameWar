using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands
{
    public class Boxes : ICommand
    {
        public string Name => "кейсы";
        public string Caption => "Здесь Вы можете посмотреть и управлять вашими кейсами!";
        public string Arguments => "(), (Вариант_выбора)";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
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

                            if (myAtr.Name == messageArray[1])
                            {

                                object result = method.Invoke(obj, new object[] { msg });
                                return (string)result;
                            }
                        }
                    }

                }
            }

                return "❌ Неизвестная подкоманда.";
        }
       

        [Attributes.Trigger("купить")]
        public static string Buy(LongPollVK.Models.AddNewMsg msg)
        {
            var boxes = new Api.Boxes(msg.PeerId);
            var messageArray = msg.Text.Split(' ');
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
                    Notifications.RemovePaymentCard(50, msg.PeerId, "покупка кейсов");
                    battleList.Add(new Models.BattleBox());
                    boxes.BattleBox = battleList;
                    return "🎉 Вы купили битвенный кейс!";
                case "строительный":
                    var battleList1 = boxes.BuildBox;
                    Notifications.RemovePaymentCard(100, msg.PeerId, "покупка кейсов");
                    battleList1.Add(new Models.BuildBox());
                    boxes.BuildBox = battleList1;
                    return "🎉 Вы купили строительный кейс!";
                default:
                    return "❌ Неизвестный тип кейса!";
            }
        }

        [Attributes.Trigger("открыть")]
        public static string Onen(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');
            string boxName = "";
            try
            {
                boxName = messageArray[2];
            }catch(IndexOutOfRangeException)
            {
                return "❌ Вы не указали название кейса!";
            }
            var boxes = new Api.Boxes(msg.PeerId);
            var resources = new Api.Resources(msg.PeerId);
            switch(boxName.ToLower())
            {
                case "битвенный":
                    if (boxes.BattleBox.Count == 0) return "❌ У Вас нет таких боксов";
                    var box = boxes.BattleBox[0];
                    var food = box.Food;
                    var money = box.Money;
                    var soldiery = box.Soldiery;
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
                            $"\n ✨ Поздравляем!";
                case "строительный":
                    if (boxes.BuildBox.Count == 0) return "❌ У Вас нет таких боксов";
                    var builds = new Api.Builds(msg.PeerId);
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
                           $"\n 🏡 Жилой дом: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 2)
                    {
                        builds.Eatery = builds.Eatery + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🏡 Столовая: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 3)
                    {
                        builds.Hangars = builds.Hangars + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🏡 Ангар: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 4)
                    {
                        builds.Mine = builds.Mine + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🏡 Шахта: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 5)
                    {
                        builds.PowerGenerators = builds.PowerGenerators + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🏡 Генератор энергии: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 6)
                    {
                        builds.WarehouseEat = builds.WarehouseEat + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🏡 Холодильник: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 7)
                    {
                        builds.WarehouseEnergy = builds.WarehouseEnergy + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🏡 Батареии: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 8)
                    {
                        builds.WarehouseWater = builds.WarehouseWater + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🏡 Бочки с водой: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else if(rand == 9)
                    {
                        builds.WaterPressureStation = builds.WaterPressureStation + count;
                        return $"✨ Вот, что Вам выпало из кейса: " +
                           $"\n 🏡 Водонапорная башня: {count}" +
                           $"\n ✨ Поздравляем!";
                    }else
                    {
                        return "❌ Возникла ошибка, которая не может возникнуть.";
                    }
                default:
                    return "❌ Неизвестный тип кейса!";
            }
        }

        private string GetBoxesText(LongPollVK.Models.AddNewMsg msg)
        {
            var boxes = new Api.Boxes(msg.PeerId);
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
