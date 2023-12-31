﻿using System;
using System.Windows.Controls;
using System.Windows.Data;
using TP.Model;

namespace TP.Control
{
    /// <summary>
    /// Класс работы с колонками DataGrid
    /// </summary>
    [Obsolete("Этот класс не используется")]
    internal class LoadDatas
    {
        private DataGrid _journalList1;
        private DataGrid _journalList2;

        public LoadDatas()
        {
            //DataSet ds = new DataSet("Org1");
            //DataTable dataTable = ds.Tables[0];
            //DataRow dataRow;
            //_journalList1 = GenerateTextColumnsList1();
            //_journalList2 = GenerateTextColumnsList2();
        }
        /// <summary>
        /// Лист журнала 1
        /// </summary>
        public DataGrid JournalList1
        {
            get => _journalList1;
        }
        /// <summary>
        /// Лист журнала 2
        /// </summary>
        public DataGrid JournalList2
        {
            get => _journalList1;
        }
        /// <summary>
        /// Создание колонок таблицы листа 1
        /// </summary>
        /// <returns>Таблица DataGrid</returns>
        public DataGrid GenerateTextColumnsList1()
        {
            //Org1List1 list1 = new Org1List1();
            DataGrid dataGrid = new DataGrid();

            //dataGrid.Columns.Add(TextColumnA(list1));
            //dataGrid.Columns.Add(TextColumnB(list1));
            //dataGrid.Columns.Add(TextColumnC(list1));
            //dataGrid.Columns.Add(TextColumnD(list1));
            //dataGrid.Columns.Add(TextColumnE(list1));
            //dataGrid.Columns.Add(TextColumnF(list1));
            //dataGrid.Columns.Add(TextColumnG(list1));
            //dataGrid.Columns.Add(TextColumnH(list1));
            //dataGrid.Columns.Add(TextColumnI(list1));
            //dataGrid.Columns.Add(TextColumnJ(list1));
            //dataGrid.Columns.Add(TextColumnK(list1));
            //dataGrid.Columns.Add(TextColumnL(list1));
            //dataGrid.Columns.Add(TextColumnM(list1));
            //dataGrid.Columns.Add(TextColumnN(list1));
            //dataGrid.Columns.Add(TextColumnO(list1));
            //dataGrid.Columns.Add(TextColumnP(list1));
            //dataGrid.Columns.Add(TextColumnQ(list1));
            //dataGrid.Columns.Add(TextColumnR(list1));
            //dataGrid.CanUserReorderColumns = false;
            //dataGrid.CanUserSortColumns = false;


            return dataGrid;
        }
        /// <summary>
        /// Создание колонок таблицы листа 2
        /// </summary>
        /// <returns>Таблица DataGrid</returns>
        public DataGrid GenerateTextColumnsList2()
        {
            //Org1List2 list2 = new Org1List2();
            DataGrid dataGrid = new DataGrid();

            //dataGrid.Columns.Add(TextColumnA(list2));
            //dataGrid.Columns.Add(TextColumnB(list2));
            //dataGrid.Columns.Add(TextColumnC(list2));
            //dataGrid.Columns.Add(TextColumnD(list2));
            //dataGrid.Columns.Add(TextColumnE(list2));
            //dataGrid.Columns.Add(TextColumnF(list2));
            //dataGrid.Columns.Add(TextColumnG(list2));
            //dataGrid.Columns.Add(TextColumnH(list2));
            //dataGrid.Columns.Add(TextColumnI(list2));
            //dataGrid.Columns.Add(TextColumnJ(list2));
            //dataGrid.Columns.Add(TextColumnK(list2));
            //dataGrid.Columns.Add(TextColumnL(list2));
            //dataGrid.Columns.Add(TextColumnM(list2));
            //dataGrid.Columns.Add(TextColumnN(list2));
            //dataGrid.Columns.Add(TextColumnO(list2));
            //dataGrid.Columns.Add(TextColumnP(list2));
            //dataGrid.Columns.Add(TextColumnQ(list2));
            //dataGrid.Columns.Add(TextColumnR(list2));
            dataGrid.CanUserReorderColumns = false;
            dataGrid.CanUserSortColumns = false;


            return dataGrid;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки А
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnA(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "№ п/п";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки В
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnB(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "№ и дата направления";
            Binding b = new Binding(list1.NumberDateDirection);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки С
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnC(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Акт отбора образцов (номер, дата)";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }
        /// <summary>
        /// Устанавливает и настраивает свойства колонки D
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnD(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Наименование продукции (образца)";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки E
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnE(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Наименование организации заказчика";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки F
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnF(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Количество образцов/масса/объём";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки G
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnG(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Номер и дата акта о непригодности образцов (при наличии)";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки H
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnH(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Дата поступления образцов";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки I
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnI(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Регистрационный номер образца";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки J
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnJ(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "ФИО ответственного исполнителя, осуществляющего проведение испытаний";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки K
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnK(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Дата выдачи образца ответственному исполнителю осуществляющего проведение испытаний";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки L
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnL(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Дата возврата образца после испытаний";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки M
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnM(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Ф.И.О. сотрудника, внесшего запись";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки N
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnN(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Примечание";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки O
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnO(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Номер протокола";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки P
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnP(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Вид продукции";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки Q
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnQ(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Заявитель";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }

        /// <summary>
        /// Устанавливает и настраивает свойства колонки R
        /// </summary>
        /// <param name="list1">Лист1 организации 1</param>
        /// <returns>Колонка таблицы</returns>
        public DataGridTextColumn TextColumnR(Org1List1 list1)
        {
            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "Изготовитель";
            Binding b = new Binding(list1.NumberProduct);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGridTextColumn.Binding = new Binding();
            return dataGridTextColumn;
        }
    }
}
