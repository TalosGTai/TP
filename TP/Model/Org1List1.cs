using Org.BouncyCastle.Asn1.Mozilla;
using System.Collections.Generic;

namespace TP.Model
{
    public class Org1List1 : NotifyPropertyChanged
    {
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
            _numberProduct = values[0];
            _numberDateDirection = values[1];
            _samplingAct = values[2];
            _sampleName = values[3];
            _organizationName = values[4];
            _numberSampleWeightCapacity = values[5];
            _numberDateUnsuitabilitySamples = values[6];
            _dateReceiptSample = values[7];
            _numberRegSample = values[8];
            _fioResponsiblePersonTest = values[9];
            _dateIssueSample = values[10];
            _dateReturnSampleAfterTest = values[11];
            _fioInsertRecord = values[12];
            _note = values[13];
            _numberProtocol = values[14];
            _productType = values[15];
            _applicant = values[16];
            _manufacturer = values[17];
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
    }
}
