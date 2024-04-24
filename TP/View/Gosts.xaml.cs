using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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

        public void LoadFromDBToGosts()
        {
            DBConnection db = new DBConnection();
            var dt = db.GetAllGostsFromDb();

            foreach (DataRow row in dt.Rows)
            {
                gosts.Add(NewGost(row[1].ToString(), row[2].ToString()));
            }

            TableGosts.ItemsSource = gosts; 
            DataContext = this;
        }

        private int GetIdRow()
        {
            return Convert.ToInt32(TableGosts.SelectedIndex.ToString());
        }

        private int GetIdGost()
        {
            return gosts[Convert.ToInt32(TableGosts.SelectedIndex.ToString())].NumberGost;
        }

        private void DeleteGost_Click(object sender, RoutedEventArgs e)
        {
            DBConnection db = new DBConnection();
            db.DeleteGost(gosts[GetIdRow()]);
            gosts.RemoveAt(GetIdRow());
            ChangeSourceTable();
        }
        
        private void ChangeSourceTable()
        {
            List<Gost> temp = new List<Gost>();
            TableGosts.ItemsSource = temp;
            TableGosts.ItemsSource = gosts;
        }

        private void ChangeGost_Click(object sender, RoutedEventArgs e)
        {
            GostsChange gostsChange = new GostsChange(GetIdGost());
            gostsChange.ChangeTitleWindow(2);
            gostsChange.StartValues(gosts[GetIdRow()].ShortNameGost, gosts[GetIdRow()].LongNameGost);

            if (gostsChange.ShowDialog() == true)
            {
                gosts[GetIdRow()].ShortNameGost = gostsChange.ShortFormTextBox.Text;
                gosts[GetIdRow()].LongNameGost = gostsChange.LongFormTextBox.Text;
                ChangeSourceTable();
            }
        }

        private Gost NewGost(string shortForm, string longForm)
        {
            return new Gost(gosts.Count + 1, shortForm, longForm);
        }

        private void AddGost_Click(object sender, RoutedEventArgs e)
        {
            GostsChange gostsChange = new GostsChange();
            gostsChange.ChangeTitleWindow(1);

            if (gostsChange.ShowDialog() == true)
            {
                Gost newGost = NewGost(gostsChange.ShortFormTextBox.Text, gostsChange.LongFormTextBox.Text);
                gosts.Add(newGost);
                ChangeSourceTable();
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

                db.AddAllGostsData(data?.GostsTuples);
                LoadToGosts(data.GostsTuples);
                ChangeSourceTable();
            }
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
