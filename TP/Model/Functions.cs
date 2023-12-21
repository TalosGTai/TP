using System.Windows;
using System.Windows.Controls;

namespace TP.Model
{
    public class Functions
    {
        Frame _frame;
        public Functions() 
        {
            _frame = FindFrameByName("ViewPages");
        }

        public Functions(string name)
        {
            _frame = FindFrameByName(name);
        }

        public Frame Frame 
        { 
            get { return _frame; } 
            set { _frame = value; }
        }
        /// <summary>
        /// Найти фрейм по названию
        /// </summary>
        /// <param name="name">Название фрейма</param>
        /// <returns>Frame</returns>
        public Frame FindFrameByName(string name)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Name == "TPWindow")
                {
                    Frame f = (Frame)window.FindName(name);
                    return f;
                }
            }
            return null;
        }
    }
}
