using System;
using VKGame.Bot.Models;

namespace VKGame.Bot.Helpers
{
    public static class Command
    {
        /// <summary>
        /// Метод перебора подкоманд в команде
        /// </summary>
        /// <param name="type">тип</param>
        /// <param name="command">имя команды</param>
        /// <param name="message">сообщение</param>
        /// <returns></returns>
        public static string CheckMethods(Type type, string command, Message message)
        {
            //создание объекта 
            object obj = Activator.CreateInstance(type);
            var methods = type.GetMethods();
            //перебор
            foreach (var method in methods)
            { 
                var attributesCustom = Attribute.GetCustomAttributes(method);
                foreach (var attribute in attributesCustom)
                {
                    if (attribute.GetType() == typeof(Attributes.Trigger))
                    {
                        var myAtr = ((Attributes.Trigger) attribute);
                        if (myAtr.Name.ToLower() == command.ToLower())
                        {
                            var user = new Api.User(message.from_id);
                            if ((long) myAtr.Access > user.Access) return "❌ У Вас нет доступа к этой подкоманде!";
                            object result = method.Invoke(obj, new object[] {message});
                            return (string) result;
                        }
                    }
                }      
            }
            //если такой подкоманды не найдено - нулл
            return null;
        }
    }
}