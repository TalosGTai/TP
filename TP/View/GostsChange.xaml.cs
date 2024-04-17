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
        public GostsChange()
        {
            InitializeComponent();
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

        private void SaveChangesBtn_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }
    }
}
