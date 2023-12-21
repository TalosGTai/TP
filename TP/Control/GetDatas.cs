using System;
using System.Collections.Generic;
using TP.Model;

namespace TP.Control
{
    /// <summary>
    /// Класс для работы с заголовком журнала
    /// </summary>
    public class GetDatas
    {
        List<Tuple<string, string>> _rows;
        DBFunctions functions;
        /// <summary>
        /// Запись заголовка журнала в Rows - список из Tuple<string, string>
        /// </summary>
        public GetDatas()
        {
            _rows = new List<Tuple<string, string>>();
            functions = new DBFunctions();
            GetJournalTitleList();
        }
        /// <summary>
        /// Получение заголовка журнала построчно
        /// </summary>
        private void GetJournalTitleList()
        {
            _rows.Add(new Tuple<string, string>(functions.GetJournalTitleRow1(), ""));
            _rows.Add(new Tuple<string, string>(functions.GetJournalTitleRow2(), ""));
            _rows.Add(new Tuple<string, string>(functions.GetJournalTitleRow3(), ""));
            _rows.Add(functions.GetJournalTitleRow4());
            _rows.Add(functions.GetJournalTitleRow5());
            _rows.Add(new Tuple<string, string>(functions.GetJournalTitleRow6(), ""));
        }
        /// <summary>
        /// Список строк полного заголовка журнала в виде списка из Tuple<string, string>
        /// </summary>
        public List<Tuple<string, string>> Rows
        {
            get { return _rows; }
        }
    }
}
