using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace TP
{
    /// <summary>
    /// Логика взаимодействия с окном справки
    /// </summary>
    public partial class InfoWindow : Window
    {
        private readonly Uri _uri = new Uri("https://t.me/GTai_IT");
        public InfoWindow()
        {
            InitializeComponent();
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonTelegramUrl_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(_uri.AbsoluteUri));
            Author.Foreground = new SolidColorBrush(Color.FromRgb(104, 29, 168));
        }
           
    }
}
