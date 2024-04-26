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
        int _countAdditionals;

        public WaitScreen(Thread thread, int choiceWait, int countAdditionals)
        {
            InitializeComponent();
            _thread = thread;
            _choiceWait = choiceWait;
            _countAdditionals = countAdditionals;
        }

        public void SetPgBStep()
        {
            switch (_countAdditionals)
            {
                case 0:
                    LoadingPgBar.LargeChange = 5;
                    break;
                case 1:
                    LoadingPgBar.LargeChange = 5;
                    break;
                case 2:
                    LoadingPgBar.LargeChange = 4;
                    break;
                case 3:
                    LoadingPgBar.LargeChange = 3;
                    break;
                case 4:
                    LoadingPgBar.LargeChange = 2;
                    break;
                case 5:
                    LoadingPgBar.LargeChange = 1;
                    break;
                case 6:
                    LoadingPgBar.LargeChange = 1;
                    break;
                case 7:
                    LoadingPgBar.LargeChange = 1;
                    break;
                case 8:
                    LoadingPgBar.LargeChange = 1;
                    break;
                case 9:
                    LoadingPgBar.LargeChange = 1;
                    break;
                case 10:
                    LoadingPgBar.LargeChange = 1;
                    break;
                case 11:
                    LoadingPgBar.LargeChange = 1;
                    break;
                case 12:
                    LoadingPgBar.LargeChange = 1;
                    break;
                case 13:
                    LoadingPgBar.LargeChange = 1;
                    break;
                case 14:
                    LoadingPgBar.LargeChange = 1;
                    break;
                default:
                    LoadingPgBar.LargeChange = 1;
                    break;
            }
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
