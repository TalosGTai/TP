using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Text.Json;

namespace TP.Model
{
    /// <summary>
    /// Класс для работы с базой данный
    /// </summary>
    internal class DBConnection
    {
        internal class Configuration
        {
            public string ConnectionString { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }

        private readonly string _connectionString;
        private MySqlConnection connection;

        public DBConnection()
        {
            string json = System.IO.File.ReadAllText("..\\..\\config.json");
            _connectionString = JsonSerializer.Deserialize<Configuration>(json).ConnectionString;
            connection = new MySqlConnection(_connectionString);
        }
        /// <summary>
        /// Открыть соединения с бд
        /// </summary>
        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }
        /// <summary>
        /// Закрыть соединение с бд
        /// </summary>
        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
        /// <summary>
        /// Получить соединение с бд
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetConnection()
        {
            return connection;
        }
        /// <summary>
        /// Получить id последнего добавленного элемента
        /// </summary>
        /// <returns>id последнего элемента</returns>
        public int SelectLastId()
        {
            try
            {
                connection.Open();
                var queryString = "SELECT LAST_INSERT_ID();";
                MySqlCommand command = new MySqlCommand(queryString, connection);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();

                return Convert.ToInt32(reader["idEditJournalVersion"].ToString());
            }
            catch (SqlException)
            {

            }
            return -1;
        }
        /// <summary>
        /// Получить строку по колонке
        /// </summary>
        /// <param name="idJournalRow">идентификатор строки</param>
        /// <param name="idColumn">идентификатор колонки</param>
        /// <returns>строка из бд</returns>
        public string SelectJournalOrg1ChangesRowByColumnId(int idJournalRow, string idColumn)
        {
            try
            {
                connection.Open();
                var queryString = $"SELECT Row{idColumn} FROM laboratory.editjournalorg1 WHERE idEditJournalOrg1={idJournalRow}";
                MySqlCommand command = new MySqlCommand(queryString, connection);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();

                return reader[0].ToString();
            }
            catch (SqlException)
            {

            }
            return "";
        }

    }
}
