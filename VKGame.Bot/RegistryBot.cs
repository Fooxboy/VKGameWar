using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot
{
    public class RegistryBot
    {
        private Database.Methods db = new Database.Methods("RegistryBot");

        public long Id => 0;

        public bool RunForReboot
        {
            get => Convert.ToBoolean((long)db.GetFromId(1, "RunForReboot"));
            set => db.Edit(1, "RunForReboot", value);
        }
    }
}
