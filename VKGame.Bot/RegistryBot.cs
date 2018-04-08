﻿using System;
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
            set => db.Edit(1, "RunForReboot", Convert.ToInt64(value));
        }

        public bool PlayInRulette
        {
            get => Convert.ToBoolean((long)db.GetFromId(1, "PlayInRulette"));
            set => db.Edit(1, "PlayInRulette", Convert.ToInt64(value));
        }
    }
}
