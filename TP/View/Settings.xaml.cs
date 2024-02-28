using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows;


namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private const string settingsPath = "config.txt";
        
        public Settings()
        {
            InitializeComponent();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string server = "server=" + ServerAdress.Text + ";",
                   port = "port=" + ServerPort.Text + ";",
                   database = "database=laboratory;",
                   user = "user=" + Login.Text + ";",
                   password = "password=" + Password.Text;

            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
            }
            else
            {
                StreamWriter sw = new StreamWriter(settingsPath);
                sw.WriteLine(server + port + database + user + password);
                sw.Close();
                MessageBox.Show("Изменения успешно внесены.", "Сохранено");
                this.Close();
            }
        }
    }
}
