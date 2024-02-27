using System;
using System.Windows;
using System.IO;
using TP.Model;
using TP.View;

namespace TP
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            int idOrg = 1;

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow infoWindow = new InfoWindow();
            infoWindow.ShowDialog();
        }

        private void ButtonSendChoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idOrg = 1;
                DBConnection dBConnection = new DBConnection();
                dBConnection.СreateTableEditJournal(idOrg);
                dBConnection.СreateTableEditProtocol(idOrg);
                if (dBConnection.SelectLastId(1) == 0 || dBConnection.SelectLastId(1) == -1)
                    dBConnection.InsertStartValuesEditJournalOrg(1);
                if (dBConnection.SelectLastIdEditProtocols(1) == 0 || dBConnection.SelectLastIdEditProtocols(1) == -1)
                    dBConnection.InsertStartValuesEditProtocolOrg(1);
                for (int i = 0; i < 2; i++)
                {
                    if (!File.Exists($"Организация{idOrg}\\Журнал{i + 1}.xlsx"))
                    {
                        CreateNewJournal createNewJournal = new CreateNewJournal(1, i + 1);
                    }
                }

                idOrg = Convert.ToInt32(ComboBoxChoiceOrganisation.SelectedIndex.ToString()) + 1;
                MainWindow mainWindow = new MainWindow(idOrg);
                Close();
                mainWindow.Show();
            }
            catch
            {
                MessageBox.Show("Для начала проверьте настройки подключения к базе данных");
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void Parametres_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }
    }
}
