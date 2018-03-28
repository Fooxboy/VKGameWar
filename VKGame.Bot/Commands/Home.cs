using System;
using VKGame.Bot.Models;
using System.Collections.Generic;

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
        public List<string> Commands => new List<string>();

        public object Execute(Message msg)
        {
            var text = GetHomeText(msg, $"Последнее обновление данных: {DateTime.Now}");
            Statistics.GoToHome();
            return text;
        }

        public static string GetHomeText(Message msg, string notify)
        {
            var user = Api.User.GetUser(msg.from_id);
            var builds = new Api.Builds(user.Id);
            Models.IResources resources = new Api.Resources(user.Id);
            Quests.GoToHome(user.Id);
            return $"‼{notify}‼" +
                          $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n" +
                          $"\n👦 КОМАНДИР {user.Name}. " +
                          $"\n🔝 Уровень: {user.Level} ({user.Experience}/ {user.Level*100})." +
                          $"\n" +
                          $"\nФИНАНСЫ➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n💰 Наличные монеты: {resources.Money}." +
                          $"\n💳 Банковский счёт: {resources.MoneyCard}." +
                          $"\n " +
                          $"\nРЕСУРСЫ➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n⚡ Энергия: {resources.Energy}/{Buildings.Api.MaxEnergy(builds.WarehouseEnergy)}." +
                          $"\n🍕 Еда: {resources.Food}/{Buildings.Api.MaxFood(builds.WarehouseEat)}." +
                          $"\n💧 Вода: {resources.Water}/{Buildings.Api.MaxWater(builds.WarehouseWater)}." +
                          $"\n" +
                          $"\nАРМИЯ➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n👨 Солдат: {resources.Soldiery}/{Buildings.Api.MaxSoldiery(builds.Apartments)}." +
                          $"\n💣 Танков: {resources.Tanks}/{Buildings.Api.MaxTanks(builds.Hangars)}." +
                          $"\n" +
                          $"\n➡ Вы можете перейти в другие разделы. В такие как:" +
                          $"\n- 🎲 Казино‍" +
                          $"\n- 🏹 Армия" +
                          $"\n- 🏡 Постройки" +
                          $"\n- ⚔ Бой" +
                          $"\n- 💎 Магазин" +
                          $"\n- ⚙ Настройки" +
                          $"\n➡ Чтобы увидеть все разделы, напишите: Разделы" +
                          $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖" +
                          $"\n⚠ {Common.GetRandomHelp()}";
        }
    }
}