using Microsoft.Scripting.Metadata;
using System;
using System.Data;
using TP.Model;

namespace TP.Control
{
    public class GetListJournalFromDB
    {
        System.Data.DataTable dt;
        public GetListJournalFromDB(int idOrg, int idJournal, int idList) 
        {
            DBConnection dBConnection = new DBConnection();
            dt = dBConnection.GetListJournalOrg(idOrg, idJournal, idList);
        }

        public void printRow()
        {
            DataRow row;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt.Rows[i];
            }
        }
    }
}
