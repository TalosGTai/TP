﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TP.Model.Scripts
{
    /// <summary>
    /// Парсер документов
    /// </summary>
    public class DocParser
    {
        WordprocessingDocument doc;
        Tuple<Dictionary<string, string>, Dictionary<string, string>> _journalParse;

        /// <summary>
        /// Парсим документ в JournalParse по пути по умолчанию ..\\..\\Model\\Directions\\Направление.docx
        /// </summary>
        public DocParser()
        {
            try
            {
                doc = WordprocessingDocument.Open("..\\..\\Model\\Directions\\Направление.docx", false);
                // Получение всех абзацев в документе
                IEnumerable<Paragraph> paragraphs = doc.MainDocumentPart.Document.Descendants<Paragraph>();
                _journalParse = Org1CheckAllLists(paragraphs);
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }

        /// <summary>
        /// Парсим документ в JournalParse по переданному пути
        /// </summary>
        /// <param name="path">путь к файлу</param>
        public DocParser(string path)
        {
            try
            {
                doc = WordprocessingDocument.Open(path, false);
                // Получение всех абзацев в документе
                IEnumerable<Paragraph> paragraphs = doc.MainDocumentPart.Document.Descendants<Paragraph>();
                _journalParse = Org1CheckAllLists(paragraphs);
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }

        /// <summary>
        /// Журнал, который распарсили по абзацам в Tuple<Dictionary<string, string>, Dictionary<string, string>>
        /// </summary>
        public Tuple<Dictionary<string, string>, Dictionary<string, string>> JournalParse
        {
            get { return _journalParse; }
            set { _journalParse = value; }
        }

        private bool isColumnB(string value)
        {
            if (value.IndexOf("НАПРАВЛЕНИЕ") != -1 || value.IndexOf("Направление") != -1)
                return true;
            return false;
        }

        /// <summary>
        /// Получение колонки В
        /// </summary>
        /// <param name="value">строка</param>
        /// <returns>значение колонки В, либо Tuple с null </returns>
        private Tuple<string, string> ColumnB(string value)
        {
            Regex regex = new Regex(@"\d\d.\d\d.\d{4}");
            MatchCollection match = regex.Matches(value);
            if (match.Count > 0)
            {
                string[] subs = value.Split(' ');
                for (int i = 0; i < subs.Length; i++)
                    if (subs[i].IndexOf("№") != -1)
                        return new Tuple<string, string>(subs[i + 1], match[0].ToString());
            }
            return new Tuple<string, string>("null", "null");
        }
        /// <summary>
        /// Получение колонки C (Акт отбора образцов)
        /// </summary>
        /// <param name="value">строка</param>
        /// <returns>значение колонки C, либо null </returns>
        private string ColumnС(string value)
        {
            string result = "";
            string pattern = "Акт отбора образцов";
            if (value.IndexOf(pattern) != -1)
            {
                int idChar = value.IndexOf(':') + 1;

                for (int i = idChar; i < value.Length; i++)
                {
                    if (IsAlpha(value[i]) == true)
                    {
                        idChar = i;
                        break;
                    }
                }
                for (int i = idChar; i < value.Length; i++)
                {
                    result += value[i];
                }
                result = result.Trim(' ');
                return result;
            }
            return "null";
        }
        /// <summary>
        /// Проверка, является ли строка колонкой D
        /// </summary>
        /// <param name="value">строка</param>
        /// <returns>true - это колонка D; false - это не колонка D</returns>
        private bool IsColumnD(string value)
        {
            int count = 0;
            List<string> patterns = new List<string>()
            {
                "Испытания",
                "провести",
                "по",
                "следующим",
                "методам",
                "показателям"
            };
            foreach (string pattern in patterns)
            {
                if (value.IndexOf(pattern) != -1)
                    count++;
            }
            if (count > 4)
                return true;
            return false;
        }
        /// <summary>
        /// Получение колонки D
        /// </summary>
        /// <param name="value">строка</param>
        /// <param name="access">есть ли доступ</param>
        /// <returns>значение колонки D, либо null </returns>
        private string ColumnD(string value, bool access)
        {
            if (access)
            {
                value = value.Trim(' ');
                return value;
            }
            return "null";
        }

        private string StillColumnD(string value)
        {
            if (value.IndexOf("шт") != -1)
                return "False";
            return value;
        }
        /// <summary>
        /// Колонка Е = "null"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string IsColumnE(string value)
        {
            return "null";
        }
        /// <summary>
        /// Значение колонки F, проверяем есть ли цифра в строке
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Если есть число, то берем переданное значение, инчае "null"</returns>
        private string IsColumnF(string value)
        {
            // Проверяем есть ли цифра в строке
            foreach (char c in value)
            {
                if (char.IsDigit(c))
                {
                    return value;
                }
            }
            return "null";
        }
        /// <summary>
        /// Является ли эта строка колонкой Q
        /// </summary>
        /// <param name="value">строка</param>
        /// <returns> 1 - текущая строка является предпараграфом</returns>
        /// <returns> 2 - текущая строка полностью подходит</returns>
        /// <returns> 0 - не подходит</returns>
        private int IsColumnQ(string value)
        {
            List<string> patterns = new List<string>()
            {
                "Образцы",
                "представлены",
                "заказчиком",
                "заявителем",
                "Заявитель:",
            };
            foreach (string pattern in patterns)
            {
                if (value.IndexOf(pattern) != -1)
                    if (value.Length > 15)
                        return 2;
                    else
                        return 1;
            }
            return 0;
        }

        /// <summary>
        /// Значение колонки Q
        /// </summary>
        /// <param name="value">строка</param>
        /// <param name="access">доступ</param>
        /// <returns>Значение колонки Q</returns>
        private string ColumnQ(string value, bool access)
        {
            if (access)
            {
                return CheckChangeString(value);
            }
            return "null";
        }

        private string CheckChangeString(string value)
        {
            if (value.IndexOf("аявитель") != -1)
                return value.Substring(10);
            return value;
        }

        /// <summary>
        /// Значение колонки R
        /// </summary>
        /// <param name="value">строка</param>
        /// <returns>Значение колонки R</returns>
        private string ColumnR(string value)
        {
            if (value.IndexOf("Изготовитель") != -1)
            {
                value = value.Trim(' ');
                int i = 0;
                while (i < value.Length && value[i] != ' ')
                    i++;
                string result = "";
                i++;
                for (; i < value.Length; i++)
                {
                    result += value[i];
                }
                return result;
            }
            return "null";
        }
        /// <summary>
        /// Является ли символ буквой русского алфавита
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsAlpha(char c)
        {
            if ((c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я')) return true;
            return false;
        }

        private Dictionary<string, string> SetDefaultValuesList1(Dictionary<string, string> listValues)
        {
            listValues["B"] = "";
            listValues["C"] = "";
            listValues["D"] = "";
            listValues["E"] = "";
            listValues["F"] = "";
            listValues["G"] = "";
            listValues["H"] = "";
            listValues["I"] = "";
            listValues["J"] = "";
            listValues["K"] = "";
            listValues["L"] = "";
            listValues["M"] = "";
            listValues["N"] = "";
            listValues["O"] = "";
            listValues["P"] = "";
            listValues["Q"] = "";
            listValues["R"] = "";
            return listValues;
        }

        private Dictionary<string, string> SetDefaultValuesList2(Dictionary<string, string> listValues)
        {
            listValues["B"] = "";
            listValues["C"] = "";
            listValues["D"] = "";
            listValues["E"] = "";
            listValues["F"] = "";
            listValues["G"] = "";
            listValues["H"] = "";
            listValues["I"] = "";
            return listValues;
        }

        /// <summary>
        /// Заполняем колонки листа 1 и листа 2
        /// </summary>
        /// <param name="paragraphs">текст в виде перечисления параграфов</param>
        /// <returns>Лист 1 и лист 2 со значениями колонок</returns>
        public Tuple<Dictionary<string, string>, Dictionary<string, string>> Org1CheckAllLists(IEnumerable<Paragraph> paragraphs)
        {
            try 
            { 
                Dictionary<string, string> list1Values = new Dictionary<string, string>();
                Dictionary<string, string> list2Values = new Dictionary<string, string>();
                bool columnB, columnC, columnD, columnE, columnF, columnQ, columnR, columnDHelp, columnQHelp,
                    columnBHelp, documentDC, documentCC;
                int countRow, columnQQHelp;

                columnB = columnC = columnD = columnE = columnF = columnQ = columnR = columnDHelp = columnQHelp =
                    columnBHelp = documentDC = documentCC = false;
                countRow = columnQQHelp = 0;
                list1Values = SetDefaultValuesList1(list1Values);
                list2Values = SetDefaultValuesList2(list2Values);

                foreach (Paragraph paragraph in paragraphs)
                {
                    countRow++;
                    columnB = isColumnB(paragraph.InnerText);
                    if (countRow < 2 && !columnBHelp)
                    {
                        documentCC = true;
                        break;
                    }
                    else if (countRow >= 3)
                    {
                        documentDC = true;
                        break;
                    }
                }

                columnBHelp = false;
                if (documentDC)
                {
                    foreach (Paragraph paragraph in paragraphs)
                    {
                        if (!columnBHelp)
                        {
                            columnBHelp = isColumnB(paragraph.InnerText);
                        }
                        if (!columnB && columnBHelp)
                        {
                            Tuple<string, string> tuple = ColumnB(paragraph.InnerText);
                            if (tuple.Item1 != "null")
                            {
                                list1Values["B"] = tuple.Item1;
                                list2Values["D"] = tuple.Item2;
                                columnB = true;
                            }
                        }
                        if (!columnC)
                        {
                            string temp = ColumnС(paragraph.InnerText);
                            if (temp != "null")
                            {
                                list1Values["C"] = temp;
                                columnC = true;
                            }
                        }
                        if (!columnD)
                        {
                            if (!columnDHelp)
                            {
                                columnDHelp = IsColumnD(paragraph.InnerText);
                            }
                            else
                            {
                                var t = StillColumnD(paragraph.InnerText);
                                if (t != "False")
                                    list1Values["D"] += t;
                                else
                                    columnD = true;
                            }
                        }
                        if (!columnF)
                        {
                            if (columnD)
                            {
                                string temp = IsColumnF(paragraph.InnerText);
                                if (temp != "null")
                                {
                                    list1Values["F"] = IsColumnF(paragraph.InnerText);
                                    columnF = true;
                                }
                            }
                        }
                        if (!columnQ)
                        {
                            if (columnQQHelp == 1)
                            {
                                list1Values["Q"] = ColumnQ(paragraph.InnerText, true);
                                columnQ = true;
                            }
                            if (!columnQHelp)
                            {
                                columnQQHelp = IsColumnQ(paragraph.InnerText);
                            }
                            if (columnQQHelp == 2)
                            {
                                list1Values["Q"] = ColumnQ(paragraph.InnerText, true);
                                columnQ = true;
                            }
                        }
                        if (!columnR)
                        {
                            string temp = ColumnR(paragraph.InnerText);
                            if (temp != "null")
                            {
                                list1Values["R"] = ColumnR(paragraph.InnerText);
                                columnR = true;
                            }
                        }
                    }
                }
                else
                {
                    foreach (Paragraph paragraph in paragraphs)
                    {
                        if (!columnBHelp)
                        {
                            columnBHelp = isColumnB(paragraph.InnerText);
                            if (!columnBHelp)
                                list1Values["Q"] += paragraph.InnerText;
                        }
                        if (!columnB && columnBHelp)
                        {
                            Tuple<string, string> tuple = ColumnB(paragraph.InnerText);
                            if (tuple.Item1 != "null")
                            {
                                // save values to Dictionary
                                list1Values["B"] = tuple.Item1;
                                list2Values["D"] = tuple.Item2;
                                columnB = true;
                            }
                        }
                        if (!columnC)
                        {
                            string temp = ColumnС(paragraph.InnerText);
                            if (temp != "null")
                            {
                                list1Values["C"] = temp;
                                columnC = true;
                            }
                            if (!columnC && columnR)
                                list1Values["R"] += paragraph.InnerText;
                        }
                        if (!columnD)
                        {
                            if (!columnDHelp)
                            {
                                columnDHelp = IsColumnD(paragraph.InnerText);
                            }
                            else
                            {
                                var t = StillColumnD(paragraph.InnerText);
                                if (t != "False")
                                    list1Values["D"] += t;
                                else
                                    columnD = true;
                            }
                        }
                        if (!columnF)
                        {
                            if (columnD)
                            {
                                string temp = IsColumnF(paragraph.InnerText);
                                if (temp != "null")
                                {
                                    list1Values["F"] = IsColumnF(paragraph.InnerText);
                                    columnF = true;
                                }
                            }
                        }
                        if (!columnR)
                        {
                            string temp = ColumnR(paragraph.InnerText);
                            if (temp != "null")
                            {
                                list1Values["R"] = ColumnR(paragraph.InnerText);
                                columnR = true;
                            }
                        }
                    }
                }

                list1Values["E"] = list1Values["Q"];
                list2Values["B"] = list1Values["O"];
                list2Values["C"] = list1Values["L"];
                list2Values["D"] = list1Values["B"];
                list2Values["E"] = list1Values["I"];
                list2Values["F"] = list2Values["B"];
                list2Values["G"] = list2Values["C"];
                list2Values["H"] = list2Values["C"];
                list2Values["I"] = list1Values["M"];
                return new Tuple<Dictionary<string, string>, Dictionary<string, string>>(list1Values, list2Values);
            }
            catch (Exception ex) { Logger.LogError(ex); throw; }
        }   
    }
}
