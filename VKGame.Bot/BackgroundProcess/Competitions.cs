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
                    var listCompetitions = Api.Competitions.GetList();
                    if (listCompetitions.List.Count != 0)
                    {
                        foreach (var idCompetition in listCompetitions.List)
                        {
                            var competition = new Api.Competitions(idCompetition);
                            competition.Time = competition.Time - 1;
                            if (competition.Time == 0)
                            {
                                Commands.Competitions.EndCompetition(competition.Id);
                            }
                        }
                    }
                }catch(Exception e)
                {
                    Logger.WriteError($"{e.Message} \n {e.StackTrace}");

                }
            }
        }
    }
}
