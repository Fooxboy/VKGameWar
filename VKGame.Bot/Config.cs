using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace VKGame.Bot
{
    public class Config
    {
        public string Version { get; set; }
        public bool IsDebug { get; set; }

        public static Config Get()
        {
            var json = String.Empty;
            using(var reader = new StreamReader(@"Files/Config.json"))
            {
                json = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Config>(json);
        }
    }
}
