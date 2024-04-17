﻿using System;
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
using TP.Model;

namespace TP.View
{
    /// <summary>
    /// Логика взаимодействия для Gosts.xaml
    /// </summary>
    public partial class Gosts : Page
    {
        public Gosts()
        {
            InitializeComponent();
        }

        private void DeleteGost_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeGost_Click(object sender, RoutedEventArgs e)
        {
            GostsChange gostsChange = new GostsChange();
            gostsChange.ChangeTitleWindow(2);
            gostsChange.ShowDialog();
        }

        private void AddGost_Click(object sender, RoutedEventArgs e)
        {
            GostsChange gostsChange = new GostsChange();
            gostsChange.ChangeTitleWindow(1);
            gostsChange.ShowDialog();
        }

        private void LoadFromFileGost_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
