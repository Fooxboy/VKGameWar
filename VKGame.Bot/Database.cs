using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace VKGame.Bot
{
    /// <summary>
    /// Класс для работы методов БД.
    /// </summary>
    public class Database
    {
        public static SqliteConnection ConnectionData = null;

        public static SqliteConnection ConnectionStat = null;


        public class Stat
        {
            private const string ConnectionString = @"Filename=Files/Stat.db;";
            private string Table = null;    
            
            public Stat(string table)
            {
                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }
               
                Table = table;
            }

            public static void DeteleAll(string Table)
            {
                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }
                string sql = $"DELETE FROM {Table}";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }

            public SqliteDataReader  GetAll()
            {
                var sql = $"SELECT * FROM {Table}";
                var command = new SqliteCommand(sql, ConnectionStat);
                var reader = command.ExecuteReader();
                return reader;
            }

            public object GetFromId(object id, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                var result = command.ExecuteScalar();
                return result;
            }

            public object GetFromKey(object key, object value, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                var result = command.ExecuteScalar();
                return result;
            }
            
            public object GetFromKey(string key)
            {
                var sql = $"SELECT Value FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                var result = command.ExecuteScalar();
                return result;
            }

            public void EditFromId(object id, string field, object value)
            {
                var sql = $"UPDATE {Table}  SET `{field}`='{value}' WHERE `Id`='{id}';";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
                
            }
            
            public void EditFromKey(object key, object valueKey, string field, object value)
            {
                var sql = $"UPDATE {Table}  SET `{field}`='{value}' WHERE `{key}`='{valueKey}';";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }
            
            public void EditFromKey(string key, object value)
            {
                var sql = $"UPDATE {Table}  SET `Value`='{value}' WHERE `Key`='{key}';";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }

            public static bool CheckFromId(object id, string Table)
            {
                string sql = $"SELECT * FROM `{Table}` WHERE Id = '{id}'";
                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }

                var command = new SqliteCommand(sql, ConnectionStat);
                var reader = command.ExecuteReader();
                bool response = reader.Read();
                reader.Close();
                return response;
            }

            public static bool CheckFromKey(object key, string value, string Table)
            {
                string sql = $"SELECT * FROM `{Table}` WHERE {key} = '{value}'";

                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }
                var command = new SqliteCommand(sql, ConnectionStat);
                var reader = command.ExecuteReader();
                bool response = reader.Read();
                reader.Close();
                return response;
            }
            
            public static bool CheckFromKey(string key, string Table)
            {
                string sql = $"SELECT * FROM `{Table}` WHERE key = '{key}'";

                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }
                var command = new SqliteCommand(sql, ConnectionStat);
                var reader = command.ExecuteReader();
                bool response = reader.Read();
                reader.Close();
                return response;
            }

            public void DeleteFromId(object id)
            {
                string sql = $"DELETE FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }
            
            public void DeleteFromId(string key)
            {
                string sql = $"DELETE FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }

            public void DeleteFromKey(object key, string value)
            {
                string sql = $"DELETE FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }

            public static void Add(List<string> fields, List<string> values, string Table)
            {
                var fieldsText = string.Empty;
                var valuesText = string.Empty;

                foreach (var field in fields) fieldsText += $"`{field}`, ";
                foreach (var value in values) valuesText += $"'{value}', ";
                fieldsText = fieldsText.Substring(0, fieldsText.Length - 2);
                valuesText = valuesText.Substring(0, valuesText.Length - 2);
                
                string sql = $@"INSERT INTO {Table} ({fieldsText}) VALUES ({valuesText});";
                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }

                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }
            
        }
        
        public class Data
        {
            private const string ConnectionString = @"Filename=Files/Data.db;";
            private string Table = null;    
            
            public Data(string table)
            {
                if(ConnectionData == null)
                {
                    ConnectionData = new SqliteConnection(ConnectionString);
                    ConnectionData.Open();
                }
                Table = table;
                Logger.NewMessage($"ТАБЛИЦА {table}");
            }

            public SqliteDataReader  GetAll()
            {
                var sql = $"SELECT * FROM {Table}";
                var command = new SqliteCommand(sql, ConnectionData);
                var reader = command.ExecuteReader();
                return reader;
            }
            
            public static void DeteleAll(string Table)
            {
                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }
                string sql = $"DELETE FROM {Table}";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }

            public object GetFromId(object id, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionData);
                var result = command.ExecuteScalar();
                return result;
            }

            public object GetFromKey(object key, object value, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionData);
                var result = command.ExecuteScalar();
                return result;
            }
            
            public object GetFromKey(string key)
            {
                var sql = $"SELECT Value FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionData);
                var result = command.ExecuteScalar();
                return result;
            }

            public void EditFromId(object id, string field, object value)
            {
                var sql = $"UPDATE {Table} SET `{field}`='{value}' WHERE `Id`='{id}';";
                Logger.NewMessage(sql);
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }
            
            public void EditFromKey(object key, object valueKey, string field, object value)
            {
                var sql = $"UPDATE {Table}  SET `{field}`='{value}' WHERE `{key}`='{valueKey}';";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }
            
            public void EditFromKey(string key, object value)
            {
                var sql = $"UPDATE {Table}  SET `Value`='{value}' WHERE `Key`='{key}';";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }

            public static bool CheckFromId(object id, string Table)
            {
                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }
                string sql = $"SELECT * FROM `{Table}` WHERE Id = '{id}'";
                var command = new SqliteCommand(sql, ConnectionData);
                var reader = command.ExecuteReader();
                bool response = reader.Read();
                reader.Close();
                return response;
            }

            public static bool CheckFromKey(object key, string value, string Table)
            {
                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }
                string sql = $"SELECT * FROM `{Table}` WHERE {key} = '{value}'";
                var command = new SqliteCommand(sql, ConnectionData);
                var reader = command.ExecuteReader();
                bool response = reader.Read();
                reader.Close();
                return response;
            }
            
            public static bool CheckFromKey(string key, string Table)
            {
                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }
                string sql = $"SELECT * FROM `{Table}` WHERE Key = '{key}'";
                var command = new SqliteCommand(sql, ConnectionData);
                var reader = command.ExecuteReader();
                bool response = reader.Read();
                reader.Close();
                return response;
            }

            public void DeleteFromId(object id)
            {
                string sql = $"DELETE FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }
            
            public void DeleteFromKey(string key)
            {
                string sql = $"DELETE FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }

            public void DeleteFromKey(object key, string value)
            {
                string sql = $"DELETE FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }

            public static void Add(List<string> fields, List<string> values, string Table)
            {
                if (ConnectionStat == null)
                {
                    ConnectionStat = new SqliteConnection(ConnectionString);
                    ConnectionStat.Open();
                }
                var fieldsText = string.Empty;
                var valuesText = string.Empty;

                foreach (var field in fields) fieldsText += $"`{field}`, ";
                foreach (var value in values) valuesText += $"'{value}', ";
                fieldsText = fieldsText.Substring(0, fieldsText.Length - 2);
                valuesText = valuesText.Substring(0, valuesText.Length - 2);
                
                string sql = $@"INSERT INTO {Table} ({fieldsText}) VALUES ({valuesText});";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }
            
        }
        
        
        /// <summary>
        /// Список методов.
        /// </summary>
        public class Methods
        {
            private string database = null;
            private string connectString = null;
            private string table = null;
            //private Methods methods = null;
            //SqliteConnection connection = null;
            
            public Methods(string db)
            {
                database = db;
                table = db;
                connectString = $@"Filename=Files/{database}.db;";
            }
            
            /// <summary>
            /// Получение по ид
            /// </summary>
            /// <param name="id"></param>
            /// <param name="Поле"></param>
            /// <returns></returns>
            public object GetFromId(long id, string field)
            {
                using (var connect = new SqliteConnection(connectString))
                {
                    connect.Open();
                    string commandText = $"SELECT {field} FROM {table} WHERE Id='{id}'" ;
                    var command = new SqliteCommand(commandText, connect);
                    var result = command.ExecuteScalar();
                    return result;
                }
            }
            
            /// <summary>
            /// Изменить поле
            /// </summary>
            /// <param name="id"></param>
            /// <param name="Поле"></param>
            /// <param name="Значение"></param>
            public void Edit(long id, string field, object value)
            {
                using (var connect = new SqliteConnection(connectString))
                {
                    connect.Open();
                    string sql = $"UPDATE {table}  SET `{field}`='{value}' WHERE `Id`='{id}';";
                    var command = new SqliteCommand(sql, connect);
                    command.ExecuteNonQuery();
                }
            }
            
            /// <summary>
            /// Проверить если ли в бд.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool Check(long id)
            {
                using (var connect = new SqliteConnection(connectString))
                {
                    connect.Open();
                    string hui = "CheckFrom";
                    string sql = $"SELECT {hui} FROM {table} WHERE Id={id}";
                    var command = new SqliteCommand(sql, connect);
                    var reader = command.ExecuteReader();
                    bool response = reader.Read();
                    reader.Close();
                    return response;
                }        
            }
            
            /// <summary>
            /// Удалить запись.
            /// </summary>
            /// <param name="id"></param>
            public void Delete(long id)
            {
                using (var connect = new SqliteConnection(connectString))
                {
                    connect.Open();
                    string sql = $"DELETE FROM {table} WHERE Id={id}";
                    var command = new SqliteCommand(sql, connect);
                    command.ExecuteNonQuery();
                }
            }
            
            /// <summary>
            /// Добавить запись.
            /// </summary>
            /// <param name="fields"></param>
            /// <param name="values"></param>
            public void Add(string fields, string values)
            {
                using (var connect = new SqliteConnection(connectString))
                {
                    connect.Open();
                    string sql = $@"INSERT INTO {table} ({fields}) VALUES ({values});";
                    var command = new SqliteCommand(sql, connect);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}