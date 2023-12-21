using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TP.Model
{

    

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

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public MySqlConnection getConnection()
        {
            return connection;
        }

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
