using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Control
{
    public class FunctionsPrint
    {
        public FunctionsPrint() 
        {

        }

        public void PrintTupleDictionary(Tuple<Dictionary<string, string>, Dictionary<string, string>> tuple)
        {
            PrintDictionary(tuple.Item1);
            PrintDictionary(tuple.Item2);
        }

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
