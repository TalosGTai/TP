using System.Windows;
using System.Windows.Controls;
using System.IO;
using TP.Model;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Protocols.xaml
    /// </summary>
    public partial class Protocols : Page
    {
        public Protocols()
        {
            InitializeComponent();
            FillProtocolsView(GetCountProtocols(1));
        }

        public Protocols(int idOrg)
        {
            InitializeComponent();
            FillProtocolsView(GetCountProtocols(idOrg));
        }

        private void FillProtocolsView(int countProtocols)
        {
            List <Protocol> listProtocols = new List<Protocol> ();
            for (int i = 1; i <= countProtocols; i++)
            {
                Protocol protocol = new Protocol(numberProtocol: i);
                listProtocols.Add(protocol);
            }
            ListProtocols.ItemsSource = listProtocols;
            DataContext = this;
        }

        private int GetCountProtocols(int idOrg)
        {
            int countProtocols = 1;

            while (Directory.Exists($"Организация{idOrg}\\Протокол{countProtocols}"))
            {
                countProtocols++;
            }

            return countProtocols - 1;
        }

        private void OpenProtocolExcel_Click(object sender, RoutedEventArgs e)
        {
            //int numProtocol = ListProtocols.SelectedItem;
            Excel.Application ex = new Excel.Application();
            ex.Workbooks.Open("C:\\Users\\GTai\\source\\repos\\TP\\TP\\bin\\Debug\\Организация1\\Протокол1\\Протокол1.xlsx");
            ex.Visible = true;
        }

        private void OpenProtocolWord_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\Users\\GTai\\source\\repos\\TP\\TP\\bin\\Debug\\Организация1\\Протокол1\\Протокол1.docx");
        }

        private void OpenProtocolFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer", "C:\\Users\\GTai\\source\\repos\\TP\\TP\\bin\\Debug\\Организация1");
        }

        private void SyncProtocols_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
