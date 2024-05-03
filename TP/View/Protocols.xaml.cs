using System.Windows;
using System.Windows.Controls;
using System.IO;
using TP.Model;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Protocols.xaml
    /// </summary>
    public partial class Protocols : Page
    {
        private List<Protocol> protocols;
        private int _idOrg;

        public Protocols()
        {
            InitializeComponent();
            _idOrg = 1;
            protocols = new List<Protocol>();
        }

        public Protocols(int idOrg)
        {
            InitializeComponent();
            _idOrg = idOrg;
            protocols = new List<Protocol>();
        }

        public void FillProtocolsView(int countProtocols)
        {
            for (int i = 1; i <= countProtocols; i++)
            {
                Protocol protocol = new Protocol(numberProtocol: i);
                protocols.Add(protocol);
            }
            ListProtocols.ItemsSource = protocols;
            DataContext = this;
        }

        public int GetCountProtocols()
        {
            int countProtocols = 1;

            while (Directory.Exists($"Организация{_idOrg}\\Протокол{countProtocols}"))
            {
                countProtocols++;
            }

            return countProtocols - 1;
        }

        private void OpenProtocolExcel_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(ListProtocols.SelectedIndex.ToString());

            if (id == -1)
            {
                MessageBox.Show("Протокол не выбран!\nВыберите протокол, который хотите открыть.", "Ошибка");
            }
            else
            {
                try
                {
                    id = protocols[id].NumberProtocol;
                    Process.Start($"Организация{_idOrg}\\Протокол{id}\\Протокол{id}.xlsx");
                }
                catch
                {
                    MessageBox.Show("Протокол не найден.", "Ошибка");
                }
            }
        }

        private void OpenProtocolWord_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(ListProtocols.SelectedIndex.ToString());

            if (id == -1)
            {
                MessageBox.Show("Протокол не выбран!\nВыберите протокол, который хотите открыть.", "Ошибка");
            }
            else
            {
                try
                {
                    id = protocols[id].NumberProtocol;
                    Process.Start($"Организация{_idOrg}\\Протокол{id}\\Протокол{id}.docx");
                }
                catch
                {
                    MessageBox.Show("Протокол не найден.", "Ошибка");
                }
            }
        }

        private void OpenProtocolFolder_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(ListProtocols.SelectedIndex.ToString());

            if (id == -1)
            {
                Process.Start("explorer", $"Организация{_idOrg}\\");
            }
            else
            {
                id = protocols[id].NumberProtocol;
                Process.Start("explorer", $"Организация{_idOrg}\\Протокол{id}");
            }
        }

        private void SyncProtocols_Click(object sender, RoutedEventArgs e)
        {
            var excludeProtocols = new List<string> ();
             foreach (var i in ListProtocols.Items)
            {
                var prt = (Protocol)i;
                excludeProtocols.Add($"\"{prt.NameProtocol}\"");
            }
            var path = "Организация1\\";
            DBConnection db = new DBConnection();
            db.GetPartOfOrgProtocolRow(1, path, excludeProtocols);
            FillProtocolsView(GetCountProtocols());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ListProtocols.Items.Count > 0)
            {
                DBConnection db = new DBConnection();
                string protocolName = protocols[Convert.ToInt32(ListProtocols.SelectedIndex.ToString())].NameProtocol;
                if (db.FindProtocol(_idOrg, protocolName) != -1)
                {
                    db.DeleteTableProtocolOrgJournal(_idOrg, protocolName);
                    protocols.RemoveAt(Convert.ToInt32(ListProtocols.SelectedIndex.ToString()));
                    ListProtocols.ItemsSource = protocols;
                }
                else
                {
                    MessageBox.Show("Протокола в Базе Данных нет\n. Здесь он отображается потому что на локальном диске он присутствует.",
                        "Протокол удалён");
                }
            }
        }
    }
}
