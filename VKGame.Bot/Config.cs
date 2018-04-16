using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace VKGame.Bot
{
    public class Config
    {
        public string Version { get; set; } = "1.4";
        public bool IsDebug { get; set; } = true;

        public static Config Get()
        {
            return new Config();
        }
    }
}
