using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Model
{
    internal class Org2List1 : NotifyPropertyChanged
    {
        // № п/п
        private string _numberProduct;
        // № и дата направления
        private string _numberDateDirection;
        // акт отбора образцов
        private string _samplingAct;
        // Наименование продукции (образца)
        private string _sampleName;
        // Наименование организации заказчика
        private string _organizationName;
        // Количество образцов/масса/обьем
        private string _numberSampleWeightCapacity;
        // Номер и дата акта о непригодности образцов (при наличии)
        private string _numberDateUnsuitabilitySamples;
        // Дата поступления образцов в лабораторию (вручную)
        private string _dateReceiptSample;
        // Регистрационный номер образца
        private string _numberRegSample;
        // ФИО ответственного исполнителя, осуществляющего проведение испытаний (вручную)
        private string _fioResponsiblePersonTest;
        // Дата выдачи образца ответственному исполнителю осуществляющего проведение испытаний (вручную)
        private string _dateIssueSample;
        // Дата возврата образца после испытаний  (вручную)
        private string _dateReturnSampleAfterTest;
        // Ф.И.О. сотрудника, внесшего запись
        private string _fioInsertRecord;
        // Примечание
        private string _note;
        // Номер протокола
        private string _numberProtocol;
        // Вид продукции
        private string _productType;
        // Заявитель
        private string _applicant;
        // Изготовитель
        private string _manufacturer;

        public string numberProduct
        {
            get => _numberProduct;
            set
            {
                _numberProduct = value;
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

        public string samplingAct
        {
            get => _samplingAct;
            set
            {
                _samplingAct = value;
                OnPropertyChanged();
            }
        }

        public string sampleName
        {
            get => _sampleName;
            set
            {
                _sampleName = value;
                OnPropertyChanged();
            }
        }

        public string organizationName
        {
            get => _organizationName;
            set
            {
                _organizationName = value;
                OnPropertyChanged();
            }
        }

        public string numberSampleWeightCapacity
        {
            get => _numberSampleWeightCapacity;
            set
            {
                _numberSampleWeightCapacity = value;
                OnPropertyChanged();
            }
        }

        public string numberDateUnsuitabilitySamples
        {
            get => _numberDateUnsuitabilitySamples;
            set
            {
                _numberDateUnsuitabilitySamples = value;
                OnPropertyChanged();
            }
        }

        public string dateReceiptSample
        {
            get => _dateReceiptSample;
            set
            {
                _dateReceiptSample = value;
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

        public string fioResponsiblePersonTest
        {
            get => _fioResponsiblePersonTest;
            set
            {
                _fioResponsiblePersonTest = value;
                OnPropertyChanged();
            }
        }

        public string dateIssueSample
        {
            get => _dateIssueSample;
            set
            {
                _dateIssueSample = value;
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

        public string fioInsertRecord
        {
            get => _fioInsertRecord;
            set
            {
                _fioInsertRecord = value;
                OnPropertyChanged();
            }
        }

        public string note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }

        public string numberProtocol
        {
            get => _numberProtocol;
            set
            {
                _numberProtocol = value;
                OnPropertyChanged();
            }
        }

        public string productType
        {
            get => _productType;
            set
            {
                _productType = value;
                OnPropertyChanged();
            }
        }

        public string applicant
        {
            get => _applicant;
            set
            {
                _applicant = value;
                OnPropertyChanged();
            }
        }

        public string manufacturer
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
