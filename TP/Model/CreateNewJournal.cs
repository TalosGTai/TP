using ClosedXML.Excel;
using Res = TP.Properties.Resources;
using TP.Control;
using System.Collections.Generic;
using System;

namespace TP.Model
{
    /// <summary>
    /// Класс работы с созданием журнала
    /// </summary>
    public class CreateNewJournal
    {
        /// <summary>
        /// Создаем новый журнал по умолчанию
        /// </summary>
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
            try
            {
                DBConnection dBConnection = new DBConnection();
                // add exists
                dBConnection.СreateTableJournalOrg1List0(1, 1);
                dBConnection.СreateTableJournalOrg1List1(1, 1);
                dBConnection.СreateTableJournalOrg1List2(1, 1);
                dBConnection.InsertStartValuesOrgJournalList1(1, 1);
                dBConnection.InsertStartValuesOrgJournalList2(1, 1);
            }
            catch
            {

            }
            finally
            {
                workbook.SaveAs(@"Организация1\Журнал1.xlsx");
            }
        }

        /// <summary>
        /// Создаем новый журнал
        /// </summary>
        /// <param name="idOrganization">идентификатор организации</param>
        /// <param name="idJournal">идентификатор журнала</param>
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
            try
            {
                DBConnection dBConnection = new DBConnection();
                // add exists
                dBConnection.СreateTableJournalOrg1List0(idOrganization, idJournal);
                dBConnection.СreateTableJournalOrg1List1(idOrganization, idJournal);
                dBConnection.СreateTableJournalOrg1List2(idOrganization, idJournal);
                dBConnection.InsertStartValuesOrgJournalList1(1, idJournal);
                dBConnection.InsertStartValuesOrgJournalList2(1, idJournal);
            }
            catch
            {

            }
            finally
            {
                workbook.SaveAs($"Организация{idOrganization}\\Журнал{idJournal}.xlsx");
            }
        }

        /// <summary>
        /// Создаем excel титульную страницу и заполняем некоторые ячейки
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Создаем excel страницу 1 с заданными колонками
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Создаем excel страницу 2 с заданными колонками
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Записать листы в таблицу
        /// </summary>
        /// <param name="idOrganization">номер организации</param>
        /// <param name="idJournal">номер журнала</param>
        /// <param name="dataList1">данные из таблицы листа1</param>
        /// <param name="dataList2">данные из таблицы листа2</param>
        public static void WriteToExcelList1(int idOrganization, int idJournal, List<Org1List1> dataList1, List<Org1List2> dataList2)
        {
            try
            {
                string filePath = $"Организация{idOrganization}\\Журнал{idJournal}.xlsx";
                var workbook = new XLWorkbook(filePath);

                var worksheet = workbook.Worksheet("Лист1");
                for (int i = 0; i < dataList1.Count; i++)
                {
                    // i от 0, а заполняем строки после 1-й
                    var row = i + 2;
                    worksheet.Cell("A" + row).Value = dataList1[i].NumberProduct;
                    worksheet.Cell("B" + row).Value = dataList1[i].NumberDateDirection;
                    worksheet.Cell("C" + row).Value = dataList1[i].SamplingAct;
                    worksheet.Cell("D" + row).Value = dataList1[i].SampleName;
                    worksheet.Cell("E" + row).Value = dataList1[i].OrganizationName;
                    worksheet.Cell("F" + row).Value = dataList1[i].NumberSampleWeightCapacity;
                    worksheet.Cell("G" + row).Value = dataList1[i].NumberDateUnsuitabilitySamples;
                    worksheet.Cell("H" + row).Value = dataList1[i].DateReceiptSample;
                    worksheet.Cell("I" + row).Value = dataList1[i].NumberRegSample;
                    worksheet.Cell("J" + row).Value = dataList1[i].FioResponsiblePersonTest;
                    worksheet.Cell("K" + row).Value = dataList1[i].DateIssueSample;
                    worksheet.Cell("L" + row).Value = dataList1[i].DateReturnSampleAfterTest;
                    worksheet.Cell("M" + row).Value = dataList1[i].FioInsertRecord;
                    worksheet.Cell("N" + row).Value = dataList1[i].Note;
                    worksheet.Cell("O" + row).Value = dataList1[i].NumberProtocol;
                    worksheet.Cell("P" + row).Value = dataList1[i].ProductType;
                    worksheet.Cell("Q" + row).Value = dataList1[i].Applicant;
                    worksheet.Cell("R" + row).Value = dataList1[i].Manufacturer;
                }

                worksheet = workbook.Worksheet("Лист2");
                for (int i = 0; i < dataList2.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell("A" + row).Value = dataList2[i].NumberProduct;
                    worksheet.Cell("B" + row).Value = dataList2[i].NumberProtocolTest;
                    worksheet.Cell("C" + row).Value = dataList2[i].DateReturnSampleAfterTest;
                    worksheet.Cell("D" + row).Value = dataList2[i].NumberDateDirection;
                    worksheet.Cell("E" + row).Value = dataList2[i].NumberRegSample;
                    worksheet.Cell("F" + row).Value = dataList2[i].NumberActUtil;
                    worksheet.Cell("G" + row).Value = dataList2[i].DateActUtil;
                    worksheet.Cell("H" + row).Value = dataList2[i].DateReturnSample;
                    worksheet.Cell("I" + row).Value = dataList2[i].FioInsertRecord;
                }

                workbook.SaveAs(filePath);

            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
