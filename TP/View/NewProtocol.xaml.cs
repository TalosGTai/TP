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
        private bool _isAdditionals;
        private DocParser _direction;
        private DocParser _additionals;
        private List<string> _pathAdditionals;
        private Tuple<Dictionary<string, string>, Dictionary<string, string>> _journal;

        public NewProtocol()
        {
            InitializeComponent();
            _idProtocol = 1;
            _isDirection = false;
            _isAdditionals = false;
        }

        public NewProtocol(int idProtocol)
        {
            InitializeComponent();
            _idProtocol = idProtocol;
            _isDirection = false;
            _isAdditionals = false;
        }

        private void CheckProtocolReady()
        {
            if (!_isDirection)
                MessageBox.Show("Для начала выберете направление.");
            else if (!_isAdditionals)
                MessageBox.Show("Для начала выберете приложения.");
        }

        private void CreateProtocol_Click(object sender, RoutedEventArgs e)
        {
            if (_isDirection && _isAdditionals)
            {
                //FunctionsPrint functionsPrint = new FunctionsPrint();
                //functionsPrint.PrintTupleDictionary(docParser.JournalParse);
                CreateProtocolFile createProtocolFile = new CreateProtocolFile(_journal, _idOrg, _idProtocol);
            }
            CheckProtocolReady();
        }

        private void BtnAdditionals_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Word (*.docx)|*.docx|All files (*.*)|*.*";
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
                _journal = _direction.JournalParse;
            }
        }
    }
}
