using System;

namespace TP.Model
{
    public class DBFunctions
    {
        public DBFunctions() { }
        /// <summary>
        /// Получить 1 строку заголовка журнала
        /// </summary>
        /// <returns>Общество с ограниченной ответственностью «Испытательный Центр Вектор»</returns>
        public string GetJournalTitleRow1()
        {
            DBConnection dBConnection = new DBConnection();
            dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "1");
            return dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "1");
        }
        /// <summary>
        /// Получить 2 строку заголовка журнала
        /// </summary>
        /// <returns>"Испытательный центр"</returns>
        public string GetJournalTitleRow2()
        {
            DBConnection dBConnection = new DBConnection();
            dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "2");
            return dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "2");
        }
        /// <summary>
        /// Получить 3 строку заголовка журнала
        /// </summary>
        /// <returns>«Журнал регистрации направлений и образцов 4»</returns>
        public string GetJournalTitleRow3()
        {
            DBConnection dBConnection = new DBConnection();
            dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "3");
            return dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "3");
        }
        /// <summary>
        /// Получить 4 строку заголовка журнала
        /// </summary>
        /// <returns>value1 = "Ответственный за ведение журнала", value2 = "Дата начала       « 09 »           января            2023 г.";</returns>
        public Tuple<string, string> GetJournalTitleRow4()
        {
            DBConnection dBConnection = new DBConnection();
            string value1, value2;
            value1 = dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "4_1");
            // подчёркивания для строки
            value2 = dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "4_2");
            return new Tuple<string, string>(value1, value2);
        }
        /// <summary>
        /// Получить 5 строку заголовка журнала
        /// </summary>
        /// <returns>value1 = "ФИО (подпись)*    Бузулуцкова С.А. ", value2 = "Дата окончания «      »        2023 г." </returns>
        public Tuple<string, string> GetJournalTitleRow5()
        {
            DBConnection dBConnection = new DBConnection();
            string value1, value2;
            // подчёркивания для строки
            value1 = dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "5_1");
            // подчёркивания для строки
            value2 = dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "5_2"); ;
            return new Tuple<string, string>(value1, value2);
        }
        /// <summary>
        /// Получить 6 строку заголовка журнала
        /// </summary>
        /// <returns>"*подпись только для журналов, ведущихся на бумажном носителе"</returns>
        public string GetJournalTitleRow6()
        {
            DBConnection dBConnection = new DBConnection();
            dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "6");
            return dBConnection.SelectJournalOrgChangesRowByColumnId(1, dBConnection.SelectLastId(1), "6");
        }

    }
}
