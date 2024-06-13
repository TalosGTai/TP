using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using TP.Properties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using Italic = DocumentFormat.OpenXml.Wordprocessing.Italic;
using DocumentFormat.OpenXml;
using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Microsoft.Office.Interop.Word;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using DocumentWord = Microsoft.Office.Interop.Word.Document;
using ParagraphWord = Microsoft.Office.Interop.Word.Paragraph;
using System.IO;
using Application = Microsoft.Office.Interop.Word.Application;
using System.Text;
using Header = DocumentFormat.OpenXml.Wordprocessing.Header;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using DocumentFormat.OpenXml.Office2010.Word.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace TP.Model.Org1
{
    /// <summary>
    /// Создание таблицы и документа Протокол{idProtocol}
    /// </summary>
    public class CreateProtocolFile
    {
        private Tuple<Dictionary<string, string>, Dictionary<string, string>> _journal;
        private int idRow;
        private int _countAdditionals;
        private readonly string FONT = "Times New Roman";
        string PROTOCOL_EXCEL_PATH = "",
               PROTOCOL_WORD_PATH  = "";
        List<string> valuesResourses;
        private string _gosts;
        private string _equipments;
        private string ColontitulText = "Протокол испытаний ";

        public CreateProtocolFile()
        {
            var workbook = new XLWorkbook();
            var worksheetTitle = workbook.Worksheets.Add("Главная");      
        }

        public CreateProtocolFile(Tuple<Dictionary<string, string>,
            Dictionary<string, string>> journal, int idOrg, int idProtocol,
            List<Tuple<List<string>, Dictionary<int, List<string>>>> additionals, string gosts, string equipments,
            int countAdditionals)
        {
            try
            {
                PROTOCOL_EXCEL_PATH = $"Организация{idOrg}\\Протокол{idProtocol}\\Протокол{idProtocol}.xlsx";
                PROTOCOL_WORD_PATH = $"Организация{idOrg}\\Протокол{idProtocol}\\Протокол{idProtocol}.docx";
                _journal = journal;
                _gosts = gosts;
                _equipments = equipments;
                _countAdditionals = countAdditionals;
                //Создание excel файла
                CreateProtocolXlsxFile(additionals);
                var workbookSave = new Aspose.Cells.Workbook(PROTOCOL_EXCEL_PATH);
                //Получаем docx файл
                workbookSave.Save(PROTOCOL_WORD_PATH, Aspose.Cells.SaveFormat.Docx);

                FixDocument(idOrg, idProtocol, PROTOCOL_WORD_PATH);
                ChangeDocFont(idOrg, idProtocol, PROTOCOL_WORD_PATH);

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
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }


        public void CreateProtocolXlsxFile(List<Tuple<List<string>, Dictionary<int, List<string>>>> additionals)
        {
            try
            {
                DBFunctions functions = new DBFunctions();
                List<string> rows = new List<string>()
            {
                functions.GetProtocolTitleByRow(1),
                functions.GetProtocolTitleByRow(2),
                functions.GetProtocolTitleByRow(3),
                functions.GetProtocolTitleByRow(4),
            };
                valuesResourses = GetValuesFromTitle(rows);

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Главная");
                var worksheet2 = workbook.Worksheets.Add("Таблицы");
                var worksheet3 = workbook.Worksheets.Add("Концовка");

                worksheet = CreateChapter1(worksheet);
                worksheet = CreateChapter2(worksheet);
                worksheet2 = CreateTablesTests(worksheet2, additionals);
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

                // ? 25 15 10 10 8
                worksheet2.Column(1).Width = 13.71;
                worksheet2.Column(2).Width = 13.71;
                worksheet2.Column(3).Width = 13.71;
                worksheet2.Column(4).Width = 13.71;
                worksheet2.Column(5).Width = 13.71;
                //Создаем excel файлл
                workbook.SaveAs(PROTOCOL_EXCEL_PATH);
                workbook.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private List<string> GetValuesFromTitle(List<string> rows)
        {
            try
            {
                valuesResourses = new List<string>()
            {
                rows[0].Substring(0,rows[0].IndexOf("дрес") - 1),
                rows[0].Substring(rows[0].IndexOf("дрес") - 1,
                                        rows[0].LastIndexOf("420000") - rows[0].IndexOf("дрес") + 1),
                rows[0].Substring(rows[0].LastIndexOf("420000"),
                                        GetLastIndHouse(GetStrFromToPos(rows[0], rows[0].LastIndexOf("420000"), rows[0].Length - 1))),
                rows[0].Substring(rows[0].IndexOf("Уникальный"),
                                        rows[0].IndexOf("телефон:") - rows[0].IndexOf("Уникальный")),
                rows[0].Substring(rows[0].IndexOf("телефон"),
                                        rows[0].Length - rows[0].IndexOf("телефон")),
                rows[1],
                rows[2],
                rows[3]
            };
                return valuesResourses;
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }

        private int GetLastIndHouse(string str)
        {
            return GetLengthNumberHouse(str, str.IndexOf("здание") + 6);
        }

        private string GetStrFromToPos(string str, int from, int to)
        {
            return str.Substring(from, to - from + 1);
        }

        private int GetLengthNumberHouse(string str, int from)
        {
            from++;
            while (from < str.Length && str[from] != ' ')
            {
                from++;
            }
            return from;
        }

        int CountPages = 0;
        /// <summary>
        /// Изменение шрифта в документе, созданном по таблице Протокол
        /// </summary>
        /// <param name="idOrg"></param>
        /// <param name="idProtocol"></param>
        private void ChangeDocFont(int idOrg, int idProtocol, string path)
        {
            try
            {
                Application wordApp = new Application();
                string filename = $"{Directory.GetCurrentDirectory()}\\" + path;
                DocumentWord myDoc = wordApp.Documents.Open(filename);
                myDoc.PageSetup.TopMargin = 0;
                myDoc.PageSetup.BottomMargin = 0;
                //InlineShape img = myDoc.InlineShapes[0];

                //InlineShape autoScaledInlineShape = myDoc.InlineShapes.AddPicture("");
                //float scaledWidth = autoScaledInlineShape.Width;
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

                        object missing = System.Reflection.Missing.Value;
                        WdStatistic stat = Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages;
                        CountPages = myDoc.ComputeStatistics(stat, ref missing);
                        foreach (ParagraphWord p in myDoc.Paragraphs)
                        {
                            if (p.Range.Text.Contains("Число страниц"))
                            {
                                p.Range.Text = $"Число страниц: {CountPages}\r";
                                break;
                            }
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
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }

        /// <summary>
        /// Получаем параграфы временного документа, созданного по таблице Протокол
        /// </summary>
        /// <param name="idOrg"></param>
        /// <param name="idProtocol"></param>
        /// <returns></returns>
        private void FixDocument(int idOrg, int idProtocol, string path)
        {
            try
            {
                List<Paragraph> paragraphItems = new List<Paragraph>();
                string prev = null;
                Table tbl;
                var doc = WordprocessingDocument.Open(path, true);

                var paragraphs = doc.MainDocumentPart.Document.Body.Descendants<Paragraph>();
                //Получаем таблицу с испытаниями
                tbl = doc.MainDocumentPart.Document.Body.Descendants<Table>().ToArray()[4];
                //Исключаем копирайтинг строки
                paragraphs = paragraphs.Where(el => !el.InnerXml.Contains(@"<w:color w:val=""FF0000"" />")
                        && !el.InnerXml.Contains(@"<w:br w:type=""page"" />")).ToArray();

                //Параграфы берем без значений таблицы
                var flagForTable = false;
                Body body = new Body();
                bool isNeedAligement = false;
                bool isTitulPage = true;
                bool isHeaderProtocolIspinatii = false;
                foreach (var el in paragraphs)
                {
                    if (el.InnerXml.Contains("pic:"))
                    {
                        if (!string.IsNullOrEmpty(el.InnerText) && !el.InnerText.Contains("М.П."))
                        {
                            var node = (Paragraph)el.CloneNode(true);
                            node.ChildElements[1].ChildElements[2].Remove();
                            //var pp = new Paragraph(new Run(new Text($"{el.InnerText}")));
                            body.AppendChild(node);
                        }

                        prev = el.InnerText;
                        continue;
                    }
                    //исключаем пустые параграфы, если их более одного подряд
                    if (!(string.IsNullOrEmpty(el.InnerText) && string.IsNullOrEmpty(prev)) && !el.InnerText.Contains("М.П."))
                    {
                        if (el.InnerText.Contains("Результаты испытаний") && !flagForTable && !isHeaderProtocolIspinatii)
                        {
                            var p = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                            body.AppendChild(p);
                            flagForTable = true;
                            isHeaderProtocolIspinatii = true;

                            body.AppendChild(el.CloneNode(true));
                        }
                        if (el.InnerText.Contains("ПРОТОКОЛ ИСПЫТАНИЙ"))
                        {
                            var pp = new Paragraph(new Run(new Text("М.П.")));
                            Justification justification1 = new Justification() { Val = JustificationValues.Right };
                            pp.ParagraphProperties = new ParagraphProperties()
                            {
                                Justification = justification1
                            };
                            body.AppendChild(pp);

                            isTitulPage = false;
                            var p = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                            //paragraphItems.Add(p);
                            body.AppendChild(p);
                        }
                        if (el.InnerText.Contains("Внимание!"))
                        {
                            flagForTable = false;
                        }
                        // Параграфы, которые не входят в таблицы
                        if (!flagForTable)
                        {
                            Paragraph cloneNode = (Paragraph)el.CloneNode(true);
                            //Выравниваем текст по ширине после "Число страниц"
                            if (prev !=null && prev.Contains(Resources.Protocol19))
                            {
                                isNeedAligement = true;
                            }
                            if (el.InnerText.Contains("УТВЕРЖДАЮ"))
                            {
                                var txt = new Paragraph(new Run(new Text()));
                                body.AppendChild(txt);
                                
                                Text t = new Text($"                                                                                     {el.InnerText}");
                                t.Space = SpaceProcessingModeValues.Preserve;


                                RunProperties run1Properties = new RunProperties();
                                run1Properties.Append(new Bold());
                                var run = new Run(t);
                                run.RunProperties = run1Properties;


                                cloneNode = new Paragraph(run);

                            }
                            //Перестаем выравнивать по ширине, если дошли до строки "Конец протокола..."
                            if (el.InnerText.Contains("Конец"))
                            {
                                isNeedAligement = false;
                            }
                            if (isNeedAligement)
                            {
                                Justification justification1 = new Justification() { Val = JustificationValues.Both };
                                cloneNode.ParagraphProperties = new ParagraphProperties()
                                {
                                    Justification = justification1
                                };
                            }
                            if (el.InnerText.Contains("Список применяемого оборудования и средств измерений"))
                            {
                                var p = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                                body.AppendChild(p);
                            }
                            
                            if (isTitulPage && !string.IsNullOrEmpty(prev))
                            {
                                if (!string.IsNullOrEmpty(el.InnerText))
                                {
                                    SpacingBetweenLines spacing = new SpacingBetweenLines() { LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                                    if (cloneNode.ParagraphProperties == null)
                                    {
                                        cloneNode.ParagraphProperties = new ParagraphProperties();
                                    }
                                    cloneNode.ParagraphProperties.Append(spacing);
                                    body.AppendChild(cloneNode);
                                }
                            }
                            else if (!string.IsNullOrEmpty(prev) && prev.Contains("Список применяемого оборудования"))
                            {
                                var equipList = _equipments.Split('\n').Where(s => s != "").ToList();

                                for (int i=0; i<equipList.Count; i++)
                                {
                                    RunProperties runProperties1 = new RunProperties();
                                    FontSize fontSize1 = new FontSize() { Val = "20" };
                                    runProperties1.Append(fontSize1);
                                    var paragraph = new Paragraph();
                                    var run = new Run();
                                    var text = new Text(equipList[i]);

                                    run.Append(runProperties1);
                                    run.Append(text);

                                    paragraph.Append(run);

                                    if (i == equipList.Count - 1)
                                    {
                                        var txt = paragraph;
                                        SpacingBetweenLines spacing = new SpacingBetweenLines() { LineRule = LineSpacingRuleValues.Auto, Before = "0"};
                                        ParagraphProperties paragraphProperties = new ParagraphProperties();
                                        paragraphProperties.Append(spacing);
                                        txt.ParagraphProperties = paragraphProperties;
                                        body.AppendChild(txt);
                                    }
                                    else
                                    {
                                        var txt = paragraph;
                                        SpacingBetweenLines spacing = new SpacingBetweenLines() { LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                                        ParagraphProperties paragraphProperties = new ParagraphProperties();
                                        paragraphProperties.Append(spacing);
                                        txt.ParagraphProperties = paragraphProperties;
                                        body.AppendChild(txt);
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(prev) && prev.Contains("Обозначения и наименования нормативных документов, устанавливающих методы испытаний:"))
                            {
                                var gostList = _gosts.Split('\n').Where(s => s != "").ToList();

                                for (int i = 0; i < gostList.Count; i++)
                                {
                                    if (i == gostList.Count - 1)
                                    {
                                        var txt = new Paragraph(new Run(new Text(gostList[i])));
                                        SpacingBetweenLines spacing = new SpacingBetweenLines() { LineRule = LineSpacingRuleValues.Auto, Before = "0" };
                                        ParagraphProperties paragraphProperties = new ParagraphProperties();
                                        paragraphProperties.Append(spacing);
                                        txt.ParagraphProperties = paragraphProperties;
                                        body.AppendChild(txt);
                                    }
                                    else
                                    {
                                        var txt = new Paragraph(new Run(new Text(gostList[i])));
                                        SpacingBetweenLines spacing = new SpacingBetweenLines() { LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                                        ParagraphProperties paragraphProperties = new ParagraphProperties();
                                        paragraphProperties.Append(spacing);
                                        txt.ParagraphProperties = paragraphProperties;
                                        body.AppendChild(txt);
                                    }
                                }
                            }
                            else
                            {
                                body.AppendChild(cloneNode);
                            }

                        }

                        if ((prev != null && prev.Contains("Результаты испытаний (")))
                        {
                            Table t = new Table();

                            // Then we just create a new row and a few cells and we give them a width
                            //var tr = new TableRow();
                            //var tc1 = new TableCell();
                            //var tc2 = new TableCell();
                            //tc1.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2000" }));
                            //tc2.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2000" }));
                            //table.Append(tr);

                            var oxl = tbl.ChildElements;
                            TableProperties props = new TableProperties();
                            TableRowHeight th = new TableRowHeight { HeightType = HeightRuleValues.Auto };
                            TableLayout tl = new TableLayout() { Type = TableLayoutValues.Fixed };
                            props.TableLayout = tl;
                            props.Append(th);
                            t.Append(props);

                            //var columns = tbl.Descendants<Column>();
                            var cells = tbl.Descendants<TableCell>();
                            foreach (TableCell cell in cells) {
                                cell.TableCellProperties.TableCellWidth = new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2000" }; // .Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2000" }));
                                //tc2.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2000" }));
                            }

                            for (int i = 3; i < oxl.Count; i++)
                            {
                                //!oxl[i].InnerText.Contains("Evaluation Only") && 
                                if (!oxl[i].InnerText.Contains("Результаты испытаний ("))
                                {
                                    OpenXmlElement child = oxl[i].CloneNode(true);
                                    t.AppendChild(child);
                               }
                            }
                            body.AppendChild(t);
                            //flagForTable = false;
                        }
                    }
                    prev = el.InnerText;
                }

                //Очищаем весь файл
                doc.MainDocumentPart.Document.Body.Remove();
                doc.MainDocumentPart.Document.AppendChild(body);

                ApplyHeader(doc);
                doc.Save();
                doc.Close();
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }

        private void ApplyHeader(WordprocessingDocument doc)
        {
            MainDocumentPart mainDocPart = doc.MainDocumentPart;

            HeaderPart headerPart1 = mainDocPart.AddNewPart<HeaderPart>("r97");
            Header header1 = new Header();

            Paragraph paragraph1 = new Paragraph() { };
            Run run1 = new Run();
            Text colonText = new Text();
            colonText.Text = ColontitulText;

            var runProp = new RunProperties();
            var test = new Italic() { };
            runProp.Append(test);
            run1.PrependChild(runProp);

            run1.Append(colonText);
            paragraph1.Append(run1);
            header1.Append(paragraph1);
            headerPart1.Header = header1;
            SectionProperties sectionProperties1 = mainDocPart.Document.Body.Descendants<SectionProperties>().FirstOrDefault();
            if (sectionProperties1 == null)
            {
                sectionProperties1 = new SectionProperties() { };
                mainDocPart.Document.Body.Append(sectionProperties1);
            }
            HeaderReference headerReference1 = new HeaderReference() { Type = HeaderFooterValues.Default, Id = "r97" };
            sectionProperties1.InsertAt(headerReference1, 0);
        }

        private void WritePageCount(string path)
        {
            Application wordApp = null;
            DocumentWord myDoc = null;
            try
            {
                //Считаем количество страниц в сформированном файле. Открыть нужно т.к. это значение динамическое и заполняется только при открытии Word
                wordApp = new Application();
                string filename = $"{Directory.GetCurrentDirectory()}\\" + path;
                myDoc = wordApp.Documents.Open(filename);
                var numberOfPages = myDoc.ComputeStatistics(WdStatistic.wdStatisticPages, false);

                myDoc?.Close();
                myDoc = null;
                wordApp?.Quit();
                wordApp = null;


                using (var doc = WordprocessingDocument.Open(path, true))
                {
                    string line = Resources.Protocol19 + $" {numberOfPages}";
                    var allDoc = doc.MainDocumentPart.Document.Body.Descendants<Paragraph>();
                    var oldChild = allDoc.First(el => el.InnerText.Contains(Resources.Protocol19));

                    //var run = new Run();
                    //var runFont = new RunFonts { Ascii = FONT };
                    //var runText = new Text(line);
                    //var runProp = new RunProperties();
                    //runProp.Append(runFont);
                    //runProp.Append(runText);
                    //run.PrependChild(runProp);
                    var newChild = new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(line)));

                    doc.MainDocumentPart.Document.Body.ReplaceChild(newChild, oldChild);
                    doc.Save();
                }





                //using (SpreadsheetDocument workbook = SpreadsheetDocument.Open(path, true))
                //{
                //    WorkbookPart workbookPart = workbook.WorkbookPart;
                //    WorksheetPart worksheetPart = workbookPart.WorksheetParts.FirstOrDefault();
                //    Worksheet worksheet = worksheetPart.Worksheet;
                //    HeaderFooter header_footer = worksheet.Descendants<HeaderFooter>().FirstOrDefault();

                //    if (header_footer != null)
                //    {
                //        var header = header_footer.FirstChild;
                //        if (header != null)
                //        {
                //            header.Remove();
                //        }

                //        var footer = header_footer.LastChild;
                //        if (footer != null)
                //        {
                //            footer.Remove();
                //        }

                //        workbook.SaveAs(outputPath);
                //    }
                //}

            }
            finally
            {
               
                
            }
        }

        public void OpenWordprocessingDocumentPage(WordprocessingDocument wordDocument)
        {
            string filepath = @"C:\...\test.docx";
            // Open a WordprocessingDocument based on a filepath.
            Dictionary<int, string> pageviseContent = new Dictionary<int, string>();
            int pageCount = 0;
            // Assign a reference to the existing document body.  
            Body body = wordDocument.MainDocumentPart.Document.Body;
            if (wordDocument.ExtendedFilePropertiesPart.Properties.Pages.Text != null)
            {
                pageCount = Convert.ToInt32(wordDocument.ExtendedFilePropertiesPart.Properties.Pages.Text);
            }
            int i = 1;
            StringBuilder pageContentBuilder = new StringBuilder();
            foreach (var element in body.ChildElements)
            {
                if (element.InnerXml.IndexOf("<w:br w:type=\"page\" />", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    pageContentBuilder.Append(element.InnerText);
                }
                else
                {
                    pageviseContent.Add(i, pageContentBuilder.ToString());
                    i++;
                    pageContentBuilder = new StringBuilder();
                }
                if (body.LastChild == element && pageContentBuilder.Length > 0)
                {
                    pageviseContent.Add(i, pageContentBuilder.ToString());
                }
            }
        }

        private IXLWorksheet CreateChapter1(IXLWorksheet worksheet)
        {
            try
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
                worksheet.Cell("A" + 4).Value = valuesResourses[0];
                worksheet.Cell("A" + 4).Style.Font.FontSize = 10;
                worksheet.Cell("A" + 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A" + 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("A4:G4").Merge();
                worksheet.Cell("A" + 5).Value = valuesResourses[1];
                worksheet.Cell("A" + 5).Style.Font.FontSize = 10;
                worksheet.Cell("A" + 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A" + 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(4).Height = 35;
                worksheet.Range("A5:G5").Merge();
                worksheet.Cell("A" + 6).Value = valuesResourses[2];
                worksheet.Cell("A" + 6).Style.Font.FontSize = 10;
                worksheet.Cell("A" + 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A" + 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(5).Height = 62;
                worksheet.Row(6).Height = 45;
                worksheet.Range("A6:G6").Merge();
                worksheet.Cell("A" + 7).Value = valuesResourses[3];
                worksheet.Cell("A" + 7).Style.Font.FontSize = 10;
                worksheet.Cell("A" + 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A" + 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(7).Height = 25;
                worksheet.Range("A7:G7").Merge();
                worksheet.Cell("A" + 8).Value = valuesResourses[4];
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
                worksheet.Cell("D" + 12).Value = "                     " + Resources.Protocol10;
                worksheet.Cell("D" + 12).Style.Font.FontSize = 10;
                worksheet.Cell("D" + 12).Style.Font.Bold = true;
                worksheet.Cell("D" + 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("D" + 12).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("D12:G12").Merge();
                worksheet.Cell("D" + 13).Value = Resources.Protocol11 + "  ";
                worksheet.Cell("D" + 13).Style.Font.FontSize = 11;
                worksheet.Cell("D" + 13).Style.Font.Bold = true;
                worksheet.Cell("D" + 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell("D" + 13).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("D13:G13").Merge();
                worksheet.Cell("D" + 14).Value = Resources.Protocol12;
                worksheet.Cell("D" + 14).Style.Font.FontSize = 11;
                worksheet.Cell("D" + 14).Style.Font.Bold = true;
                worksheet.Cell("D" + 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell("D" + 14).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("D14:G14").Merge();
                worksheet.Cell("E" + 15).Value = "                          " + valuesResourses[5];
                worksheet.Cell("E" + 15).Style.Font.FontSize = 11;
                worksheet.Cell("E" + 15).Style.Font.Underline = XLFontUnderlineValues.Single;
                worksheet.Cell("E" + 15).Style.Font.Bold = true;
                worksheet.Cell("E" + 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell("E" + 15).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("E15:G15").Merge();

                // вставка картинки
                // непрозрачность 32%
                DBConnection db = new DBConnection();
                var img = db.GetImage(1);
                worksheet.AddPicture(img.Item2).MoveTo(worksheet.Cell("E" + 15)).ScaleHeight(0.6);
                worksheet.AddPicture(img.Item1).MoveTo(worksheet.Cell("F" + 16));
                img.Item1.Close();
                img.Item2.Close();

                worksheet.Cell("B" + 16).Value = Resources.Protocol14;
                worksheet.Cell("B" + 16).Style.Font.FontSize = 8;
                worksheet.Cell("B" + 16).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell("B" + 16).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("B16:G16").Merge();
                worksheet.Cell("C" + 17).Value = "                                " + _journal.Item2["C"]; // дата (исправить)
                worksheet.Cell("C" + 17).Style.Font.FontSize = 11;
                worksheet.Cell("C" + 17).Style.Font.Bold = true;
                worksheet.Cell("C" + 17).Style.Font.Underline = XLFontUnderlineValues.Single;
                worksheet.Cell("C" + 17).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell("C" + 17).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("C17:G17").Merge();
                worksheet.Cell("B" + 18).Value = Resources.Protocol16;
                worksheet.Cell("B" + 18).Style.Font.FontSize = 8;
                worksheet.Cell("B" + 18).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell("B" + 18).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("B18:G18").Merge();
                worksheet.Cell("F" + 19).Value = Resources.Protocol17;
                worksheet.Cell("F" + 19).Style.Font.FontSize = 11;
                worksheet.Cell("F" + 19).Style.Font.Bold = true;
                worksheet.Cell("F" + 19).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell("F" + 19).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("F19:F19").Merge();

                return worksheet;
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }

        private IXLWorksheet CreateChapter2(IXLWorksheet worksheet)
        {
            try { 
                worksheet.Cell("A" + 23).Value = Resources.Protocol18;
                worksheet.Cell("A" + 23).Style.Font.FontSize = 12;
                worksheet.Cell("A" + 23).Style.Font.Bold = true;
                worksheet.Cell("A" + 23).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A" + 23).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("A23:G23").Merge();
                ColontitulText += "№ " + _journal.Item1["O"] + " от " + _journal.Item1["H"];
                worksheet.Cell("A" + 24).Value = "№ " + _journal.Item1["O"] + " от " + _journal.Item1["H"];
                worksheet.Cell("A" + 24).Style.Font.FontSize = 12;
                worksheet.Cell("A" + 24).Style.Font.Bold = true;
                worksheet.Cell("A" + 24).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A" + 24).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("A24:G24").Merge();
                // число страниц (исправить) - значение
                worksheet.Cell("A" + 25).Value = Resources.Protocol19; 
                worksheet.Cell("A" + 25).Style.Font.FontSize = 10;
                worksheet.Cell("A" + 25).Style.Font.Bold = true;
                worksheet.Cell("A" + 25).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + 25).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("A25:G25").Merge();
                // Регистрационный (условный) номер, характеристика испытуемого образца: 
                worksheet.Cell("A" + 26).Value = Resources.Protocol20;
                worksheet.Cell("A" + 26).Style.Font.FontSize = 10;
                worksheet.Cell("A" + 26).Style.Font.Bold = true;
                worksheet.Cell("A" + 26).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + 26).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("A26:G26").Merge();

                worksheet.Cell("A" + 27).Value = "(образец " + _journal.Item1["I"] + "): - " + _journal.Item1["D"];
                worksheet.Cell("A" + 27).Style.Font.FontSize = 10;
                worksheet.Cell("A" + 27).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + 27).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(27).Height = 65;
                worksheet.Range("A27:G27").Merge();
                // образцы
                idRow = 28; // который свободен для записи номер строки

                worksheet.Cell("A" + idRow).Value = Resources.Protocol21;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;
                worksheet.Cell("A" + idRow).Value = _journal.Item1["H"];
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;

                worksheet.Cell("A" + idRow).Value = Resources.Protocol22;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Row(idRow).Height = 20;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;
                worksheet.Cell("A" + idRow).Value = valuesResourses[6];
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
                worksheet.Row(idRow).Height = 90;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;

                worksheet.Cell("A" + idRow).Value = Resources.Protocol23;// 29
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;
                worksheet.Cell("A" + idRow).Value = _journal.Item1["Q"];// 29
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(idRow).Height = 55; 
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;

                worksheet.Cell("A" + idRow).Value = Resources.Protocol24 ;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(idRow).Height = 45; // 30
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;
                worksheet.Cell("A" + idRow).Value = _journal.Item1["R"];
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(idRow).Height = 45; // 30
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;

                worksheet.Cell("A" + idRow).Value = Resources.Protocol25;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;
                worksheet.Cell("A" + idRow).Value = " № " + _journal.Item1["B"];
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
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
                worksheet.Row(idRow).Height = 45; // 33
                idRow++;
                worksheet.Cell("A" + idRow).Value = _gosts;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                worksheet.Row(idRow).Height = 45; // 33
                idRow++;

                worksheet.Cell("A" + idRow).Value = Resources.Protocol26_43; 
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(idRow).Height = 20; // 34
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;
                worksheet.Cell("A" + idRow).Value = _equipments;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(idRow).Height = 125; // 34
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;

                worksheet.Cell("A" + idRow).Value = Resources.Protocol27;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;
                // Реквизиты акта отбора образцов  + " " + _journal.Item1["C"]
                worksheet.Cell("A" + idRow).Value = Resources.Protocol28;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Row(idRow).Height = 20; // 36
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;
                worksheet.Cell("A" + idRow).Value = " № " + _journal.Item1["B"].Split(' ')[0];
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
                worksheet.Row(idRow).Height = 20; // 36
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;

                worksheet.Cell("A" + idRow).Value = Resources.Protocol29;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(idRow).Height = 20;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;
                worksheet.Cell("A" + idRow).Value =_journal.Item1["H"] + "-" + _journal.Item2["C"];
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(idRow).Height = 20;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                idRow++;

                worksheet.Cell("A" + idRow).Value = Resources.Protocol30;
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                worksheet.Row(idRow).Height = 50;
                idRow++;
                worksheet.Cell("A" + idRow).Value = valuesResourses[7];
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = false;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:G{idRow}").Merge();
                worksheet.Row(idRow).Height = 50;
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
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }

        private IXLWorksheet MergeColumns(IXLWorksheet worksheet, int countRows)
        {
            try
            {
                int iRow, i;
                
                iRow = 4;
                for (i = 5; i <= countRows; i++)
                {
                    if (worksheet.Cell("C" + iRow).Value.ToString() != worksheet.Cell("C" + i).Value.ToString())
                    {
                        worksheet.Range($"C{iRow}:C{i - 1}").Merge();
                        iRow = i;
                    }
                }
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
            return worksheet;
        }

        private IXLWorksheet CreateTablesTests(IXLWorksheet worksheet, List<Tuple<List<string>, Dictionary<int, List<string>>>> values)
        {
            // 64 + 180 + 110 + 75 + 75 = 260 + 244 = 504 / 5 = 101

            try
            { 
                idRow = 1;
                worksheet.Cell("A" + idRow).Value = Resources.Protocol36 + "№ " + _journal.Item1["I"] + "):";
                worksheet.Cell("A" + idRow).Style.Font.FontSize = 10;
                worksheet.Cell("A" + idRow).Style.Font.Bold = true;
                worksheet.Cell("A" + idRow).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell("A" + idRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell("A" + idRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A{idRow}:E{idRow}").Merge();
                idRow++;
                worksheet.Cell("A" + idRow).Value = Resources.Protocol37;
                worksheet.Cell("B" + idRow).Value = Resources.Protocol38;
                worksheet.Cell("C" + idRow).Value = Resources.Protocol39;
                worksheet.Cell("D" + idRow).Value = Resources.Protocol41;
                worksheet.Cell("E" + idRow).Value = Resources.Protocol42;
                worksheet.Row(idRow).Height = 80;
                worksheet.Range($"A{idRow}:E{idRow}").Style.Font.FontSize = 10;
                worksheet.Range($"A{idRow}:E{idRow}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{idRow}:E{idRow}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range($"A{idRow}:E{idRow}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                idRow++;
                worksheet.Cell("A" + idRow).Value = "1";
                worksheet.Cell("B" + idRow).Value = "2";
                worksheet.Cell("C" + idRow).Value = "3";
                worksheet.Cell("D" + idRow).Value = "4";
                worksheet.Cell("E" + idRow).Value = "5";
                worksheet.Range($"A{idRow}:E{idRow}").Style.Font.FontSize = 10;
                worksheet.Range($"A{idRow}:E{idRow}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{idRow}:E{idRow}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range($"A{idRow}:E{idRow}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                idRow++;
                for (int i = 0; i < _countAdditionals; i++)
                {
                    worksheet.Cell("A" + idRow).Value = i.ToString();
                    worksheet.Cell("B" + idRow).Value = values[i].Item1[1];
                    worksheet.Cell("C" + idRow).Value = values[i].Item1[0];
                    worksheet.Cell("D" + idRow).Value = values[i].Item1[2];
                    worksheet.Cell("E" + idRow).Value = values[i].Item1[3];
                    worksheet.Range($"A{idRow}:E{idRow}").Style.Font.FontSize = 10;
                    worksheet.Range($"A{idRow}:E{idRow}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Range($"A{idRow}:E{idRow}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Range($"A{idRow}:E{idRow}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    idRow++;
                }
                idRow++;

                return MergeColumns(worksheet, idRow);
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }
        
        private IXLWorksheet CreateLastChapter(IXLWorksheet worksheet)
        {
            try { 
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
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }
    }
}
