using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.PublicAPI.Yarik
{
    public static class Damage
    {
        public static long Soildery
        {
            get
            {
                var db = new Database.Public("Damage");
                return (long)db.GetFromKey("Soildery");
            }
            set
            {
                var db = new Database.Public("Damage");
                db.EditFromKey("Soildery", value);
            }
        }

        public static long SoilderyLevel
        {
            get
            {
                var db = new Database.Public("Damage");
                return (long)db.GetFromKey("SoilderyLevel");
            }set
            {
                var db = new Database.Public("Damage");
                db.EditFromKey("SoilderyLevel", value);
            }
        }
    }
}
