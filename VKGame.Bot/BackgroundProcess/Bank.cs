using System.Threading;
using System;

namespace VKGame.Bot.BackgroundProcess
{
    public class Bank 
    {
        public static void CheckCreditList() 
        {
            while(true) 
            {
                Thread.Sleep(3600000);
                try
                {
                    var listCredits = Api.Credits.All;
                    foreach (var userId in listCredits)
                    {
                        var registry = new Api.Registry(userId);
                        if(registry.Credit != 0)
                        {
                            var credit = new Api.Credits(registry.Credit);
                            var creditTime = --credit.Time;
                            if (creditTime == 0)
                            {
                                var resources = new Api.Resources(userId);
                                resources.MoneyCard = resources.MoneyCard - credit.Price;
                                Api.Message.Send($"✨ С Вашего счёта был снята сумма за кредит. Баланс: {resources.MoneyCard}", userId);
                                registry.Credit = 0;
                                credit.Delete();
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