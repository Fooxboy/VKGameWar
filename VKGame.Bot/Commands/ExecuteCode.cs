using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace VKGame.Bot.Commands
{
    public class ExecuteCode:ICommand
    {
        public string Name => "exe";
        public string Caption => "ааа";
        public string Arguments => "ааа";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(LongPollVK.Models.AddNewMsg msg)
        {
            var messageArray = msg.Text.Split(' ');

            var user = Api.User.GetUser(msg.PeerId);
            if (user.Access < 4) return "Вам недоступна эта команда.";
            string code = String.Empty;

            for (int i = 1; messageArray.Length > i; i++) code += $"{messageArray[i]} ";

            code = code.Replace("&quot;", "\"");
            code = code.Replace("&lt;", "<");
            code = code.Replace("&gt;", ">");

            Logger.WriteDebug($"ВЫПОЛНЕНИЕ КОДА: {code}");
            try
            {
                var result = CSharpScript.EvaluateAsync(code,
                ScriptOptions.Default.WithReferences(typeof(Program).Assembly));
                return result.Result.ToString();
            }catch(CompilationErrorException e)
            {
                return e.Message;
            }
            
        }
    }
}
