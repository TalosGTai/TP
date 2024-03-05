using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using TP.Model;


namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private string settingsPath = "config.json";
        public Settings()
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

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        ConnectionString = "server=localhost;port=3306;user=root;password=12345"
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
                    jsonObj["ConnectionString"] = $"server={server};port={port};user={user};password={password}";
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(settingsPath, output);

                    MessageBox.Show("Изменения успешно внесены.", "Сохранено");
                    TP.Model.DBConnection db = new TP.Model.DBConnection();
                    db.CheckAndCreateSchema();
                    this.Close();
                }
            }
            catch (JsonException ex)
            {
                Logger.LogError(ex, "Ошибка Newtonsoft json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ошибка работы с файлом конфигурации. Проверьте не открыт ли config.json в другом приложении");
            }
        }
    }
}
