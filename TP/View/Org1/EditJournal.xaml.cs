using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TP.Control;
using TP.Model;

namespace TP.View.Org1
{
    /// <summary>
    /// Логика взаимодействия для EditJournal.xaml
    /// </summary>
    public partial class EditJournal : Page
    {
        public EditJournal()
        {
            InitializeComponent();

            GetDatas getDatas = new GetDatas();
            var rows = getDatas.Rows;
            FillRows(rows);
            LblSaveChanges.Visibility = Visibility.Hidden;
        }

        private void FillRows(List<Tuple<string, string>> rows)
        {
            TxtBoxRow1.Text = rows[0].Item1;
            TxtBoxRow2.Text = rows[1].Item1;
            TxtBoxRow3.Text = rows[2].Item1;
            TxtBoxRow4.Text = rows[3].Item1;
            TxtBoxRow41.Text = rows[3].Item2;
            TxtBoxRow5.Text = rows[4].Item1;
            TxtBoxRow51.Text = rows[4].Item2;
            TxtBoxRow6.Text = rows[5].Item1;
        }

        private void SaveChangesJournalTitle_Click(object sender, RoutedEventArgs e)
        {
            LblSaveChanges.Visibility = Visibility.Visible;
            List<string> saveChanges = new List<string>
            {
                TxtBoxRow1.Text,
                TxtBoxRow2.Text,
                TxtBoxRow3.Text,
                TxtBoxRow4.Text,
                TxtBoxRow41.Text,
                TxtBoxRow5.Text,
                TxtBoxRow51.Text,
                TxtBoxRow6.Text
            };
            DBConnection dBConnection = new DBConnection();
            dBConnection.InsertJournalOrgChangesRow(1, saveChanges);
        }
    }
}
