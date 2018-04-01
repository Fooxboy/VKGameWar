using System;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace VKGame.Bot
{
 /// <summary>
 /// Класс для работы  с апи бота.
 /// </summary>
    public class Api
    {
        public class Registry
        {
            public static Models.Registry GetRegistry(long userId)
            {
                var json = "";
                try
                {
                    using (var reader = new StreamReader($@"Files/Registry/{userId}.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }catch(System.IO.IOException)
                {
                    Thread.Sleep(1500);
                    using (var reader = new StreamReader($@"Files/Registry/{userId}.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }
               
                return JsonConvert.DeserializeObject<Models.Registry>(json);
            }

            public static void SetRegistry(Models.Registry model)
            {
                var json = JsonConvert.SerializeObject(model);
                try
                {
                    using (var writer = new StreamWriter($@"Files/Registry/{model.Id}.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (var writer = new StreamWriter($@"Files/Registry/{model.Id}.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }
                
            }

            public static Models.Registry Register(long userId)
            {
                var model = new Models.Registry()
                {
                    Id = userId,
                    CountMessage = 0,
                    CountBattles = 0,
                    CountWinBattles = 0,
                    CountCreateBattles = 0,
                    LastMessage = String.Empty,
                    StartThread = false,
                    Credit = 0,
                    DateReg = String.Empty,
                    isSetup = false,
                    isHelp = false,
                    isReferal = false,
                    isBonusInGroupJoin = false,
                    isLeaveIsGroup = false
                };

                try
                {
                    using (File.Create($@"Files/Registry/{model.Id}.json"))
                    {

                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (File.Create($@"Files/Registry/{model.Id}.json"))
                    {

                    }
                }
                

                SetRegistry(model);
                return model;
            }
        }

        public class CacheMessages
        {
            public static Models.MessagesCache GetList()
            {
                var json = "";
                try
                {
                    using (var reader = new StreamReader(@"Files/CacheMessage.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }catch(IOException)
                {
                    Thread.Sleep(2000);
                    using (var reader = new StreamReader(@"Files/CacheMessage.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }
               
                return JsonConvert.DeserializeObject<Models.MessagesCache>(json);
            }

            public static void SetList(Models.MessagesCache model)
            {
                var json = JsonConvert.SerializeObject(model);
                try
                {
                    using (var writer = new StreamWriter(@"Files/CacheMessage.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }catch(IOException)
                {
                    Thread.Sleep(2000);
                    using (var writer = new StreamWriter(@"Files/CacheMessage.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }
               
            }
        }

        public class Competitions
        {
            private long id = 0;
            private Database.Methods db = new Database.Methods("Competitions");

            public static long New(string Name, long Price, long Time)
            {
                Database.Methods db = new Database.Methods("Competitions");
                var members = new List<Models.CompetitionsList.Member>();
                var membersJson = JsonConvert.SerializeObject(members);
                var top = new List<Models.CompetitionsList.TopMember>();
                var topJson = JsonConvert.SerializeObject(top);
                var id = (long)db.GetFromId(1, "Time") + 1;
                var fields = $"`Id`, `Name`, `Members`, `Price`, `Time`, `Top`";
                var value = $"'{id}', '{Name}', '{membersJson}','{Price}', '{Time}' , '{topJson}'";
                db.Edit(1, "Time", id);
                db.Add(fields, value);
                return id;
            }
            
            public Competitions(long compId)
            {
                id = compId;
            }

            public long Id => id;
            public string Name
            {
                get => (string)db.GetFromId(id, "Name");
                set => db.Edit(id, "Name", value);
            }

            public long FreeBattle
            {
                get => (long)db.GetFromId(id, "FreeBattle");
                set => db.Edit(id, "FreeBattle", value);
            }

            public List<Models.CompetitionsList.Member> Members
            {
                get
                {
                    string membersStr = (string)db.GetFromId(id, "Members");

                    if(membersStr == "[]")
                    {
                        var members = new List<Models.CompetitionsList.Member>();
                        return members;
                    }
                    else
                    {
                        string[] membersArray = membersStr.Split(',');
                        var members = new List<Models.CompetitionsList.Member>();
                        foreach (var member in membersArray) members.Add(JsonConvert.DeserializeObject<Models.CompetitionsList.Member>(member));
                        return members;
                    }
                    
                }set
                {
                    var members = value;
                    string memberStr = "";
                    foreach (var member in members) memberStr += $"{JsonConvert.SerializeObject(member)},";
                    memberStr = memberStr.Remove(memberStr.Length - 1);
                    db.Edit(id, "Members", memberStr);
                }
            }

            public List<Models.CompetitionsList.TopMember> Top
            {
                get
                {
                    string membersStr = (string)db.GetFromId(id, "Top");
                    if(membersStr == "[]")
                    {
                        var members = new List<Models.CompetitionsList.TopMember>();
                        return members;
                    }
                    else
                    {
                        string[] membersArray = membersStr.Split(',');
                        var members = new List<Models.CompetitionsList.TopMember>();
                        foreach (var member in membersArray) members.Add(JsonConvert.DeserializeObject<Models.CompetitionsList.TopMember>(member));
                        return members;
                    }         
                }
                set
                {
                    var members = value;
                    string memberStr = "";
                    foreach (var member in members) memberStr += $"{JsonConvert.SerializeObject(member)},";
                    memberStr = memberStr.Remove(memberStr.Length - 1);
                    db.Edit(id, "Top", memberStr);
                }
            }

            public bool isEnd
            {
                get => Convert.ToBoolean((long)db.GetFromId(id, "IsEnd"));
                set => db.Edit(id, "IsEnd", Convert.ToInt64(value));
            }

            public static bool Check(long CompId)
            {
                Database.Methods db = new Database.Methods("Competitions");
                return db.Check(CompId);
            }

            public long Price
            {
                get => (long)db.GetFromId(id, "Price");
                set => db.Edit(id, "Price", value);
            }

            public long Time
            {
                get => (long)db.GetFromId(id, "Time");
                set => db.Edit(id, "Time", value);
            }

            public static Models.CompetitionsList GetList()
            {
                var json = "";
                using (var reader = new StreamReader($@"Files/CompetitionsList.json"))
                {
                    json = reader.ReadToEnd();
                }
                return JsonConvert.DeserializeObject<Models.CompetitionsList>(json);
            }

            public static void SetList(Models.CompetitionsList model)
            {
                var json = JsonConvert.SerializeObject(model);
                using (var writer = new StreamWriter($@"Files/CompetitionsList.json", false, System.Text.Encoding.Default))
                {
                    writer.Write(json);
                }
            }
        }

        public class Referrals
        {
            public static Models.Referrals GetList(long userId)
            {
                var json = "";
                try
                {
                    using (var reader = new StreamReader($@"Files/ReferralsFiles/Refferals_{userId}.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (var reader = new StreamReader($@"Files/ReferralsFiles/Refferals_{userId}.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }
                
                return JsonConvert.DeserializeObject<Models.Referrals>(json);
            }

            public static void SetList(Models.Referrals model, long userId)
            {
                var json = JsonConvert.SerializeObject(model);
                try
                {
                    using (var writer = new StreamWriter($@"Files/ReferralsFiles/Refferals_{userId}.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (var writer = new StreamWriter($@"Files/ReferralsFiles/Refferals_{userId}.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }
                
            }
        }

        public class Roulette
        {
            public static Models.Roulette GetList()
            {
                var json = "";
                try
                {
                    using (var reader = new StreamReader(@"Files/Roultettes.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (var reader = new StreamReader(@"Files/Roultettes.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }
                
                return JsonConvert.DeserializeObject<Models.Roulette>(json);
            }

            public static void SetList(Models.Roulette model)
            {
                var json = JsonConvert.SerializeObject(model);
                try
                {
                    using (var writer = new StreamWriter(@"Files/Roultettes.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (var writer = new StreamWriter(@"Files/Roultettes.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }
                
            }
        }

        public class Clans
        {
            private long id = 0;
            private Database.Methods db = new Database.Methods("Clans");

            public Clans(long clan)
            {
                id = clan;
            }

            public static bool Check(long clanId)
            {
                Database.Methods db = new Database.Methods("Clans");
                return db.Check(clanId);
            }

            public static void Delete(long clanId)
            {
                Database.Methods db = new Database.Methods("Clans");
                db.Delete(clanId);
            }

            public static long New(long userId, string Name)
            {
                Database.Methods db = new Database.Methods("Clans");
                var id = (long)db.GetFromId(1, "Creator") + 1;
                var fields = $"`Id`, `Name`, `Members`, `Creator`";
                var value = $"'{id}', '{Name}', '{userId}','{userId}'";
                db.Edit(1, "Creator", id);
                db.Add(fields, value);
                return id;
            }

            public long Id
            {
                get => id;
            }
            public string Name
            {
                get => (string)db.GetFromId(id, "Name");
                set => db.Edit(id, "Name", value);
            }

            public List<long> Members
            {
                get
                {
                    string membersStr = (string)db.GetFromId(id, "Members");
                    string[] membersArray = membersStr.Split(',');
                    List<long> members = new List<long>();
                    foreach (var member in membersArray) members.Add(Int64.Parse(member));
                    return members;
                }
                set
                {
                    List<long> members = value;
                    string memberStr = "";
                    foreach (var member in members) memberStr += $"{member},";
                    memberStr = memberStr.Remove(memberStr.Length - 1);
                    db.Edit(id, "Member", memberStr);
                }
            }      

            public long Creator
            {
                get => (long)db.GetFromId(id, "Creator");
                set => db.Edit(id, "Creator", value);
            }

            public bool War
            {
                get => Convert.ToBoolean((long)db.GetFromId(id, "War"));
                set => db.Edit(id, "War", Convert.ToInt64(value));
            }

            public long WarId
            {
                get => (long)db.GetFromId(id, "WarId");
                set => db.Edit(id, "WarId", value);
            }

            public long Fund
            {
                get => (long)db.GetFromId(id, "Fund");
                set => db.Edit(id, "Fund", value);
            }

        }

        public class Quests
        {
            private long id = 0;
            private Database.Methods db = new Database.Methods("Quests");

            public Quests(long userId)
            {
                id = userId;
            }

            public long Id => id;

            public bool IsOnline
            {
                get => Convert.ToBoolean((long)db.GetFromId(id, "isOnline"));
                set => db.Edit(id, "isOnline", Convert.ToInt32(value));
            }

            public Models.Quests.Users Users
            {
                get
                {
                    var json = (string)db.GetFromId(id, "Users");
                    Models.Quests.Users model;
                    if (json == " ")
                    {
                        model = new Models.Quests.Users() { List = new List<Models.Quests.User>() };
                    }else
                    {
                        model = JsonConvert.DeserializeObject<Models.Quests.Users>(json);
                    }

                    return model;
                }
                set
                {
                    var json = JsonConvert.SerializeObject(value);
                    db.Edit(id, "Users", json);
                }
            }
            
            public static bool Check(long id)
            {
               Database.Methods db = new Database.Methods("Quests");
               return db.Check(id);
            }

            public string Name
            {
                get => (string)db.GetFromId(id, "Name");
                set => db.Edit(id, "Name", value);
            }

            public long Price
            {
                get => (long)db.GetFromId(id, "Price");
                set => db.Edit(id, "Price", value);
            }
            
        }

        public class Credit 
        {
            private long id = 0;
            private Database.Methods db = new Database.Methods("Credits");
            public Credit(long credit)
            {
                id = credit;
            }

            public static long New(long userId, long price) 
            {
                Database.Methods db = new Database.Methods("Credits");
                var id = (long)db.GetFromId(1, "Price") +1;
                var fields = $"`Id`, `Price`, `User`";
                var value = $"'{id}', '{price}', '{userId}'";
                db.Edit(1, "Price", id);
                db.Add(fields, value);
                return id;
            }

            public long Id  
            {
                get => id;
            }
            public long Price 
            {
                get => (long)db.GetFromId(id, "Price");
                set => db.Edit(id, "Price", value);
            }
            public long Time 
            {
                get => (long)db.GetFromId(id, "Time");
                set =>db.Edit(id, "Time", value);
            }
            public long User 
            {
                get => (long)db.GetFromId(id, "User");
                set => db.Edit(id, "User", value);
            }
        }

        public class Promocode 
        {
            private long id = 0;
            private Database.Methods db = new Database.Methods("Promocodes");
            public Promocode(long promo)
            {
                id = promo;
            }

            public static void Create(int Id, int Count, int money)
            {
                Database.Methods db = new Database.Methods("Promocodes");
                var fields = $"`Id`, `MoneyCard`, `Count`";
                var value = $"'{Id}', '{money}', '{Count}'";
                db.Add(fields, value);
            }

            public long Id 
            {
                get => id;
            }
            public long MoneyCard 
            {
                get => (long)db.GetFromId(id, "MoneyCard");
                set => db.Edit(id,"MoneyCard", value);
            }
            public static bool Check(long promocode) 
            {
                Database.Methods db = new Database.Methods("Promocodes");
                return db.Check(promocode);
            }

            public long Count 
            {
                get => (long)db.GetFromId(id, "Count");
                set => db.Edit(id, "Count", value);
            }

            public List<long> Users 
            {
                get 
                {
                    var usersString = (string)db.GetFromId(id, "Users");
                    Logger.WriteDebug(usersString);
                    if(usersString == "") return new List<long>();
                    var usersArray = usersString.Split(',');
                    var userList = new List<long>();
                    foreach(var userId in usersArray) 
                    {
                        Logger.WriteDebug(userId);
                        userList.Add(Int64.Parse(userId));
                    } 

                    return userList;
                }
                set 
                {
                    string users = "";
                    foreach(var userId in value) users = users + $"{userId},";
                    db.Edit(id, "Users", users);
                }
            }
        }

        public class CreditList
        {
            public static Models.CreditList GetList()
            {
                var json = "";
                try
                {
                    using (var reader = new StreamReader(@"Files/Credits.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (var reader = new StreamReader(@"Files/Credits.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }
                
                return JsonConvert.DeserializeObject<Models.CreditList>(json);
            }

            public static void SetList(Models.CreditList model)
            {
                var json = JsonConvert.SerializeObject(model);
                try
                {
                    using (var writer = new StreamWriter(@"Files/Credits.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (var writer = new StreamWriter(@"Files/Credits.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }
                
            }
        }

        public class UserList 
        {
            public static Models.UsersList GetList()
            {
                var json = "";
                try
                {
                    using (var reader = new StreamReader(@"Files/Users.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (var reader = new StreamReader(@"Files/Users.json"))
                    {
                        json = reader.ReadToEnd();
                    }
                }
                
                return JsonConvert.DeserializeObject<Models.UsersList>(json);
            }

            public static void SetList(Models.UsersList model) 
            {
                var json = JsonConvert.SerializeObject(model);
                try
                {
                    using (var writer = new StreamWriter(@"Files/Users.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }catch(IOException)
                {
                    Thread.Sleep(1500);
                    using (var writer = new StreamWriter(@"Files/Users.json", false, System.Text.Encoding.Default))
                    {
                        writer.Write(json);
                    }
                }
                
            }
        }

        public static bool MessageEdit(string text, long messageId, long peerId)
        {
            var vk = Common.GetVk();

            try
            {
                var result = vk.Messages.Edit(new MessageEditParams
                {
                    Message = text,
                    PeerId = peerId,
                    MessageId = messageId,
                    KeepForwardMessages = true,
                    KeepSnippets = true
                });
                return result;
            }
            catch(Exception e)
            {
                Logger.WriteError(e);
                Bot.Statistics.NewError();
                return false;
            }         
        }

        public static long MessageSend(string text, long peerId)
        {
            try
            {
                if (text == null) text = "❌ Выполнение кода ничего не вернуло.";

                //var data = new Common();
                var vk = Common.GetVk();

                var idMessage = vk.Messages.Send(new MessagesSendParams()
                {
                    UserId = peerId,
                    Message = text
                });
                return idMessage;
            }catch(Exception e)
            {
                Logger.WriteError(e);
                Bot.Statistics.NewError();
                return 0;
            }
           
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
                set => db.Edit(id, "Apartments", value);
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
                set => db.Edit(id, "WaterPressureStation", value);
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

        public class Battles : Models.IBattle
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
                   var Id = (long)database.GetFromId(1,"UserOne");
                   var battleId = Id+1;
                   database.Edit(1,"UserOne", (Id+1));
                    string fields = @"`Id`, `UserOne`, `HpOne`, `Price`, `Creator`, `UserCourse`, `Body`";
                    string values = $@"'{battleId}', '{UserId}', '{hp}', '{price}', '{UserId}', '{UserId}', '{body}'";
                    database.Add(fields, values);

                    return battleId;
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
                get => System.Convert.ToBoolean((long)db.GetFromId(id, "IsStart"));
                set => db.Edit(id, "IsStart", Convert.ToInt64(value));
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
                string fields = @"`Id`, `MoneyCard`";
                string values = $@"'{id}', '100'";
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

            public long TicketsCompetition
            {
                get => (long)db.GetFromId(Id, "TicketsCompetitions");
                set => db.Edit(Id, "TicketsCompetitions", value);
            }
        }
       
        public static class User
        {

            public static bool SetUser(Models.User user)
            {
                var database = new Database.Methods("Users");
                try
                {
                    database.Edit(user.Id, "Name", user.Name);
                    database.Edit(user.Id, "Level", user.Level);
                    database.Edit(user.Id, "IdBattle", user.IdBattle);
                    database.Edit(user.Id, "Experience", user.Experience);
                    database.Edit(user.Id, "Clan", user.Clan);
                    database.Edit(user.Id, "Competition", user.Competition);
                    database.Edit(user.Id, "Access", user.Access);
                    database.Edit(user.Id, "Quest", user.Quest);
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
                    long quest = 0;
                    try
                    {
                        quest = (long)database.GetFromId(id, "Quest");
                    }catch(Exception)
                    {
                        quest = 0;
                    }
                   
                    
                    var model = new Models.User()
                    {
                        Id = id,
                        Name = (string)database.GetFromId(id, "Name"),
                        Level = (long)database.GetFromId(id, "Level"),
                        IdBattle = (long)database.GetFromId(id, "IdBattle"),
                        Experience = (long)database.GetFromId(id, "Experience"),
                        Clan = (long)database.GetFromId(id, "Clan"),
                        Competition = (long)database.GetFromId(id, "Competition"),
                        Access = (long)database.GetFromId(id, "Access"),
                        Quest = quest
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
                    string fields = @"`Id`, `Name`";
                    var list = new List<long>();
                    //var common = new Common();
                    var name = Common.GetVk().Users.Get(new List<long>() {id})[0].FirstName;
                    string values = $@"'{id}', '{name}'";
                    database.Add(fields, values);
                    
                    Statistics.NewRegistation();
                    return true;
                }
            }
        }

        public class Boxes
        {
            long id = 0;
            private Database.Methods db = new Database.Methods("Boxes");
            public Boxes(long UserId)
            {
                id = UserId;
            }

            public static void Register(long userId)
            {
                Database.Methods db = new Database.Methods("Boxes");
                var fields = $"`Id`";
                var value = $"'{userId}'";
                db.Add(fields, value);
            }

            public long Id
            {
                get => id;
            }

            public List<Models.BattleBox> BattleBox
            {
                get
                {
                    var count = (long)db.GetFromId(id, "BattleBox");
                    var model = new List<Models.BattleBox>();
                    for(int i=0; count > i; i++)
                    {
                        model.Add(new Models.BattleBox());
                    }
                    return model;
                }
                set
                {
                    db.Edit(id, "BattleBox", value.Count);
                }
            }

            public List<Models.BuildBox> BuildBox
            {
                get
                {
                    var count = (long)db.GetFromId(id, "BuildBox");
                    var model = new List<Models.BuildBox>();
                    for (int i = 0; count > i; i++)
                    {
                        model.Add(new Models.BuildBox());
                    }
                    return model;
                }
                set
                {
                    db.Edit(id, "BuildBox", value.Count);
                }
            }
        }
    }
}