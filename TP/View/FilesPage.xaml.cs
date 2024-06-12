using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TP.Model;

namespace TP.View
{
    public partial class FilesPage : Page
    {
        int _idOrg;
        DBConnection db = new DBConnection();
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
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                LabelSignature.Visibility = Visibility.Visible;
                var fullPath = openFileDialog.FileName;
                var image = File.ReadAllBytes(fullPath);
                db.SaveImage(image, _idOrg, ImageNameType.Signature);
            }
        }

        private void StampFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg,*.jpg)|*.png;*.jpeg;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                LabelStamp.Visibility = Visibility.Visible;
                LabelSignature.Visibility = Visibility.Visible;
                var fullPath = openFileDialog.FileName;
                var image = File.ReadAllBytes(fullPath);
                db.SaveImage(image, _idOrg, ImageNameType.Stamp);
            }
        }
    }
}
