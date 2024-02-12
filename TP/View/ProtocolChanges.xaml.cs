using System.Windows;
using System.Windows.Controls;
using TP.Model;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для ProtocolChanges.xaml
    /// </summary>
    public partial class ProtocolChanges : Page
    {
        public ProtocolChanges()
        {
            InitializeComponent();
        }

        public void LoadDatas()
        {
            DBFunctions functions = new DBFunctions();
            TxtBoxRow1.Text = functions.GetProtocolTitleByRow(1);
            TxtBoxRow2.Text = functions.GetProtocolTitleByRow(2);
            TxtBoxRow3.Text = functions.GetProtocolTitleByRow(3);
            TxtBoxRow4.Text = functions.GetProtocolTitleByRow(4);
        }

        private void SaveAllChanges_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
