using Microsoft.Win32;
using System;
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
        private List<Gost> gosts;

        public Gosts()
        {
            InitializeComponent();
            gosts = new List<Gost>();
        }

        private int GetIdRow()
        {
            return Convert.ToInt32(TableGosts.SelectedIndex.ToString());
        }

        private void DeleteGost_Click(object sender, RoutedEventArgs e)
        {
            DBConnection db = new DBConnection();
            var id = GetIdRow();
            gosts.RemoveAt(id);
            db.DeleteGost(id);
        }

        private void ChangeGost_Click(object sender, RoutedEventArgs e)
        {
            GostsChange gostsChange = new GostsChange();
            gostsChange.ChangeTitleWindow(2);
            //gostsChange.StartValues(gosts[GetIdRow()].ShortNameGost, gosts[GetIdRow()].LongNameGost);

            if (gostsChange.ShowDialog() == true)
            {
                gosts[GetIdRow()].ShortNameGost = gostsChange.ShortFormTextBox.Text;
                gosts[GetIdRow()].LongNameGost = gostsChange.LongFormTextBox.Text;   
            }
        }

        private Gost NewGost(string shortForm, string longForm)
        {
            return new Gost(shortForm, longForm);
        }

        private void AddGost_Click(object sender, RoutedEventArgs e)
        {
            GostsChange gostsChange = new GostsChange();
            gostsChange.ChangeTitleWindow(1);

            Gost newGost = NewGost(gostsChange.ShortFormTextBox.Text, gostsChange.LongFormTextBox.Text);
            if (gostsChange.ShowDialog() == true)
            {
                gosts.Add(newGost);
            }
        }

        private void LoadFromFileGost_Click(object sender, RoutedEventArgs e)
        {
            DBConnection db = new DBConnection();

            //1 - первая колонка, краткий ГОСТ; 2 - второй столбец, полный ГОСТ)
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            ExcelParseAdditionals data = null;
            if (openFileDialog.ShowDialog() == true)
            {
                data = new ExcelParseAdditionals(openFileDialog.FileName, true);
            }
            // добавление в DB
            db.AddAllGostsData(data?.GostsTuples);
            LoadToGosts(data.GostsTuples);
        }
        
        private void LoadToGosts(List<Tuple<string, string>> values)
        {
            foreach (var item in values)
            {
                gosts.Add(NewGost(item.Item1, item.Item2));
            }
        }
    }
}
