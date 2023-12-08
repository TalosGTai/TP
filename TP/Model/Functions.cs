using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TP.View;

namespace TP.Model
{
    public class Functions
    {
        Frame frame {  get; set; }
        public Functions() 
        {
            frame = FindFrameByName("ViewPages");
        }

        public Functions(string name)
        {
            frame = FindFrameByName(name);
        }

        public Frame Frame 
        { 
            get { return frame; } 
            set { frame = value; }
        }

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
