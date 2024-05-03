using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TP.Model.Scripts
{
    public class ExcelWorker
    {
        public List<List<string>> Rows = new List<List<string>>();
        string _path;
        List<Org1List1> _list1;
        List<Org1List2> _list2;

        public ExcelWorker() 
        {

        }

        public ExcelWorker(string path)
        {
            _path = path;
        }
        
        public ExcelWorker(string path, List<Org1List1> list1, List<Org1List2> list2)
        {
            _path = path;
            _list1 = list1;
            _list2 = list2;
        }

        public IXLWorksheet FileOpen(string path, int idWorkshet)
        {
            var workbook = new XLWorkbook(path);
            var ws1 = workbook.Worksheet(idWorkshet);

            foreach (var xlRow in ws1.RangeUsed().Rows())
            {
                Rows.Add(new List<string>());

                foreach (var xlCell in xlRow.Cells())
                {
                    var formula = xlCell.FormulaA1;
                    var value = xlCell.Value.ToString();

                    string targetCellValue = (formula.Length == 0) ? value : "=" + formula;

                    Rows[Rows.Count - 1].Add(targetCellValue);
                }
            }

            return ws1;
        }

        public void FileSave(string path)
        {
            try { 
            CreateDirIfNotExist(path, true);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Титул");

                for (int row = 0; row < Rows.Count; row++)
                {
                    for (int col = 0; col < Rows[row].Count; col++)
                    {
                        var cellAdress = GetExcelPos(row, col);

                        if (Rows[row][col].StartsWith("="))
                        {
                            workSheet.Cell(cellAdress).FormulaA1 = Rows[row][col];
                        }
                        else
                        {
                            workSheet.Cell(cellAdress).Value = Rows[row][col];
                        }
                    }
                }

                wb.SaveAs(path);
            }
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }

        private IXLWorksheet WorkSheet1()
        {
            var workbook = new XLWorkbook(_path);
            var ws1 = workbook.Worksheet("Лист1");

            for (int i = 1; i <= _list1.Count; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    ws1.Cell(GetExcelPos(i, j)).Value = GetValueByIdList1(i - 1, j);
                }
            }

            workbook.Save();
            return ws1;
        }

        private IXLWorksheet WorkSheet2()
        {
            var workbook = new XLWorkbook(_path);
            var ws2 = workbook.Worksheet("Лист2");

            for (int i = 1; i <= _list2.Count; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ws2.Cell(GetExcelPos(i, j)).Value = GetValueByIdList2(i - 1, j);
                }
            }

            return ws2;
        }

        public void SaveAllWorksheets(string value1, string value2)
        {
            try
            {
                var workbook = new XLWorkbook();
                var oldworkbook = new XLWorkbook(_path);
                int count = 0;
                bool val1, val1Help, val2, val2Help;

                val1 = val1Help = val2 = val2Help = false;
                foreach (IXLWorksheet worksheet in oldworkbook.Worksheets)
                {
                    count++;
                    if (count == 1)
                    {
                        for (int i = 1; i < worksheet.RowCount(); i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (!val1)
                                {
                                    if (!val1Help)
                                        val1Help = IsRegNumber(worksheet.Cell(GetExcelPos(i, j)).Value.ToString());
                                    else
                                    {
                                        val1 = true;
                                        worksheet.Cell(GetExcelPos(i, j)).Value = value1;
                                    }
                                }
                                else if (!val2)
                                {
                                    if (!val2Help)
                                        val2Help = IsDate(worksheet.Cell(GetExcelPos(i, j)).Value.ToString());
                                    else
                                    {
                                        val2 = true;
                                        worksheet.Cell(GetExcelPos(i, j)).Value = value2.Split('-')[0];
                                        worksheet.Cell(GetExcelPos(i, j + 1)).Value = value2.Split('-')[1].Split()[0];
                                    }
                                }
                            }
                        }
                    }
                    workbook.AddWorksheet(worksheet);
                }
                File.Delete(_path);
                workbook.SaveAs(_path);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private bool IsRegNumber(string value)
        {
            if (value.IndexOf("Рег") != -1 && value.IndexOf("номер") != -1)
                return true;
            return false;
        }

        private bool IsDate(string value)
        {
            if (value.IndexOf("Дата") != -1 && value.Split(' ').Length <= 2)
                return true;
            return false;
        }

        private IXLWorksheet ChangeAdditional(IXLWorksheet worksheet)
        {

            return worksheet;
        }

        public void SaveWorksheets()
        {
            var workbook = new XLWorkbook();
            var oldworkbook = new XLWorkbook(_path);
            workbook.AddWorksheet(oldworkbook.Worksheet("Титул"));
            workbook.AddWorksheet(WorkSheet1());
            workbook.AddWorksheet(WorkSheet2());
            File.Delete(_path);
            workbook.SaveAs(_path);
        }

        private string GetValueByIdList1(int row, int col)
        {
            switch (col)
            {
                case 0:
                    return _list1[row].NumberProduct;
                case 1:
                    return _list1[row].NumberDateDirection;
                case 2:
                    return _list1[row].SamplingAct;
                case 3:
                    return _list1[row].SampleName;
                case 4:
                    return _list1[row].OrganizationName;
                case 5:
                    return _list1[row].NumberSampleWeightCapacity;
                case 6:
                    return _list1[row].NumberDateUnsuitabilitySamples;
                case 7:
                    return _list1[row].DateReceiptSample;
                case 8:
                    return _list1[row].NumberRegSample;
                case 9:
                    return _list1[row].FioResponsiblePersonTest;
                case 10:
                    return _list1[row].DateIssueSample;
                case 11:
                    return _list1[row].DateReturnSampleAfterTest;
                case 12:
                    return _list1[row].FioInsertRecord;
                case 13:
                    return _list1[row].Note;
                case 14:
                    return _list1[row].NumberProtocol;
                case 15:
                    return _list1[row].ProductType;
                case 16:
                    return _list1[row].Applicant;
                case 17:
                    return _list1[row].Manufacturer;
                default:
                    return "";
            }
        }

        private string GetValueByIdList2(int row, int col)
        {
            switch (col)
            {
                case 0:
                    return _list2[row].NumberProduct;
                case 1:
                    return _list2[row].NumberProtocolTest;
                case 2:
                    return _list2[row].DateReturnSampleAfterTest;
                case 3:
                    return _list2[row].NumberDateDirection;
                case 4:
                    return _list2[row].NumberRegSample;
                case 5:
                    return _list2[row].NumberActUtil;
                case 6:
                    return _list2[row].DateActUtil;
                case 7:
                    return _list2[row].DateReturnSample;
                case 8:
                    return _list2[row].FioInsertRecord;
                default:
                    return "";
            }
        }

        public void AddRow(params string[] cells)
        {
            Rows.Add(cells.ToList());
        }

        public static string GetExcelPos(int row, int cell)
        {
            char[] alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            int count = cell / 26;
            string alphResult = string.Empty;

            if (count > 0)
            {
                alphResult = alph[count] + alph[count % 26].ToString();
            }
            else
            {
                alphResult = alph[cell].ToString();
            }

            return alphResult + (row + 1);
        }

        private void CreateDirIfNotExist(string dirPath, bool removeFilename = false)
        {
            if (removeFilename)
            {
                dirPath = Directory.GetParent(dirPath).FullName;
            }

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}
