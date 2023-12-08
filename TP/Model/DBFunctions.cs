using System;
using System.Collections.Generic;
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
            // считывание из БД
            return "Общество с ограниченной ответственностью «Испытательный Центр Вектор»";
        }

        public string GetJournalTitleRow2()
        {
            // считывание из БД
            return "Испытательный центр";
        }

        public string GetJournalTitleRow3()
        {
            // считывание из БД
            return "«Журнал регистрации направлений и образцов 4»";
        }

        public Tuple<string, string> GetJournalTitleRow4()
        {
            // считывание из БД
            string value1, value2;
            value1 = "Ответственный за ведение журнала";
            // подчёркивания для строки
            value2 = "Дата начала       « 09 »           января            2023 г.";
            return new Tuple<string, string>(value1, value2);
        }

        public Tuple<string, string> GetJournalTitleRow5()
        {
            // считывание из БД
            string value1, value2;
            // подчёркивания для строки
            value1 = "ФИО (подпись)*    Бузулуцкова С.А. ";
            // подчёркивания для строки
            value2 = "Дата окончания «      »        2023 г.";
            return new Tuple<string, string>(value1, value2);
        }

        public string GetJournalTitleRow6()
        {
            // считывание из БД
            return "*подпись только для журналов, ведущихся на бумажном носителе";
        }
    }

    
}
