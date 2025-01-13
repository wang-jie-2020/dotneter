using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using npoi_draw;
using OfficeOpenXml;
using System;
using System.IO;

namespace npoi_transfer_epplus
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cell1 = new NPOI.SS.Util.CellAddress("B2");
            Console.WriteLine("---npoi---");
            Console.WriteLine(cell1.Row);
            Console.WriteLine(cell1.Column);

            var cell2 = new OfficeOpenXml.ExcelAddress("B2");
            Console.WriteLine("---epplus---");
            Console.WriteLine(cell2.Start.Row);
            Console.WriteLine(cell2.Start.Column);
            Console.WriteLine(cell2.End.Row);
            Console.WriteLine(cell2.End.Column);

            var cell3 = new OfficeOpenXml.ExcelAddress("A1");
            Console.WriteLine("---epplus---");
            Console.WriteLine(cell3.Start.Row);
            Console.WriteLine(cell3.Start.Column);
            Console.WriteLine(cell3.End.Row);
            Console.WriteLine(cell3.End.Column);

            //var demo = new NPOIChart();
            //demo.RunDemo();
        }
    }
}
