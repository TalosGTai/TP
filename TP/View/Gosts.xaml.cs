using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Gosts.xaml
    /// </summary>
    public partial class Gosts : Page
    {
        public Gosts()
        {
            InitializeComponent();
        }

        private void DeleteGost_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeGost_Click(object sender, RoutedEventArgs e)
        {
            GostsChange gostsChange = new GostsChange();
            gostsChange.ChangeTitleWindow(2);
            gostsChange.ShowDialog();
        }

        private void AddGost_Click(object sender, RoutedEventArgs e)
        {
            GostsChange gostsChange = new GostsChange();
            gostsChange.ChangeTitleWindow(1);
            gostsChange.ShowDialog();
        }

        private void LoadFromFileGost_Click(object sender, RoutedEventArgs e)
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
