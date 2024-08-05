using DocumentFormat.OpenXml.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TP.Model;

namespace TP.View.Org1
{
    public partial class ChoiceAdditionalsPage : Page
    {
        private int _idOrg;
        private string _direction;
        private List<List<string>> _additionals;
        private int _idJournal;
        private int _idProtocol;
        private int _idProduct;
        private List<AdditionalTemplateUserControl> _additionalTemplates;

        public ChoiceAdditionalsPage()
        {
            InitializeComponent();
        }

        public ChoiceAdditionalsPage(string direction, int idJournal, int idProtocol, int idProduct)
        {
            InitializeComponent();
            _idOrg = 1;
            _direction = direction;
            _idJournal = idJournal;
            _idProtocol = idProtocol;
            _idProduct = idProduct;
            _additionalTemplates = new List<AdditionalTemplateUserControl>();
            _additionals = new List<List<string>>();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NewProtocol newProtocol= new NewProtocol(1, _idJournal, _idProtocol, _idProduct);
            Functions functions = new Functions();
            functions.Frame.Content = newProtocol;
        }

        // Кнопка "Продолжить"
        // Переход в создание протокола
        private void ChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var elem in _additionalTemplates)
            {
                if (elem.PathAdditionals.Count > 0)
                {
                    _additionals.Add(elem.PathAdditionals);
                }
            }
            CopyAdditionals();
            Thread threadAdditionals = new Thread(CopyAdditionals);
            threadAdditionals.Start();
            NewProtocol newProtocol = new NewProtocol(1, _idJournal, _idProtocol, _idProduct, _direction, _additionals);
            Functions functions = new Functions();
            functions.Frame.Content = newProtocol;
        }

        private void AddAdditionalButton_Click(object sender, RoutedEventArgs e)
        {
            AdditionalTemplateUserControl additionalTemplateUserControl = new AdditionalTemplateUserControl(AdditionalsAddPanel.Children.Count);
            additionalTemplateUserControl.Margin = new Thickness(10, 10, 10, 20);
            _additionalTemplates.Add(additionalTemplateUserControl);
            AdditionalsAddPanel.Children.Insert(AdditionalsAddPanel.Children.Count - 1, additionalTemplateUserControl);
        }

        private void CopyAdditionals()
        {
            string PROTOCOL_EXCEL_PATH = $"Организация{_idOrg}\\Протокол{_idProtocol}\\";
            DirectoryInfo directory = new DirectoryInfo(PROTOCOL_EXCEL_PATH);
            if (!directory.Exists)
            {
                directory.Create();
            }
            for (int j = 0; j < _additionals.Count; j++)
            {
                for (int i = 0; i < _additionals[j].Count; i++)
                {
                    FileInfo fileInfo = new FileInfo(_additionals[j][i]);
                    FileInfo fileInfo2 = new FileInfo(PROTOCOL_EXCEL_PATH + GetFileName(_additionals[j][i]));
                    if (!fileInfo2.Exists)
                        fileInfo.CopyTo(PROTOCOL_EXCEL_PATH + GetFileName(_additionals[j][i]));
                }
            }
        }

        private string GetFileName(string path)
        {
            try
            {
                string fileName = "";
                for (int i = Math.Max(path.LastIndexOf("\\"), path.LastIndexOf("/")) + 1;
                    (i < path.Length || i < path.LastIndexOf('.')); i++)
                {
                    fileName += path[i];
                }
                return fileName;
            }
            catch
            {

            }
            return "";
        }
    }
}
