using System.Collections.Generic;
using System.Collections;


namespace VKGame.Bot.Models
{
    public class ActiveBattles 
    {
        public List<long> Battles {get;set;}
    }
    
    public interface IBattle 
    {
         long Id {get;}
         bool IsStart {get;set;}
         long UserOne {get;set;}
         long UserTwo {get;set;}
         long HpOne {get;set;}
         long HpTwo {get;set;}
         long Creator {get;set;}
         long UserCourse {get;set;}
         string Body {get;set;}
    }
}