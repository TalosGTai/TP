using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TP.View.Org1
{
    public partial class ChoiceAdditionalsPage : Page
    {
        string _direction;
        List<List<string>> _additionals;

        public ChoiceAdditionalsPage()
        {
            InitializeComponent();
            _direction = string.Empty;
        }

        public ChoiceAdditionalsPage(string direction)
        {
            InitializeComponent();
            _direction = direction;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChoiceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddAdditionalButton_Click(object sender, RoutedEventArgs e)
        {
            AdditionalTemplateUserControl additionalTemplateUserControl = new AdditionalTemplateUserControl(AdditionalsAddPanel.Children.Count);
            additionalTemplateUserControl.Margin = new Thickness(10, 10, 10, 20);
            AdditionalsAddPanel.Children.Insert(AdditionalsAddPanel.Children.Count - 1, additionalTemplateUserControl);
        }
    }
}
