using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Windows;


namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private string settingsPath = Directory.GetCurrentDirectory() + "\\" + "config.json";
        public Settings()
        {
            try
            {
                InitializeComponent();

                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    string connectionString = jsonObj["ConnectionString"].ToString();

                    var builder = new MySqlConnectionStringBuilder(connectionString);
                    ServerAdress.Text = builder.Server;
                    ServerPort.Text = builder.Port.ToString();
                    Login.Text = builder.UserID.ToString();
                    Password.Text = builder.Password.ToString();
                }
            }
            catch (Exception ex)
            {
                File.Create("log.txt");
                File.AppendText(ex.Message);
            }

        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string json = "";
            if (File.Exists(settingsPath)) 
            {
                json = File.ReadAllText(settingsPath);
            }
            else
            {
                var data = new
                {
                    ConnectionString = "server=localhost;port=3306;Database=laboratory;user=root;password=12345"
                };
                json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                File.WriteAllText(settingsPath, json);
            }
        
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string server = ServerAdress.Text,
                   port = ServerPort.Text,
                   user = Login.Text,
                   password = Password.Text;

            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
            }
            else
            {
                jsonObj["ConnectionString"] = $"server={server};port={port};Database=laboratory;user={user};password={password}";
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(settingsPath, output);

                MessageBox.Show("Изменения успешно внесены.", "Сохранено");
                this.Close();
            }
        }
    }
}
