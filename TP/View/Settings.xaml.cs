using System.Data.SqlClient;
using System.IO;
using System.Windows;


namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();


            string json = File.ReadAllText("..\\config.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string connectionString = jsonObj["ConnectionString"].ToString();

            var builder = new SqlConnectionStringBuilder(connectionString);
            if (builder.TryGetValue("password", out var pwd)) 
            { 
                //builder["password"] = pwd;
                Password.Text = pwd.ToString();
            }

        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText("..\\config.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string server = ServerAdress.Text,
                port = ServerPort.Text,
                user = Login.Text,
                password = Password.Text;

            if (string.IsNullOrEmpty(server))
            {
                MessageBox.Show("Поле сервер обязатель");
            }

            jsonObj["ConnectionString"] = $"server={server};port={port};Database=laboratory;user={user};password={password}";
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("..\\config.json", output);

            MessageBox.Show("Изменения успешно внесены.", "Сохранено");
            // добавить сохранение 
            this.Close();
        }
    }
}
