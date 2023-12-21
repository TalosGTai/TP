using System;
using System.Data.SqlClient;
using System.Text.Json;

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
        private SqlConnection connection;

        public DBConnection()
        {
            string json = System.IO.File.ReadAllText("..\\..\\config.json");
            _connectionString = JsonSerializer.Deserialize<Configuration>(json).ConnectionString;
        }

        public bool IsServerConnected(string connectionString)
        {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
        }

        public void Select(string connectionString)
        {
            try
            {
                connection.Open();

                var queryString = "SELECT * FROM...";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}",
                        reader["tPatCulIntPatIDPk"], reader["tPatSFirstname"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            catch (SqlException)
            {

            }
        }
        
    }
}
