using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace VKGame.Bot.Commands
{
    public class Bank :ICommand 
    {
        public string Name => "Банк";
        public string Caption => "Этот раздел предназначан для работы с банком";
        public string Arguments => "(), (Вариант_Выбора)";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>() { "обмен", "кредит"};

        public object Execute(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetBankText(msg);
            else
            {
                var type = typeof(Bank);
                object obj = Activator.CreateInstance(type);
                var methods = type.GetMethods();

                foreach (var method in methods)
                {
                    var attributesCustom = Attribute.GetCustomAttributes(method);

                    foreach (var attribute in attributesCustom)
                    {
                        if (attribute.GetType() == typeof(Attributes.Trigger))
                        {

                            var myAtr = ((Attributes.Trigger)attribute);

                            if (myAtr.Name == messageArray[1])
                            { 
                                object result = method.Invoke(obj, new object[] { msg });
                                return (string)result;
                            }
                        }
                    }
                }
            }
            var word = Common.SimilarWord(messageArray[0], Commands);
            return $"❌ Неизвестная подкоманда." +
                    $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        public static class Api
        {
            public static long SumCredit(long userLvl) => userLvl * 500;
            public static bool NewCredit(long userId, long price)
            {
                var idCredit = Bot.Api.Credit.New(userId, price);
                var registry = Bot.Api.Registry.GetRegistry(userId);
                registry.Credit = idCredit;
                var listCredit = Bot.Api.CreditList.GetList();
                listCredit.Credits.Add(userId);
                Statistics.NewCredit();
                Bot.Api.CreditList.SetList(listCredit);
                Notifications.EnterPaymentCard(Convert.ToInt32(price), userId, "кредит");
                Bot.Api.Registry.SetRegistry(registry);
                return true;
            }
        }

        [Attributes.Trigger("обмен")]
        public static string Exchange(Models.Message msg) 
        {
            var messageArray = msg.body.Split(' ');
            long count = 0;
            try 
            {
                count = Int64.Parse(messageArray[2]);
            }catch(IndexOutOfRangeException) 
            {
                return "❌ Вы не указали количество монет. Например: Банк обмен 10";
            }catch(FormatException) 
            {
                return $"❌ {messageArray[2]} не является числом.";
            }
            var resources = new Bot.Api.Resources(msg.from_id);
            if(count > resources.Money) return $"❌ У Вас недостаточно монет. Ваш баланс: {resources.Money} наличных монет. Необходимо: {count} наличных монет. Наличные монеты можно получить с кейсов или из промо акций. А так же купить за реальные деньги!";
            resources.Money = resources.Money - count;
            Notifications.EnterPaymentCard(Convert.ToInt32(count), msg.from_id, "банк");
            return $"✅ Вы успешно обменяли {count} наличных монет на электронные!";
        }

        [Attributes.Trigger("кредит")]
        public static string Loan(Models.Message msg) 
        {
            var user = Bot.Api.User.GetUser(msg.from_id);
            var registry = Bot.Api.Registry.GetRegistry(msg.from_id);
            var messageArray = msg.body.Split(' ');
            long count = 0;
            try
            {
                count = Int64.Parse(messageArray[2]);
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали количество монет.";
            }
            catch (FormatException)
            {
                return $"❌ {messageArray[2]} не является числом!";
            }
            if(count > (Api.SumCredit(user.Level))) return $"❌ Ваша сумма слишком большая. Вам доступно: {Api.SumCredit(user.Level)}. Чем больше уровень, тем больше размер кредита!";
            if(registry.Credit != 0) return "❌ На Вас ещё числится кредит!";

            Api.NewCredit(msg.from_id, count);
            
            return $"✅ Вы успешно взяли кредит в размере {count}, сумма снимется через 10 часов!";
        }

        private string GetBankText(Models.Message msg) 
        {
            var resources = new Bot.Api.Resources(msg.from_id);
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💰 БАНК \"ВАШИ ДЕНЬГИ\""+
                   $"\n💳 Состояние Вашего банковского счёта: {resources.MoneyCard}" +
                   $"\n✨ Состояние Вашего наличного кошелька: {resources.Money}"+
                   $"\n"+
                   $"\nСПИСОК УСЛУГ➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💵 ОБМЕН"+
                   $"\n❓ Вы можете положить наличные деньги на Ваш банковский счёт." +
                   $"\n❓ Использование: Банк обмен сумма" +
                   $"\n"+
                   $"\n💶 КРЕДИТ"+
                   $"\n❓ Вы можете взять деньги в долг, а потом вернуть." +
                   $"\n❓ Использование: Банк кредит сумма" +
                   $"\n"+
                   $"\n▶ Подробную помощь Вы можете посмотреть в группе." +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }
    }
}