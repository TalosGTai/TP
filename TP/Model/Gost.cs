using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Model
{
    public class Gost : NotifyPropertyChanged
    {
        private int _numberGost;
        private string _shortNameGost;
        private string _longNameGost;

        public Gost() { }

        public Gost(string shortName, string longName) 
        {
            _shortNameGost = shortName;
            _longNameGost = longName;
        }

        public int NumberGost
        {
            get => _numberGost;
            set
            {
                _numberGost = value;
                OnPropertyChanged();
            }
        }

        public string ShortNameGost
        {
            get => _shortNameGost;
            set
            {
                _shortNameGost = value;
                OnPropertyChanged();
            }
        }

        public string LongNameGost
        {
            get => _longNameGost;
            set
            {
                _longNameGost = value;
                OnPropertyChanged();
            }
        }
    }
}
