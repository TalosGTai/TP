using System.Windows;
using System.Windows.Input;
using TP.Model;
using TP.View;
using TP.View.Org1;
using TP.View.Org2;

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
            InfoWindow infoWindow = new InfoWindow();
            infoWindow.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChoiceLab_Click(object sender, RoutedEventArgs e)
        {
            SwitchOrganisation();
        }

        private void Journals_Click(object sender, RoutedEventArgs e)
        {
            if (idOrg == 1)
                ViewPages.Content = new Org1Journals();
            else if (idOrg == 2)
                ViewPages.Content = new Org2Journals();
            else
                MessageBox.Show("Ошибка загрузки журналов!");
        }

        private void Protocols_Click(object sender, RoutedEventArgs e)
        {
            Protocols protocols = new Protocols(idOrg);
            ViewPages.Content = protocols;
            protocols.FillProtocolsView(protocols.GetCountProtocols());
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

        private void EditJournalsTitul_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (idOrg == 1)
                ViewPages.Content = new ChooseChanges();
            else if (idOrg == 2)
                ViewPages.Content = new ChooseChanges();
            else
                MessageBox.Show("Ошибка загрузки страницы изменения журналов и протоколов!");
        }
    }
}
