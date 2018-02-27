﻿using System;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace VKGame.Bot
{
 /// <summary>
 /// Класс для работы  с апи бота.
 /// </summary>
    public class Api
    {
        public static void MessageSend(string text, long peerId)
        {
            var data = new Common();
            var vk = data.GetVk();

            vk.Messages.Send(new MessagesSendParams()
            {
                UserId = peerId,
                Message = text
            });
            Statistics.SendMessage();
        }

        public class Builds: Models.IBuilds 
        {
            private long id =0;
            private Database.Methods db = new Database.Methods("Builds");
            public Builds(long userId)
            {
                id = userId;
            }

            public static void Register(long id)
            {
                var database = new Database.Methods("Builds");
                string fields = @"`Id`";
                string values = $@"'{id}'";
                database.Add(fields, values);
            }
            public long Id => id;
            /// <summary>
        /// Жилые квартиры.
        /// </summary>
            public long Apartments {
                get => (long) db.GetFromId(Id, "Apartments");
                set => db.Edit(id, "Food", value);
            }
            /// <summary>
        /// Генераторы электроэнергии.
        /// </summary>
            public long PowerGenerators {
                get => (long) db.GetFromId(Id, "PowerGenerators");
                set => db.Edit(id, "PowerGenerators", value);
            }
            /// <summary>
        /// Шахты добычи нефти.
        /// </summary>
            public long Mine {
                get => (long) db.GetFromId(Id, "Mine");
                set => db.Edit(id, "Mine", value);
            }

            /// <summary>
        /// Водонапорные башни.
        /// </summary>
            public long WaterPressureStation {
                get => (long) db.GetFromId(Id, "WaterPressureStation");
                set => db.Edit(id, "Food", value);
            }
            /// <summary>
        /// Закусочные.
        /// </summary>
            public long Eatery {
                get => (long) db.GetFromId(Id, "Eatery");
                set => db.Edit(id, "Eatery", value);
            }
             /// <summary>
        /// Батарейки
        /// </summary>
            public long WarehouseEnergy {
                get => (long) db.GetFromId(Id, "WarehouseEnergy");
                set => db.Edit(id, "WarehouseEnergy", value);
            }
 /// <summary>
        /// Бочки с водой
        /// </summary>
            public long WarehouseWater {
                get => (long) db.GetFromId(Id, "WarehouseWater");
                set => db.Edit(id, "WarehouseWater", value);
            }

 /// <summary>
        /// Холодильники
        /// </summary>
            public long WarehouseEat {
                get => (long) db.GetFromId(Id, "WarehouseEat");
                set => db.Edit(id, "WarehouseEat", value);
            }
 /// <summary>
        /// Ангары
        /// </summary>

             public long Hangars {
                get => (long) db.GetFromId(Id, "Hangars");
                set => db.Edit(id, "Hangars", value);
            }
        }

        public class Battles 
        {
            private long id = 0;
            private Database.Methods db = new Database.Methods("Battles");
            public long Id => id;

            public static Models.ActiveBattles GetListBattles() 
            {
                var json = "";
                using(var reader = new StreamReader(@"Files/Battles.json")) 
                {
                    json = reader.ReadToEnd();
                }
                return JsonConvert.DeserializeObject<Models.ActiveBattles>(json);
            }

            public static void SetListBattles(Models.ActiveBattles model) 
            {
                var json = JsonConvert.SerializeObject(model);
                using(var writer = new StreamWriter(@"Files/Battles.json", false, System.Text.Encoding.Default)) 
                {
                    writer.Write(json);
                }
            }

            public static long NewBattle(long UserId, string body, long hp, int price)
            {
                var database = new Database.Methods("Battles");
               // if (database.Check(id)) return false;
               // else
               // {
                   var Id = (long)database.GetFromId(1,"UserOne");
                   var battleId = Id+1;
                   database.Edit(1,"UserOne", (Id+1));
                    string fields = @"`Id`, `UserOne`, `HpOne`, `Price`, `Creator`, `UserCourse`, `Body`";
                    string values = $@"'{battleId}', '{UserId}', '{hp}', '{price}', '{UserId}', '{UserId}', '{body}'";
                    database.Add(fields, values);

                   // Statistics.NewRegistation();
                    return battleId;
                //}
            }
            public Battles(long Id ) 
            {
id = Id;
            }

            public static bool Check(long id) 
            {
                var database = new Database.Methods("Battles");
                return database.Check(id);
            }

            public bool IsStart  
            {
                get => (bool)db.GetFromId(id, "IsStart");
                set => db.Edit(id, "IsStart", value);
            }

            public long UserOne 
            {
                get => (long)db.GetFromId(id, "UserOne");
                set => db.Edit(id, "UserOne", value);
            }

            public long UserTwo
            {
                get => (long)db.GetFromId(id, "UserTwo");
                set => db.Edit(id, "UserTwo", value);
            }

            public long HpOne
            {
                get => (long)db.GetFromId(id, "HpOne");
                set => db.Edit(id, "HpOne", value);
            }

            public long HpTwo
            {
                get => (long)db.GetFromId(id, "HpTwo");
                set => db.Edit(id, "HpTwo", value);
            }

            public long Creator
            {
                get => (long)db.GetFromId(id, "Creator");
                set => db.Edit(id, "Creator", value);
            }

            public long UserCourse
            {
                get => (long)db.GetFromId(id, "UserCourse");
                set => db.Edit(id, "UserCourse", value);
            }

            public long Price
            {
                get => (long)db.GetFromId(id, "Price");
                set => db.Edit(id, "Price", value);
            }

            public string Body
            {
                get => (string)db.GetFromId(id, "Body");
                set => db.Edit(id, "Body", value);
            }
        }

        public class Resources: Models.IResources
        {
            private long id = 0;
            private Database.Methods db = new Database.Methods("Resources");
            
            public Resources(long userId)
            {
                id = userId;
            }

            public static void Register(long id)
            {
                var database = new Database.Methods("Resources");
                string fields = @"`Id`";
                string values = $@"'{id}'";
                database.Add(fields, values);
            }

            public long Id
            {
                get => id;
            }
            /// <summary>
            /// Еда
            /// </summary>
            public long Food
            {
                get => (long) db.GetFromId(Id, "Food");
                set => db.Edit(id, "Food", value);
            }
            
            /// <summary>
            /// Наличные монеты.
            /// </summary>
            public long Money
            {
                get => (long) db.GetFromId(Id, "Money");
                set => db.Edit(id, "Money", value);
            }
            
            /// <summary>
            /// Монеты в банковском счету.
            /// </summary>
            public long MoneyCard
            {
                get => (long) db.GetFromId(Id, "MoneyCard");
                set => db.Edit(id, "MoneyCard", value);
            }
            
            /// <summary>
            /// Энергия
            /// </summary>
            public long Energy
            {
                get => (long) db.GetFromId(Id, "Energy");
                set => db.Edit(id, "Energy", value);
            }
            
            /// <summary>
            /// Вода
            /// </summary>
            public long Water
            {
                get => (long) db.GetFromId(Id, "Water");
                set => db.Edit(id, "Water", value);
            }
            
            /// <summary>
            /// Солдаты
            /// </summary>
            public long Soldiery
            {
                get => (long) db.GetFromId(Id, "Soldiery");
                set => db.Edit(id, "Soldiery", value);
            }
            
            /// <summary>
            /// Танки
            /// </summary>
            public long Tanks
            {
                get => (long) db.GetFromId(Id, "Tanks");
                set => db.Edit(id, "Tanks", value);
            }
        }
        
        /// <summary>
        /// Класс для работы с пользователем.
        /// </summary>
        public static class User
        {

            public static bool SetUser(Models.User user)
            {
                var database = new Database.Methods("Users");
                try
                {
                    database.Edit(user.Id, "Name", user.Name);
                    database.Edit(user.Id, "Setup", Convert.ToInt32(user.isSetup));
                    database.Edit(user.Id, "Level", user.Level);
                    database.Edit(user.Id, "CountBattles", user.CountBattles);
                    database.Edit(user.Id, "CountWinBattles", user.CountWinBattles);
                    database.Edit(user.Id, "CountCreateBattles", user.CountCreateBattles);
                    database.Edit(user.Id, "IdBattle", user.IdBattle);
                    return true;
                }
                catch
                {
                    return false;
                }
                
            }
            
            public static Models.User GetUser(long id)
            {
                var database = new Database.Methods("Users");
                if (database.Check(id))
                {
                    var model = new Models.User()
                    {
                        Id = id,
                        Name = (string) database.GetFromId(id, "Name"),
                        DateReg = (string) database.GetFromId(id, "TimeReg"),
                        Level = (long)database.GetFromId(id, "Level"),
                        CountBattles = (long)database.GetFromId(id, "CountBattles"),
                        CountWinBattles = (long)database.GetFromId(id, "CountWinBattles"),
                        CountCreateBattles = (long)database.GetFromId(id, "CountCreateBattles"),
                        IdBattle = (long)database.GetFromId(id, "IdBattle")
                    };
                    return model;
                }
                else return null;
            }
            
            /// <summary>
            /// Создание нового юзера.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public static bool NewUser(long id)
            {
                var database = new Database.Methods("Users");
                if (database.Check(id)) return false;
                else
                {
                    string fields = @"`Id`, `Name`, `TimeReg`";
                    var list = new List<long>();
                    list.Add(id);
                    var common = new Common();
                    var name = common.GetVk().Users.Get(new List<long>() {id})[0].FirstName;
                    string values = $@"'{id}', '{name}', '{DateTime.Now}'";
                    database.Add(fields, values);
                    
                    Statistics.NewRegistation();
                    return true;
                }
            }
        }
    }
}