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
                var listCredits = Api.CreditList.GetList();
                foreach(var userId in listCredits.Credits) 
                {
                    var user = Api.User.GetUser(userId);
                    var credit = new Api.Credit(user.Credit);
                    credit.Time = credit.Time -1;
                    if(credit.Time == 0) 
                    {
                        Notifications.RemovePaymentCard(Convert.ToInt32(credit.Price), userId, "Оплата кредита.");
                        user.Credit = 0;
                        listCredits.Credits.Remove(userId);
                        Api.CreditList.SetList(listCredits);
                        Api.User.SetUser(user);
                    }
                }
                Thread.Sleep(3600000);
            }
        }
    }
}