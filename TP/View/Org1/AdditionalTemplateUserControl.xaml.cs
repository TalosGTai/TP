using System.Windows;
using System.Windows.Controls;


namespace TP.View.Org1
{
    public partial class AdditionalTemplateUserControl : UserControl
    {
        public AdditionalTemplateUserControl()
        {
            InitializeComponent();
        }

        public AdditionalTemplateUserControl(int tableNumber)
        {
            InitializeComponent();
            TableNumberLabel.Content += tableNumber.ToString();
        }

        private void ChoiceAdditionalsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
