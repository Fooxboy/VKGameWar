﻿namespace VKGame.Bot.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DateReg { get; set; }
        public bool isSetup { get; set; }
        public long Level {get;set;}
       // public long HP {get;set;}
       public long CountBattles {get;set;}
       public long CountWinBattles {get;set;}
       public long CountCreateBattles {get;set;}
       public long IdBattle {get;set;}

       public string LastMessage {get;set;}
       public bool StartThread {get;set;}
    }
}