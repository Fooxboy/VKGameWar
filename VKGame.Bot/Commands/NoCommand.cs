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

                            if (myAtr.Name.ToLower() == messageArray[0].ToLower())
                            {
                                var text = $"{command.Name} ";
                                foreach(var word in messageArray)
                                {
                                    text += $"{word} ";
                                }
                                msg.body = text;
                                object result = method.Invoke(obj, new object[] { msg });
                                if ((string)result != null) response = (string)result;
                                else
                                {
                                    var word = Common.SimilarWord(messageArray[0], command.Commands);
                                    response = $"❌ Неизвестная подкоманда." +
                                            $"\n ❓ Возможно, Вы имели в виду - {word}";
                                }
                            }
                        }
                    }
                }

                Api.MessageSend(response, msg.from_id);
            }catch(KeyNotFoundException)
            {
                Api.MessageSend("❌ Неизвестная команда!", msg.from_id);

            }
        }
    }
}
