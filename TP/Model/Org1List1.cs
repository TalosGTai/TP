using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;

namespace TP.Model
{
    public class Org1List1 : NotifyPropertyChanged, IEquatable<Org1List1>
    {
        private string _id;
        private string _numberProduct;
        private string _numberDateDirection;
        private string _samplingAct;
        private string _sampleName;
        private string _organizationName;
        private string _numberSampleWeightCapacity;
        private string _numberDateUnsuitabilitySamples;
        private string _dateReceiptSample;
        private string _numberRegSample;
        private string _fioResponsiblePersonTest;
        private string _dateIssueSample;
        private string _dateReturnSampleAfterTest;
        private string _fioInsertRecord;
        private string _note;
        private string _numberProtocol;
        private string _productType;
        private string _applicant;
        private string _manufacturer;

        public Org1List1()
        {

        }

        public Org1List1(List<string> values) 
        {
            _id = values[0];
            _numberProduct = values[1];
            _numberDateDirection = values[2];
            _samplingAct = values[3];
            _sampleName = values[4];
            _organizationName = values[5];
            _numberSampleWeightCapacity = values[6];
            _numberDateUnsuitabilitySamples = values[7];
            _dateReceiptSample = values[8];
            _numberRegSample = values[9];
            _fioResponsiblePersonTest = values[10];
            _dateIssueSample = values[11];
            _dateReturnSampleAfterTest = values[12];
            _fioInsertRecord = values[13];
            _note = values[14];
            _numberProtocol = values[15];
            _productType = values[16];
            _applicant = values[17];
            _manufacturer = values[18];
        }

        /// <summary>
        /// id
        /// </summary>
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// № п/п
        /// </summary>
        public string NumberProduct { 
            get=> _numberProduct;
            set
            { 
                _numberProduct = value;
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
        /// акт отбора образцов
        /// </summary>
        public string SamplingAct
        {
            get => _samplingAct;
            set
            {
                _samplingAct = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Наименование продукции (образца)
        /// </summary>
        public string SampleName
        {
            get => _sampleName;
            set
            {
                _sampleName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Наименование организации заказчика
        /// </summary>
        public string OrganizationName
        {
            get => _organizationName;
            set
            {
                _organizationName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Количество образцов/масса/обьем
        /// </summary>
        public string NumberSampleWeightCapacity
        {
            get => _numberSampleWeightCapacity;
            set
            {
                _numberSampleWeightCapacity = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Номер и дата акта о непригодности образцов (при наличии)
        /// </summary>
        public string NumberDateUnsuitabilitySamples
        {
            get => _numberDateUnsuitabilitySamples;
            set
            {
                _numberDateUnsuitabilitySamples = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Дата поступления образцов (вручную)
        /// </summary>
        public string DateReceiptSample
        {
            get => _dateReceiptSample;
            set
            {
                _dateReceiptSample = value;
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
        /// ФИО ответственного исполнителя, осуществляющего проведение испытаний (вручную)
        /// </summary>
        public string FioResponsiblePersonTest
        {
            get => _fioResponsiblePersonTest;
            set
            {
                _fioResponsiblePersonTest = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Дата выдачи образца ответственному исполнителю осуществляющего проведение испытаний (вручную)
        /// </summary>
        public string DateIssueSample
        {
            get => _dateIssueSample;
            set
            {
                _dateIssueSample = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Дата возврата образца после испытаний  (вручную)
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

        /// <summary>
        /// Примечание
        /// </summary>
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Номер протокола
        /// </summary>
        public string NumberProtocol
        {
            get => _numberProtocol;
            set
            {
                _numberProtocol = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Вид продукции
        /// </summary>
        public string ProductType
        {
            get => _productType;
            set
            {
                _productType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Заявитель
        /// </summary>
        public string Applicant
        {
            get => _applicant;
            set
            {
                _applicant = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Изготовитель
        /// </summary>
        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                _manufacturer = value;
                OnPropertyChanged();
            }
        }

        public bool Equals(Org1List1 o)
        {
            bool condition = this.Applicant == o.Applicant
               && this.DateIssueSample == o.DateIssueSample
               && this.DateReceiptSample == o.DateReceiptSample
               && this.DateReturnSampleAfterTest == o.DateReturnSampleAfterTest
               && this.FioInsertRecord == o.FioInsertRecord
               && this.FioResponsiblePersonTest == o.FioResponsiblePersonTest
               && this.Id == o.Id
               && this.Manufacturer == o.Manufacturer
               && this.Note == o.Note
               && this.NumberDateDirection == o.NumberDateDirection
               && this.NumberDateUnsuitabilitySamples == o.NumberDateUnsuitabilitySamples
               && this.NumberProduct == o.NumberProduct
               && this.NumberProtocol == o.NumberProtocol
               && this.NumberRegSample == o.NumberRegSample
               && this.NumberSampleWeightCapacity == o.NumberSampleWeightCapacity
               && this.OrganizationName == o.OrganizationName
               && this.SampleName == o.SampleName
               && this.ProductType == o.ProductType
               && this.SamplingAct == o.SamplingAct;

            return condition;
        }

        public int GetHashCode(Org1List1 x)
        {
            var hash = 19;
            hash = hash * 23 + x.Id.GetHashCode();
            hash = hash * 23 + x.NumberDateDirection.GetHashCode();
            hash = hash * 23 + x.NumberProduct.GetHashCode();
            hash = hash * 23 + x.SamplingAct.GetHashCode();
            hash = hash * 23 + x.SampleName.GetHashCode();
            hash = hash * 23 + x.OrganizationName.GetHashCode();
            hash = hash * 23 + x.NumberSampleWeightCapacity.GetHashCode();
            hash = hash * 23 + x.NumberDateUnsuitabilitySamples.GetHashCode();
            hash = hash * 23 + x.DateReceiptSample.GetHashCode();
            hash = hash * 23 + x.NumberRegSample.GetHashCode();
            hash = hash * 23 + x.FioResponsiblePersonTest.GetHashCode();
            hash = hash * 23 + x.DateIssueSample.GetHashCode();
            hash = hash * 23 + x.DateReturnSampleAfterTest.GetHashCode();
            hash = hash * 23 + x.FioInsertRecord.GetHashCode();
            hash = hash * 23 + x.Note.GetHashCode();
            hash = hash * 23 + x.NumberProtocol.GetHashCode();
            hash = hash * 23 + x.ProductType.GetHashCode();
            hash = hash * 23 + x.Applicant.GetHashCode();
            hash = hash * 23 + x.Manufacturer.GetHashCode();
            return hash;
        }


    }

    public class Org1List1Comparer : IEqualityComparer<Org1List1>
    {
        public bool Equals(Org1List1 x, Org1List1 y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Org1List1 x)
        {
            return x.GetHashCode();
        }
    }
}
