using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
        public int SelectLastId(int idOrg)
        {
            try
            {
                OpenConnection();
                var queryString = $"SELECT COUNT(*) FROM laboratory.org{idOrg}editjournal;";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                CloseConnection();

                return result;
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
            return -1;
        }
        /// <summary>
        /// Получить строку по колонке
        /// </summary>
        /// <param name="idJournalRow">идентификатор строки</param>
        /// <param name="idColumn">идентификатор колонки</param>
        /// <returns>строка из бд</returns>
        public string SelectJournalOrgChangesRowByColumnId(int idOrg, int idJournalRow, string idColumn)
        {
            try
            {
                OpenConnection();
                var queryString = $"SELECT Row{idColumn} FROM laboratory.org{idOrg}editjournal WHERE idOrg{idOrg}editjournal={idJournalRow}";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                string result = command.ExecuteScalar().ToString();
                CloseConnection();
                return result;
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
            return "";
        }

        public void createTableEditJournal(int idOrg)
        {
            string query = $"create table if not exists laboratory.org{idOrg}editjournal (";
            query += $"idorg{idOrg}editjournal int NOT NULL AUTO_INCREMENT,";
            query += "Row1 varchar(200),";
            query += "Row2 varchar(200),";
            query += "Row3 varchar(200),";
            query += "Row4_1 varchar(200),";
            query += "Row4_2 varchar(200),";
            query += "Row5_1 varchar(200),";
            query += "Row5_2 varchar(200),";
            query += "Row6 varchar(200),";
            query += $"PRIMARY KEY (idorg{idOrg}editjournal))";
            try
            {
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void createTableJournalOrg1List0(int idOrg, int idJournal)
        {
            string query = $"create table if not exists laboratory.org{idOrg}journal{idJournal}list0 (";
            query += $"org{idOrg}idjournal{idJournal}list0 int NOT NULL AUTO_INCREMENT,";
            query += "Row1 varchar(200),";
            query += "Row2 varchar(200),";
            query += "Row3 varchar(200),";
            query += "Row4_1 varchar(200),";
            query += "Row4_2 varchar(200),";
            query += "Row5_1 varchar(200),";
            query += "Row5_2 varchar(200),";
            query += "Row6 varchar(200),";
            query += $"PRIMARY KEY (org{idOrg}idjournal{idJournal}list0))";
            try
            {
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void createTableJournalOrg1List1(int idOrg, int idJournal)
        {
            string query = $"create table if not exists  laboratory.org{idOrg}journal{idJournal}list1 (";
            query += $"org{idOrg}idjournal{idJournal}list1 int NOT NULL AUTO_INCREMENT,";
            query += "A Text,";
            query += "B Text,";
            query += "C Text,";
            query += "D Text,";
            query += "E Text,";
            query += "F Text,";
            query += "G Text,";
            query += "H Text,";
            query += "I Text,";
            query += "J Text,";
            query += "K Text,";
            query += "L Text,";
            query += "M Text,";
            query += "N Text,";
            query += "O Text,";
            query += "P Text,";
            query += "Q Text,";
            query += "R Text,";
            query += $"PRIMARY KEY (org{idOrg}idjournal{idJournal}list1))";
            try
            {
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void createTableJournalOrg1List2(int idOrg, int idJournal)
        {
            string query = $"create table if not exists  laboratory.org{idOrg}journal{idJournal}list2 (";
            query += $"org{idOrg}idjournal{idJournal}list2 int NOT NULL AUTO_INCREMENT,";
            query += "A Text,";
            query += "B Text,";
            query += "C Text,";
            query += "D Text,";
            query += "E Text,";
            query += "F Text,";
            query += "G Text,";
            query += "H Text,";
            query += "I Text,";
            query += $"PRIMARY KEY (org{idOrg}idjournal{idJournal}list2))";
            try
            {
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException)
            {

            }
        }

        public void InsertJournalOrgChangesRow(int idOrg, List<string> values)
        {
            try
            {
                OpenConnection();
                var queryString = $"INSERT INTO laboratory.org{idOrg}editjournal";
                queryString += "(Row1, Row2, Row3, Row4_1, Row4_2, Row5_1, Row5_2, Row6)";
                queryString += $" Values (\"{values[0]}\", \"{values[1]}\", \"{values[2]}\", \"{values[3]}\", ";
                queryString += $"\"{values[4]}\", \"{values[5]}\", \"{values[6]}\", \"{values[7]}\")";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public void InsertStartValuesEditJournalOrg(int idOrg)
        {
            try
            {
                OpenConnection();
                var queryString = $"INSERT INTO laboratory.org{idOrg}editjournal";
                queryString += "(Row1, Row2, Row3, Row4_1, Row4_2, Row5_1, Row5_2, Row6)";
                queryString += $" Values (\"{Properties.Resources.Org1EditJournalStartValue1}\", ";
                queryString += $"\"{Properties.Resources.Org1EditJournalStartValue2}\", \"{Properties.Resources.Org1EditJournalStartValue3}\", ";
                queryString += $"\"{Properties.Resources.Org1EditJournalStartValue4_1}\", ";
                queryString += $"\"{Properties.Resources.Org1EditJournalStartValue4_2}\", \"{Properties.Resources.Org1EditJournalStartValue5_1}\", ";
                queryString += $"\"{Properties.Resources.Org1EditJournalStartValue5_1}\", \"{Properties.Resources.Org1EditJournalStartValue6}\")";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public void InsertStartValuesOrgJournalList1(int idOrg, int idJournal)
        {
            try
            {
                // 18 columns without idColumn
                OpenConnection();
                var queryString = $"INSERT INTO laboratory.org{idOrg}journal{idJournal}list1";
                queryString += "(A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R)";
                queryString += $" Values (\"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\", \"\")";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public void InsertStartValuesOrgJournalList2(int idOrg, int idJournal)
        {
            try
            {
                // 9 columns without idColumn
                OpenConnection();
                var queryString = $"INSERT INTO laboratory.org{idOrg}journal{idJournal}list2";
                queryString += "(A, B, C, D, E, F, G, H, I)";
                queryString += $" Values (\"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\", \"\", ";
                queryString += $"\"\")";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public string GetAllTables()
        {
            try
            {
                OpenConnection();
                var queryString = $"SHOW FULL TABLES from laboratory";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                string result = "";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result += reader[0] + "|";
                }
                reader.Close();
                CloseConnection();
                return result;
            }
            catch (SqlException)
            {

            }
            return "";
        }

        public DataTable GetListJournalOrg(int idOrg, int idJournal, int idList)
        {
            try
            {
                OpenConnection();
                string query = $"select * from laboratory.org{idOrg}journal{idJournal}list{idList}";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(query, GetConnection());
                DataSet ds = new DataSet("list");
                mySqlDataAdapter.Fill(ds);
                CloseConnection();
                return ds.Tables[0];
            }
            catch (SqlException)
            {

            }
            return null;
        }
    }
}
