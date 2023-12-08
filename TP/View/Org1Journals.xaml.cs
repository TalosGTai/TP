using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using System.Data;
using Res = TP.Properties.Resources;
using TP.View.Org1;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Windows.Interop;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Journal1.xaml
    /// </summary>
    public partial class Org1Journals : System.Windows.Controls.Page
    {
        private List<(List<Org1List1>, List<Org1List2>)> _journalsList;
        private List<Org1List1> _journalsList1;
        private List<Org1List2> _journalsList2;

        public Org1Journals()
        {
            InitializeComponent();
            DataContext = this;

            _journalsList1 = new List<Org1List1>();
            _journalsList2 = new List<Org1List2>();
            _journalsList = new List<(List<Org1List1>, List<Org1List2>)>();

            _journalsList.Add((_journalsList1, _journalsList2));
            _journalsList.Add((_journalsList1, _journalsList2));
            TableJournals.ItemsSource = _journalsList[0].Item1;

            if (!File.Exists("Организация1\\Журнал1.xlsx"))
            {
                CreateNewJournal createNewJournal = new CreateNewJournal();
            }
            if (!File.Exists("Организация1\\Журнал2.xlsx"))
            {
                CreateNewJournal createNewJournal = new CreateNewJournal(1, 2);
            }
        }

        private void BtnCreateJournal_Click(object sender, RoutedEventArgs e)
        {
            int idJournal = CmbBoxChoiceJournal.Items.Count + 1;
            CmbBoxChoiceJournal.Items.Add("Журнал " + idJournal.ToString());
            List<Org1List1> list1 = new List<Org1List1>();
            List<Org1List2> list2 = new List<Org1List2>();
            _journalsList.Add((list1, list2));
            ChangeSourceTable(idJournal - 1, 1);
            CmbBoxChoiceJournal.SelectedItem = CmbBoxChoiceJournal.Items[idJournal - 1];
            CmbBoxChoiceList.SelectedItem = CmbBoxChoiceList.Items[0];
            CreateNewJournal createNewJournal = new CreateNewJournal(1, idJournal);
        }

        private void ChoiceJournal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idJournal = CmbBoxChoiceJournal.SelectedIndex;
            LabelJournalNumber.Content = "Журнал " + (Convert.ToInt32(idJournal.ToString()) + 1).ToString();

            if (!(CmbBoxChoiceList is null) && CmbBoxChoiceList.Items.Count > 0)
            {
                CmbBoxChoiceList.SelectedItem = CmbBoxChoiceList.Items[0];
                ChangeSourceTable(idJournal, 1);
            }
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

        private void CreateProtocol_Click(object sender, RoutedEventArgs e)
        {
            Functions functions = new Functions();
            functions.Frame.Content = new NewProtocol();
        }

        private void OpenCurrentJournal_Click(object sender, RoutedEventArgs e)
        {
            string currentJournal = CmbBoxChoiceJournal.SelectedItem.ToString();
            Excel.Application application = null;
            string currentDirectory = Environment.CurrentDirectory;
            int idJournal = CmbBoxChoiceJournal.SelectedIndex + 1;
            try
            {
                application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Open(currentDirectory + $"\\Организация1\\Журнал{idJournal}.xlsx");
                application.Visible = true;
            }
            catch 
            {
                
            }
            finally
            {
                Marshal.ReleaseComObject(application);
            }
        }
    }
}
