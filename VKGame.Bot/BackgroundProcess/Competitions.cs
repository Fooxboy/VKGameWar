using System.Threading;
using System.Collections.Generic;
using System.Text;
using System;

namespace VKGame.Bot.BackgroundProcess
{
    public class Competitions
    {
        public static void StartCompetition()
        {
            while(true)
            {
                Thread.Sleep(3600000);
                try
                {
                    var listCompetitions = Api.Competitions.AllList;
                    if (listCompetitions.Count != 0)
                    {
                        foreach (var idCompetition in listCompetitions)
                        {
                            var competition = new Api.Competitions(idCompetition);
                            if (!competition.IsEnd)
                            {
                                competition.Time = competition.Time - 1;
                                if (competition.Time == 0)
                                    Commands.Competitions.EndCompetition(competition.Id);
                            }            
                        }
                    }
                }catch(Exception e)
                {
                    Bot.Statistics.NewError();
                    Logger.WriteError(e);
                }
            }
        }
    }
}
