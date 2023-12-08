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
using TP.Control;
using TP.Model;
using TP.Model.Scripts;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для NewProtocol.xaml
    /// </summary>
    public partial class NewProtocol : Page
    {
        private int _idProtocol;

        public NewProtocol()
        {
            InitializeComponent();
        }

        public NewProtocol(int idProtocol)
        {
            InitializeComponent();
            _idProtocol = idProtocol;
        }

        private void CreateProtocol_Click(object sender, RoutedEventArgs e)
        {
            // Создание Протокола
            //DocParser docParser = new DocParser();
            //FunctionsPrint functionsPrint = new FunctionsPrint();
            //functionsPrint.PrintTupleDictionary(docParser.JournalParse);
            CreateNewJournal createNewJournal = new CreateNewJournal();
        }

        private void BtnAdditionals_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDirection_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
