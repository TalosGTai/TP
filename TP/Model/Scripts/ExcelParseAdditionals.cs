﻿using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace TP.Model.Scripts
{
    public class ExcelParseAdditionals
    {
        Excel.Application app = new Excel.Application();
        Excel.Workbooks workbooks = null;
        Excel.Workbook workbook = null;
        Excel.Sheets sheets = null;
        object MissingObj = System.Reflection.Missing.Value;
        object rOnly = true;
        object SaveChanges = false;
        Tuple<List<string>, Dictionary<int, List<string>>> _values;

        List<Tuple<string, string>> _gostTuples;


        // множество gosts для хранения всех гостов с приложения
        HashSet<string> gosts;
        HashSet<string> equipments;
        HashSet<string> numberEquipments;

        public ExcelParseAdditionals(string filename, bool isGosts = false)
        {
            gosts = new HashSet<string>();
            equipments = new HashSet<string>();
            numberEquipments = new HashSet<string>();
            workbooks = app.Workbooks;
            workbook = workbooks.Open(filename, MissingObj, rOnly, MissingObj, MissingObj,
                                MissingObj, MissingObj, MissingObj, MissingObj, MissingObj,
                                MissingObj, MissingObj, MissingObj, MissingObj, MissingObj);
            // Получение всех страниц докуента
            sheets = workbook.Sheets;
            if (isGosts)
            {
                _gostTuples = GetDataFromGostsExcel();
            }
            else
            {
                _values = GetDataFromExcel();
            }
            app.Quit();
        }

        public Tuple<List<string>, Dictionary<int, List<string>>> Values
        {
            get => _values;
            set => _values = value;
        }

        public HashSet<string> Gosts
        {
            get => gosts;
            set => gosts = value;
        }

        public List<Tuple<string, string>> GostsTuples
        {
            get => _gostTuples;
            set => _gostTuples = value;
        }

        public HashSet<string> Equipments
        {
            get => equipments;
            set => equipments = value;
        }

        public HashSet<string> NumberEquipments
        {
            get => numberEquipments;
            set => numberEquipments = value;
        }

        private bool IsCell1(string value)
        {
            if (value.IndexOf("НД на методы испытаний") != -1)
                return true;
            return false;
        }

        private bool IsCell2(string value)
        {
            if (value.IndexOf("Показатель") != -1 && value.Split(' ').Length < 3)
                return true;
            return false;
        }

        private bool IsCell3(string value)
        {
            if (value.IndexOf("Норма") != -1 && value.Split(' ').Length < 3)
                return true;
            return false;
        }

        private bool IsCell4(string value)
        {
            if (value.IndexOf("Итоговый результат") != -1 && value.Split(' ').Length < 4)
                return true;
            return false;
        }

        public Tuple<List<string>, Dictionary<int, List<string>>> GetDataFromExcel()
        {
            try 
            { 
                List<string> list1 = new List<string>();
                List<string> col1 = new List<string>();
                List<string> col2 = new List<string>();
                List<string> col3 = new List<string>();
                Dictionary<int, List<string>> list2 = new Dictionary<int, List<string>>();
                int countLists, zavNumber;
                bool cell1, cell1Help, cell2, cell2Help, cell3, cell3Help, cell4, cell4Help, cell4HelpT;

                cell1 = cell1Help = cell2 = cell2Help = cell3 = cell3Help = cell4 = cell4Help = cell4HelpT = false;
                countLists = zavNumber = 0;

                foreach (Excel.Worksheet worksheet in sheets)
                {
                    countLists++;
                    // Получаем диапазон используемых на странице ячеек
                    Excel.Range UsedRange = worksheet.UsedRange;
                    // Получаем строки в используемом диапазоне
                    Excel.Range urRows = UsedRange.Rows;
                    // Получаем столбцы в используемом диапазоне
                    Excel.Range urColums = UsedRange.Columns;

                    // Количества строк и столбцов
                    int RowsCount = urRows.Count;
                    int ColumnsCount = urColums.Count;

                    for (int i = 2; i <= RowsCount; i++)
                    {
                        for (int j = 1; j <= ColumnsCount; j++)
                        {
                            Excel.Range CellRange = UsedRange.Cells[i, j];
                            // Получение текста ячейки
                            string CellText = (CellRange == null || CellRange.Value2 == null) ? null :
                                                (CellRange as Excel.Range).Value2.ToString();

                            if (CellText != null)
                            {
                                if (countLists == 1)
                                {   
                                    // Проверка госта
                                    if (!cell1)
                                    {
                                        if (!cell1Help)
                                            cell1Help = IsCell1(CellText);
                                        else
                                        {
                                            cell1 = true;
                                            list1.Add(CellText);
                                            gosts.Add(CellText);
                                        }
                                    }
                                    else if (!cell2)
                                    {
                                        if (!cell2Help)
                                            cell2Help = IsCell2(CellText);
                                        else
                                        {
                                            cell2 = true;
                                            list1.Add(CellText);
                                        }
                                    }
                                    else if (!cell3)
                                    {
                                        if (!cell3Help)
                                            cell3Help = IsCell3(CellText);
                                        else
                                        {
                                            cell3 = true;
                                            list1.Add(CellText);
                                        }
                                    }
                                    else if (!cell4)
                                    {
                                        if (!cell4Help)
                                            cell4Help = IsCell4(CellText);
                                        else if (!cell4HelpT)
                                            cell4HelpT = true;
                                        else
                                        {
                                            cell4 = true;
                                            list1.Add(CellText);
                                        }
                                    }
                                }
                                else if (worksheet.Name.ToLower().IndexOf("оборудов") != -1)
                                {
                                    for (int k = 1; k < 2; k++)
                                    {
                                        for (int z = 1; z < 7; z++)
                                        {
                                            Excel.Range CellRange2 = UsedRange.Cells[k, z];
                                            // Получение текста ячейки
                                            string CellText2 = (CellRange2 == null || CellRange2.Value2 == null) ? null :
                                                                (CellRange2 as Excel.Range).Value2.ToString();

                                            if (zavNumber == 0 && CellText2 == "Зав№")
                                            {
                                                zavNumber = z;
                                                break;
                                            }
                                        }
                                        if (zavNumber != 0)
                                            break;
                                    }
                                   
                                    if (((j == 1) || (j == 2) || (j == 3)) && i > 1)
                                    {
                                        if (j == 1)
                                        {
                                            equipments.Add(CellText);
                                            col1.Add(CellText);
                                        }
                                        else if (j == 2)
                                            col2.Add(CellText);
                                        else
                                        {

                                            col3.Add(CellText);
                                        }
                                        if (j == zavNumber)
                                            numberEquipments.Add(CellText);
                                    }
                                }
                                //Console.Write($"{i}, {j}  | ");
                                //Console.WriteLine(CellText);
                            }
                        }
                    }
                    // Очистка неуправляемых ресурсов
                    if (urRows != null) Marshal.ReleaseComObject(urRows);
                    if (urColums != null) Marshal.ReleaseComObject(urColums);
                    if (UsedRange != null) Marshal.ReleaseComObject(UsedRange);

                    // Очистка неуправляемых ресурсов
                    if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                }
                list2[1] = col1;
                list2[2] = col2;
                list2[3] = col3;
                return new Tuple<List<string>, Dictionary<int, List<string>>>(list1, list2);
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }

        /// <summary>
        /// Получить все ГОСТы из таблицы
        /// </summary>
        /// <returns></returns>
        public List<Tuple<string, string>> GetDataFromGostsExcel()
        {
            try
            {
                List<string> col1 = new List<string>();
                List<string> col2 = new List<string>();
                Dictionary<int, List<string>> list2 = new Dictionary<int, List<string>>();
                Excel.Worksheet worksheet = sheets[2];

                // Получаем диапазон используемых на странице ячеек
                Excel.Range UsedRange = worksheet.UsedRange;
                // Получаем строки в используемом диапазоне
                Excel.Range urRows = UsedRange.Rows;
                // Получаем столбцы в используемом диапазоне
                Excel.Range urColums = UsedRange.Columns;

                // Количества строк и столбцов
                int RowsCount = urRows.Count;
                int ColumnsCount = urColums.Count;

                for (int i = 2; i <= RowsCount; i++)
                {
                    for (int j = 1; j <= ColumnsCount; j++)
                    {
                        Excel.Range CellRange = UsedRange.Cells[i, j];
                        // Получение текста ячейки
                        string CellText = (CellRange == null || CellRange.Value2 == null) ? null :
                                            (CellRange as Excel.Range).Value2.ToString();

                        if (CellText != null)
                        {
                            if (((j == 1) || (j == 2) || (j == 3)) && i > 1)
                            {
                                if (j == 1)
                                {
                                    equipments.Add(CellText);
                                    col1.Add(CellText);
                                }
                                else if (j == 2)
                                    col2.Add(CellText);
                            }
                        }
                    }
                }
                // Очистка неуправляемых ресурсов
                if (urRows != null) Marshal.ReleaseComObject(urRows);
                if (urColums != null) Marshal.ReleaseComObject(urColums);
                if (UsedRange != null) Marshal.ReleaseComObject(UsedRange);

                // Очистка неуправляемых ресурсов
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);

                //list2[1] = col1;
                //list2[2] = col2;

                var shortG = col1;
                var longG = col2;
                var result = shortG.Zip(longG, (x, y) => new Tuple<string, string>(x, y))
                                .ToList();
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }       
    }
}
