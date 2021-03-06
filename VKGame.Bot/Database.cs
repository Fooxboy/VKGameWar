using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace VKGame.Bot
{
    /*=============================================================
     * Здесь собраны все методы для работы с каждой БД
     * Странно, что для каждой БД используется отдельный класс
     * Причем, в классе этом одинаковые методы
     * Нужно будет переписать когда-нибудь
     * Но работает и так
     * Так шо похуй)0))(0
     * =============================================================*/

    /// <summary>
    /// Класс для работы методов БД.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Соеденение с БД data
        /// </summary>
        public static SqliteConnection ConnectionData = null;

        /// <summary>
        /// Соеденение с БД Stat
        /// </summary>
        public static SqliteConnection ConnectionStat = null;

        /// <summary>
        /// Соеденение с БД Public
        /// </summary>
        public static SqliteConnection ConnectionPublic = null;

        /// <summary>
        /// Класс для работы с базой данных Public
        /// </summary>
        public class Public
        {
            private const string ConnectionString = @"Filename=Files/Public.db;";
            private string Table = null;

            public Public(string table)
            {
                if (ConnectionPublic == null)
                {
                    ConnectionPublic = new SqliteConnection(ConnectionString);
                    ConnectionPublic.Open();
                }

                Table = table;
            }

            /// <summary>
            /// Удалить все
            /// </summary>
            /// <param name="Table">Таблица</param>
            public static void DeteleAll(string Table)
            {
                if (ConnectionPublic == null)
                {
                    ConnectionPublic = new SqliteConnection(ConnectionString);
                    ConnectionPublic.Open();
                }
                string sql = $"DELETE FROM {Table}";
                var command = new SqliteCommand(sql, ConnectionPublic);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Получить все
            /// </summary>
            /// <returns>скл ридер</returns>
            public SqliteDataReader GetAll()
            {
                var sql = $"SELECT * FROM {Table}";
                var command = new SqliteCommand(sql, ConnectionPublic);
                var reader = command.ExecuteReader();
                return reader;
            }

            /// <summary>
            /// Получить по ИД
            /// </summary>
            /// <param name="id">Ид</param>
            /// <param name="field">Поле</param>
            /// <returns>Результат</returns>
            public object GetFromId(object id, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionPublic);
                var result = command.ExecuteScalar();
                return result;
            }

            /// <summary>
            /// Получить по ключу
            /// </summary>
            /// <param name="key">Название ключа</param>
            /// <param name="value">Значение ключа</param>
            /// <param name="field">Поле</param>
            /// <returns>Результат</returns>
            public object GetFromKey(object key, object value, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionPublic);
                var result = command.ExecuteScalar();
                return result;
            }

            /// <summary>
            /// Получить по ключу с именем Key
            /// </summary>
            /// <param name="key">Значение ключа</param>
            /// <returns>Результат</returns>
            public object GetFromKey(string key)
            {
                var sql = $"SELECT Value FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionPublic);
                var result = command.ExecuteScalar();
                return result;
            }

            /// <summary>
            /// Редактировать по ид
            /// </summary>
            /// <param name="id">ид</param>
            /// <param name="field">поле</param>
            /// <param name="value">новое значение</param>
            public void EditFromId(object id, string field, object value)
            {
                var sql = $"UPDATE {Table}  SET `{field}`='{value}' WHERE `Id`='{id}';";
                var command = new SqliteCommand(sql, ConnectionPublic);
                command.ExecuteNonQuery();

            }

            /// <summary>
            /// Редактировать по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="valueKey">Значение ключа</param>
            /// <param name="field">Поле</param>
            /// <param name="value">новое значение</param>
            public void EditFromKey(object key, object valueKey, string field, object value)
            {
                var sql = $"UPDATE {Table}  SET `{field}`='{value}' WHERE `{key}`='{valueKey}';";
                var command = new SqliteCommand(sql, ConnectionPublic);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Изменить по ключу с именем key
            /// </summary>
            /// <param name="key">значение ключа</param>
            /// <param name="value">новое значение</param>
            public void EditFromKey(string key, object value)
            {
                var sql = $"UPDATE {Table}  SET `Value`='{value}' WHERE `Key`='{key}';";
                var command = new SqliteCommand(sql, ConnectionPublic);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Проверка на существование записи по ИД
            /// </summary>
            /// <param name="id">ИД</param>
            /// <param name="Table">Таблица</param>
            /// <returns>Значение</returns>
            public static bool CheckFromId(object id, string Table)
            {
                string sql = $"SELECT * FROM `{Table}` WHERE Id = '{id}'";
                if (ConnectionPublic == null)
                {
                    ConnectionPublic = new SqliteConnection(ConnectionString);
                    ConnectionPublic.Open();
                }

                var command = new SqliteCommand(sql, ConnectionPublic);
                var reader = command.ExecuteReader();
                bool response = reader.Read();
                reader.Close();
                return response;
            }

            /// <summary>
            /// Проверка на существование записи по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="value">Значение ключа</param>
            /// <param name="Table">Таблица</param>
            /// <returns>Значение</returns>
            public static bool CheckFromKey(object key, string value, string Table)
            {
                string sql = $"SELECT * FROM `{Table}` WHERE {key} = '{value}'";

                if (ConnectionPublic == null)
                {
                    ConnectionPublic = new SqliteConnection(ConnectionString);
                    ConnectionPublic.Open();
                }
                var command = new SqliteCommand(sql, ConnectionPublic);
                var reader = command.ExecuteReader();
                bool response = reader.Read();
                reader.Close();
                return response;
            }


            /// <summary>
            /// Проверка на существование записи по ключу с именем key
            /// </summary>
            /// <param name="key">Значение ключа</param>
            /// <param name="Table">Таблица</param>
            /// <returns>Значение</returns>
            public static bool CheckFromKey(string key, string Table)
            {
                string sql = $"SELECT * FROM `{Table}` WHERE key = '{key}'";

                if (ConnectionPublic == null)
                {
                    ConnectionPublic = new SqliteConnection(ConnectionString);
                    ConnectionPublic.Open();
                }
                var command = new SqliteCommand(sql, ConnectionPublic);
                var reader = command.ExecuteReader();
                bool response = reader.Read();
                reader.Close();
                return response;
            }

            /// <summary>
            /// Удалить запись по ИД
            /// </summary>
            /// <param name="id">ид</param>
            public void DeleteFromId(object id)
            {
                string sql = $"DELETE FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionPublic);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Удалить по ключу с именем Key
            /// </summary>
            /// <param name="key">Значение ключа</param>
            public void DeleteFromKey(string key)
            {
                string sql = $"DELETE FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionPublic);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Удалить по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="value">Значение ключа</param>
            public void DeleteFromKey(object key, string value)
            {
                string sql = $"DELETE FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionPublic);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Добавить новую запись
            /// </summary>
            /// <param name="fields">Поля</param>
            /// <param name="values">Значения</param>
            /// <param name="Table">Таблица</param>
            public static void Add(List<string> fields, List<object> values, string Table)
            {
                var fieldsText = string.Empty;
                var valuesText = string.Empty;

                foreach (var field in fields) fieldsText += $"`{field}`, ";
                foreach (var value in values) valuesText += $"'{value}', ";
                fieldsText = fieldsText.Substring(0, fieldsText.Length - 2);
                valuesText = valuesText.Substring(0, valuesText.Length - 2);

                string sql = $@"INSERT INTO {Table} ({fieldsText}) VALUES ({valuesText});";
                if (ConnectionPublic == null)
                {
                    ConnectionPublic = new SqliteConnection(ConnectionString);
                    ConnectionPublic.Open();
                }

                var command = new SqliteCommand(sql, ConnectionPublic);
                command.ExecuteNonQuery();
            }


            /// <summary>
            /// Добавить новую запись
            /// </summary>
            /// <param name="fields">Поля</param>
            /// <param name="values">Значения</param>
            /// <param name="Table">Таблица</param>
            public static void Add(List<string> fields, List<string> values, string Table)
            {
                var fieldsText = string.Empty;
                var valuesText = string.Empty;

                foreach (var field in fields) fieldsText += $"`{field}`, ";
                foreach (var value in values) valuesText += $"'{value}', ";
                fieldsText = fieldsText.Substring(0, fieldsText.Length - 2);
                valuesText = valuesText.Substring(0, valuesText.Length - 2);

                string sql = $@"INSERT INTO {Table} ({fieldsText}) VALUES ({valuesText});";
                if (ConnectionPublic == null)
                {
                    ConnectionPublic = new SqliteConnection(ConnectionString);
                    ConnectionPublic.Open();
                }

                var command = new SqliteCommand(sql, ConnectionPublic);
                command.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Класс для работы с БД Stat
        /// </summary>
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

            /// <summary>
            /// Удалить Все
            /// </summary>
            /// <param name="Table">таблица</param>
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

            /// <summary>
            /// Получить все
            /// </summary>
            /// <returns>объект дата ридер</returns>
            public SqliteDataReader  GetAll()
            {
                var sql = $"SELECT * FROM {Table}";
                var command = new SqliteCommand(sql, ConnectionStat);
                var reader = command.ExecuteReader();
                return reader;
            }

            /// <summary>
            /// Получить по ИД
            /// </summary>
            /// <param name="id">Ид</param>
            /// <param name="field">Поле</param>
            /// <returns>Результат</returns>
            public object GetFromId(object id, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                var result = command.ExecuteScalar();
                return result;
            }

            /// <summary>
            /// Получение по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="value">Значение ключа</param>
            /// <param name="field">Поле</param>
            /// <returns>Результат</returns>
            public object GetFromKey(object key, object value, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                var result = command.ExecuteScalar();
                return result;
            }
            
            /// <summary>
            /// Получение по ключу с именем key
            /// </summary>
            /// <param name="key">Значение ключа</param>
            /// <returns>Результат</returns>
            public object GetFromKey(string key)
            {
                var sql = $"SELECT Value FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                var result = command.ExecuteScalar();
                return result;
            }

            /// <summary>
            /// Изменить по ИД
            /// </summary>
            /// <param name="id">ИД</param>
            /// <param name="field">Поле</param>
            /// <param name="value">Значение</param>
            public void EditFromId(object id, string field, object value)
            {
                var sql = $"UPDATE {Table}  SET `{field}`='{value}' WHERE `Id`='{id}';";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
                
            }
            
            /// <summary>
            /// Изменить по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="valueKey">Значение ключа</param>
            /// <param name="field">Поле</param>
            /// <param name="value">Новое значение</param>
            public void EditFromKey(object key, object valueKey, string field, object value)
            {
                var sql = $"UPDATE {Table}  SET `{field}`='{value}' WHERE `{key}`='{valueKey}';";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }
            
            /// <summary>
            /// Изменить по ключу с именем key
            /// </summary>
            /// <param name="key">Значение кключа</param>
            /// <param name="value">Новое значение</param>
            public void EditFromKey(string key, object value)
            {
                var sql = $"UPDATE {Table}  SET `Value`='{value}' WHERE `Key`='{key}';";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Проверка на существование записи в БД по ИД
            /// </summary>
            /// <param name="id">ИД</param>
            /// <param name="Table">Таблица</param>
            /// <returns>Значение</returns>
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

            /// <summary>
            /// Проверка на существование записи в БД по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="value">Значение ключа</param>
            /// <param name="Table">Таблица</param>
            /// <returns></returns>
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

            /// <summary>
            /// Проверка на существование записи в БД по ключу и именем key
            /// </summary>
            /// <param name="key">Значение ключа</param>
            /// <param name="Table">Таблица</param>
            /// <returns>Значение</returns>
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

            /// <summary>
            /// Удалить по ИД
            /// </summary>
            /// <param name="id">ИД</param>
            public void DeleteFromId(object id)
            {
                string sql = $"DELETE FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }
            
            /// <summary>
            /// Удалить по ключу с именем Key
            /// </summary>
            /// <param name="key">Значение ключа</param>
            public void DeleteFromKey(string key)
            {
                string sql = $"DELETE FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Удалить по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="value">Значение</param>
            public void DeleteFromKey(object key, string value)
            {
                string sql = $"DELETE FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionStat);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Добавить новую запись в таблицу
            /// </summary>
            /// <param name="fields">Поля</param>
            /// <param name="values">Значения</param>
            /// <param name="Table">Таблицы</param>
            public static void Add(List<string> fields, List<object> values, string Table)
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


            /// <summary>
            /// Добавить новую запись в таблицу
            /// </summary>
            /// <param name="fields">Поля</param>
            /// <param name="values">Значения</param>
            /// <param name="Table">Таблицы</param>
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
        
        /// <summary>
        /// Класс для работы с ДБ Data
        /// </summary>
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
                //Logger.NewMessage($"ТАБЛИЦА {table}");
            }

            /// <summary>
            /// Получить все
            /// </summary>
            /// <returns>дата ридер</returns>
            public SqliteDataReader  GetAll()
            {
                var sql = $"SELECT * FROM {Table}";
                var command = new SqliteCommand(sql, ConnectionData);
                var reader = command.ExecuteReader();
                return reader;
            }
            
            /// <summary>
            /// Удалить все
            /// </summary>
            /// <param name="Table"></param>
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

            /// <summary>
            /// Получить по ИД
            /// </summary>
            /// <param name="id">ИД</param>
            /// <param name="field">Поле</param>
            /// <returns>результат</returns>
            public object GetFromId(object id, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionData);
                var result = command.ExecuteScalar();
                return result;
            }

            /// <summary>
            /// Получить по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="value">Значение ключа</param>
            /// <param name="field">Поле</param>
            /// <returns>резульат</returns>
            public object GetFromKey(object key, object value, string field)
            {
                var sql = $"SELECT {field} FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionData);
                var result = command.ExecuteScalar();
                return result;
            }
            
            /// <summary>
            /// Получить по  ключу с именем key
            /// </summary>
            /// <param name="key">Значение ключа</param>
            /// <returns>Резуьтат</returns>
            public object GetFromKey(string key)
            {
                var sql = $"SELECT Value FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionData);
                var result = command.ExecuteScalar();
                return result;
            }

            /// <summary>
            /// Редактировать по Id
            /// </summary>
            /// <param name="id">ИД</param>
            /// <param name="field">Поле</param>
            /// <param name="value">Новое значение</param>
            public void EditFromId(object id, string field, object value)
            {
                var sql = $"UPDATE {Table} SET `{field}`='{value}' WHERE `Id`='{id}';";
                //Logger.NewMessage(sql);
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }
            
            /// <summary>
            /// Редактировать по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="valueKey">Значение кключа</param>
            /// <param name="field">ПОле</param>
            /// <param name="value">Новое значение</param>
            public void EditFromKey(object key, object valueKey, string field, object value)
            {
                var sql = $"UPDATE {Table}  SET `{field}`='{value}' WHERE `{key}`='{valueKey}';";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }
            
            /// <summary>
            /// Редактировть по ключу с именем key
            /// </summary>
            /// <param name="key">Значение клоюча</param>
            /// <param name="value">Новое значение</param>
            public void EditFromKey(string key, object value)
            {
                var sql = $"UPDATE {Table}  SET `Value`='{value}' WHERE `Key`='{key}';";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Проверка на существования записи в ДБ по ИД
            /// </summary>
            /// <param name="id">Ид</param>
            /// <param name="Table">Таблица</param>
            /// <returns>Значение</returns>
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

            /// <summary>
            ///  Проверка на существования записи в ДБ по ключу 
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="value">Значение ключа</param>
            /// <param name="Table">Таблица</param>
            /// <returns>Значение</returns>
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

            /// <summary>
            ///  Проверка на существования записи в ДБ по ключу с именем key
            /// </summary>
            /// <param name="key">Значение ключа</param>
            /// <param name="Table">Табилца</param>
            /// <returns>Значение</returns>
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

            /// <summary>
            /// Удалить по ИД
            /// </summary>
            /// <param name="id">Ид</param>
            public void DeleteFromId(object id)
            {
                string sql = $"DELETE FROM {Table} WHERE Id='{id}'";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }
            
            /// <summary>
            /// Удалить по ключу с именем key
            /// </summary>
            /// <param name="key">Значение ключа</param>
            public void DeleteFromKey(string key)
            {
                string sql = $"DELETE FROM {Table} WHERE Key='{key}'";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Удалить по ключу
            /// </summary>
            /// <param name="key">Имя ключа</param>
            /// <param name="value">Значение ключа</param>
            public void DeleteFromKey(object key, string value)
            {
                string sql = $"DELETE FROM {Table} WHERE {key}='{value}'";
                var command = new SqliteCommand(sql, ConnectionData);
                command.ExecuteNonQuery();
            }

            /// <summary>
            /// Добавить новую запись в ДБ
            /// </summary>
            /// <param name="fields">Поля</param>
            /// <param name="values">Значения</param>
            /// <param name="Table">Таблица</param>
            public static void Add(List<string> fields, List<object> values, string Table)
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


            /// <summary>
            /// Добавить новую запись в ДБ
            /// </summary>
            /// <param name="fields">Поля</param>
            /// <param name="values">Значения</param>
            /// <param name="Table">Таблица</param>
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


        /* ========================================================
         * Дальше идут старые методы.
         * Эти методы в новом коде уже не используются.
         * Но есть старые участки кода, где это используется.
         * Для сохранения обратной совместимости я оставил их.
         * НЕ В КОЕМ СЛУЧАЕ НЕ ИСПОЛЬЗОВАТЬ ИХ.
         * Они не очень производительные и проводят к множеству ошибок.
         * ========================================================*/
        
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
 