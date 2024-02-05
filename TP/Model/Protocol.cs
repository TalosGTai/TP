using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Model
{
    public class Protocol : NotifyPropertyChanged
    {
        private int _numberProtocol;
        private string _nameProtocol;

        public Protocol() 
        {

        }

        public Protocol(int numberProtocol)
        {
            _numberProtocol = numberProtocol;
            _nameProtocol = "Протокол" + numberProtocol.ToString();
        }

        public int NumberProtocol
        {
            get => _numberProtocol;
            set
            {
                _numberProtocol = value;
                OnPropertyChanged();
            }
        }

        public string NameProtocol
        {
            get => _nameProtocol;
            set
            {
                _nameProtocol = value;
                OnPropertyChanged();
            }
        }
    }
}
