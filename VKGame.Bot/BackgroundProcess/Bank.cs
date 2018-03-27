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
                    var listCredits = Api.CreditList.GetList();
                    foreach (var userId in listCredits.Credits)
                    {
                        var registry = Api.Registry.GetRegistry(userId);
                        var credit = new Api.Credit(registry.Credit);
                        var creditTime = credit.Time;
                        credit.Time = --creditTime;
                        if (creditTime == 0)
                        {
                            Notifications.RemovePaymentCard(Convert.ToInt32(credit.Price), userId, "оплата кредита.");
                            registry.Credit = 0;
                            listCredits.Credits.Remove(userId);
                            Api.CreditList.SetList(listCredits);
                            Api.Registry.SetRegistry(registry);
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