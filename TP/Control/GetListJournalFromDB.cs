using Microsoft.Scripting.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Documents;
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

        public List<Org1List1> GetList1()
        {
            List<Org1List1> listOrg1 = new List<Org1List1>();
            DataRow row;
            List<string> values = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt.Rows[i];
                for (int j = 1; j < row.ItemArray.Length; j++)
                {
                    values.Add(row[j].ToString());
                }
                Org1List1 list1 = new Org1List1(values);
                listOrg1.Add(list1);
                values.Clear();
            }

            return listOrg1;
        }

        public List<Org1List2> GetList2()
        {
            List<Org1List2> listOrg1 = new List<Org1List2>();
            DataRow row;
            List<string> values = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt.Rows[i];
                for (int j = 1; j < row.ItemArray.Length; j++)
                {
                    values.Add(row[j].ToString());
                }
                Org1List2 list2 = new Org1List2(values);
                listOrg1.Add(list2);
                values.Clear();
            }
            return listOrg1;
        }
    }
}
