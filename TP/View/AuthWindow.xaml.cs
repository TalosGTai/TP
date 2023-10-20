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
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            // create_window
        }

        private void ButtonSendChoice_Click(object sender, RoutedEventArgs e)
        {
            int idOrg = Convert.ToInt32(ComboBoxChoiceOrganisation.SelectedIndex.ToString()) + 1;
            MainWindow mainWindow = new MainWindow(idOrg);
            Close();
            mainWindow.Show();
        }
    }
}
