namespace VKGame.Bot.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public long Level {get;set;}
        public long IdBattle {get;set;}    
        public long Experience {get;set;}
        public long Clan { get; set; }
        public long Competition { get; set; }
        public long Access { get; set; }
        public long Quest { get; set; }
    }
}