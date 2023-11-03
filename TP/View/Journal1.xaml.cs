using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TP.Model;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Journal1.xaml
    /// </summary>
    public partial class Journal1 : Page
    {
        public Journal1()
        {
            InitializeComponent();
            DataContext = this;

            List<Org1Journal1> journal1 = new List<Org1Journal1>();
            TableJournal1.ItemsSource = journal1;
        }

        private void BtnJournal2_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window.Name == "TPWindow")
                {
                    MainWindow current = window as MainWindow;
                    current.ViewPages.Content = new Journal2();
                }
            }
        }

        private void TableJournal1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }
    }
}
