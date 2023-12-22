namespace TP.Model
{
    internal class Org1List2 : NotifyPropertyChanged
    {
        
        private string _numberProduct;       
        private string _numberProtocolTest;        
        private string _dateReturnSampleAfterTest;
        private string _numberDateDirection;
        private string _numberRegSample;
        private string _numberActUtil;
        private string _dateActUtil;
        private string _dateReturnSample;
        private string _fioInsertRecord;

        /// <summary>
        /// № п/п
        /// </summary>
        public string NumberProduct
        {
            get => _numberProduct;
            set
            {
                _numberProduct = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Номер протокола испытаний
        /// </summary>
        public string NumberProtocolTest
        {
            get => _numberProtocolTest;
            set
            {
                _numberProtocolTest = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Дата протокола испытаний
        /// </summary>
        public string DateReturnSampleAfterTest
        {
            get => _dateReturnSampleAfterTest;
            set
            {
                _dateReturnSampleAfterTest = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// № и дата направления
        /// </summary>
        public string NumberDateDirection
        {
            get => _numberDateDirection;
            set
            {
                _numberDateDirection = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Регистрационный номер образца
        /// </summary>
        public string NumberRegSample
        {
            get => _numberRegSample;
            set
            {
                _numberRegSample = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Номер акта на списание (утилизацию)/возврата образцов
        /// </summary>
        public string NumberActUtil
        {
            get => _numberActUtil;
            set
            {
                _numberActUtil = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Дата акта на списание (утилизацию)/возврата образцов
        /// </summary>
        public string DateActUtil
        {
            get => _dateActUtil;
            set
            {
                _dateActUtil = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Дата выдачи протокола испытаний
        /// </summary>
        public string DateReturnSample
        {
            get => _dateReturnSample;
            set
            {
                _dateReturnSample = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Ф.И.О. сотрудника, внесшего запись
        /// </summary>
        public string FioInsertRecord
        {
            get => _fioInsertRecord;
            set
            {
                _fioInsertRecord = value;
                OnPropertyChanged();
            }
        }
    }
}
