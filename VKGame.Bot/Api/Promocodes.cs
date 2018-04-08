﻿using System;
 using System.Collections.Generic;
 using VkNet.Model;
 
 namespace VKGame.Bot.Api
 {
     public class Promocodes
     {
         private Database.Data DB = null;
         private string promo;
 
         public Promocodes(string promo)
         {
             DB = new Database.Data("Credits");
             this.promo = promo;
         }
 
         public string Promo => promo;
 
         public long MoneyCard => (long)DB.GetFromKey("Promo", promo, "MoneyCard");
 
         public long Count
         {
             get => (long) DB.GetFromKey("Promo", promo, "Count");
             set => DB.EditFromKey("Promo", promo, "Count", value);
         }
 
         public List<long> Users
         {
             get
             {
                 var usersString = (string)DB.GetFromKey("Promo",promo, "Users");
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
                 users = users.Substring(0, users.Length -1);
                 DB.EditFromKey("Promo", promo, "Users", users);
             }
         }
 
         public static bool Check(string Promo) => Database.Data.CheckFromKey("Promo", Promo, "Promocodes");
         
         public static void Create(string Promo, string Count, string money)
         {
             var db = new Database.Data("Promocodes");
             var fields = new List<string>() { "Id", "MoneyCard", "Count"};
             var value = new List<string>() {Promo, money, Count };
             Database.Data.Add(fields, value, "Promocodes");
         }
     }
 }