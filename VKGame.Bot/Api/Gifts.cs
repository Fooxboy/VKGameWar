using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot.Api
{
    /// <summary>
    /// Подарки
    /// </summary>
    public class Gifts
    {
        private Database.Data DB = null;
        private long id;

        public Gifts(long id)
        {
            DB = new Database.Data("Gifts");
            this.id = id;
        }

        public long Id => id;

        /// <summary>
        /// Открыт?
        /// </summary>
        public bool IsOpen
        {
            get => Convert.ToBoolean((long)DB.GetFromId(id, "isOpen"));
            set => DB.EditFromId(id, "isOpen", Convert.ToInt64(value));
        } 

        public long Price
        {
            get => (long)DB.GetFromId(id, "Price");
        }

        /// <summary>
        /// От кого
        /// </summary>
        public long From
        {
            get => (long)DB.GetFromId(id, "From");
        }

        /// <summary>
        /// Кому
        /// </summary>
        public long To
        {
            get => (long)DB.GetFromId(id, "ToUser");
        }

        public static bool Check(long id) => Database.Data.CheckFromId(id, "Gifts");
        

        /// <summary>
        /// Новый подарок
        /// </summary>
        /// <param name="from">отк кого</param>
        /// <param name="to">кому</param>
        /// <param name="price">внутри</param>
        /// <returns></returns>
        public static long New(long from, long to, long price)
        {
            var db = new Database.Data("Gifts");
            var id = (long)db.GetFromId(0, "isOpen");
            ++id;
            db.EditFromId(id, "isOpen", id);
            var fields = new List<string> {"Id", "Price", "From", "ToUser" };
            var values = new List<string> {
                id.ToString(),
                price.ToString(),
                from.ToString(),
                to.ToString()
            };
            Database.Data.Add(fields, values, "Gifts");

            return id;
        }
    }
}
