using System;
using System.Collections.Generic;
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

        public ExcelParseAdditionals(string filename)
        {
            workbooks = app.Workbooks;
            workbook = workbooks.Open(filename, MissingObj, rOnly, MissingObj, MissingObj,
                                MissingObj, MissingObj, MissingObj, MissingObj, MissingObj,
                                MissingObj, MissingObj, MissingObj, MissingObj, MissingObj);
            // Получение всех страниц докуента
            sheets = workbook.Sheets;
            _values = GetDataFromExcel();
            app.Quit();
        }

        public Tuple<List<string>, Dictionary<int, List<string>>> Values
        {
            get => _values;
            set => _values = value;
        }

        public Tuple<List<string>, Dictionary<int, List<string>>> GetDataFromExcel()
        {
            List<string> list1 = new List<string>();
            List<string> col1 = new List<string>();
            List<string> col2 = new List<string>();
            List<string> col3 = new List<string>();
            Dictionary<int, List<string>> list2 = new Dictionary<int, List<string>>();
            int countLists = 0;
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
                                if (isValuesList1(i, j))
                                    list1.Add(CellText);
                            }
                            else if (countLists == 2)
                            {
                                if (((j == 1) || (j == 2) || (j == 3)) && i > 1)
                                {
                                    if (j == 1)
                                        col1.Add(CellText);
                                    else if (j == 2)
                                        col2.Add(CellText);
                                    else
                                        col3.Add(CellText);
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

        private bool isValuesList1(int row, int col)
        {
            if (row == 3 && col == 3)
            {
                return true;
            }
            else if (row == 4 && col == 2)
            {
                return true;
            }
            else if (row == 45 && col == 2)
            {
                return true;
            }
            else if (row == 46 && col == 3)
            {
                return true;
            }
            return false;
        }
    }
}
