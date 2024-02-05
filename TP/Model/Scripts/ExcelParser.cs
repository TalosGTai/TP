using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace TP.Model.Scripts
{
    /// <summary>
    /// Excel парсер
    /// </summary>
    public class ExcelParser
    {
        Excel.Application app = new Excel.Application();
        Excel.Workbooks workbooks = null;
        Excel.Workbook workbook = null;
        Excel.Sheets sheets = null;
        object MissingObj = System.Reflection.Missing.Value;
        object rOnly = true;
        object SaveChanges = false;

        public ExcelParser(string filename, int idList)
        {
            workbooks = app.Workbooks;
            workbook = workbooks.Open(filename, MissingObj, rOnly, MissingObj, MissingObj,
                                MissingObj, MissingObj, MissingObj, MissingObj, MissingObj,
                                MissingObj, MissingObj, MissingObj, MissingObj, MissingObj);
            // Получение всех страниц докуента
            sheets = workbook.Sheets;
            GetDataFromExcel(idList);
        }

        /// <summary>
        /// Получить данные из Excel
        /// </summary>
        /// <param name="numberList">номер листа</param>
        public void GetDataFromExcel(int numberList)
        {
            int iWorksheet = 0;
            foreach (Excel.Worksheet worksheet in sheets)
            {
                iWorksheet++;
                if (iWorksheet == numberList)
                {
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
                                Console.WriteLine(CellText);
                            }
                        }
                    }
                    // Очистка неуправляемых ресурсов
                    if (urRows != null) Marshal.ReleaseComObject(urRows);
                    if (urColums != null) Marshal.ReleaseComObject(urColums);
                    if (UsedRange != null) Marshal.ReleaseComObject(UsedRange);
                }
                
                // Очистка неуправляемых ресурсов
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
            }
        }
    }
}
