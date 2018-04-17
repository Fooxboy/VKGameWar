using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Commands.Admin
{
    public class Stat:ICommand
    {
        public string Name => "Статистика";
        public string Caption => "аа";
        public string Arguments => "";
        public List<string> Commands => new List<string>();
        public TypeResponse Type => TypeResponse.Text;
        public Access Access => Access.Admin;

        public object Execute(Models.Message msg)
        {
            var stat = Bot.Statistics.GetAll;
            string text = $"📩 Принято сообщений за день: {stat.InMessageDay}" +
                        $"\n📤 Отправлено сообщений за день: {stat.OutMessageDay}" +
                        $"\n✉ Всего сообщений: {stat.AllMessages}" +
                        $"\n❌ Ошибок:" +
                        $"\n -- ➡ За день: {stat.Errors.Day}" +
                        $"\n -- ➡ Всего: {stat.Errors.All + stat.Errors.Day}" +
                        $"\n⚔ Боёв:" +
                        $"\n -- ➡ За день: {stat.Battles.Day}" +
                        $"\n -- ➡ Всего: {stat.Battles.All + stat.Battles.Day}" +
                        $"\n😁 Регистраций:" +
                        $"\n -- ➡ За день: {stat.Registrations.Day}" +
                        $"\n -- ➡ Всего: {stat.Registrations.Day + stat.Registrations.Day}" +
                        $"\n✨ Выигрышы в казино:" +
                        $"\n -- ➡ За день: {stat.WinCasino.Day}" +
                        $"\n -- ➡ Всего: {stat.WinCasino.All + stat.WinCasino.Day}" +
                        $"\n👊 Обучено солдат:" +
                        $"\n -- ➡ За день: {stat.CreateArmy.DaySol}" +
                        $"\n -- ➡ Всего: {stat.CreateArmy.AllSol + stat.CreateArmy.DaySol}" +
                        $"\n💣 Создано танков:" +
                        $"\n -- ➡ За день: {stat.CreateArmy.DayTanks}" +
                        $"\n -- ➡ Всего: {stat.CreateArmy.AllTanks + stat.CreateArmy.DayTanks}" +
                        $"\n📦 Кейсов: " +
                        $"\n -- 💰 Куплено в магазине:" +
                        $"\n -- -- ➡ За день: {stat.Boxs.BuyStoreDay}" +
                        $"\n -- -- ➡ Всего: {stat.Boxs.BuyStoreAll + stat.Boxs.BuyStoreDay}" +
                        $"\n -- 🗡 Поучено в битвах:" +
                        $"\n -- -- ➡ За день: {stat.Boxs.WinBattleDay}" +
                        $"\n -- -- ➡ Всего: {stat.Boxs.WinBattleAll + stat.Boxs.WinBattleDay}" +
                        $"\n🛡 Создано кланов: " +
                        $"\n -- ➡ За день: {stat.CreateClans.Day}" +
                        $"\n -- ➡ Всего: {stat.CreateClans.All + stat.CreateClans.Day}" +
                        $"\n📝 Активировано промокодов: {stat.PromocodesAll}" +
                        $"\n💸 Сумма кредита: {stat.KreditsAll}" +
                        $"\n🏠 Количество переходов домой сегодня: {stat.GoHomeDay}" +
                        $"\n🏆 Соревнования: " +
                        $"\n -- ➡ Участвовало всего: {stat.Competitions.JoinPeopleAll}" +
                        $"\n -- ➡ Участвовало сегодня: {stat.Competitions.JoinPeopleDay}" +
                        $"\n -- ➡ Всего: {stat.Competitions.All}" +
                        $"\n -- ➡ Битв всего: {stat.Competitions.BattleCompetitionAll}" +
                        $"\n -- ➡ Битв сегодня: {stat.Competitions.BattleCompetitionDay}" +
                        $"\n😎 Всего рефералов: {stat.RefferalAll}" +
                        $"\n💳 Выграли в битвах за все время: {stat.WinBattleAll}" +
                        $"\n💳 Выграли в битвах за сегодня: {stat.WinBattleDay}" +
                        $"";
            return text;
        }
    }
}
