using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VKGame.Bot.Models
{
    public class Boxes
    {
        public struct Battle
        {
            public long Money
            {
                get
                {
                    var r = new Random();
                    var rMain = r.Next(1, 3);
                    if (rMain == 1 || rMain == 2)
                        return r.Next(5, 50);
                    else return r.Next(10, 80);
                }
            }

            public long Soldiery
            {
                get
                {
                    var r = new Random();
                    var rMain = r.Next(1, 2);
                    if (rMain == 1) return r.Next(8, 15);
                    return r.Next(10, 18);
                }
            }

            public long Tanks
            {
                get
                {
                    var r = new Random();
                    var rMain = r.Next(1, 2);
                    if (rMain == 1) return r.Next(0, 3);
                    return r.Next(1, 5);
                }
            }

            public long Food
            {
                get
                {
                    var r = new Random();
                    var rMain = r.Next(1, 2);
                    if (rMain == 2) return r.Next(10, 50);
                    return r.Next(20, 80);
                }
            }
        }

        public struct Build
        {
            public long Count
            {
                get
                {
                    var r = new Random();
                    var rMain = r.Next(1, 2);
                    if (rMain == 1) return 0;

                    return r.Next(2, 7);
                }
            }
        }
    }
}
