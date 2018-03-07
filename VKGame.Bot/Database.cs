using Microsoft.Data.Sqlite;

namespace VKGame.Bot
{
    /// <summary>
    /// Класс для работы методов БД.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Список методов.
        /// </summary>
        public class Methods
        {
            private string database = null;
            private string connectString = null;
            private string table = null;
            private Methods methods = null; 
            
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
                    string commandText = $"SELECT {field} FROM {table} WHERE Id={id}";
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