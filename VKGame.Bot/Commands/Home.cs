using System;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Класс для команды Домой.
    /// </summary>
    public class Home:ICommand
    {
        public string Name => "Домой";
        public string Arguments => "()";
        public string Caption => "Возвращает на Ваш домашний экран!";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            var text = GetHomeText(msg, $"Последнее обновление данных: {DateTime.Now}");
            Statistics.GoToHome();
            return text;
        }

        public static string GetHomeText(LongPollVK.Models.AddNewMsg msg, string notify)
        {
            var user = Api.User.GetUser(msg.PeerId);
            var builds = new Api.Builds(msg.PeerId);
            Models.IResources resources = new Api.Resources(msg.PeerId);
            return $"‼{notify}‼" +
                          $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n" +
                          $"\n👦 КОМАНДИР {user.Name}. 🔝Уровень: {user.Level}." +
                          $"\n" +
                          $"\nФИНАНСЫ➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n💰 Наличные монеты: {resources.Money}." +
                          $"\n💳 Банковский счёт: {resources.MoneyCard}." +
                          $"\n " +
                          $"\nРЕСУРСЫ➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n⚡ Энергия: {resources.Energy}/{builds.WarehouseEnergy * 100}." +
                          $"\n🍕 Еда: {resources.Food}/{builds.WarehouseEat * 100}." +
                          $"\n💧 Вода: {resources.Water}/{builds.WarehouseWater * 100}." +
                          $"\n" +
                          $"\nАРМИЯ➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n👨 Солдат: {resources.Soldiery}/{builds.Apartments * 10}." +
                          $"\n💣 Танков: {resources.Tanks}/{builds.Hangars*5}." +
                          $"\n" +
                          $"\n▶ Вы можете перейти в другие разделы. В такие как:" +
                          $"\n- 🎲 Казино‍" +
                          $"\n- 🏹 Армия" +
                          $"\n- 🏡 Постройки" +
                          $"\n- ⚔ Бой" +
                          $"\n- 💎 Магазин" +
                          $"\n- 💰 Банк" +
                          $"\n- 🎁 Кейсы" +
                          $"\n- 🤑 Соревнования" +
                          $"\n- ⚙ Настройки" +
                          $"\n-  Квесты" +
                          $"\n-  Кланы" +
                          $"\n➡Чтобы увидеть все разделы, напишите: Разделы" +
                          $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n⚠ Random help.";
        }
    }
}