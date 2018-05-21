using System;
using System.Collections.Generic;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Боксы или ящики или хз ка кназвать
    /// </summary>
    public class Boxs
    {
        private Database.Data DB = null;
        private long id;
        
        /// <summary>
        /// Публичный конструктор
        /// </summary>
        /// <param name="id">ид пользователя</param>
        public Boxs(long id)
        {
            DB = new Database.Data("Boxs");
            this.id = id;
        }

        public long Id => id;

        /// <summary>
        /// Регистрация кейсов
        /// </summary>
        /// <param name="userId">ид пользователя</param>
        public static void Register(long userId)
        {
            List<string> fields = new List<string>() {"Id"};
            var values = new List<string>() {userId.ToString()};         
            Database.Data.Add(fields, values, "Boxs");
        }

        /// <summary>
        /// Получение кол-ва кейсов за битвы
        /// </summary>
        public long Battle
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Battle");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Battle", value);
        }
        
        /// <summary>
        /// Получение кол-ва кейсов "битвенных"
        /// </summary>
        public long Build
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Build");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Build", value);
        }
        
        /// <summary>
        /// ПРоучение кол-ва кейсов "ВИП"
        /// </summary>
        public long Vip
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Vip");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Vip", value);
        }
        
        /// <summary>
        /// Получение кол-ва кейсов "тест"
        /// </summary>
        public long Test
        {
            get
            {
                long defaultValue = 0;
                var result = DB.GetFromId(id, "Test");
                if (result is DBNull) return defaultValue;
                return (long) result;
            }
            set => DB.EditFromId(id, "Test", value);
        }
    }
}