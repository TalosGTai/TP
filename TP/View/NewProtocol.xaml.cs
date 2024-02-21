using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TP.Model;
using TP.Model.Scripts;
using System.IO;
using TP.Model.Org1;
using System.Threading;
using System.Globalization;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для NewProtocol.xaml
    /// </summary>
    public partial class NewProtocol : Page
    {
        private int _idOrg;
        private int _idProtocol;
        private int _idJournal;
        private bool _isDirection;
        private string _directionFileName;
        private bool _isAdditionals;
        private DocParser _direction;
        private List<string> _pathAdditionals;
        private Tuple<Dictionary<string, string>, Dictionary<string, string>> _directionDict;
        List<string> _list1;
        List<string> _list2;

        public NewProtocol()
        {
            InitializeComponent();
            _idOrg = 1;
            _idProtocol = 1;
            _isDirection = false;
            _isAdditionals = false;
        }

        public NewProtocol(int idOrg, int idJournal, int idProtocol)
        {
            InitializeComponent();
            _idOrg = idOrg;
            _idProtocol = idProtocol;
            _idJournal = idJournal;
            _isDirection = false;
            _isAdditionals = false;
            LabelProtocolNumber.Content = LabelProtocolNumber.Content + _idProtocol.ToString();
        }

        private bool CheckProtocolReady()
        {
            if (!_isDirection)
            {
                MessageBox.Show("Для начала выберите направление.");
                return false;
            }
            else if (!_isAdditionals)
            {
                MessageBox.Show("Для начала выберите приложения.");
                return false;
            }
            return true;
        }

        private void CreateProtocol_Click(object sender, RoutedEventArgs e)
        {
            if (CheckProtocolReady())
            {

                List<Tuple<List<string>, Dictionary<int, List<string>>>> additionals = new List<Tuple<List<string>, Dictionary<int, List<string>>>>();
                for (int i = 0; i < _pathAdditionals.Count; i++)
                {
                    ExcelParseAdditionals excelParseAdditionals = new ExcelParseAdditionals(_pathAdditionals[i]);
                    additionals.Add(excelParseAdditionals.Values);
                }
                UpdateJournal();
                Tuple<Dictionary<string, string>, Dictionary<string, string>> journal = new Tuple<Dictionary<string, string>, Dictionary<string, string>>(ConvertListToDict(_list1), ConvertListToDict(_list2));
                CreateProtocolFile createProtocolFile = new CreateProtocolFile(journal, 1, _idProtocol, additionals);
                MessageBox.Show("Протокол успешно создан!", "Создание протокола");

                Functions functions = new Functions();
                var protocols = new Protocols(_idOrg);
                protocols.FillProtocolsView(protocols.GetCountProtocols());
                functions.Frame.Content = protocols;
            }
        }

        private void CopyDirection()
        {
            string PROTOCOL_DIRECTION_PATH = $"Организация{_idOrg}\\Протокол{_idProtocol}\\";
            DirectoryInfo directory = new DirectoryInfo(PROTOCOL_DIRECTION_PATH);
            if (!directory.Exists)
            {
                directory.Create();
            }
            FileInfo fileInfo = new FileInfo(_directionFileName);
            FileInfo fileInfo2 = new FileInfo(PROTOCOL_DIRECTION_PATH + GetFileName(_directionFileName));
            if (!fileInfo2.Exists)
                fileInfo.CopyTo(PROTOCOL_DIRECTION_PATH + GetFileName(_directionFileName));
        }

        private void CopyAdditionals()
        {
            string PROTOCOL_EXCEL_PATH = $"Организация{_idOrg}\\Протокол{_idProtocol}\\";
            DirectoryInfo directory = new DirectoryInfo(PROTOCOL_EXCEL_PATH);
            if (!directory.Exists)
            {
                directory.Create();
            }
            for (int i = 0; i < _pathAdditionals.Count; i++)
            {
                FileInfo fileInfo = new FileInfo(_pathAdditionals[i]);
                FileInfo fileInfo2 = new FileInfo(PROTOCOL_EXCEL_PATH + GetFileName(_directionFileName));
                if (!fileInfo2.Exists)
                    fileInfo.CopyTo(PROTOCOL_EXCEL_PATH + GetFileName(_pathAdditionals[i]));
            }
        }

        private string GetFileName(string path)
        {
            string fileName = "";
            for (int i = Math.Max(path.LastIndexOf("\\"), path.LastIndexOf("/")) + 1;
                (i < path.Length || i < path.LastIndexOf('.')); i++)
            {
                fileName += path[i];
            }

            fileName += GetFileExtension(path);
            return fileName;
        }

        public string GetFileExtension(string path)
        {
            string ext = "";
            for (int i = path.LastIndexOf('.'); i < path.Length; i++)
            {
                ext += path[i];
            }
            return ext;
        }

        private void BtnAdditionals_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            _pathAdditionals = new List<string>();
            if (openFileDialog.ShowDialog() == true)
            {
                _isAdditionals = true;
                foreach (string filename in openFileDialog.FileNames)
                    _pathAdditionals.Add(filename);
            }
            Thread threadAdditionals = new Thread(CopyAdditionals);
            threadAdditionals.Start();
        }

        private void BtnDirection_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word (*.docx)|*.docx|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                _isDirection = true;
                _direction = new DocParser(openFileDialog.FileName);
                _directionFileName = openFileDialog.FileName;
                _directionDict = _direction.JournalParse;
            }
            Thread threadDirection = new Thread(CopyDirection);
            threadDirection.Start();
        }

        private char GetAlphaById(int id)
        {
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return alpha[id];
        }

        private Dictionary<string, string> ConvertListToDict(List<string> list)
        {
            Dictionary<string, string> dList = new Dictionary<string, string>();
            for (int i = 0; i < list.Count; i++)
            {
                dList[GetAlphaById(i).ToString()] = list[i];
            }
            return dList;
        }

        private string GetRegNumber()
        {
            DBConnection conn = new DBConnection();
            string date = _list1[7];
            int countDates = Math.Max(conn.GetCountColumnWithSameValue(_idOrg, _idJournal + 1, 1, "H", date), 1);
            return $"л-/{countDates}/{date}";
        }

        private string GetWeekFromDate()
        {
            // количество строк, где строка равна строке (H)
            var dt = DateTime.Now.Date;            
            var cal = new GregorianCalendar();
            var weekNumber = cal.GetWeekOfYear(dt, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            return weekNumber.ToString();
        }

        private string GetNumberProtocol()
        {
            // количество строк, где строка равна строке (H)
            // количество строк, где строка равна строке (A)
            DBConnection conn = new DBConnection();
            string date = _list1[7];
            int countDates = Math.Max(conn.GetCountColumnWithSameValue(_idOrg, _idJournal + 1, 1, "H", date), 1);
            int countA = Math.Max(conn.GetCountColumnWithSameValue(_idOrg, _idJournal + 1, 1, "A", _list1[0]), 1);
            var dt = DateTime.Now;
            return $"{GetWeekFromDate()}-{_idJournal + 1}-{countDates}/{countA}/{dt.Year}";
        }

        private void UpdateJournal()
        {
            // _idJournal + 1
            // A = _idProtocol
            DBConnection conn = new DBConnection();
            List<string> journal = conn.SelectOrgJournalList1ByColumnId(1, _idJournal + 1, _idProtocol);
            _list1 = new List<string>()
            {
                journal[0],
                _directionDict.Item1["B"],
                _directionDict.Item1["C"],
                _directionDict.Item1["D"],
                _directionDict.Item1["E"],
                _directionDict.Item1["F"],
                journal[6],
                journal[7],
                "8",
                journal[9],
                journal[10],
                journal[11],
                journal[12],
                journal[13],
                "14",
                journal[15],
                _directionDict.Item1["Q"],
                _directionDict.Item1["R"]
            };
            _list1[8] = GetRegNumber();
            _list1[14] = GetNumberProtocol();
            _list2 = new List<string>()
            {
                journal[0],
                _list1[14],
                _list1[11],
                _list1[1],
                _list1[8],
                _list1[14],
                _list1[2],
                _list1[2],
                _list1[12],
            };

            // занесение в БД
            conn.UpdateTableJournalOrg1List1(1, _idJournal + 1, new Org1List1(_list1));
            conn.UpdateTableJournalOrg1List2(1, _idJournal + 1, new Org1List2(_list2));
        }
    }
}
