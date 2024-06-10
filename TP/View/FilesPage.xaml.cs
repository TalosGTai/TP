using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace TP.View
{
    public partial class FilesPage : Page
    {
        int _idOrg;
        public FilesPage()
        {
            InitializeComponent();
            _idOrg = 1;
        }

        public FilesPage(int idOrg)
        {
            InitializeComponent();
            _idOrg = idOrg;
        }

        private void SignatureFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                LabelSignature.Visibility = Visibility.Visible;
            }
        }

        private void StampFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                LabelStamp.Visibility = Visibility.Visible;
            }
        }
    }
}
