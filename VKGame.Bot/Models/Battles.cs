using System.Collections.Generic;
using System.Collections;


namespace VKGame.Bot.Models
{


    public class ActiveBattles 
    {
        public List<long> Battles {get;set;}
    }
    
    public class Battle 
    {
        public int Id {get; set;}
        public bool IsStart {get;set;} = false;
        public long UserOne {get;set;}
        public long UserTwo {get;set;}
        public long HpOne {get;set;}
        public long HpTwo {get;set;}
        public long Creator {get;set;}
        public long UserCourse {get;set;}
        public string Body {get;set;}
        public long Price {get;set;}
    }
}