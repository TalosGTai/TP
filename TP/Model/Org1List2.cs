using System;
using System.Collections.Generic;

namespace TP.Model
{
    public class Org1List2 : NotifyPropertyChanged, IEquatable<Org1List2>
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

        public Org1List2()
        {

        }

        public Org1List2(List<string> values)
        {
            _numberProduct = values[0];
            _numberProtocolTest = values[1];
            _dateReturnSampleAfterTest = values[2];
            _numberDateDirection = values[3];
            _numberRegSample = values[4];
            _numberActUtil = values[5];
            _dateActUtil = values[6];
            _dateReturnSample = values[7];
            _fioInsertRecord = values[8];
        }       

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

        public bool Equals(Org1List2 o)
        {
            bool condition = this.NumberProduct == o.NumberProduct
               && this.NumberProtocolTest == o.NumberProtocolTest
               && this.NumberDateDirection == o.NumberDateDirection
               && this.DateReturnSampleAfterTest == o.DateReturnSampleAfterTest
               && this.NumberRegSample == o.NumberRegSample
               && this.NumberActUtil == o.NumberActUtil
               && this.DateActUtil == o.DateActUtil
               && this.DateReturnSample == o.DateReturnSample
               && this.FioInsertRecord == o.FioInsertRecord;

            return condition;
        }

        public int GetHashCode(Org1List2 x)
        {
            var hash = 19;
            hash = hash * 23 + x.NumberProduct.GetHashCode();
            hash = hash * 23 + x.NumberProtocolTest.GetHashCode();
            hash = hash * 23 + x.NumberDateDirection.GetHashCode();
            hash = hash * 23 + x.DateReturnSample.GetHashCode();
            hash = hash * 23 + x.DateReturnSampleAfterTest.GetHashCode();
            hash = hash * 23 + x.NumberRegSample.GetHashCode();
            hash = hash * 23 + x.NumberActUtil.GetHashCode();
            hash = hash * 23 + x.DateActUtil.GetHashCode();
            hash = hash * 23 + x.DateReturnSample.GetHashCode();
            hash = hash * 23 + x.FioInsertRecord.GetHashCode();
            return hash;
        }
    }

    public class Org1List2Comparer : IEqualityComparer<Org1List2>
    {
        public bool Equals(Org1List2 x, Org1List2 y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Org1List2 x)
        {
            return x.GetHashCode();
        }
    }
}
