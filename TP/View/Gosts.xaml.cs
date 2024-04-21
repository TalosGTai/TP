using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TP.Model;
using TP.Model.Scripts;
using Ubiety.Dns.Core.Common;


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
            gosts.RemoveAt(GetIdRow());
            // delete in DB this row by id
        }

        private void ChangeGost_Click(object sender, RoutedEventArgs e)
        {
            GostsChange gostsChange = new GostsChange();
            gostsChange.ChangeTitleWindow(2);
            //gostsChange.StartValues(gosts[GetIdRow()].ShortNameGost, gosts[GetIdRow()].LongNameGost);

            if (gostsChange.ShowDialog() == true)
            {
                // GetIdRow() - строка, которую меняем
                // gostsChange.ShortFormTextBox.Text, gostsChange.LongFormTextBox.Text - новые значения
                // заносим в ДБ
                gosts[GetIdRow()].ShortNameGost = gostsChange.ShortFormTextBox.Text;
                gosts[GetIdRow()].ShortNameGost = gostsChange.LongFormTextBox.Text;
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

            if (gostsChange.ShowDialog() == true)
            {
                // GetIdRow() - строка, которую меняем
                // gostsChange.ShortFormTextBox.Text, gostsChange.LongFormTextBox.Text - новые значения
                // заносим в ДБ
                gosts.Add(NewGost(gostsChange.ShortFormTextBox.Text, gostsChange.LongFormTextBox.Text));
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
            // заполнение в gosts
            //db.AddGost(_idOrg, "test1" , "test2");
        }
    }
}
