using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Journal2.xaml
    /// </summary>
    public partial class Journal2 : Page
    {
        public Journal2()
        {
            InitializeComponent();
            DataContext = this;

            List<Model.Org1List2> journal1 = new List<Model.Org1List2>();
            TableJournal2.ItemsSource = journal1;
        }

        private void BtnJournal1_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window.Name == "TPWindow")
                {
                    MainWindow current = window as MainWindow;
                    current.ViewPages.Content = new Journal1();
                }
            }
        }

        private void TableJournal2_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }
    }
}
