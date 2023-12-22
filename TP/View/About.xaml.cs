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
        //Ссылка на телеграм программиста
        private readonly Uri _uri = new Uri("https://t.me/GTai_IT");
        public InfoWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Закрытие окна "О программе"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Событие при нажатии по ссылке телеграма
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTelegramUrl_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(_uri.AbsoluteUri));
            Author.Foreground = new SolidColorBrush(Color.FromRgb(104, 29, 168));
        }
           
    }
}
