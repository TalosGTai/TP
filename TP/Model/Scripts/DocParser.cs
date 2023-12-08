using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TP.Model.Scripts
{
    public class DocParser
    {
        WordprocessingDocument doc;
        Tuple<Dictionary<string, string>, Dictionary<string, string>> journalParse { get; set; }

        public DocParser() 
        {
            doc = WordprocessingDocument.Open("..\\..\\Model\\Directions\\Направление.docx", false);
            // Получение всех абзацев в документе
            IEnumerable<Paragraph> paragraphs = doc.MainDocumentPart.Document.Descendants<Paragraph>();
            journalParse = Org1CheckAllLists(paragraphs);
        }

        public DocParser(string path)
        {
            doc = WordprocessingDocument.Open(path, false);
            // Получение всех абзацев в документе
            IEnumerable<Paragraph> paragraphs = doc.MainDocumentPart.Document.Descendants<Paragraph>();
            journalParse = Org1CheckAllLists(paragraphs);
        }

        public Tuple<Dictionary<string, string>, Dictionary<string, string>> JournalParse
        {
            get { return journalParse; }
            set { journalParse = value; }
        }

        private Tuple<string, string> IsColumnB(string value)
        {
            Regex regex = new Regex(@"\d\d.\d\d.\d{4}");
            MatchCollection match = regex.Matches(value);
            string[] subs = value.Split(' ');
            if (match.Count > 0)
            {
                string num = "";

                foreach (var sub in subs)
                {
                    foreach (char c in sub)
                    {
                        if (char.IsDigit(c))
                            num += c;
                    }
                    if (num.Length > 0)
                        break;
                }
                return new Tuple<string, string>(num, match[0].ToString());
            }
            return new Tuple<string, string>("null", "null");
        }

        private string IsColumnС(string value)
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

        private bool IsColumnDHelp(string value)
        {
            string pattern = "Испытания провести по следующим методам, показателям:";
            if (value.IndexOf(pattern) != -1)
                return true;
            return false;
        }

        private string IsColumnD(string value, bool access)
        {
            if (access)
            {
                value = value.Trim(' ');
                return value;
            }
            return "null";
        }

        private string IsColumnE(string value)
        {
            return "null";
        }

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

        private bool IsColumnQHelp(string value)
        {
            string pattern = "Образцы представлены заказчиком/заявителем:";
            if (value.IndexOf(pattern) != -1)
                return true;
            return false;
        }

        private string IsColumnQ(string value, bool access)
        {
            if (access)
            {
                return value;
            }
            return "null";
        }

        private string IsColumnR(string value)
        {
            string pattern = "Изготовитель:";
            if (value.IndexOf(pattern) != -1)
            {
                string result = "";
                for (int i = value.IndexOf(pattern) + 13; i < value.Length; i++)
                {
                    result += value[i];
                }
                result = result.Trim(' ');
                return result;
            }
            return "null";
        }

        public bool IsAlpha(char c)
        {
            if ((c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я')) return true;
            return false;
        }

        public Tuple<Dictionary<string, string>, Dictionary<string, string>> Org1CheckAllLists(IEnumerable<Paragraph> paragraphs)
        {
            Dictionary<string, string> list1Values = new Dictionary<string, string>();
            Dictionary<string, string> list2Values = new Dictionary<string, string>();
            bool columnB, columnC, columnD, columnE, columnF, columnQ, columnR, columnDHelp, columnQHelp;

            columnB = columnC = columnD = columnE = columnF = columnQ = columnR = columnDHelp = columnQHelp = false;

            foreach (Paragraph paragraph in paragraphs)
            {
                if (!columnB)
                {
                    Tuple<string, string> tuple = IsColumnB(paragraph.InnerText);
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
                    string temp = IsColumnС(paragraph.InnerText);
                    if (temp != "null")
                    {
                        // save values to Dictionary
                        list1Values["C"] = temp;
                        columnC = true;
                    }
                }
                if (!columnD)
                {
                    if (!columnDHelp)
                    {

                        columnDHelp = IsColumnDHelp(paragraph.InnerText);
                    }
                    else
                    {
                        list1Values["D"] = IsColumnD(paragraph.InnerText, columnDHelp);
                        columnD = true;
                    }
                }
                if (!columnE)
                {

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
                    if (!columnQHelp)
                    {
                        columnQHelp = IsColumnQHelp(paragraph.InnerText);
                    }
                    else
                    {
                        list1Values["Q"] = IsColumnQ(paragraph.InnerText, columnQHelp);
                        columnQ = true;
                    }
                }
                if (!columnR)
                {
                    string temp = IsColumnR(paragraph.InnerText);
                    if (temp != "null")
                    {
                        list1Values["R"] = IsColumnR(paragraph.InnerText);
                        columnR = true;
                    }
                }
            }
            return new Tuple<Dictionary<string, string>, Dictionary<string, string>>(list1Values, list2Values);
        }
    }
}
