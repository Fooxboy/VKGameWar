namespace VKGame.Bot.Models
{
    /// <summary>
    /// Модель ресурсов.
    /// </summary>
    public interface IResources
    {
        /// <summary>
        /// ид
        /// </summary>
        long Id { get; }
        /// <summary>
        /// Еда
        /// </summary>
        long Food { get; set; }
        /// <summary>
        /// Наличные монеты
        /// </summary>
        long Money { get; set; }
        /// <summary>
        /// Монеты на банковском счету
        /// </summary>
        long MoneyCard { get; set; }
        /// <summary>
        /// Енергия
        /// </summary>
        long Energy { get; set; }
        /// <summary>
        /// Вода
        /// </summary>
        long Water { get; set; }
        /// <summary>
        /// Солдаты
        /// </summary>
        long Soldiery { get; set; }
        /// <summary>
        /// Танки.
        /// </summary>
        long Tanks { get; set; }
        /// <summary>
        /// Билеты на соревнования.
        /// </summary>
        long TicketsCompetition { get; set; }
    }
}