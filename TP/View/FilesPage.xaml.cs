using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для FilesPage.xaml
    /// </summary>
    public partial class FilesPage : Page
    {
        public FilesPage()
        {
            InitializeComponent();
        }

        private void DirectionJournal_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel 2003 (*.xls)|*.xls|Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                LabelDirection.Visibility = Visibility.Visible;
                // Org
            }
        }

        private void SignatureFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                LabelSignature.Visibility = Visibility.Visible;
                // Org
            }
        }
    }
}
