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
using System.Windows.Shapes;

namespace TP.Model
{
    /// <summary>
    /// Логика взаимодействия для GostsChange.xaml
    /// </summary>
    public partial class GostsChange : Window
    {
        int _idGost;

        public GostsChange()
        {
            InitializeComponent();
        }

        public GostsChange(int idGost)
        {
            InitializeComponent();
            _idGost = idGost;
        }

        public void ChangeTitleWindow(int value)
        {
            switch (value)
            {
                case 1:
                    Title = "Добавление ГОСТа";
                    SaveChangesBtn.Content = "Добавить ГОСТ";
                    break;
                case 2:
                    Title = "Изменение ГОСТа";
                    break;
            }
        }

        public void StartValues(string shortForm, string longForm)
        {
            ShortFormTextBox.Text = shortForm;
            LongFormTextBox.Text = longForm;
        }

        private bool CheckFields()
        {
            if (ShortFormTextBox.Text.Length > 0 && LongFormTextBox.Text.Length > 0)
                return true;
            return false;
        }

        private void SaveChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFields())
                MessageBox.Show("Не все данные заполнены.", "Ошибка!");
            else
            {
                if (Title == "Изменение ГОСТа")
                {
                    DBConnection db = new DBConnection();
                    db.UpdateGost(_idGost, ShortFormTextBox.Text, LongFormTextBox.Text);
                }
                else
                {
                    // добавление
                    DBConnection db = new DBConnection();
                    db.AddGost(ShortFormTextBox.Text, LongFormTextBox.Text);
                }
                this.DialogResult = true;
                Close();
            }
        }
    }
}
