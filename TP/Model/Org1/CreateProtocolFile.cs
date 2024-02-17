﻿using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using TP.Properties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using DocumentFormat.OpenXml;
using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Microsoft.Office.Interop.Word;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using DocumentWord = Microsoft.Office.Interop.Word.Document;
using ParagraphWord = Microsoft.Office.Interop.Word.Paragraph;
using System.IO;
using TableStyle = DocumentFormat.OpenXml.Wordprocessing.TableStyle;
using Application = Microsoft.Office.Interop.Word.Application;


namespace TP.Model.Org1
{
    /// <summary>
    /// Создание таблицы и документа Протокол{idProtocol}
    /// </summary>
    public class CreateProtocolFile
    {
        private Tuple<Dictionary<string, string>, Dictionary<string, string>> _journal;
        private int idRow;
        private readonly string FONT = "Times New Roman";
        string PROTOCOL_EXCEL_PATH = "",
                PROTOCOL_WORD_PATH = "";

        public CreateProtocolFile()
        {
            var workbook = new XLWorkbook();
            var worksheetTitle = workbook.Worksheets.Add("Главная");      
        }

        public void CreateProtocolXlsxFile(List<Tuple<List<string>, Dictionary<int, List<string>>>> values)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Главная");
            var worksheet2 = workbook.Worksheets.Add("Таблицы");
            var worksheet3 = workbook.Worksheets.Add("Концовка");
            worksheet = CreateChapter1(worksheet);
            worksheet = CreateChapter2(worksheet);
            worksheet2 = CreateTablesTests(worksheet2, values);
            worksheet3 = CreateLastChapter(worksheet3);
            worksheet.Style.Font.FontName = FONT;
            worksheet2.Style.Font.FontName = FONT;
            worksheet3.Style.Font.FontName = FONT;
            worksheet.Style.Alignment.WrapText = true;
            worksheet2.Style.Alignment.WrapText = true;
            worksheet3.Style.Alignment.WrapText = true;
            worksheet.Column(2).Width = 32;
            worksheet.Column(3).Width = 14;
            worksheet.Column(4).Width = 14;
            worksheet.Column(5).Width = 14;
            worksheet.Column(6).Width = 8;

            worksheet2.Column(2).Width = 25;
            worksheet2.Column(3).Width = 15;
            worksheet2.Column(4).Width = 10;
            worksheet2.Column(5).Width = 10;
            worksheet2.Column(6).Width = 8;
            //Создаем excel файлл
            workbook.SaveAs(PROTOCOL_EXCEL_PATH);
            workbook.Dispose();
        }

