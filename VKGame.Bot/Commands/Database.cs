using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using VKGame.Bot.Models;


namespace VKGame.Bot.Commands
{
    public class Database : ICommand
    {
        public string Name => "Бд";
        public string Arguments => "";
        public string Caption => "";
        public TypeResponse Type => TypeResponse.Text;

        public object Execute(Message msg)
        {
            var user = Api.User.GetUser(msg.from_id);

            if (user.Access < 4)
            {
                var messageArray = msg.body.Split(' ');
                if (messageArray.Length == 1)
                    return "не указаны аргументы";
                else
                {
                    var type = typeof(Database);
                    object obj = Activator.CreateInstance(type);
                    var methods = type.GetMethods();

                    foreach (var method in methods)
                    {
                        var attributesCustom = Attribute.GetCustomAttributes(method);

                        foreach (var attribute in attributesCustom)
                        {
                            if (attribute.GetType() == typeof(Attributes.Trigger))
                            {
                                var myAtr = ((Attributes.Trigger)attribute);

                                if (myAtr.Name == messageArray[1])
                                {
                                    object result = method.Invoke(obj, new object[] { msg });
                                    return (string)result;
                                }
                            }
                        }
                    }
                    return "❌ Неизвестная подкоманда.";
                }
            }
            else return "Вам недоступна команда!";     
        }

        [Attributes.Trigger("изменить")]
        public static string Edit(Message msg)
        {
            try
            {
                var messageArray = msg.body.Split(' ');

                var database = messageArray[2];
                var clumn = messageArray[3];
                var id = messageArray[4];
                string value = String.Empty;
                try
                {
                    for (int i = 5; messageArray.Length > i; i++) value += $"{messageArray[i]} ";
                }
                catch (IndexOutOfRangeException)
                {
                    value = "";
                }

                var db = new Bot.Database.Methods(database);

                db.Edit(Int64.Parse(id), clumn, value);

                return $"Вы успешно изменили изменили значение в таблице {database} на {value} для пользователя с id {id} в колонке {clumn}";

            }catch(Exception e)
            {
                return $"{e.Message} \n {e.StackTrace}";

            }
        }

        [Attributes.Trigger("получить")]
        public static string Get(Message msg)
        {
            try
            {
                var messageArray = msg.body.Split(' ');
                var database = messageArray[2];
                var clumn = messageArray[3];
                var id = messageArray[4];

                var db = new Bot.Database.Methods(database);
                var value = (string)db.GetFromId(Int64.Parse(id), clumn);
                return value;
            }catch(Exception e)
            {
                return $"{e.Message} \n {e.StackTrace}";
            }
            

        }

        [Attributes.Trigger("sql")]
        public static string Sql(Message msg)
        {
            try
            {
                var messageArray = msg.body.Split(' ');
                var database = messageArray[2];
                var column = messageArray[3];
                var connectString = $@"Filename=Files/{database}.db;";
                var sqlStr = String.Empty;
                for (int i = 4; messageArray.Length > i; i++) sqlStr += $"{messageArray[i]} ";

                var result = "";

                using (var connect = new SqliteConnection(connectString))
                {
                    connect.Open();
                    var command = new SqliteCommand(sqlStr, connect);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        result += $"{reader[column]} \n";
                    }
                }
                return result;
            }catch(Exception e)
            {
                return $"{e.Message} \n {e.StackTrace}";
            }

        }
    }
}
