using DocumentFormat.OpenXml.Drawing;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using TP.View;

namespace TP.Model
{
    /// <summary>
    /// Класс для работы с базой данный
    /// </summary>
    internal class DBConnection
    {
        internal class Configuration
        {
            /// <summary>
            /// Строка подключения к БД
            /// </summary>
            public string ConnectionString { get; set; }
        }

        private readonly string _connectionString;
        private MySqlConnection connection;

        public DBConnection()
        {
            try
            {
                var path = "config.json";
                string json = System.IO.File.ReadAllText(path);
                _connectionString = JsonSerializer.Deserialize<Configuration>(json).ConnectionString;
                connection = new MySqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                var path = "log.txt";
                var errorText = $"Ошибка подключения к базе данных. Message = {ex.Message}, " +
                    $"StackTrace = {ex.StackTrace}";

                MessageBox.Show("Ошибка подключения к базе данных. Проверьте настройки");
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(errorText);
                }
            }
        }
        /// <summary>
        /// Открыть соединения с бд
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
            }
            catch (Exception e)
            {
                Logger.LogDbError(e);
                throw new Exception("Настройте подключение к базе данных");
            }
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
                if (!CheckTable($"org{idOrg}editjournal"))
                {
                    СreateTableEditJournal(idOrg);
                }
                OpenConnection();
                var queryString = $"SELECT COUNT(*) FROM laboratory.org{idOrg}editjournal;";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                CloseConnection();

                return result;
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            return -1;
        }

        public int SelectLastIdEditProtocols(int idOrg)
        {
            try
            {
                if (!CheckTable($"org{idOrg}editprotocol"))
                {
                    СreateTableEditProtocol(idOrg);
                }
                OpenConnection();
                var queryString = $"SELECT COUNT(*) FROM laboratory.org{idOrg}editprotocol;";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                CloseConnection();

                return result;
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
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
                if (!CheckTable($"org{idOrg}editjournal"))
                {
                    СreateTableEditJournal(idOrg);
                }
                OpenConnection();
                var queryString = $"SELECT Row{idColumn} FROM laboratory.org{idOrg}editjournal WHERE idOrg{idOrg}editjournal={idJournalRow}";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                string result = command.ExecuteScalar().ToString();
                return result;
            }
            catch (SqlException ex)
            {
                Logger.LogDbError(ex);
            }
            finally {
                CloseConnection();
            }
            return "";
        }
        /// <summary>
        /// Получаем по одной строке из таблицу editprotocol
        /// </summary>
        /// <param name="idJournalRow">идентификатор строки</param>
        /// <param name="idColumn">идентификатор колонки</param>
        /// <returns>строка из бд</returns>
        public string SelectProtocolOrgChangesRowByColumnId(int idOrg, int idJournalRow, string idColumn)
        {
            try
            {
                if (!CheckTable($"org{idOrg}editprotocol"))
                {
                    СreateTableEditProtocol(idOrg);
                }
                OpenConnection();
                var queryString = $"SELECT Row{idColumn} FROM laboratory.org{idOrg}editprotocol " +
                    $"WHERE idOrg{idOrg}editprotocol={idJournalRow}";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                string result = command.ExecuteScalar().ToString();
                CloseConnection();
                return result;
            }
            catch (SqlException ex)
            {
                Logger.LogDbError(ex);
            }
            return "";
        }

        /// <summary>
        /// Проверка таблицы на существование
        /// </summary>
        /// <param name="tableName">название таблицы</param>
        /// <returns>true - таблицы существует</returns>
        private bool CheckTable(string tableName)
        {
            try
            {
                OpenConnection();
                var queryString = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES " +
                    $"WHERE TABLE_SCHEMA = 'laboratory' " +
                    $"AND TABLE_NAME = '{tableName}'";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());

                int count = 0;
                var rd = command.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    count = rd.GetInt32(0);
                }
                return count > 0;
            }
            catch (SqlException ex)
            {
                Logger.LogDbError(ex);
            }
            finally
            {
                CloseConnection();
            }
            return false;
        }

        public void CheckAndCreateSchema()
        {
            try
            {
                var query = $"create SCHEMA if not exists laboratory;";
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                //CREATE SCHEMA IF NOT EXISTS test AUTHORIZATION joe
               // query = $"create SCHEMA if not exists laboratory CREATE SCHEMA `laboratory`; ";
                command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();

            }
            catch (SqlException ex)
            {
                Logger.LogDbError(ex);
            }
        }

        /// <summary>
        /// Создать таблицу для EditJournal
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        public void СreateTableEditJournal(int idOrg)
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
            catch (SqlException ex)
            {
                Logger.LogDbError(ex);
            }
            finally { CloseConnection(); }
        }

        /// <summary>
        /// Создать таблицу для листа0 журнала
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        /// <param name="idJournal">номер журнала</param>
        public void СreateTableJournalOrg1List0(int idOrg, int idJournal)
        {
            if (!CheckTable($"laboratory.org{idOrg}journal{idJournal}list0"))
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
                catch (SqlException ex)
                {
                    Logger.LogDbError(ex);
                }
                finally { CloseConnection(); }
            }
        }

        /// <summary>
        /// Создать таблицу листа1 журнала
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        /// <param name="idJournal">номер журнала</param>
        public void СreateTableJournalOrg1List1(int idOrg, int idJournal)
        {
            if (!CheckTable($"laboratory.org{idOrg}journal{idJournal}list1"))
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
                catch (SqlException ex)
                {
                    Logger.LogDbError(ex);
                }
                finally { CloseConnection(); }
            }
        }

        /// <summary>
        /// Сохранить в таблицу листа1 журнала
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        /// <param name="idJournal">номер журнала</param>
        public void SaveTableJournalOrg1List1(int idOrg, int idJournal, List<Org1List1> listFromUI)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list1"))
                {
                    СreateTableJournalOrg1List1(idOrg, idJournal);
                }
                MySqlTransaction tr = null;
                try
                {
                    OpenConnection();
                    var con = GetConnection();
                    tr = con.BeginTransaction();
                    string query = $"TRUNCATE laboratory.org{idOrg}journal{idJournal}list1; ";
                    MySqlCommand command = new MySqlCommand(query, con);
                    command.ExecuteNonQuery();
                    query = "";
                    foreach (var dif in listFromUI)
                    {
                        query += $"INSERT INTO laboratory.org{idOrg}journal{idJournal}list1 " +
                            $"(A, B, C, D, E, F, G, H , I, J, K, L, M, N, O, P, Q, R) " +
                            $"VALUES(" +
                            $"\"{ToStringDataBase(dif.NumberProduct)}\"," +
                            $"\"{ToStringDataBase(dif.NumberDateDirection)}\"," +
                            $"\"{ToStringDataBase(dif.SamplingAct)}\"," +
                            $"\"{ToStringDataBase(dif.SampleName)}\"," +
                            $"\"{ToStringDataBase(dif.OrganizationName)}\"," +
                            $"\"{ToStringDataBase(dif.NumberSampleWeightCapacity)}\"," +
                            $"\"{ToStringDataBase(dif.NumberDateUnsuitabilitySamples)}\"," +
                            $"\"{ToStringDataBase(dif.DateReceiptSample)}\"," +
                            $"\"{ToStringDataBase(dif.NumberRegSample)}\"," +
                            $"\"{ToStringDataBase(dif.FioResponsiblePersonTest)}\"," +
                            $"\"{ToStringDataBase(dif.DateIssueSample)}\"," +
                            $"\"{ToStringDataBase(dif.DateReturnSampleAfterTest)}\"," +
                            $"\"{ToStringDataBase(dif.FioInsertRecord)}\"," +
                            $"\"{ToStringDataBase(dif.Note)}\"," +
                            $"\"{ToStringDataBase(dif.NumberProtocol)}\"," +
                            $"\"{ToStringDataBase(dif.ProductType)}\"," +
                            $"\"{ToStringDataBase(dif.Applicant)}\"," +
                            $"\"{ToStringDataBase(dif.Manufacturer)}\"); ";
                    }
                    command = new MySqlCommand(query, GetConnection());
                    command.ExecuteNonQuery();
                    tr.Commit();
                }
                catch
                {
                    try
                    {
                        tr?.Rollback();
                        CloseConnection();
                    }
                    catch (MySqlException ex1)
                    {
                        Logger.LogDbError(ex1);
                    }
                }
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        /// <summary>
        /// Получить из базы лист1 в виде списка
        /// </summary>
        /// <param name="idOrg"></param>
        /// <param name="idJournal"></param>
        /// <returns></returns>
        public List<Org1List1> GetOrgList1(int idOrg, int idJournal)
        {
            try
            {
                //Все значения из базы
                if (!CheckTable($"org{idOrg}journal{idJournal}list1"))
                {
                    СreateTableJournalOrg1List1(idOrg, idJournal);
                }
                var listTable = GetListJournalOrg(idOrg, idJournal, 1);
                var listFromDb = new List<Org1List1>();
                for (int i = 0; i < listTable.Rows.Count; i++)
                {
                    var row = listTable.Rows[i];
                    var listString = new List<string>();
                    listString.Add(row.Field<string>("A"));
                    listString.Add(row.Field<string>("B"));
                    listString.Add(row.Field<string>("C"));
                    listString.Add(row.Field<string>("D"));
                    listString.Add(row.Field<string>("E"));
                    listString.Add(row.Field<string>("F"));
                    listString.Add(row.Field<string>("G"));
                    listString.Add(row.Field<string>("H"));
                    listString.Add(row.Field<string>("I"));
                    listString.Add(row.Field<string>("J"));
                    listString.Add(row.Field<string>("K"));
                    listString.Add(row.Field<string>("L"));
                    listString.Add(row.Field<string>("M"));
                    listString.Add(row.Field<string>("N"));
                    listString.Add(row.Field<string>("O"));
                    listString.Add(row.Field<string>("P"));
                    listString.Add(row.Field<string>("Q"));
                    listString.Add(row.Field<string>("R"));

                    listFromDb.Add(new Org1List1(listString));
                }
                return listFromDb;
            }
            catch (Exception ex)
            {
                Logger.LogDbError(ex);
                throw;
            }
        }

        /// <summary>
        /// Получить из базы лист2 в виде списка
        /// </summary>
        /// <param name="idOrg"></param>
        /// <param name="idJournal"></param>
        /// <returns></returns>
        public List<Org1List2> GetOrgList2(int idOrg, int idJournal)
        {
            if (!CheckTable($"org{idOrg}journal{idJournal}list2"))
            {
                СreateTableJournalOrg1List2(idOrg, idJournal);
            }
            //Все значения из базы
            var listTable = GetListJournalOrg(idOrg, idJournal, 2);
            var listFromDb = new List<Org1List2>();
            for (int i = 0; i < listTable.Rows.Count; i++)
            {
                var row = listTable.Rows[i];
                var listString = new List<string>();
                listString.Add(row.Field<string>("A"));
                listString.Add(row.Field<string>("B"));
                listString.Add(row.Field<string>("C"));
                listString.Add(row.Field<string>("D"));
                listString.Add(row.Field<string>("E"));
                listString.Add(row.Field<string>("F"));
                listString.Add(row.Field<string>("G"));
                listString.Add(row.Field<string>("H"));
                listString.Add(row.Field<string>("I"));

                listFromDb.Add(new Org1List2(listString));
            }
            return listFromDb;
        }

        /// <summary>
        /// Сохранить в таблицу листа2 журнала
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        /// <param name="idJournal">номер журнала</param>
        public void SaveTableJournalOrg1List2(int idOrg, int idJournal, List<Org1List2> listFromUI)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list2"))
                {
                    СreateTableJournalOrg1List2(idOrg, idJournal);
                }
                //Объявление транзакции
                MySqlTransaction tr = null;
                try
                {
                    OpenConnection();
                    var con = GetConnection();
                    tr = con.BeginTransaction();
                    string query = "";

                    query = $"TRUNCATE laboratory.org{idOrg}journal{idJournal}list2; ";
                    MySqlCommand command = new MySqlCommand(query, con);
                    command.ExecuteNonQuery();
                    query = "";
                    foreach (var dif in listFromUI)
                    {
                        query += $"INSERT INTO laboratory.org{idOrg}journal{idJournal}list2 " +
                            $"(A, B, C, D, E, F, G, H , I) " +
                            $"VALUES(" +
                            $"\"{ToStringDataBase(dif.NumberProduct)}\"," +
                            $"\"{ToStringDataBase(dif.NumberProtocolTest)}\"," +
                            $"\"{ToStringDataBase(dif.DateReturnSampleAfterTest)}\"," +
                            $"\"{ToStringDataBase(dif.NumberDateDirection)}\"," +
                            $"\"{ToStringDataBase(dif.NumberRegSample)}\"," +
                            $"\"{ToStringDataBase(dif.NumberActUtil)}\"," +
                            $"\"{ToStringDataBase(dif.DateActUtil)}\"," +
                            $"\"{ToStringDataBase(dif.DateReturnSample)}\"," +
                            $"\"{ToStringDataBase(dif.FioInsertRecord)}\"" +
                            $"); ";
                    }
                    command = new MySqlCommand(query, con);
                    command.ExecuteNonQuery();
                    tr.Commit();
                }
                catch
                {
                    try
                    {
                        tr?.Rollback();
                        CloseConnection();
                    }
                    catch (MySqlException ex1)
                    {
                        Logger.LogDbError(ex1);
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.LogDbError(ex);
            }
            finally { CloseConnection(); }
        }

        /// <summary>
        /// Обновить таблицу листа1 журнала
        /// </summary>
        public void UpdateTableJournalOrg1List1(int idOrg, int idJournal, Org1List1 dif)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list1"))
                {
                    СreateTableJournalOrg1List1(idOrg, idJournal);
                }
                string query = $"UPDATE laboratory.org{idOrg}journal{idJournal}list1 " +
                    $"SET " +
                    $"B=\"{ToStringDataBase(dif.NumberDateDirection)}\"," +
                    $"C=\"{ToStringDataBase(dif.SamplingAct)}\"," +
                    $"D=\"{ToStringDataBase(dif.SampleName)}\"," +
                    $"E=\"{ToStringDataBase(dif.OrganizationName)}\"," +
                    $"F=\"{ToStringDataBase(dif.NumberSampleWeightCapacity)}\"," +
                    $"G=\"{ToStringDataBase(dif.NumberDateUnsuitabilitySamples)}\"," +
                    $"H=\"{ToStringDataBase(dif.DateReceiptSample)}\"," +
                    $"I=\"{ToStringDataBase(dif.NumberRegSample)}\"," +
                    $"J=\"{ToStringDataBase(dif.FioResponsiblePersonTest)}\"," +
                    $"K=\"{ToStringDataBase(dif.DateIssueSample)}\"," +
                    $"L=\"{ToStringDataBase(dif.DateReturnSampleAfterTest)}\"," +
                    $"M=\"{ToStringDataBase(dif.FioInsertRecord)}\"," +
                    $"N=\"{ToStringDataBase(dif.Note)}\"," +
                    $"O=\"{ToStringDataBase(dif.NumberProtocol)}\"," +
                    $"P=\"{ToStringDataBase(dif.ProductType)}\"," +
                    $"Q=\"{ToStringDataBase(dif.Applicant)}\"," +
                    $"R=\"{ToStringDataBase(dif.Manufacturer)}\" " +
                    $"WHERE A = \"{ToStringDataBase(dif.NumberProduct)}\";";
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        private string ToStringDataBase(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            string result = "";
            foreach (char c in value)
            {
                if (c == '\'')
                {
                    result += "\'";
                }
                else if (c == '"')
                {
                    result += "\\\"";
                }
                else 
                {
                    result += c; 
                }
            }
            return result;
        }

        /// <summary>
        /// Обновить таблицу листа2 журнала
        /// </summary>
        public void UpdateTableJournalOrg1List2(int idOrg, int idJournal, Org1List2 dif)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list2"))
                {
                    СreateTableJournalOrg1List2(idOrg, idJournal);
                }

                string query = $"UPDATE laboratory.org{idOrg}journal{idJournal}list2 " +
                    $"SET " +
                    $"B=\"{ToStringDataBase(dif.NumberProtocolTest)}\"," +
                    $"C=\"{ToStringDataBase(dif.DateReturnSampleAfterTest)}\"," +
                    $"D=\"{ToStringDataBase(dif.NumberDateDirection)}\"," +
                    $"E=\"{ToStringDataBase(dif.NumberRegSample)}\"," +
                    $"F=\"{ToStringDataBase(dif.NumberActUtil)}\"," +
                    $"G=\"{ToStringDataBase(dif.DateActUtil)}\"," +
                    $"H=\"{ToStringDataBase(dif.DateReturnSample)}\"," +
                    $"I=\"{ToStringDataBase(dif.FioInsertRecord)}\" " +
                    $"WHERE A = \"{ToStringDataBase(dif.NumberProduct)}\";";
                    OpenConnection();
                    MySqlCommand command = new MySqlCommand(query, GetConnection());
                    command.ExecuteNonQuery();
                    CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }


        /// <summary>
        /// Создать таблицу для листа2 журнала
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        /// <param name="idJournal">номер журнала</param>
        public void СreateTableJournalOrg1List2(int idOrg, int idJournal)
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
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }      

        public void InsertJournalOrgChangesRow(int idOrg, List<string> values)
        {
            try
            {
                if (!CheckTable($"org{idOrg}editjournal"))
                {
                    СreateTableEditJournal(idOrg);
                }
                OpenConnection();
                var queryString = $"INSERT INTO laboratory.org{idOrg}editjournal";
                queryString += "(Row1, Row2, Row3, Row4_1, Row4_2, Row5_1, Row5_2, Row6)";
                queryString += $" Values (\"{ToStringDataBase(values[0])}\", \"{ToStringDataBase(values[1])}\", \"{ToStringDataBase(values[2])}\", \"{ToStringDataBase(values[3])}\", ";
                queryString += $"\"{ToStringDataBase(values[4])}\", \"{ToStringDataBase(values[5])}\", \"{ToStringDataBase(values[6])}\", \"{ToStringDataBase(values[7])}\")";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        public void InsertProtocolOrgChangesRow(int idOrg, List<string> values)
        {
            try
            {
                if (!CheckTable($"org{idOrg}editprotocol"))
                {
                    СreateTableEditProtocol(idOrg);
                }
                OpenConnection();
                var queryString = $"INSERT INTO laboratory.org{idOrg}editprotocol";
                queryString += "(Row1, Row2, Row3, Row4)";
                queryString += $" Values (\"{ToStringDataBase(values[0])}\", \"{ToStringDataBase(values[1])}\", \"{ToStringDataBase(values[2])}\", \"{ToStringDataBase(values[3])}\")";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        /// <summary>
        /// Внести начальные значения в EditJournal
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        public void InsertStartValuesEditJournalOrg(int idOrg)
        {
            try
            {
                if (!CheckTable($"org{idOrg}editjournal"))
                {
                    СreateTableEditJournal(idOrg);
                }
                OpenConnection();
                var queryString = $"INSERT INTO laboratory.org{idOrg}editjournal";
                queryString += "(Row1, Row2, Row3, Row4_1, Row4_2, Row5_1, Row5_2, Row6)";
                queryString += $" Values (\"{ToStringDataBase(Properties.Resources.Org1EditJournalStartValue1)}\", ";
                queryString += $"\"{ToStringDataBase(Properties.Resources.Org1EditJournalStartValue2)}\", \"{ToStringDataBase(Properties.Resources.Org1EditJournalStartValue3)}\", ";
                queryString += $"\"{ToStringDataBase(Properties.Resources.Org1EditJournalStartValue4_1)}\", ";
                queryString += $"\"{ToStringDataBase(Properties.Resources.Org1EditJournalStartValue4_2)}\", \"{ToStringDataBase(Properties.Resources.Org1EditJournalStartValue5_1)}\", ";
                queryString += $"\"{ToStringDataBase(Properties.Resources.Org1EditJournalStartValue5_1)}\", \"{ToStringDataBase(Properties.Resources.Org1EditJournalStartValue6)}\")";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        public void InsertStartValuesEditProtocolOrg(int idOrg)
        {
            try
            {
                if (!CheckTable($"org{idOrg}editprotocol"))
                {
                    СreateTableEditProtocol(idOrg);
                }
                OpenConnection();
                var queryString = $"INSERT INTO laboratory.org{idOrg}editprotocol";
                queryString += "(Row1, Row2, Row3, Row4)";
                queryString += $" Values (\"{ToStringDataBase(Properties.Resources.Org1EditProtocolStartValue1)}\", ";
                queryString += $"\"{ToStringDataBase(Properties.Resources.Org1EditProtocolStartValue2)}\", \"{ToStringDataBase(Properties.Resources.Org1EditProtocolStartValue3)}\", ";
                queryString += $"\"{ToStringDataBase(Properties.Resources.Org1EditProtocolStartValue4)}\")";
                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        /// <summary>
        /// Внести начальные значения в лист1 журнала
        /// </summary>
        /// <param name="idOrg">номер организцаии</param>
        /// <param name="idJournal">номер журнала</param>
        public void InsertStartValuesOrgJournalList1(int idOrg, int idJournal)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list1"))
                {
                    СreateTableJournalOrg1List1(idOrg, idJournal);
                }
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
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        /// <summary>
        /// Внести начальные значения в лист2 журнала
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        /// <param name="idJournal">номер журнала</param>
        public void InsertStartValuesOrgJournalList2(int idOrg, int idJournal)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list2"))
                {
                    СreateTableJournalOrg1List2(idOrg, idJournal);
                }
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
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        /// <summary>
        /// Получить все таблицы бд
        /// </summary>
        /// <returns>Список таблиц через | </returns>
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
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
            return "";
        }

        /// <summary>
        /// Получить лист журнала
        /// </summary>
        /// <param name="idOrg">идентификатор/номер организации</param>
        /// <param name="idJournal">идентификатор/номер журнала</param>
        /// <param name="idList">идентификатор/номер листа</param>
        /// <returns>Таблица с данными листа</returns>
        public DataTable GetListJournalOrg(int idOrg, int idJournal, int idList)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list{idList}"))
                {
                    switch (idList)
                    {
                        case 0:
                            СreateTableJournalOrg1List0(idOrg, idJournal);
                            break;
                        case 1:
                            СreateTableJournalOrg1List1(idOrg, idJournal);
                            break;
                        case 2:
                            СreateTableJournalOrg1List2(idOrg, idJournal);
                            break;
                    }
                }
                OpenConnection();
                string query = $"select * from laboratory.org{idOrg}journal{idJournal}list{idList}";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(query, GetConnection());
                DataSet ds = new DataSet("list");
                mySqlDataAdapter.Fill(ds);
                CloseConnection();
                return ds.Tables[0];
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
            return null;
        }


        /// <summary>
        /// Создать таблицу org1editprotocol
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        public void СreateTableEditProtocol(int idOrg)
        {
            string query = $"create table if not exists laboratory.org{idOrg}editprotocol (";
            query += $"idorg{idOrg}editprotocol int NOT NULL AUTO_INCREMENT,";
            query += "Row1 TEXT,";
            query += "Row2 varchar(100),";
            query += "Row3 TEXT,";
            query += "Row4 TEXT, ";
            query += $"PRIMARY KEY (idorg{idOrg}editprotocol))";
            try
            {
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        public void СreateTableProtocolOrgJournal(int idOrg)
        {
            string query = $"create table if not exists  laboratory.org{idOrg}Protocol (";
            query += $"org{idOrg}ProtocolId int NOT NULL AUTO_INCREMENT, ";
            query += "ProtocolId VARCHAR(128), ";
            query += "ProtocolDoc BLOB, ";
            query += "ProtocolXls BLOB, ";
            query += $"PRIMARY KEY (org{idOrg}ProtocolId))";
            try
            {
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        public int FindProtocol(int idOrg, string protocolName)
        {
            int res = -1;
            try
            {
                string query = $"SELECT org{idOrg}ProtocolId FROM laboratory.org{idOrg}Protocol " +
                $"WHERE ProtocolId = \"{protocolName}\";";
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                res = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally 
            { 
                CloseConnection(); 
            }
            return res;
        }

        public void DeleteTableProtocolOrgJournal(int idOrg, string protocolName)
        {
            try
            {
                string query = $"DELETE FROM laboratory.org{idOrg}Protocol " +
                $"WHERE ProtocolId = \"{protocolName}\";";
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        public void InsertOrUpdateOrgProtocolRow(int idOrg, int idProtocol, byte[] docFile, byte[] xlsFile)
        {
            try
            {
                if (!CheckTable($"org{idOrg}Protocol"))
                {
                    СreateTableProtocolOrgJournal(idOrg);
                }

                var protocolName = "Протокол" + idProtocol;
                if (IsExistProtocol(idOrg, protocolName))
                {
                    OpenConnection();
                    using (var sqlWrite = new MySqlCommand($"Update laboratory.org{idOrg}Protocol " +
                        $"SET ProtocolDoc = @File1, ProtocolDoc = @File2 " +
                        $"WHERE ProtocolId = \"{protocolName}\"", GetConnection()))
                    {
                        sqlWrite.Parameters.Add("@File1", MySqlDbType.VarBinary, docFile.Length).Value = docFile;
                        sqlWrite.Parameters.Add("@File2", MySqlDbType.VarBinary, xlsFile.Length).Value = xlsFile;
                        sqlWrite.ExecuteNonQuery();
                    }
                    CloseConnection();
                }
                else
                {
                    OpenConnection();
                    using (var sqlWrite = new MySqlCommand($"INSERT INTO laboratory.org{idOrg}Protocol (ProtocolId, ProtocolDoc, ProtocolXls) " +
                        $"Values(@idProtocol, @File1, @File2)", GetConnection()))
                    {
                        sqlWrite.Parameters.Add("@idProtocol", MySqlDbType.VarString).Value = "Протокол" + idProtocol;
                        sqlWrite.Parameters.Add("@File1", MySqlDbType.VarBinary, docFile.Length).Value = docFile;
                        sqlWrite.Parameters.Add("@File2", MySqlDbType.VarBinary, xlsFile.Length).Value = xlsFile;
                        sqlWrite.ExecuteNonQuery();
                    }
                    CloseConnection();
                }

                
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
            }
            finally { CloseConnection(); }
        }

        public bool IsExistProtocol(int idOrg, string idProtocol)
        {
            try
            {
                if (!CheckTable($"org{idOrg}Protocol"))
                {
                    СreateTableProtocolOrgJournal(idOrg);
                }

                OpenConnection();
                var count = 0;
                using (var sqlQuery = new MySqlCommand($"SELECT COUNT(*) FROM laboratory.org{idOrg}Protocol WHERE ProtocolId = \"{idProtocol}\"", GetConnection()))
                {
                    count = Convert.ToInt32(sqlQuery.ExecuteScalar());
                }
                CloseConnection();
                return count > 0;
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally { CloseConnection(); }
        }

        public (MemoryStream docFile, MemoryStream xlsFile) GetPartOfOrgProtocolRow(int idOrg, string path, IReadOnlyCollection<string> excludeProtocols)
        {
            try
            {
                if (!CheckTable($"org{idOrg}Protocol"))
                {
                    СreateTableProtocolOrgJournal(idOrg);
                }

                OpenConnection();

                MemoryStream msDoc = new MemoryStream();
                MemoryStream msXls = new MemoryStream();
                var excludeString = String.Join(", ", excludeProtocols.ToArray());
                var query = "";
                if (!string.IsNullOrEmpty(excludeString))
                {
                    query = $"SELECT ProtocolId, ProtocolDoc, ProtocolXls FROM laboratory.org{idOrg}Protocol WHERE ProtocolId NOT IN ({excludeString})";
                }
                else
                {
                    query = $"SELECT ProtocolId, ProtocolDoc, ProtocolXls FROM laboratory.org{idOrg}Protocol";
                }
                var sqlQuery = new MySqlCommand(query, GetConnection());

                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                {
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {
                            if (sqlQueryResult.HasRows)
                            {
                                string protocolName = (string)sqlQueryResult["ProtocolId"];

                                var catalog = Directory.CreateDirectory(path + protocolName).FullName;

                                if (!File.Exists($"{catalog}\\{protocolName}.docx"))
                                {
                                    byte[] colProtocolDoc = (byte[])sqlQueryResult["ProtocolDoc"];
                                    msDoc.Write(colProtocolDoc, 0, colProtocolDoc.Length);
                                    using (var fs = new FileStream($"{catalog}\\{protocolName}.docx", FileMode.Create, FileAccess.Write))
                                        fs.Write(colProtocolDoc, 0, colProtocolDoc.Length);
                                }

                                if (!File.Exists($"{catalog}\\{protocolName}.xlsx"))
                                {
                                    byte[] colProtocolXls = (byte[])sqlQueryResult["ProtocolXls"];
                                    msXls.Write(colProtocolXls, 0, colProtocolXls.Length);
                                    using (var fs = new FileStream($"{catalog}\\{protocolName}.xlsx", FileMode.Create, FileAccess.Write))
                                        fs.Write(colProtocolXls, 0, colProtocolXls.Length);
                                }
                            }
                        }
                    }

                }
                
                CloseConnection();
                return (msDoc, msXls);
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally { CloseConnection(); }
        }

        /// <summary>
        /// Получить строку по колонке
        /// </summary>
        /// <param name="idJournalRow">идентификатор строки</param>
        /// <param name="idColumn">идентификатор колонки</param>
        /// <returns>строка из бд</returns>
        public List<string> SelectOrgJournalList1ByColumnId(int idOrg, int idJournal, int idProduct)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list1"))
                {
                    СreateTableJournalOrg1List1(idOrg, idJournal);
                }
                OpenConnection();
                var queryString = $"SELECT * FROM laboratory.org{idOrg}journal{idJournal}list1 " +
                    $"WHERE A=\"{idProduct}\"";

                var result = new List<string>();
                int count = 0;
                using (var sqlQuery = new MySqlCommand(queryString, GetConnection()))
                {
                    using (var sqlQueryResult = sqlQuery.ExecuteReader())
                        if (sqlQueryResult != null)
                        {
                            if (sqlQueryResult.Read() && sqlQueryResult.HasRows && count < 1)
                            {
                                count++;
                                result.Add((string)sqlQueryResult["A"]);
                                result.Add((string)sqlQueryResult["B"]);
                                result.Add((string)sqlQueryResult["C"]);
                                result.Add((string)sqlQueryResult["D"]);
                                result.Add((string)sqlQueryResult["E"]);
                                result.Add((string)sqlQueryResult["F"]);
                                result.Add((string)sqlQueryResult["G"]);
                                result.Add((string)sqlQueryResult["H"]);
                                result.Add((string)sqlQueryResult["I"]);
                                result.Add((string)sqlQueryResult["J"]);
                                result.Add((string)sqlQueryResult["K"]);
                                result.Add((string)sqlQueryResult["L"]);
                                result.Add((string)sqlQueryResult["M"]);
                                result.Add((string)sqlQueryResult["N"]);
                                result.Add((string)sqlQueryResult["O"]);
                                result.Add((string)sqlQueryResult["P"]);
                                result.Add((string)sqlQueryResult["Q"]);
                                result.Add((string)sqlQueryResult["R"]);
                            }
                        }
                }
                return result;
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Получить строку по колонке
        /// </summary>
        /// <param name="idJournalRow">идентификатор строки</param>
        /// <param name="idColumn">идентификатор колонки</param>
        /// <returns>строка из бд</returns>
        public List<string> SelectOrgJournalList2ByColumnId(int idOrg, int idJournal, int idProtocol)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list2"))
                {
                    СreateTableJournalOrg1List2(idOrg, idJournal);
                }
                OpenConnection();
                var queryString = $"SELECT * FROM laboratory.org{idOrg}journal{idJournal}list2 " +
                    $"WHERE A=\"{idProtocol}\"";

                var result = new List<string>();
                using (var sqlQuery = new MySqlCommand(queryString, GetConnection()))
                {
                    using (var sqlQueryResult = sqlQuery.ExecuteReader())
                        if (sqlQueryResult != null)
                        {
                            sqlQueryResult.Read();
                            result.Add((string)sqlQueryResult["A"]);
                            result.Add((string)sqlQueryResult["B"]);
                            result.Add((string)sqlQueryResult["C"]);
                            result.Add((string)sqlQueryResult["D"]);
                            result.Add((string)sqlQueryResult["E"]);
                            result.Add((string)sqlQueryResult["F"]);
                            result.Add((string)sqlQueryResult["G"]);
                            result.Add((string)sqlQueryResult["H"]);
                            result.Add((string)sqlQueryResult["I"]);
                        }
                }
                return result;
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetCountColumnWithSameValue(int idOrg, int idJournal, int idList, string column, string value)
        {
            try
            {
                if (!CheckTable($"org{idOrg}journal{idJournal}list{idList}"))
                {
                    switch (idList)
                    {
                        case 1: СreateTableJournalOrg1List1(idOrg, idJournal); break;
                        case 2: СreateTableJournalOrg1List2(idOrg, idJournal);break;
                    }
                }
                OpenConnection();
                var queryString = $"SELECT COUNT(*) FROM laboratory.org{idOrg}journal{idJournal}list{idList} " +
                    $"WHERE {column}=\"{value}\"";

                MySqlCommand command = new MySqlCommand(queryString, GetConnection());
                int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                CloseConnection();
                return result;
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Создать таблицу ГОСТов
        /// </summary>
        /// <param name="idOrg">номер организации</param>
        public void СreateTableGosts()
        {
            if (!CheckTable($"laboratory.Gosts"))
            {
                string query = $"create table if not exists  laboratory.Gosts (";
                query += $"id int NOT NULL AUTO_INCREMENT, ";
                query += "shortName Text, ";
                query += "longName Text, ";
                query += $"PRIMARY KEY (id))";
                try
                {
                    OpenConnection();
                    MySqlCommand command = new MySqlCommand(query, GetConnection());
                    command.ExecuteNonQuery();
                    CloseConnection();
                }
                catch (SqlException ex)
                {
                    Logger.LogDbError(ex);
                }
                finally { CloseConnection(); }
            }
        }

        /// <summary>
        /// Добавить все ГОСТы из файла
        /// </summary>
        public void AddAllGostsData(List<Tuple<string,string>> data)
        {
            try
            {
                if (!CheckTable($"Gosts"))
                {
                    СreateTableGosts();
                }

                string queryString = $"INSERT INTO laboratory.Gosts " +
                    $"(shortName, longName) " +
                    "VALUES(@short, @long)";

                OpenConnection();
                var conn = GetConnection();
                using (MySqlCommand cmd = new MySqlCommand(queryString, conn))
                {
                    cmd.Parameters.Add("@short", MySqlDbType.Text);
                    cmd.Parameters.Add("@long", MySqlDbType.Text);

                    for (int i = 0; i < data.Count; i++)
                    {
                        cmd.Parameters["@short"].Value = data[i].Item1;
                        cmd.Parameters["@long"].Value = data[i].Item2;
                        cmd.ExecuteNonQuery();
                    }
                }
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ошибка в AddGostData");
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Добавить один новый гост
        /// </summary>
        /// <param name="dataLeft">short gost name</param>
        /// <param name="dataRight">long gost name</param>
        public void AddGost(string dataLeft, string dataRight)
        {
            try
            {
                if (!CheckTable($"orgGosts"))
                {
                    СreateTableGosts();
                }

                string queryString = $"INSERT INTO laboratory.Gosts " +
                    $"(shortName, longName) " +
                    "VALUES(@short, @long)";

                var shortNamesGosts = GetAllShortNameGostsFromDb();
                if (shortNamesGosts.Contains(dataLeft))
                {
                    return;
                }

                OpenConnection();
                var conn = GetConnection();
                using (MySqlCommand cmd = new MySqlCommand(queryString, conn))
                {
                    cmd.Parameters.Add("@short", MySqlDbType.Text);
                    cmd.Parameters.Add("@long", MySqlDbType.Text);

                    cmd.Parameters["@short"].Value = dataLeft;
                    cmd.Parameters["@long"].Value = dataRight;
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }


        /// <summary>
        /// Получить все короткие названия ГОСТов из таблицы
        /// </summary>
        /// <returns>Список коротких названий ГОСТов</returns>
        public List<string> GetAllShortNameGostsFromDb()
        {
            try
            {
                if (!CheckTable($"Gosts"))
                {
                    СreateTableGosts();
                }

                string queryString = $"SELECT shortName FROM laboratory.Gosts; ";


                OpenConnection();
                using (MySqlCommand cmd = new MySqlCommand(queryString, GetConnection()))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    var shortNamesList = new List<string>();
                    while (dataReader.Read())
                    {
                        shortNamesList.Add(dataReader["shortName"].ToString());
                    }
                    CloseConnection();

                    return shortNamesList;

                }
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Получить ГОСТы
        /// </summary>
        /// <returns>Список названий ГОСТов</returns>
        public DataTable GetAllGostsFromDb()
        {
            try
            {
                if (!CheckTable($"Gosts"))
                {
                    СreateTableGosts();
                }

                OpenConnection();
                string query = $"SELECT * FROM laboratory.Gosts; ";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(query, GetConnection());
                DataSet ds = new DataSet();
                mySqlDataAdapter.Fill(ds);
                CloseConnection();
                return ds.Tables[0];


            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Изменение существующего ГОСТа
        /// </summary>
        /// <param name="id">id ГОСТа</param>
        /// <param name="sGost">новое короткое название</param>
        /// <param name="lGost">новое полное название</param>
        public void UpdateGost(int id, string sGost, string lGost)
        {
            try
            {
                if (!CheckTable($"Gosts"))
                {
                    СreateTableGosts();
                }

                string queryString = $"UPDATE laboratory.Gosts SET shortName = {sGost}, longName = {lGost} WHERE id = {id};";

                OpenConnection();
                using (MySqlCommand cmd = new MySqlCommand(queryString, GetConnection()))
                {

                    cmd.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Удаление ГОСТа из таблицы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sGost"></param>
        /// <param name="lGost"></param>
        public void DeleteGost(Gost gost)
        {
            try
            {
                if (!CheckTable($"Gosts"))
                {
                    СreateTableGosts();
                }
                string query = $"DELETE FROM laboratory.Gosts WHERE shortName = \"{gost.ShortNameGost}\" AND longName = \"{gost.LongNameGost}\" ;";

                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, GetConnection());
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (SqlException e)
            {
                Logger.LogDbError(e);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
