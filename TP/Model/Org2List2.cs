using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Model
{
    internal class Org2List2 : NotifyPropertyChanged
    {
        // № п/п
        private string _numberProduct;
        // Номер протокола испытаний
        private string _numberProtocolTest;
        // Дата протокола испытаний
        private string _dateReturnSampleAfterTest;
        // № и дата направления
        private string _numberDateDirection;
        // Регистрационный номер образца
        private string _numberRegSample;
        // Номер акта на списание (утилизацию)/возврата образцов
        private string _numberActUtil;
        // Дата акта на списание (утилизацию)/возврата образцов
        private string _dateActUtil;
        // Дата выдачи протокола испытаний
        private string _dateReturnSample;
        // Ф.И.О. сотрудника, внесшего запись
        private string _fioInsertRecord;

        public string numberProduct
        {
            get => _numberProduct;
            set
            {
                _numberProduct = value;
                OnPropertyChanged();
            }
        }

        public string numberProtocolTest
        {
            get => _numberProtocolTest;
            set
            {
                _numberProtocolTest = value;
                OnPropertyChanged();
            }
        }

        public string dateReturnSampleAfterTest
        {
            get => _dateReturnSampleAfterTest;
            set
            {
                _dateReturnSampleAfterTest = value;
                OnPropertyChanged();
            }
        }

        public string numberDateDirection
        {
            get => _numberDateDirection;
            set
            {
                _numberDateDirection = value;
                OnPropertyChanged();
            }
        }

        public string numberRegSample
        {
            get => _numberRegSample;
            set
            {
                _numberRegSample = value;
                OnPropertyChanged();
            }
        }

        public string numberActUtil
        {
            get => _numberActUtil;
            set
            {
                _numberActUtil = value;
                OnPropertyChanged();
            }
        }

        public string dateActUtil
        {
            get => _dateActUtil;
            set
            {
                _dateActUtil = value;
                OnPropertyChanged();
            }
        }

        public string dateReturnSample
        {
            get => _dateReturnSample;
            set
            {
                _dateReturnSample = value;
                OnPropertyChanged();
            }
        }

        public string fioInsertRecord
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