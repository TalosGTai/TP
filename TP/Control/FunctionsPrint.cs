using System;
using System.Collections.Generic;

namespace TP.Control
{
    /// <summary>
    /// Класс для вывода словарей в консоль
    /// </summary>
    public class FunctionsPrint
    {
        public FunctionsPrint() 
        {

        }
        /// <summary>
        /// Вывод двух словарей в консоль
        /// </summary>
        /// <param name="tuple">Тьюпл из двух словарей</param>
        public void PrintTupleDictionary(Tuple<Dictionary<string, string>, Dictionary<string, string>> tuple)
        {
            PrintDictionary(tuple.Item1);
            PrintDictionary(tuple.Item2);
        }
        /// <summary>
        /// Вывод словаря в консоль
        /// </summary>
        /// <param name="dic">словарь из string</param>
        public void PrintDictionary(Dictionary<string, string> dic)
        {
            foreach (var key in dic.Keys)
            {
                Console.Write($"{key}:{dic[key]}");
                Console.WriteLine();
            }
        }
    }
}
