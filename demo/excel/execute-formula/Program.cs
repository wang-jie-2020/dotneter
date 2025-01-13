using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace execute_formula
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var path = @"C:\Users\Administrator\Desktop\device\cell05-8-2023.04.17.17.10.07.524_L-CHC-516.CH4_T23031045C-005-222Z13202071_RPT01.xlsx";
            var newPath = @"C:\Users\Administrator\Desktop\device\newfile.xlsx";


            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook book = null;
            bool status = false;
            try
            {
                book = xlApp.Workbooks.Open(path);
                book.SaveAs(newPath);
                status = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                book.Close(false);
                xlApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            }




            //Application app = new Application();
            //app.Application.Workbooks.Open(@"D:\Code\dotnet\demo\Excel\psych.xlam");
            //var g = app.AddIns.Application.Run("Psych", 1, 2, 3, 4, 5, 6);
            //Console.WriteLine(g);
            //Console.ReadKey();
        }
    }
}
