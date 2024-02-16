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
        private bool _isDirection;
        private string _directionFileName;
        private bool _isAdditionals;
        private DocParser _direction;
        private DocParser _additionals;
        private List<string> _pathAdditionals;
        private Tuple<Dictionary<string, string>, Dictionary<string, string>> _journal;

        public NewProtocol()
        {
            InitializeComponent();
            _idOrg = 1;
            _idProtocol = 1;
            _isDirection = false;
            _isAdditionals = false;
        }

        public NewProtocol(int idOrg, int idProtocol)
        {
            InitializeComponent();
            _idOrg = idOrg;
            _idProtocol = idProtocol;
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
                List<Tuple<List<string>, Dictionary<int, List<string>>>> values = new List<Tuple<List<string>, Dictionary<int, List<string>>>>();
                for (int i = 0; i < _pathAdditionals.Count; i++)
                {
                    ExcelParseAdditionals excelParseAdditionals = new ExcelParseAdditionals(_pathAdditionals[i]);
                    values.Add(excelParseAdditionals.Values);
                }
                CreateProtocolFile createProtocolFile = new CreateProtocolFile(_journal, 1, _idProtocol, values);
                MessageBox.Show("Протокол успешно создан!");
                Functions functions = new Functions();
                functions.Frame.Content = new Protocols(_idOrg);
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
                _journal = _direction.JournalParse;
            }
        }
    }
}
