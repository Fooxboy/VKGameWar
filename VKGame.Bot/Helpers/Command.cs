using System;
using VKGame.Bot.Models;

namespace VKGame.Bot.Helpers
{
    public static class Command
    {
        public static string CheckMethods(Type type, string command, Message message)
        {
            object obj = Activator.CreateInstance(type);
            var methods = type.GetMethods();

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
                            if ((long) myAtr.Access < user.Access) return "❌ У Вас нет доступа к этой подкоманде!";
                            object result = method.Invoke(obj, new object[] {message});
                            return (string) result;
                        }
                    }
                }      
            }
            return null;
        }
    }
}