using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace TP.View
{
    public partial class WaitScreen : Window
    {
        Thread _thread;
        int _choiceWait;

        public WaitScreen(Thread thread, int choiceWait)
        {
            InitializeComponent();
            _thread = thread;
            _choiceWait = choiceWait;
        }

        public void SetWaitMsg()
        {
            if (_choiceWait == 1)
            {
                WaitLabel.Content = "Идёт загрузка ГОСТов, это займёт некоторое время. Пожалуйста, подождите.";
            }
            else
            {
                WaitLabel.Content = "Идёт создание протокола, это займёт некоторое время. Пожалуйста, подождите.";
            }
        }

        public void StartLoading()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_thread.IsAlive)
            {
                Console.WriteLine(_thread.IsAlive);
                LoadingPgBar.Value += LoadingPgBar.LargeChange;
            }
            else
            {
                if (LoadingPgBar.Value == 100)
                    Close();
                LoadingPgBar.Value = 100;
            }
        }
    }
}