        public CreateProtocolFile(Tuple<Dictionary<string, string>,
            Dictionary<string, string>> journal, int idOrg, int idProtocol,
            List<Tuple<List<string>, Dictionary<int, List<string>>>> values)
        {
            PROTOCOL_EXCEL_PATH = $"Организация{idOrg}\\Протокол{idProtocol}\\Протокол{idProtocol}.xlsx";
           // PROTOCOL_WORD_PATH = $"Организация{idOrg}\\Протокол{idProtocol}\\Протокол{idProtocol}.docx";
            PROTOCOL_WORD_PATH = $"Организация{idOrg}\\Протокол{idProtocol}\\Протокол{idProtocol}.docx";
            _journal = journal;
            //Создание excel файла
            CreateProtocolXlsxFile(values);
            var workbookSave = new Aspose.Cells.Workbook(PROTOCOL_EXCEL_PATH);
            //Получаем docx файл
            workbookSave.Save(PROTOCOL_WORD_PATH, Aspose.Cells.SaveFormat.Docx);

            ChangeDocFont(idOrg, idProtocol, PROTOCOL_WORD_PATH);
            //CreateFile(PROTOCOL_WORD_PATH, ParseDocument(idOrg, idProtocol));
            FixDocument(idOrg, idProtocol, PROTOCOL_WORD_PATH);

            //Подготавливаем файлы для сохранения в БД
            FileStream fs = new FileStream(PROTOCOL_EXCEL_PATH, FileMode.Open, FileAccess.Read);
            byte[] protocolXls = new byte[fs.Length];
            fs.Read(protocolXls, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();

            fs = new FileStream(PROTOCOL_WORD_PATH, FileMode.Open, FileAccess.Read);
            byte[] protocolDoc = new byte[fs.Length];
            fs.Read(protocolDoc, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();

            //Сохраняем протоколы в БД
            var db = new DBConnection();
            db.InsertOrUpdateOrgProtocolRow(idOrg, idProtocol, protocolDoc, protocolXls);
        }

        /// <summary>
        /// Изменение шрифта в документе, созданном по таблице Протокол
        /// </summary>
        /// <param name="idOrg"></param>
        /// <param name="idProtocol"></param>
        private void ChangeDocFont(int idOrg, int idProtocol, string path)
        {
            Application wordApp = new Application();
            string filename = $"{Directory.GetCurrentDirectory()}\\"+ path;

            DocumentWord myDoc = wordApp.Documents.Open(filename);
            myDoc.PageSetup.TopMargin = 0;
            myDoc.PageSetup.BottomMargin = 0;
            try
            {
                if (myDoc.Paragraphs.Count > 0)
                {
                    foreach (ParagraphWord p in myDoc.Paragraphs)
                    {
                        p.Range.Font.Name = FONT;                        
                    }
                    foreach (Microsoft.Office.Interop.Word.Table t in myDoc.Tables)
                    {
                        var wTable = t;
                        wTable.Range.Cells.HeightRule = WdRowHeightRule.wdRowHeightAuto;
                    }
                }
            }
            finally
            {
                myDoc.Save();
                myDoc.Close();
                myDoc = null;
                wordApp.Quit();
                wordApp = null;
            }
        }

        /// <summary>
        /// Получаем параграфы временного документа, созданного по таблице Протокол
        /// </summary>
        /// <param name="idOrg"></param>
        /// <param name="idProtocol"></param>
        /// <returns></returns>
        private void FixDocument(int idOrg, int idProtocol, string path)
        {
            List<Paragraph> paragraphItems = new List<Paragraph>();
            string prev = null;
            Table tbl;
            using (var doc = WordprocessingDocument.Open(path, true))
            {
                var paragraphs = doc.MainDocumentPart.Document.Body.Descendants<Paragraph>();
                //Получаем таблицу с испытаниями
                tbl = doc.MainDocumentPart.Document.Body.Descendants<Table>().ToArray()[4];
                //Исключаем копирайтинг строки
                paragraphs = paragraphs.Where(el => !el.InnerXml.Contains(@"<w:color w:val=""FF0000"" />")
                        && !el.InnerXml.Contains(@"<w:br w:type=""page"" />")).ToArray();

                //Параграфы берем без значений таблицы
                var flagForTable = false;

                Body body = new Body();

                foreach (var el in paragraphs)
                {
                    //исключаем пустые параграфы, если их более одного подряд
                    if (!(string.IsNullOrEmpty(el.InnerText) && string.IsNullOrEmpty(prev)))
                    {
                        if (el.InnerText.Contains("Результаты испытаний:"))
                        {
                            var p = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                            body.AppendChild(p);
                            flagForTable = true;

                            body.AppendChild(el.CloneNode(true));
                        }
                        if (el.InnerText.Contains("ПРОТОКОЛ ИСПЫТАНИЙ"))
                        {
                            var p = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                            //paragraphItems.Add(p);
                            body.AppendChild(p);
                        }
                        if (el.InnerText.Contains("Внимание!"))
                        {
                            flagForTable = false;
                        }
                        if (!flagForTable)
                        {
                            body.AppendChild(el.CloneNode(true));
                        }

                        if ((prev != null && prev.Contains("Результаты испытаний:")))
                        {
                            Table t = new Table();
                            OpenXmlElementList oxl = tbl.ChildElements;
                            TableProperties props = new TableProperties();
                            TableWidth tw = new TableWidth() { Width = "1000", Type = TableWidthUnitValues.Auto };
                            TableRowHeight th = new TableRowHeight { HeightType = HeightRuleValues.Auto };
                            //TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };
                            props.Append( tw, th);

                            // styling
                            //SectionProperties sectionProp1 =
                            //               body.Descendants<SectionProperties>()?.FirstOrDefault() ??
                            //               body.AppendChild(new SectionProperties());

                            //var pageSize = sectionProp1.Descendants<PageSize>()?.FirstOrDefault() ??
                            //               sectionProp1.AppendChild(new PageSize());
                            //pageSize.Width = 35840;
                            //pageSize.Height = 22240;
                            //pageSize.Orient = PageOrientationValues.Landscape;


                            t.Append(props);
                            foreach (var c in oxl)
                            {
                                if (!c.InnerText.Contains("Evaluation Only") && !c.InnerText.Contains("Результаты испытаний:"))
                                {
                                    OpenXmlElement child = c.CloneNode(true);                                   
                                    t.AppendChild(child);
                                }
                            }
                            body.AppendChild(t);
                        }
                    }
                    prev = el.InnerText;
                }

                //Очищаем весь файл
                doc.MainDocumentPart.Document.Body.Remove();
                doc.MainDocumentPart.Document.AppendChild(body);

                doc.Save();
            }
        }
          
        private IXLWorksheet CreateChapter1(IXLWorksheet worksheet)
        {
            worksheet.Cell("A" + 1).Value = Resources.Protocol1;
            worksheet.Cell("A" + 1).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 1).Style.Font.Bold = true;
            worksheet.Cell("A" + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A1:G1").Merge();
            worksheet.Cell("A" + 2).Value = Resources.Protocol2;
            worksheet.Cell("A" + 2).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 2).Style.Font.Bold = true;
            worksheet.Cell("A" + 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A2:G2").Merge();
            worksheet.Cell("A" + 3).Value = Resources.Protocol3;
            worksheet.Cell("A" + 3).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A3:G3").Merge();
            worksheet.Cell("A" + 4).Value = Resources.Protocol4;
            worksheet.Cell("A" + 4).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A4:G4").Merge();
            worksheet.Cell("A" + 5).Value = Resources.Protocol5;
            worksheet.Cell("A" + 5).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(4).Height = 35;
            worksheet.Range("A5:G5").Merge();
            worksheet.Cell("A" + 6).Value = Resources.Protocol6;
            worksheet.Cell("A" + 6).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(5).Height = 62;
            worksheet.Row(6).Height = 45;
            worksheet.Range("A6:G6").Merge();
            worksheet.Cell("A" + 7).Value = Resources.Protocol7;
            worksheet.Cell("A" + 7).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A7:G7").Merge();
            worksheet.Cell("A" + 8).Value = Resources.Protocol8;
            worksheet.Cell("A" + 8).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A8:G8").Merge();
            worksheet.Cell("A" + 9).Value = Resources.Protocol9;
            worksheet.Cell("A" + 9).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A9:G9").Merge();
            // правая часть (подписи)
            worksheet.Cell("B" + 12).Value = Resources.Protocol10;
            worksheet.Cell("B" + 12).Style.Font.FontSize = 10;
            worksheet.Cell("B" + 12).Style.Font.Bold = true;
            worksheet.Cell("B" + 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell("B" + 12).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("B12:G12").Merge();
            worksheet.Cell("B" + 13).Value = Resources.Protocol11;
            worksheet.Cell("B" + 13).Style.Font.FontSize = 11;
            worksheet.Cell("B" + 13).Style.Font.Bold = true;
            worksheet.Cell("B" + 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell("B" + 13).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("B13:G13").Merge();
            worksheet.Cell("B" + 14).Value = Resources.Protocol12;
            worksheet.Cell("B" + 14).Style.Font.FontSize = 11;
            worksheet.Cell("B" + 14).Style.Font.Bold = true;
            worksheet.Cell("B" + 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell("B" + 14).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("B14:G14").Merge();
            worksheet.Cell("B" + 15).Value = Resources.Protocol13;
            worksheet.Cell("B" + 15).Style.Font.FontSize = 11;
            worksheet.Cell("B" + 15).Style.Font.Bold = true;
            worksheet.Cell("B" + 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell("B" + 15).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("B15:G15").Merge();
            worksheet.Cell("B" + 16).Value = Resources.Protocol14;
            worksheet.Cell("B" + 16).Style.Font.FontSize = 8;
            worksheet.Cell("B" + 16).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell("B" + 16).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("B16:G16").Merge();
            worksheet.Cell("C" + 17).Value = "_________________________" + _journal.Item2["C"] + "_______________";
            worksheet.Cell("C" + 17).Style.Font.FontSize = 11;
            worksheet.Cell("C" + 17).Style.Font.Bold = true;
            worksheet.Cell("C" + 17).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell("C" + 17).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("C17:G17").Merge();
            worksheet.Cell("B" + 18).Value = Resources.Protocol16;
            worksheet.Cell("B" + 18).Style.Font.FontSize = 8;
            worksheet.Cell("B" + 18).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell("B" + 18).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("B18:G18").Merge();
            worksheet.Cell("C" + 19).Value = Resources.Protocol17;
            worksheet.Cell("C" + 19).Style.Font.FontSize = 11;
            worksheet.Cell("C" + 19).Style.Font.Bold = true;
            worksheet.Cell("C" + 19).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell("C" + 19).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("C19:G19").Merge();

            return worksheet;
        }

        private IXLWorksheet CreateChapter2(IXLWorksheet worksheet)
        {
            worksheet.Cell("A" + 23).Value = Resources.Protocol18;
            worksheet.Cell("A" + 23).Style.Font.FontSize = 12;
            worksheet.Cell("A" + 23).Style.Font.Bold = true;
            worksheet.Cell("A" + 23).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 23).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A23:G23").Merge();
            worksheet.Cell("A" + 24).Value = _journal.Item1["O"];
            worksheet.Cell("A" + 24).Style.Font.FontSize = 12;
            worksheet.Cell("A" + 24).Style.Font.Bold = true;
            worksheet.Cell("A" + 24).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + 24).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A24:G24").Merge();
            worksheet.Cell("A" + 25).Value = Resources.Protocol19;
            worksheet.Cell("A" + 25).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 25).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + 25).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A25:G25").Merge();
            worksheet.Cell("A" + 26).Value = Resources.Protocol20;
            worksheet.Cell("A" + 26).Style.Font.FontSize = 10;
            worksheet.Cell("A" + 26).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + 26).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A26:G26").Merge();
            // образцы
            idRow = 27; // который свободен для записи номер строки

            worksheet.Cell("A" + idRow).Value = Resources.Protocol21 + _journal.Item1["H"];
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol22;
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Row(idRow).Height = 70;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol23 + _journal.Item1["Q"];
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol24 + " " + _journal.Item1["R"];
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol25 + " " + _journal.Item1["B"];
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol26;
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol27;
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol28 + _journal.Item1["C"];
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol29 + _journal.Item1["H"] + "-" + _journal.Item2["C"];
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol30 + Resources.Protocol31;
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            worksheet.Row(idRow).Height = 27;
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol32;
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            idRow++;

            return worksheet;
        }

        private IXLWorksheet CreateTablesTests(IXLWorksheet worksheet, List<Tuple<List<string>, Dictionary<int, List<string>>>> values)
        {
            idRow = 1;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol36;
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:F{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol37;
            worksheet.Cell("B" + idRow).Value = Resources.Protocol38;
            worksheet.Cell("C" + idRow).Value = Resources.Protocol39;
            worksheet.Cell("D" + idRow).Value = Resources.Protocol40;
            worksheet.Cell("E" + idRow).Value = Resources.Protocol41;
            worksheet.Cell("F" + idRow).Value = Resources.Protocol42;
            worksheet.Row(idRow).Height = 80;
            worksheet.Range($"A{idRow}:F{idRow}").Style.Font.FontSize = 10;
            worksheet.Range($"A{idRow}:F{idRow}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A{idRow}:F{idRow}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Range($"A{idRow}:F{idRow}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            idRow++;
            worksheet.Cell("A" + idRow).Value = "1";
            worksheet.Cell("B" + idRow).Value = "2";
            worksheet.Cell("C" + idRow).Value = "3";
            worksheet.Cell("D" + idRow).Value = "4";
            worksheet.Cell("E" + idRow).Value = "5";
            worksheet.Cell("F" + idRow).Value = "6";
            worksheet.Range($"A{idRow}:F{idRow}").Style.Font.FontSize = 10;
            worksheet.Range($"A{idRow}:F{idRow}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A{idRow}:F{idRow}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Range($"A{idRow}:F{idRow}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            idRow++;
            for (int i = 0; i < values[0].Item2[1].Count; i++)
            {
                worksheet.Cell("A" + idRow).Value = i.ToString();
                worksheet.Cell("B" + idRow).Value = values[0].Item1[1];
                worksheet.Cell("C" + idRow).Value = values[0].Item1[0];
                worksheet.Cell("D" + idRow).Value = values[0].Item2[1][i] + " " + values[0].Item2[2][i] + " " + values[0].Item2[3][i];
                worksheet.Cell("E" + idRow).Value = values[0].Item1[2];
                worksheet.Cell("F" + idRow).Value = values[0].Item1[3];
                worksheet.Range($"A{idRow}:F{idRow}").Style.Font.FontSize = 10;
                worksheet.Range($"A{idRow}:F{idRow}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{idRow}:F{idRow}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range($"A{idRow}:F{idRow}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                idRow++;
            }
            idRow++;

            return worksheet;
        }
        
        private IXLWorksheet CreateLastChapter(IXLWorksheet worksheet)
        {
            idRow = 1;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol33;
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 8;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(idRow).Height = 65;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol34;
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 8;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(idRow).Height = 65;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            idRow++;
            worksheet.Cell("A" + idRow).Value = Resources.Protocol35;
            worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
            worksheet.Cell("A" + idRow).Style.Font.Bold = true;
            worksheet.Cell("A" + idRow).Style.Font.Underline = XLFontUnderlineValues.Single;
            worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"A{idRow}:G{idRow}").Merge();
            idRow++;
            return worksheet;
        }
    }
}
