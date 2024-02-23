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
        private int _idOrg;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(int idOrg)
        {
            InitializeComponent();
            this._idOrg = idOrg;
            Title += idOrg.ToString();
            LabelLab.Content += idOrg.ToString();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow infoWindow = new InfoWindow();
            infoWindow.ShowDialog();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
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
            if (_idOrg == 1)
            {
                Org1Journals org1Journals = new Org1Journals();
                ViewPages.Content = org1Journals;
                org1Journals.CreateJournalsFoldersDB();
            }
            else if (_idOrg == 2)
                ViewPages.Content = new Org2Journals();
            else
                MessageBox.Show("Ошибка загрузки журналов!");
        }

        private void Protocols_Click(object sender, RoutedEventArgs e)
        {
            Protocols protocols = new Protocols(_idOrg);
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
            ViewPages.Content = new FilesPage(_idOrg);
        }

        private void EditJournalsTitul_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewPages.Content = new ChooseChanges(_idOrg);
        }
    }
}
