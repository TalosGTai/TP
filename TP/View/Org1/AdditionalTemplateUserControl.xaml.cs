using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TP.Model.Scripts;


namespace TP.View.Org1
{
    public partial class AdditionalTemplateUserControl : UserControl
    {
        private List<string> _pathAdditionals;

        public AdditionalTemplateUserControl()
        {
            InitializeComponent();
        }

        public AdditionalTemplateUserControl(int tableNumber)
        {
            InitializeComponent();
            _pathAdditionals = new List<string>();
            TableNumberLabel.Content += tableNumber.ToString();
        }

        public List<string> PathAdditionals
        {
            get { return _pathAdditionals; }
        }

        private void ChoiceAdditionalsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Excel (*.xlsx)|*.xlsx|All files|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    _pathAdditionals.Add(filename);
            }
        }
    }
}
