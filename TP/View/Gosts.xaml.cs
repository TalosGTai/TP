using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TP.Model;
using TP.Model.Scripts;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Gosts.xaml
    /// </summary>
    public partial class Gosts : Page
    {
        int _idOrg;
        public Gosts(int idOrg)
        {
            InitializeComponent();
            _idOrg = idOrg;
        }

        private void DeleteGost_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeGost_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddGost_Click(object sender, RoutedEventArgs e)
        {
            DBConnection db = new DBConnection();
            //1 - первая колонка, краткий ГОСТ; 2 - второй столбец, полный ГОСТ)
            db.AddGost(_idOrg, "test1" , "test2");
        }
        private void AddAllGosts_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            var _pathAdditionals = new List<string>();
            ExcelParseAdditionals data = null;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    _pathAdditionals.Add(filename);
                data = new ExcelParseAdditionals(openFileDialog.FileName, true);
            }

            DBConnection db = new DBConnection();
            //1 - первая колонка, краткий ГОСТ; 2 - второй столбец, полный ГОСТ)
            db.AddGostData(_idOrg, data.GostsTable[1], data.GostsTable[2]);
        }
    }
}
