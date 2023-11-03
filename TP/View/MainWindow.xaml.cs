using System;
using System.Collections.Generic;
using System.IO;
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
using TP.View;

namespace TP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int idOrg;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(int idOrg)
        {
            InitializeComponent();
            this.idOrg = idOrg;
            Title += idOrg.ToString();
            LabelLab.Content += idOrg.ToString();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            // create_window
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChoiceLab_Click(object sender, RoutedEventArgs e)
        {
            SwitchOrganisation();
        }

        private void NewProtocol_Click(object sender, RoutedEventArgs e)
        {
            if (idOrg == 1)
                ViewPages.Content = new NewProtocol();
            else if (idOrg == 2)
                ViewPages.Content = new NewProtocol();
            else
                MessageBox.Show("Ошибка загрузки контента!");
        }

        private void Journals_Click(object sender, RoutedEventArgs e)
        {
            if (idOrg == 1)
                ViewPages.Content = new Journal1();
            else if (idOrg == 2)
                ViewPages.Content = new Journal2();
            else
                MessageBox.Show("Ошибка загрузки журналов!");
        }

        private void Protocols_Click(object sender, RoutedEventArgs e)
        {
            if (idOrg == 1)
                ViewPages.Content = new Protocols();
            else if (idOrg == 2)
                ViewPages.Content = new Protocols();
            else
                MessageBox.Show("Ошибка загрузки протоколов!");
        }

        private void ChoiceOrganization_Click(object sender, RoutedEventArgs e)
        {
            SwitchOrganisation();
        }

        private void SwitchOrganisation()
        {
            AuthWindow authWindow = new AuthWindow();
            Close();
            authWindow.Show();
        }

        private void Files_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (idOrg == 1)
                ViewPages.Content = new FilesPage();
            else if (idOrg == 2)
                ViewPages.Content = new FilesPage();
            else
                MessageBox.Show("Ошибка загрузки файлов!");
        }
    }
}
