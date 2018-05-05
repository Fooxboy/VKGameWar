using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace VKGame.Bot.Helpers
{
    public static class Reboot
    {
        public static void BotOnWindows()
        {
            SaveData();
            Process.Start("reboot.bat");
            Logger.WriteWaring("Перезагрузка...");
            Process.GetCurrentProcess().Kill();
        }

        private static void SaveData()
        {
            var turnTanks = Bot.Common.TurnCreateTanks;
            var turnSoldiery = Bot.Common.TurnCreateSoildery;
            using(File.Create(@"Files/Temp/tempTanks.json"))
            {

            }
            using (File.Create(@"Files/Temp/tempSol.json"))
            {

            }
            var jsonTanks = JsonConvert.SerializeObject(turnTanks);
            var jsonSoildery = JsonConvert.SerializeObject(turnSoldiery);

            using(var writer = new StreamWriter(@"Files/Temp/tempTanks.json"))
            {
                writer.Write(jsonTanks);
            }

            using (var writer = new StreamWriter(@"Files/Temp/tempSol.json"))
            {
                writer.Write(jsonSoildery);
            }
            var registryBot = new RegistryBot();
            registryBot.RunForReboot = true;
        }

        public static void BotOnLinux()
        {
            SaveData();
            Process.Start("reboot.sh");
            Logger.WriteWaring("Перезагрузка...");
            Process.GetCurrentProcess().Kill();
        }
    }
}
