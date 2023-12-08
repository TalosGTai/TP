using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Res = TP.Properties.Resources;
using TP.Control;

namespace TP.Model
{
    public class CreateNewJournal
    {
        public CreateNewJournal()
        {
            var workbook = new XLWorkbook();
            var worksheetTitle = workbook.Worksheets.Add("Титул");
            var worksheetList1 = workbook.Worksheets.Add("Лист1");
            var worksheetList2 = workbook.Worksheets.Add("Лист2");

            worksheetTitle = CreateTitleList(worksheetTitle);
            worksheetList1 = CreateColumnsList1(worksheetList1);
            worksheetList2 = CreateColumnsList2(worksheetList2);
            //workbook.SaveAs("..\\..\\Datas\\Journals\\test.xlsx");
            workbook.SaveAs("Организация1\\Журнал1.xlsx");
        }

        public CreateNewJournal(int idOrganization, int idJournal)
        {
            var workbook = new XLWorkbook();
            var worksheetTitle = workbook.Worksheets.Add("Титул");
            var worksheetList1 = workbook.Worksheets.Add("Лист1");
            var worksheetList2 = workbook.Worksheets.Add("Лист2");

            worksheetTitle = CreateTitleList(worksheetTitle);
            worksheetList1 = CreateColumnsList1(worksheetList1);
            worksheetList2 = CreateColumnsList2(worksheetList2);
            //workbook.SaveAs("..\\..\\Datas\\Journals\\test.xlsx");
            workbook.SaveAs($"Организация{idOrganization}\\Журнал{idJournal}.xlsx");
        }

        private IXLWorksheet CreateTitleList(IXLWorksheet worksheet)
        {
            GetDatas getDatas = new GetDatas();
            worksheet.Cell("E" + 5).Value = getDatas.Rows[0].Item1;
            worksheet.Cell("E" + 5).Style.Font.Bold = true;
            worksheet.Cell("E" + 5).Style.Font.FontSize = 9;
            worksheet.Cell("E" + 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("E" + 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("E5:K5").Merge();
            worksheet.Cell("G" + 9).Value = getDatas.Rows[1].Item1;
            worksheet.Cell("G" + 9).Style.Font.FontSize = 9;
            worksheet.Cell("G" + 9).Style.Font.Italic = true;
            worksheet.Cell("G" + 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("G" + 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("G9:I9").Merge();
            worksheet.Cell("F" + 17).Value = getDatas.Rows[2].Item1;
            worksheet.Cell("F" + 17).Style.Font.FontSize = 9;
            worksheet.Cell("F" + 17).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("F" + 17).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("F17:J17").Merge();
            worksheet.Cell("B" + 27).Value = getDatas.Rows[3].Item1;
            worksheet.Cell("B" + 27).Style.Font.FontSize = 9;
            worksheet.Cell("J" + 27).Value = getDatas.Rows[3].Item2;
            worksheet.Cell("J" + 27).Style.Font.FontSize = 9;
            worksheet.Cell("B" + 28).Value = getDatas.Rows[4].Item1;
            worksheet.Cell("B" + 28).Style.Font.FontSize = 9;
            worksheet.Cell("J" + 28).Value = getDatas.Rows[4].Item2;
            worksheet.Cell("J" + 28).Style.Font.FontSize = 9;
            worksheet.Cell("B" + 29).Value = getDatas.Rows[5].Item1;
            worksheet.Cell("B" + 29).Style.Font.FontSize = 9;
            return worksheet;
        }

        private IXLWorksheet CreateColumnsList1(IXLWorksheet worksheet)
        {
            worksheet.ColumnWidth = 21;
            worksheet.Cell("A" + 1).Value = Res.ResourceManager.GetString("List1A");
            worksheet.Cell("B" + 1).Value = Res.ResourceManager.GetString("List1B");
            worksheet.Cell("C" + 1).Value = Res.ResourceManager.GetString("List1C");
            worksheet.Cell("D" + 1).Value = Res.ResourceManager.GetString("List1D");
            worksheet.Cell("E" + 1).Value = Res.ResourceManager.GetString("List1E");
            worksheet.Cell("F" + 1).Value = Res.ResourceManager.GetString("List1F");
            worksheet.Cell("G" + 1).Value = Res.ResourceManager.GetString("List1G");
            worksheet.Cell("H" + 1).Value = Res.ResourceManager.GetString("List1H");
            worksheet.Cell("I" + 1).Value = Res.ResourceManager.GetString("List1I");
            worksheet.Cell("J" + 1).Value = Res.ResourceManager.GetString("List1J");
            worksheet.Cell("K" + 1).Value = Res.ResourceManager.GetString("List1K");
            worksheet.Cell("L" + 1).Value = Res.ResourceManager.GetString("List1L");
            worksheet.Cell("M" + 1).Value = Res.ResourceManager.GetString("List1M");
            worksheet.Cell("N" + 1).Value = Res.ResourceManager.GetString("List1N");
            worksheet.Cell("O" + 1).Value = Res.ResourceManager.GetString("List1O");
            worksheet.Cell("P" + 1).Value = Res.ResourceManager.GetString("List1P");
            worksheet.Cell("Q" + 1).Value = Res.ResourceManager.GetString("List1Q");
            worksheet.Cell("R" + 1).Value = Res.ResourceManager.GetString("List1R");
            worksheet.Style.Font.FontName = "Times New Roman";
            worksheet.Style.Font.FontSize = 10;
            worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            worksheet.Style.Alignment.WrapText = true;
            worksheet.RowHeight = 70;
            return worksheet;
        }

        private IXLWorksheet CreateColumnsList2(IXLWorksheet worksheet)
        {
            worksheet.ColumnWidth = 21;
            worksheet.Cell("A" + 1).Value = Res.ResourceManager.GetString("List2A");
            worksheet.Cell("B" + 1).Value = Res.ResourceManager.GetString("List2B");
            worksheet.Cell("C" + 1).Value = Res.ResourceManager.GetString("List2C");
            worksheet.Cell("D" + 1).Value = Res.ResourceManager.GetString("List2D");
            worksheet.Cell("E" + 1).Value = Res.ResourceManager.GetString("List2E");
            worksheet.Cell("F" + 1).Value = Res.ResourceManager.GetString("List2F");
            worksheet.Cell("G" + 1).Value = Res.ResourceManager.GetString("List2G");
            worksheet.Cell("H" + 1).Value = Res.ResourceManager.GetString("List2H");
            worksheet.Cell("I" + 1).Value = Res.ResourceManager.GetString("List2I");
            worksheet.Style.Font.FontName = "Times New Roman";
            worksheet.Style.Font.FontSize = 10;
            worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            worksheet.Style.Alignment.WrapText = true;
            worksheet.RowHeight = 70;
            return worksheet;
        }
    }
}
