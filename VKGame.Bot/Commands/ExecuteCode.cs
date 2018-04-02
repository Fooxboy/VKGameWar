using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using VKGame.Bot.Models;

namespace VKGame.Bot.Commands
{
    public class ExecuteCode:ICommand
    {
        public string Name => "exe";
        public string Caption => "ааа";
        public string Arguments => "ааа";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>();
        public Access Access => Access.Admin;


        public object Execute(Message msg)
        {
            var messageArray = msg.body.Split(' ');

            var user = Api.User.GetUser(msg.from_id);
            if (user.Access < 4) return "Вам недоступна эта команда.";
            string code = String.Empty;

            for (int i = 1; messageArray.Length > i; i++) code += $"{messageArray[i]} ";

            code = code.Replace("&quot;", "\"");
            code = code.Replace("&lt;", "<");
            code = code.Replace("&gt;", ">");

            Logger.WriteWaring($">>>>>ВЫПОЛНЕНИЕ КОДА: {code}");
            try
            {
                var result = CSharpScript.EvaluateAsync($"using System; using VKGame.Bot;" +
                    $" using System.Collections.Generic; using System.Threading; " +
                    $"using System.IO; using System.Net; using Newtosoft.Json;" +
                    $"using System.Text; using VNet; {code}",
                ScriptOptions.Default.WithReferences(typeof(Program).Assembly));
                var resultA = result.Result.ToString();
                if (resultA == null) resultA = "Результат выполнения кода равен null";
                return resultA;
            }catch(CompilationErrorException e)
            {
                return e.Message;
            }
            
        }
    }
}
