using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TP.Model
{
    public class DBFunctions
    {
        public DBFunctions() { }

        public string GetJournalTitleRow1()
        {
            DBConnection dBConnection = new DBConnection();
            dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "1");
            //return "Общество с ограниченной ответственностью «Испытательный Центр Вектор»";
            return dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "1");
        }

        public string GetJournalTitleRow2()
        {
            DBConnection dBConnection = new DBConnection();
            dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "2");
            //return "Испытательный центр";
            return dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "2");
        }

        public string GetJournalTitleRow3()
        {
            DBConnection dBConnection = new DBConnection();
            dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "3");
            //return "«Журнал регистрации направлений и образцов 4»";
            return dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "3");
        }

        public Tuple<string, string> GetJournalTitleRow4()
        {
            DBConnection dBConnection = new DBConnection();
            string value1, value2;
            //value1 = "Ответственный за ведение журнала";
            value1 = dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "4_1");
            // подчёркивания для строки
            //value2 = "Дата начала       « 09 »           января            2023 г.";
            value2 = dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "4_2");
            return new Tuple<string, string>(value1, value2);
        }

        public Tuple<string, string> GetJournalTitleRow5()
        {
            DBConnection dBConnection = new DBConnection();
            string value1, value2;
            // подчёркивания для строки
            //value1 = "ФИО (подпись)*    Бузулуцкова С.А. ";
            value1 = dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "5_1");
            // подчёркивания для строки
            //value2 = "Дата окончания «      »        2023 г.";
            value2 = dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "5_2"); ;
            return new Tuple<string, string>(value1, value2);
        }

        public string GetJournalTitleRow6()
        {
            DBConnection dBConnection = new DBConnection();
            dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "6");
            //return "*подпись только для журналов, ведущихся на бумажном носителе";
            return dBConnection.SelectJournalOrg1ChangesRowByColumnId(dBConnection.SelectLastId(), "6");
        }
    }

    
}
