using VkNet.Model.RequestParams;
using System.Collections.Generic;
using System;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Команда, которая срабатывает, когда не единой команды не выполнено.
    /// </summary>
    public class NoCommand
    {
        public static void Execute(Models.Message msg)
        {
            try
            {
                var lastcommand = Common.LastCommand;
                var command = lastcommand[msg.from_id];
                var response = String.Empty;
                var type = command.GetType();
                object obj = Activator.CreateInstance(type);
                var methods = type.GetMethods();
                var messageArray = msg.body.Split(' ');

                foreach (var method in methods)
                {
                    var attributesCustom = Attribute.GetCustomAttributes(method);

                    foreach (var attribute in attributesCustom)
                    {
                        if (attribute.GetType() == typeof(Attributes.Trigger))
                        {

                            var myAtr = ((Attributes.Trigger)attribute);

                            if (myAtr.Name == messageArray[0])
                            {
                                var text = $"{command.Name} ";
                                foreach(var word in messageArray)
                                {
                                    text += $"{word} ";
                                }
                                msg.body = text;
                                object result = method.Invoke(obj, new object[] { msg });
                                response = (string)result;
                            }
                        }
                    }
                }

                Api.MessageSend(response, msg.from_id);
            }catch(KeyNotFoundException)
            {
                Api.MessageSend("Неизвестная команда!", msg.from_id);

            }
        }
    }
}
