using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Models
{
    public class Referrals
    {
        public List<Referral> ReferralsList { get; set; }
        public long MouthCash { get; set; }
        public long SumCash { get; set; }
        public class Referral
        {
            public string Name { get; set; }
            public long Id { get; set; }
            public string DateRegistration { get; set; }
            public long FarmMoney { get; set; }
        }
    }
}
