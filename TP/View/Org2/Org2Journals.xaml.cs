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
using TP.Model;

namespace TP.View.Org2
{
    /// <summary>
    /// Логика взаимодействия для Org2Journals.xaml
    /// </summary>
    public partial class Org2Journals : Page
    {
        private List<(List<Org2List1>, List<Org2List2>)> _journalsList;
        private List<Org2List1> _journalsList1;
        private List<Org2List2> _journalsList2;
        public Org2Journals()
        {
            InitializeComponent();

            DataContext = this;

            _journalsList1 = new List<Org2List1>();
            _journalsList2 = new List<Org2List2>();
            _journalsList = new List<(List<Org2List1>, List<Org2List2>)>();

            _journalsList.Add((_journalsList1, _journalsList2));
            _journalsList.Add((_journalsList1, _journalsList2));
            TableJournals.ItemsSource = _journalsList[0].Item1;
        }

        private void CmbBoxChoiceJournal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idJournal = CmbBoxChoiceJournal.SelectedIndex;
            LabelJournalNumber.Content = "Журнал " + (Convert.ToInt32(idJournal.ToString()) + 1).ToString();

            if (!(CmbBoxChoiceList is null) && CmbBoxChoiceList.Items.Count > 0)
            {
                CmbBoxChoiceList.SelectedItem = CmbBoxChoiceList.Items[0];
                ChangeSourceTable(idJournal, 1);
            }
        }

        private void CmbBoxChoiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idList = CmbBoxChoiceList.SelectedIndex;
            int idJournal = CmbBoxChoiceJournal.SelectedIndex;

            if (!(CmbBoxChoiceList is null) && CmbBoxChoiceList.Items.Count > 0)
            {
                CmbBoxChoiceList.SelectedItem = CmbBoxChoiceList.Items[idList];
                ChangeSourceTable(idJournal, idList + 1);
            }
        }

        private void BtnCreateJournal_Click(object sender, RoutedEventArgs e)
        {
            //int idJournal = CmbBoxChoiceJournal.Items.Count + 1;
            //CmbBoxChoiceJournal.Items.Add("Журнал " + idJournal.ToString());
            //CmbBoxChoiceJournal.SelectedItem = CmbBoxChoiceJournal.Items[idJournal - 1];
            //CmbBoxChoiceList.SelectedItem = CmbBoxChoiceList.Items[0];
        }

        private void ChangeSourceTable(int idJournal, int idList)
        {
            if (!(CmbBoxChoiceList is null) && !(CmbBoxChoiceJournal is null) && !(_journalsList is null))
            {
                switch (idList)
                {
                    case 1:
                        TableJournals.ItemsSource = _journalsList[idJournal].Item1;
                        TableJournals.Visibility = Visibility.Visible;
                        TableJournalsList2.Visibility = Visibility.Hidden;
                        break;
                    case 2:
                        TableJournalsList2.ItemsSource = _journalsList[idJournal].Item2;
                        TableJournals.Visibility = Visibility.Hidden;
                        TableJournalsList2.Visibility = Visibility.Visible;
                        break;
                    default:
                        TableJournals.ItemsSource = _journalsList[0].Item1;
                        TableJournals.Visibility = Visibility.Visible;
                        TableJournalsList2.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }
    }
}
