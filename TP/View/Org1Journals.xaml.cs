using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TP.Model;
using Res = TP.Properties.Resources;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using TP.Model.Scripts;
using TP.Control;
using System.Data.Common;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;

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
        int _idJournal;
        int _idList;
        string _currentDirectory;
        bool _firstStart;

        public Org1Journals()
        {
            InitializeComponent();
            _firstStart = true;
            if (_journalsList == null)
                _journalsList = new List<(List<Org1List1>, List<Org1List2>)>();
            TableJournals.ItemsSource = _journalsList1;
            DataContext = this;
            _currentDirectory = Environment.CurrentDirectory;
        }

        public void CreateJournalsFoldersDB()
        {
            _journalsList = new List<(List<Org1List1>, List<Org1List2>)>();
            for (int i = 0; i < Math.Max(GetCountJournals(), 2); i++)
            {
                if (!File.Exists($"Организация1\\Журнал{i + 1}.xlsx"))
                {
                    CreateNewJournal createNewJournal = new CreateNewJournal(1, i + 1);
                }
                if (i >= 2)
                {
                    CmbBoxChoiceJournal.Items.Add("Журнал " + (i + 1).ToString());
                }
                GetListJournalFromDB getListJournalFromDB = new GetListJournalFromDB(1, i + 1, 1);
                List<Org1List1> list1 = new List<Org1List1>();
                List<Org1List2> list2 = new List<Org1List2>();
                list1 = getListJournalFromDB.GetList1();
                list2 = getListJournalFromDB.GetList2();
                _journalsList.Add((list1, list2));
            }
            TableJournals.ItemsSource = _journalsList[0].Item1;
        }

        private int GetCountJournals()
        {
            DBConnection dBConnection = new DBConnection();
            var tables = dBConnection.GetAllTables().Split('|');
            int countTables = -1;
            foreach (string table in tables)
            {
                if (table.IndexOf("rg1jour") != -1)
                    countTables++;
            }
            return countTables / 3;
        }

        private void BtnCreateJournal_Click(object sender, RoutedEventArgs e)
        {
            int idJournal = CmbBoxChoiceJournal.Items.Count + 1;
            _firstStart = false;
            CmbBoxChoiceJournal.Items.Add("Журнал " + idJournal.ToString());
            _journalsList.Add((new List<Org1List1>(), new List<Org1List2>()));
            ChangeSourceTable(idJournal - 1, 1);
            CmbBoxChoiceJournal.SelectedItem = CmbBoxChoiceJournal.Items[idJournal - 1];
            CmbBoxChoiceList.SelectedItem = CmbBoxChoiceList.Items[0];
            CreateNewJournal createNewJournal = new CreateNewJournal(1, idJournal);
        }

        private void ChoiceJournal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _idJournal = CmbBoxChoiceJournal.SelectedIndex;

            if (_journalsList == null)
                _journalsList = new List<(List<Org1List1>, List<Org1List2>)>();
            if (_firstStart)
                SaveChanges(_idJournal);
            LabelJournalNumber.Content = "Журнал " + (Convert.ToInt32(_idJournal.ToString()) + 1).ToString();

            if (!(CmbBoxChoiceList is null) && CmbBoxChoiceList.Items.Count > 0)
            {
                CmbBoxChoiceList.SelectedItem = CmbBoxChoiceList.Items[0];
                ChangeSourceTable(_idJournal, 1);
            }
        }

        private void ChangeSourceTable(int idJournal, int idList)
        {
            while (_journalsList.Count <= idJournal)
                _journalsList.Add((new List<Org1List1>(), new List<Org1List2>()));

            if (!(CmbBoxChoiceList is null) && !(CmbBoxChoiceJournal is null) && !(_journalsList is null) && !(TableJournals is null)) 
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
                        TableJournals.ItemsSource = _journalsList[idJournal].Item1;
                        TableJournals.Visibility = Visibility.Visible;
                        TableJournalsList2.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }

        private void SaveChanges(int idJournal)
        {
            string path = _currentDirectory + $"\\Организация1\\Журнал{idJournal + 1}.xlsx";
            //var dialog = MessageBox.Show("Сохранить все изменения?", "Сохранение изменений", MessageBoxButton.YesNo);
            //if (dialog == MessageBoxResult.Yes)
            //    //Thread localJournal = new Thread();
            //    //Thread dbJournal = new Thread();
            //    //localJournal.Start();
            //    //dbJournal.Start();
            // журнал
            if (_journalsList.Count > 0)
            {
                // локально
                ExcelWorker excelWorker = new ExcelWorker(path, _journalsList[idJournal].Item1, _journalsList[idJournal].Item2);
                excelWorker.SaveWorksheets();
                // бд
                DBConnection dBConnection = new DBConnection();
                dBConnection.SaveTableJournalOrg1List1(1, _idJournal + 1, _journalsList[idJournal].Item1);
                dBConnection.SaveTableJournalOrg1List2(1, _idJournal + 1, _journalsList[idJournal].Item2);
                MessageBox.Show("Все изменения успешно внесены", "Сохранение");
            }
            else
            {
                MessageBox.Show("Невозможно сохранить пустые значения.", "Ошибка");
            }
        }

        private void CmbBoxChoiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _idList = CmbBoxChoiceList.SelectedIndex;
            _idJournal = CmbBoxChoiceJournal.SelectedIndex;
            
            if (_firstStart)
                SaveChanges(_idJournal);
            if (_journalsList is null)
                _journalsList = new List<(List<Org1List1>, List<Org1List2>)>();

            if (!(CmbBoxChoiceList is null) && CmbBoxChoiceList.Items.Count > 0)
            {
                CmbBoxChoiceList.SelectedItem = CmbBoxChoiceList.Items[_idList];
                ChangeSourceTable(_idJournal, _idList + 1);
            }
        }

        private void CreateProtocol_Click(object sender, RoutedEventArgs e)
        {
            int idProtocol = GetCountProtocols();
            _idJournal = CmbBoxChoiceJournal.SelectedIndex;
            SaveChanges(_idJournal);
            Functions functions = new Functions();
            functions.Frame.Content = new NewProtocol(1, _idJournal, idProtocol);
        }

        public int GetCountProtocols()
        {
            int countProtocols = 1;

            while (Directory.Exists($"Организация{1}\\Протокол{countProtocols}"))
            {
                countProtocols++;
            }

            return countProtocols;
        }

        private void OpenCurrentJournal_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application application = null;
            _idJournal = CmbBoxChoiceJournal.SelectedIndex;
            int idJournal = CmbBoxChoiceJournal.SelectedIndex + 1;
            SaveChanges(_idJournal);

            try
            {
                application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Open(_currentDirectory + $"\\Организация1\\Журнал{idJournal}.xlsx");
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

        private void SaveJournals_Click(object sender, RoutedEventArgs e)
        {
            _idJournal = CmbBoxChoiceJournal.SelectedIndex;
            _firstStart = false;
            SaveChanges(_idJournal);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<Org1List1> t1 = new List<Org1List1>();
            List<Org1List2> t2 = new List<Org1List2>();

            _idList = CmbBoxChoiceList.SelectedIndex + 1;
            _idJournal = CmbBoxChoiceJournal.SelectedIndex;
            _firstStart = false;

            if (_journalsList.Count <= _idJournal)
            {
                _journalsList.Add((new List<Org1List1>(), new List<Org1List2>()));
            }

            if (_idList == 1)
            {
                _journalsList[_idJournal].Item1.Add(new Org1List1());
                TableJournals.ItemsSource = t1;
                TableJournals.ItemsSource = _journalsList[_idJournal].Item1;
            }
            else if (_idList == 2)
            {
                _journalsList[_idJournal].Item2.Add(new Org1List2());
                TableJournalsList2.ItemsSource = t2;
                TableJournalsList2.ItemsSource = _journalsList[_idJournal].Item2;
            }
            else
            {
                MessageBox.Show($"Ошибка с выбором листа {_idList}", "Ошибка");
            }
        }
    }
}
