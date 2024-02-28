using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace TP.Model
{

    public static class Logger
    {
        private const string path = "log.txt";

        public static void LogDbError(Exception ex)
        {
            var s = new StackTrace(ex);
            var thisasm = Assembly.GetExecutingAssembly();
            var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

            var errorText = $"Ошибка при вызове {methodname}. Message = {ex.Message}, " +
                       $"StackTrace = {ex.StackTrace}";

            MessageBox.Show("Ошибка работы с бд");
            using (var sw = new StreamWriter(path, true))
            {
                sw.WriteLine(errorText);
            }
        }

        public static void LogError(Exception ex, string message = null)
        {
            var s = new StackTrace(ex);
            var thisasm = Assembly.GetExecutingAssembly();
            var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

            var errorText = $"Ошибка при вызове {methodname}. Message = {ex.Message}, " +
                       $"StackTrace = {ex.StackTrace}";

            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message);
            }
            using (var sw = new StreamWriter(path, true))
            {
                sw.WriteLine(errorText);
            }
        }
    }

}