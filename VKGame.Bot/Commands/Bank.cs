using System;
using Newtonsoft.Json;
using System.IO;

namespace VKGame.Bot.Commands
{
    public class Bank :ICommand 
    {
        public string Name => "банк";
        public string Caption => "Этот раздел предназначан для работы с банком";
        public string Arguments => "(), (Вариант_Выбора)";
        public TypeResponse Type => TypeResponse.Text;
        public object Execute(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');
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
            return "❌ Неизвестная подкоманда";
        }

        [Attributes.Trigger("обмен")]
        public static string Exchange(LongPollVK.Models.AddNewMsg msg) 
        {
            var messageArray = msg.Text.Split(' ');
            long count = 0;
            try 
            {
                count = Int64.Parse(messageArray[2]);
            }catch(IndexOutOfRangeException) 
            {
                return "❌ Вы не указали количество монет.";
            }catch(FormatException) 
            {
                return "❌ Вы указали неверное число.";
            }
            var resources = new Api.Resources(msg.PeerId);
            if(count > resources.Money) return "❌ У Вас недостаточно монет.";
            resources.Money = resources.Money - count;
            Notifications.EnterPaymentCard(Convert.ToInt32(count), msg.PeerId, "банк");
            return "✅ Вы успешно обменяли Ваши монеты!";
        }

        [Attributes.Trigger("кредит")]
        public static string Loan(LongPollVK.Models.AddNewMsg msg) 
        {
            var user = Api.User.GetUser(msg.PeerId);
            var messageArray = msg.Text.Split(' ');
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
                return "❌ Вы указали неверное число.";
            }
            if(count > (user.Level *500)) return $"❌ Ваша сумма слишком большая. Вам доступно: {user.Level *500}";
            if(user.Credit != 0) return "❌ На Вас ещё числится кредит!";
            var idCredit = Api.Credit.New(msg.PeerId, count);
            user.Credit = idCredit;
            var listCredit = Api.CreditList.GetList();
            listCredit.Credits.Add(user.Id);
            Statistics.NewCredit();
            Api.CreditList.SetList(listCredit);
            Notifications.EnterPaymentCard(Convert.ToInt32(count), user.Id, "кредит");
            Api.User.SetUser(user);
            return "✅ Вы успешно взяли кредит, сумма снимется через 10 часов!";
        }

        private string GetBankText(LongPollVK.Models.AddNewMsg msg) 
        {
            var resources = new Api.Resources(msg.PeerId);
            return $"➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💰 БАНК \"ВАШИ ДЕНЬГИ\""+
                   $"\n💳 Состояние Вашего банковского счёта: {resources.MoneyCard}"+
                   $"\n"+
                   $"\nСПИСОК УСЛУГ➖➖➖➖➖➖➖➖➖➖"+
                   $"\n💵 ОБМЕН"+
                   $"\n❓ Вы можете положить наличные деньги на Ваш банковский счёт."+
                   $"\n"+
                   $"\n💶 КРЕДИТ"+
                   $"\n❓ Вы можете взять деньги в долг, а потом вернуть."+
                   $"\n"+
                   $"\n▶ Подробную помощь Вы можете посмотреть тут: (ссылка на помощь)" +
                   $"\n➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖➖";
        }
    }
}