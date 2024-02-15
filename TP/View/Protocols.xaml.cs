using System.Windows;
using System.Windows.Controls;
using System.IO;
using TP.Model;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
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
            Excel.Application ex = null;

            if (id == -1)
            {
                MessageBox.Show("Протокол не выбран!\nВыберите протокол, который хотите открыть.", "Ошибка");
            }
            else
            {
                try
                {
                    id = protocols[id].NumberProtocol;
                    ex = new Excel.Application();
                    ex.Workbooks.Open($"C:\\Users\\GTai\\source\\repos\\TP\\TP\\bin\\Debug\\Организация{_idOrg}\\Протокол{id}\\Протокол{id}.xlsx");
                    ex.Visible = true;
                }
                catch
                {
                    MessageBox.Show("", "Ошибка");
                }
                finally
                {
                    Marshal.ReleaseComObject(ex);
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
                    Process.Start($"C:\\Users\\GTai\\source\\repos\\TP\\TP\\bin\\Debug\\Организация{_idOrg}\\Протокол{id}\\Протокол{id}.docx");
                }
                catch
                {

                }
            }
        }

        private void OpenProtocolFolder_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(ListProtocols.SelectedIndex.ToString());

            if (id == -1)
            {
                MessageBox.Show("Протокол не выбран!\nВыберите протокол, который хотите открыть.", "Ошибка");
            }
            else
            {
                id = protocols[id].NumberProtocol;
                Process.Start("explorer", $"C:\\Users\\GTai\\source\\repos\\TP\\TP\\bin\\Debug\\Организация{_idOrg}\\Протокол{id}");
            }
        }

        private void SyncProtocols_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
