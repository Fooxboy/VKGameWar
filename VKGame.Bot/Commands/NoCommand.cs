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
            var messageArray = msg.body.Split(' ');

            try
            {
                var lastcommand = Common.LastCommand;
                var command = lastcommand[msg.from_id];
                var response = String.Empty;
                var type = command.GetType();
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

                            if (myAtr.Name.ToLower() == messageArray[0].ToLower())
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

                if(response == String.Empty)
                {
                    var word = Common.SimilarWord(messageArray[0], command.Commands);
                    var commandList = new List<string>();
                    foreach (var commandM in Core.Commands) commandList.Add(commandM.Name.ToLower());
                    var comG = Common.SimilarWord(messageArray[0], commandList);
                    response = $"❌ Неизвестная подкоманда." +
                            $"\n ❓ Возможно, Вы имели в виду - {word}" +
                            $"\n ❓ Или, может быть, Вы хотели написать {comG}";
                }

                Api.MessageSend(response, msg.from_id);
            }catch(KeyNotFoundException)
            {
                var commandList = new List<string>();
                foreach (var commandM in Core.Commands) commandList.Add(commandM.Name);
                var comG = Common.SimilarWord(messageArray[0], commandList);

                Api.MessageSend($"❌ Неизвестная команда!" +
                    $"\n❓ Может быть. Вы хотели написать {comG}", msg.from_id);
            }
        }
    }
}
