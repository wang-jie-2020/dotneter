using System;
using System.IO;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

namespace DeviceOut
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\Administrator\Desktop\device\cell05-8-2023.04.17.17.10.07.524_L-CHC-516.CH4_T23031045C-005-222Z13202071_RPT01.xlsx";
            var newPath = @"C:\Users\Administrator\Desktop\device\newfile.xlsx";

            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            //ExcelPackage.Configure(o =>
            //{
            //    o.SuppressInitializationExceptions = true;
            //});
            //{
            //    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            //    {
            //        var package = new ExcelPackage(stream);
            //        var book = package.Workbook;
            //        var sheet = package.Workbook.Worksheets["全局配置"];
            //        var cell = sheet.Cells["B2"];
            //    }
            //}

            //{
            //    var package = new ExcelPackage(new FileInfo(path));
            //    var sheet = package.Workbook.Worksheets["全局配置"];
            //    var cell = sheet.Cells["B2"];
            //}

            var excel = new XSSFWorkbook(new FileInfo(path));
            var last = excel.GetSheetAt(1);
            var data = last.GetRow(3).GetCell(2);
            Console.WriteLine(data);

            using (var stream = new FileStream(newPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                excel.Write(stream);
            }

            Console.ReadLine();
        }
    }
}
