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
                try
                {
                    var listCredits = Api.CreditList.GetList();
                    foreach (var userId in listCredits.Credits)
                    {
                        var user = Api.User.GetUser(userId);
                        var credit = new Api.Credit(user.Credit);
                        var creditTime = credit.Time;
                        creditTime -= 1;
                        credit.Time = creditTime;
                        if (creditTime == 0)
                        {
                            Notifications.RemovePaymentCard(Convert.ToInt32(credit.Price), userId, "оплата кредита.");
                            user.Credit = 0;
                            listCredits.Credits.Remove(userId);
                            Api.CreditList.SetList(listCredits);
                            Api.User.SetUser(user);
                        }
                    }
                }catch(Exception e)
                {
                    Bot.Statistics.NewError();
                    Logger.WriteError($"{e.Message} \n {e.StackTrace}");
                }

                Thread.Sleep(3600000);
            }
        }
    }
}