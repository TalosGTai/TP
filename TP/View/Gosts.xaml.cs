using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
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
        DBConnection db;
        OpenFileDialog openFileDialog;

        public Gosts()
        {
            InitializeComponent();
            gosts = new List<Gost>();
            db = new DBConnection();
            openFileDialog = new OpenFileDialog();
        }

        public void LoadFromDBToGosts()
        {
            var dt = db.GetAllGostsFromDb();

            foreach (DataRow row in dt.Rows)
            {
                if (!IsGostExist(row[1].ToString()))
                    gosts.Add(NewGost(row[1].ToString(), row[2].ToString()));
            }

            TableGosts.ItemsSource = gosts; 
            DataContext = this;
        }

        private bool IsGostExist(string shortName)
        {
            foreach (Gost gost in gosts)
            {
                if (gost.ShortNameGost == shortName)
                    return true;
            }
            return false;
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
            DeleteGostEvent();
        }
        
        private void ChangeSourceTable()
        {
            List<Gost> temp = new List<Gost>();
            TableGosts.ItemsSource = temp;
            TableGosts.ItemsSource = gosts;
        }

        private void ChangeGost_Click(object sender, RoutedEventArgs e)
        {
            ChangeGostEvent();
        }

        private Gost NewGost(string shortForm, string longForm)
        {
            return new Gost(gosts.Count + 1, shortForm, longForm);
        }

        private void AddGostEvent()
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
        
        private void ChangeGostEvent()
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

        private void DeleteGostEvent()
        {
            db.DeleteGost(gosts[GetIdRow()]);
            gosts.RemoveAt(GetIdRow());
            ChangeSourceTable();
        }

        private void AddGost_Click(object sender, RoutedEventArgs e)
        {
            AddGostEvent();
        }

        private void LoadFromFileGost_Click(object sender, RoutedEventArgs e)
        {
            //1 - первая колонка, краткий ГОСТ; 2 - второй столбец, полный ГОСТ)
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                Thread threadLoadGost = new Thread(ThreadLoadGosts);
                threadLoadGost.Start();
                WaitScreen waitScreen = new WaitScreen(threadLoadGost, 1);
                waitScreen.StartLoading();
                waitScreen.SetWaitMsg();
                waitScreen.ShowDialog();
            }
            ChangeSourceTable();
        }

        private void ThreadLoadGosts()
        {
            ExcelParseAdditionals data = null;
            data = new ExcelParseAdditionals(openFileDialog.FileName, true);
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

        private void MenuAddGost_Click(object sender, RoutedEventArgs e)
        {
            AddGostEvent();
        }

        private void MenuChangeGost_Click(object sender, RoutedEventArgs e)
        {
            ChangeGostEvent();
        }

        private void MenuDeleteGost_Click(object sender, RoutedEventArgs e)
        {
            DeleteGostEvent();
        }
    }
}
