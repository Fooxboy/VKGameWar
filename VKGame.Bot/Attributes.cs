namespace VKGame.Bot
{
    /// <summary>
    /// Атрибуты для методов и классов
    /// </summary>
    public class Attributes
    {
        /// <summary>
        /// Атрибут реакции на подкоманду. 
        /// </summary>
        public class Trigger : System.Attribute
        {
            /// <summary>
            /// Имя
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Атрибут реакции на подкоманду.
            /// </summary>
            /// <param name="Имя подкоманды"></param>
            public Trigger(string name)
            {
                Name = name;
            }
        }
    }
}