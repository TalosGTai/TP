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
        private int _idProduct;
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

        public NewProtocol(int idOrg, int idJournal, int idProtocol, int idProduct)
        {
            InitializeComponent();
            _idOrg = idOrg;
            _idProtocol = idProtocol;
            _idProduct = idProduct;
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

        private HashSet<string> MergeGosts(HashSet<string> gostsAll, HashSet<string> gostsNew)
        {
            foreach (string gost in gostsNew)
                gostsAll.Add(gost);
            return gostsAll;
        }

        private void CreateProtocol_Click(object sender, RoutedEventArgs e)
        {
            if (CheckProtocolReady())
            {
                try
                {
                    // множество gosts для хранения всех гостов с приложения
                    HashSet<string> gosts = new HashSet<string>();
                    List<Tuple<List<string>, Dictionary<int, List<string>>>> additionals = new List<Tuple<List<string>, Dictionary<int, List<string>>>>();
                    for (int i = 0; i < _pathAdditionals.Count; i++)
                    {
                        ExcelParseAdditionals excelParseAdditionals = new ExcelParseAdditionals(_pathAdditionals[i]);
                        additionals.Add(excelParseAdditionals.Values);
                        gosts = MergeGosts(gosts, excelParseAdditionals.Gosts);
                    }
                    List<string> additionalValues = UpdateJournal();
                    Tuple<Dictionary<string, string>, Dictionary<string, string>> journal = new Tuple<Dictionary<string, string>, Dictionary<string, string>>(ConvertListToDict(_list1), ConvertListToDict(_list2));
                    CreateProtocolFile createProtocolFile = new CreateProtocolFile(journal, 1, _idProtocol, additionals);

                    string path = $"Организация{_idOrg}\\Протокол{_idProtocol}\\";
                    for (int i = 0; i < _pathAdditionals.Count; i++)
                    {
                        ExcelWorker excelWorker = new ExcelWorker(path + GetFileName(_pathAdditionals[i]));
                        excelWorker.SaveAllWorksheets(additionalValues[0], additionalValues[1]);
                    }

                    MessageBox.Show("Протокол успешно создан!", "Создание протокола");

                    Functions functions = new Functions();
                    var protocols = new Protocols(_idOrg);
                    protocols.FillProtocolsView(protocols.GetCountProtocols());
                    functions.Frame.Content = protocols;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
            }
        }

        private void CopyDirection()
        {
            try
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
            catch
            {

            }
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
                FileInfo fileInfo2 = new FileInfo(PROTOCOL_EXCEL_PATH + GetFileName(_pathAdditionals[i]));
                if (!fileInfo2.Exists)
                    fileInfo.CopyTo(PROTOCOL_EXCEL_PATH + GetFileName(_pathAdditionals[i]));
            }
        }

        private string GetFileName(string path)
        {
            try
            {
                string fileName = "";
                for (int i = Math.Max(path.LastIndexOf("\\"), path.LastIndexOf("/")) + 1;
                    (i < path.Length || i < path.LastIndexOf('.')); i++)
                {
                    fileName += path[i];
                }

                //fileName += GetFileExtension(path);
                return fileName;
            }
            catch
            {

            }
            return "";
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
            // date = _list1[7];
            int countDates = Math.Max(conn.GetCountColumnWithSameValue(_idOrg, _idJournal + 1, 1, "H", _list1[7]), 1);
            return $"л-{countDates}/{_list1[7]}";
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

        private string GetNewDate(string date)
        {
            var newDate = DateTime.Parse(_list1[7]);
            newDate = newDate.AddDays(3);
            return newDate.ToString();
        }

        private List<string> UpdateJournal()
        {
            try
            {
                // _idJournal + 1
                // A = _idProtocol
                DBConnection conn = new DBConnection();
                List<string> journal = conn.SelectOrgJournalList1ByColumnId(1, _idJournal + 1, _idProduct);
                _list1 = new List<string>()
                {
                    ToStringDataBase(journal[0]),
                    ToStringDataBase(_directionDict.Item1["B"]),
                    ToStringDataBase(_directionDict.Item1["C"]),
                    ToStringDataBase(_directionDict.Item1["D"]),
                    ToStringDataBase(_directionDict.Item1["E"]),
                    ToStringDataBase(_directionDict.Item1["F"]),
                    ToStringDataBase(journal[6]),
                    ToStringDataBase(journal[7]),
                    "8",
                    ToStringDataBase(journal[9]),
                    ToStringDataBase(journal[10]),
                    ToStringDataBase(journal[11]),
                    ToStringDataBase(journal[12]),
                    ToStringDataBase(journal[13]),
                    "14",
                    ToStringDataBase(journal[15]),
                    ToStringDataBase(_directionDict.Item1["Q"]),
                    ToStringDataBase(_directionDict.Item1["R"])
                };
                _list1[8] = GetRegNumber();
                _list1[14] = GetNumberProtocol();
                _list2 = new List<string>()
                {
                    ToStringDataBase(journal[0]),
                    ToStringDataBase(_list1[14]),
                    ToStringDataBase(_list1[11]),
                    ToStringDataBase(_list1[1]),
                    ToStringDataBase(_list1[8]),
                    ToStringDataBase(_list1[14]),
                    ToStringDataBase(_list1[2]),
                    ToStringDataBase(_list1[2]),
                    ToStringDataBase(_list1[12]),
                };

                // занесение в БД
                conn.UpdateTableJournalOrg1List1(1, _idJournal + 1, new Org1List1(_list1));
                conn.UpdateTableJournalOrg1List2(1, _idJournal + 1, new Org1List2(_list2));

                return new List<string> { _list1[8], _list1[7] + "-" + GetNewDate(_list1[7]) };
            }
            catch (Exception ex)
            {
                Logger.LogDbError(ex);
                throw;
            }
        }

        private string ToStringDataBase(string value)
        {
            string result = "";
            foreach (char c in value)
            {
                if (c == '\'')
                {
                    result += "\'";
                }
                else if (c == '"')
                {
                    result += '\"';
                }
                else
                {
                    result += c;
                }
            }
            return result;
        }
    }
}