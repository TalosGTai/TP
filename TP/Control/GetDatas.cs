using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.Model;

namespace TP.Control
{
    

    public class GetDatas
    {
        List<Tuple<string, string>> rows { get; }
        DBFunctions functions;

        public GetDatas()
        {
            rows = new List<Tuple<string, string>>();
            functions = new DBFunctions();
            GetJournalTitleList();
        }

        private void GetJournalTitleList()
        {
            rows.Add(new Tuple<string, string>(functions.GetJournalTitleRow1(), ""));
            rows.Add(new Tuple<string, string>(functions.GetJournalTitleRow2(), ""));
            rows.Add(new Tuple<string, string>(functions.GetJournalTitleRow3(), ""));
            rows.Add(functions.GetJournalTitleRow4());
            rows.Add(functions.GetJournalTitleRow5());
            rows.Add(new Tuple<string, string>(functions.GetJournalTitleRow6(), ""));
        }

        public List<Tuple<string, string>> Rows
        {
            get { return rows; }
        }
    }
}
