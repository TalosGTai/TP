using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using TP.Control;
using TP.Model;
using TP.Model.Scripts;
using System.IO;
using TP.Model.Org1;
using System.Threading;

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
                Thread threadDirection = new Thread(CopyDirection);
                Thread threadAdditionals = new Thread(CopyAdditionals);
                threadDirection.Start();
                threadAdditionals.Start();
                List<Tuple<List<string>, Dictionary<int, List<string>>>> additionals = new List<Tuple<List<string>, Dictionary<int, List<string>>>>();
                for (int i = 0; i < _pathAdditionals.Count; i++)
                {
                    ExcelParseAdditionals excelParseAdditionals = new ExcelParseAdditionals(_pathAdditionals[i]);
                    additionals.Add(excelParseAdditionals.Values);
                }
                //CreateProtocolFile createProtocolFile = new CreateProtocolFile(_journal, 1, _idProtocol, additionals);
                MessageBox.Show("Протокол успешно создан!");
                Functions functions = new Functions();
                var protocols = new Protocols(_idOrg);
                protocols.FillProtocolsView(protocols.GetCountProtocols());
                functions.Frame.Content = protocols;
            }
        }

        private void CopyDirection()
        {
            string PROTOCOL_EXCEL_PATH = $"Организация{_idOrg}\\Протокол{_idProtocol}\\";
            File.Copy(_directionFileName, PROTOCOL_EXCEL_PATH + GetFileName(_directionFileName), true);
        }

        private void CopyAdditionals()
        {
            string PROTOCOL_EXCEL_PATH = $"Организация{_idOrg}\\Протокол{_idProtocol}\\";
            for (int i = 0; i < _pathAdditionals.Count; i++)
            {
                File.Copy(_pathAdditionals[i], PROTOCOL_EXCEL_PATH + GetFileName(_pathAdditionals[i]), true);
            }
        }

        private string GetFileName(string path)
        {
            string fileName = "";
            for (int i = Math.Max(path.LastIndexOf("\\"), path.LastIndexOf("/")) + 1; i < path.Length; i++)
            {
                if (path[i] != '.')
                    fileName += path[i];
                else
                    break;
            }
            fileName += ".docx";
            return fileName;
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

        private void UpdateJournal()
        {
            // _idJournal + 1
            // _idProtocol
            // A = _idProtocol
            List<string> journal = new List<string>();
            List<string> list1 = new List<string>()
            {
                journal[0],
                _directionDict.Item1["B"],
                _directionDict.Item1["C"],
                _directionDict.Item1["D"],
                _directionDict.Item1["E"],
                _directionDict.Item1["F"],
                journal[6],
                journal[7],
                journal[8],
                journal[9],
                journal[10],
                journal[11],
                journal[12],
                journal[13],
                journal[14],
                journal[15],
                _directionDict.Item1["Q"],
                _directionDict.Item1["R"]
            };
            List<string> list2 = new List<string>()
            {
                journal[0],
                _directionDict.Item2["B"],
                _directionDict.Item2["C"],
                _directionDict.Item2["D"],
                _directionDict.Item2["E"],
                _directionDict.Item2["F"],
                _directionDict.Item2["G"],
                _directionDict.Item2["H"],
                _directionDict.Item2["I"],
            };

            // занесение в БД
            // list1
            // list2
        }
    }
}
