using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

namespace demo
{
    /// <summary>
    /// README
    ///     1.NPOI 可以PASS
    ///     2.EPPlus 综合表现良好,两个注意点:(1)SHEET数量对性能有影响(2)存在一个最大支持数量(CELL)的问题(Array dimensions)目前测试结果是4335w cells - 39w row
    ///     3.OPEN-SDK 和EPPlus的表现类似,核心问题还是xml的反序列化
    ///     4.LINQ2XML 效率最高,但必须忍受一些丢失,总的来说适用性肯定不如封装过的
    /// </summary>
    internal class ExcelReading
    {
        public void RunDemo()
        {
            while (true)
            {
                Console.WriteLine("请输入命令：0; 退出程序，功能命令：1 - n");
                string input = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                if (input == "0")
                {
                    break;
                }

                Stopwatch sw = Stopwatch.StartNew();

                Type? type = MethodBase.GetCurrentMethod()?.DeclaringType;
                if (type != null)
                {
                    object? o = Activator.CreateInstance(type);
                    type.InvokeMember("Method" + input,
                        BindingFlags.Static | BindingFlags.Instance |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.InvokeMethod,
                        null, o,
                        new object[] { });
                }

                sw.Stop();
                Console.WriteLine("Method" + input + ":" + sw.ElapsedMilliseconds / 1000);
            }
        }

        /// <summary>
        ///     NPOI 全部加载到内存再操作
        /// </summary>
        void Method1()
        {
            //new 4s
            var excel = new XSSFWorkbook(new FileInfo(Shard.TinyFile));

            //read <1s
            var last = excel.GetSheet("0121X14610010");
            var data = last.GetRow(10).GetCell(5).NumericCellValue;
            Console.WriteLine(data);

        }

        /// <summary>
        ///     NPOI超过20min,内存持续90+,直接pass
        /// </summary>
        void Method2()
        {
            var excel = new XSSFWorkbook(File.OpenRead(Shard.MiddlingFile));

            var last = excel.GetSheet("TestRecord");
            var data = last.GetRow(10).GetCell(5).NumericCellValue;
            Console.WriteLine(data);
        }

        /// <summary>
        ///     NPOI超过20min,内存持续90+,直接pass
        /// </summary>
        void Method3()
        {
            var excel = new XSSFWorkbook(new FileInfo(Shard.MiddlingFile));
        }

        /// <summary>
        ///     Magicodes.IE 包含了EPPlus的内容，但是不能确定是否包含EPPlus全部特性，暂且还是以其作为测试项
        /// </summary>
        void Method5()
        {
            //new <1s   实例对象实际上还未进行内存操作
            var excel = new ExcelPackage(new FileInfo(Shard.TinyFile));

            //read <1s
            var last = excel.Workbook.Worksheets["0121X14610010"];

            var data = last?.Cells["E10"].Value;
            Console.WriteLine(data);
        }

        void Method6()
        {
            //new 5s
            var excel = new ExcelPackage(new FileInfo(Shard.MiddlingFile));

            var t = excel.Workbook;

            //read 55s
            var last = excel.Workbook.Worksheets["TestRecord"];
            var data = last?.Cells["E10"].Value;
            Console.WriteLine(data);
        }

        /// <summary>
        ///     Magicodes.IE 对较大文件的支持还行,内存会飙一段时间
        /// </summary>
        void Method7()
        {
            //new 36s
            var excel = new ExcelPackage(new FileInfo(Shard.LargeFile));

            //read 320s
            var last = excel.Workbook.Worksheets["TestRecord"];
            var data = last?.Cells["E10"].Value;
            Console.WriteLine(data);
        }

        /// <summary>
        ///     也许非MULTI-SHEET的情况会快一些 = =  虽然扔了错误 Array dimensions exceeded supported range. 👵
        /// </summary>
        void Method8()
        {
            //var excel = new ExcelPackage(new FileInfo(Shard.LargeFile2)); // Array dimensions exceeded supported range.
            //var excel = new ExcelPackage(new FileInfo(Shard.LargeFile3));   // Array dimensions exceeded supported range.
            //var excel = new ExcelPackage(new FileInfo(Shard.LargeFile4));   // Array dimensions exceeded supported range.

            //total 159s
            var excel = new ExcelPackage(new FileInfo(Shard.LargeFile5));

            var last = excel.Workbook.Worksheets["TestRecord"];
            var data = last?.Cells["E10"].Value;
            Console.WriteLine(data);
        }

        void Method9()
        {
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var file = @"D:\Code\dotnet\demo\magicodes-demo\size-demo\2022-07-09@18-24-56_32_PACK_重卡_0FNPB122300101C360000013_020Deg_SOC累计误差_Part0.xlsx";
            var excel = new ExcelPackage(new FileInfo(file));
            var last = excel.Workbook.Worksheets["TestRecord"];
            var data = last?.Cells["E10"].Value;
            Console.WriteLine(data);

        }

        /// <summary>
        ///     microsoft.open-xml-sdk 是巨硬自己的规范,也是第三方的基础,简单来说就是xml存取
        /// </summary>
        void Method10()
        {
            //total <1s
            using (var doc = SpreadsheetDocument.Open(Shard.TinyFile, false))
            {
                Sheets? sheets = doc.WorkbookPart?.Workbook.GetFirstChild<Sheets>();
                Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "0121X14610010");

                var name = sheet.Name;
                var id = sheet.Id;
                var sheetId = sheet.SheetId;

                WorksheetPart worksheetpart = (WorksheetPart)doc.WorkbookPart.GetPartById(id);
                Worksheet? worksheet = worksheetpart?.Worksheet;

                var sheetDimension = worksheet.SheetDimension;

                SheetData? sheetData = worksheet.GetFirstChild<SheetData>();

                var count = sheetData?.ChildElements.Count;
                Row? row = (Row)sheetData?.ChildElements[(count % 10 ?? 1)];
                var rows = sheetData?.ChildElements.OfType<Row>();
                Console.WriteLine(rows.Count());

                var count2 = row?.ChildElements.Count;
                Cell? cell = (Cell)row?.ChildElements[count2 % 10 ?? 1];

                double t = 0;
                cell.CellValue?.TryGetDouble(out t);
                Console.WriteLine(cell.CellValue?.InnerText);

            }
        }

        /// <summary>
        ///     microsoft.open-xml-sdk 对中等大小的文件读取超过预期,甚至不如EPPlus,猜测原因内部对xml的读取是加载内存
        ///     但注意:它的内存消耗远低于EPPlus,缓慢上涨到90再缓慢降低周而复始
        /// </summary>
        void Method11()
        {
            //total 232s
            using (var doc = SpreadsheetDocument.Open(Shard.MiddlingFile, false))
            {
                Sheets? sheets = doc.WorkbookPart?.Workbook.GetFirstChild<Sheets>();
                Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "TestRecord");

                var name = sheet.Name;
                var id = sheet.Id;
                var sheetId = sheet.SheetId;

                WorksheetPart worksheetpart = (WorksheetPart)doc.WorkbookPart.GetPartById(id);
                Worksheet? worksheet = worksheetpart?.Worksheet;

                var sheetDimension = worksheet.SheetDimension;

                SheetData? sheetData = worksheet.GetFirstChild<SheetData>();

                var count = sheetData?.ChildElements.Count;
                Row? row = (Row)sheetData?.ChildElements[(count % 10 ?? 1)];
                //var rows = sheetData?.ChildElements.OfType<Row>();
                //Console.WriteLine(rows.Count());

                var count2 = row?.ChildElements.Count;
                Cell? cell = (Cell)row?.ChildElements[count2 % 10 ?? 1];

                double t = 0;
                cell.CellValue?.TryGetDouble(out t);
                Console.WriteLine(cell.CellValue?.InnerText);
            }
        }

        /// <summary>
        ///     比较对xml的处理,EPPlus明显是已经对XML结构作了处理,猜测是如x-element之类的分片去读
        ///         既然它行,那我也行
        /// </summary>
        void Method20()
        {
            var doc = SpreadsheetDocument.Open(Shard.MiddlingFile, false);

            Sheets? sheets = doc.WorkbookPart?.Workbook.GetFirstChild<Sheets>();
            //Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "0121X14610010");
            Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "TestRecord");
            WorksheetPart worksheetpart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);

            var stream = worksheetpart.GetStream(FileMode.Open, FileAccess.Read);

            //Worksheet? worksheet = worksheetpart?.Worksheet;  //todo 这里做了序列化,有问题
            //var sdkXml = worksheet.OuterXml;

            //var fileStream = new FileStream("temp", FileMode.OpenOrCreate);
            //var fileStream = new MemoryStream();
            //fileStream.Write(Encoding.UTF8.GetBytes(sdkXml));
            //fileStream.Seek(0, SeekOrigin.Begin);

            //30s
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XElement xeRow = null;
                XElement xeCol = null;
                XElement xeValue = null;

                int i = 0;

                var values = new List<KeyValuePair<string, string>>();

                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                        continue;

                    if (reader.Name == "x:sheetData" || reader.Name == "sheetData")
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "x:row" || reader.Name == "row")
                            {
                                xeRow = XElement.ReadFrom(reader) as XElement;

                                while (reader.Read())
                                {
                                    if (reader.Name == "x:c" || reader.Name == "c")
                                    {
                                        xeCol = XElement.ReadFrom(reader) as XElement;

                                        while (reader.Read())
                                        {
                                            if (reader.Name == "x:v" || reader.Name == "v")
                                            {
                                                i++;

                                                if (i % 1000 == 0)
                                                {
                                                    Console.WriteLine(i);
                                                }

                                                xeValue = XElement.ReadFrom(reader) as XElement;

                                                //values.Add(new KeyValuePair<string, string>(xeCol.Attribute("r").Value, xeValue.Value));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///    证实了这么处理可以绕开存在的多SHEET的读取问题
        /// </summary>
        void Method21()
        {
            var doc = SpreadsheetDocument.Open(Shard.LargeFile, false);

            Sheets? sheets = doc.WorkbookPart?.Workbook.GetFirstChild<Sheets>();
            //Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "0121X14610010");
            Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "TestRecord");
            WorksheetPart worksheetpart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);

            var stream = worksheetpart.GetStream(FileMode.Open, FileAccess.Read);

            //Worksheet? worksheet = worksheetpart?.Worksheet;  //todo 这里做了序列化,有问题
            //var sdkXml = worksheet.OuterXml;

            //var fileStream = new FileStream("temp", FileMode.OpenOrCreate);
            //var fileStream = new MemoryStream();
            //fileStream.Write(Encoding.UTF8.GetBytes(sdkXml));
            //fileStream.Seek(0, SeekOrigin.Begin);

            //30s
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XElement xeRow = null;
                XElement xeCol = null;
                XElement xeValue = null;

                int i = 0;

                var values = new List<KeyValuePair<string, string>>();

                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                        continue;

                    if (reader.Name == "x:sheetData" || reader.Name == "sheetData")
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "x:row" || reader.Name == "row")
                            {
                                xeRow = XElement.ReadFrom(reader) as XElement;

                                while (reader.Read())
                                {
                                    if (reader.Name == "x:c" || reader.Name == "c")
                                    {
                                        xeCol = XElement.ReadFrom(reader) as XElement;

                                        while (reader.Read())
                                        {
                                            if (reader.Name == "x:v" || reader.Name == "v")
                                            {
                                                i++;

                                                if (i % 1000 == 0)
                                                {
                                                    Console.WriteLine(i);
                                                }

                                                xeValue = XElement.ReadFrom(reader) as XElement;

                                                //values.Add(new KeyValuePair<string, string>(xeCol.Attribute("r").Value, xeValue.Value));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///    增加测试数量
        /// </summary>
        void Method22()
        {
            //130s
            var doc = SpreadsheetDocument.Open(Shard.LargeFile2, false);

            Sheets? sheets = doc.WorkbookPart?.Workbook.GetFirstChild<Sheets>();
            //Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "0121X14610010");
            Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "TestRecord");
            WorksheetPart worksheetpart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);

            var stream = worksheetpart.GetStream(FileMode.Open, FileAccess.Read);

            //Worksheet? worksheet = worksheetpart?.Worksheet;  //todo 这里做了序列化,有问题
            //var sdkXml = worksheet.OuterXml;

            //var fileStream = new FileStream("temp", FileMode.OpenOrCreate);
            //var fileStream = new MemoryStream();
            //fileStream.Write(Encoding.UTF8.GetBytes(sdkXml));
            //fileStream.Seek(0, SeekOrigin.Begin);

            //30s
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XElement xeRow = null;
                XElement xeCol = null;
                XElement xeValue = null;

                int i = 0;

                var values = new List<KeyValuePair<string, string>>();

                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                        continue;

                    if (reader.Name == "x:sheetData" || reader.Name == "sheetData")
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "x:row" || reader.Name == "row")
                            {
                                xeRow = XElement.ReadFrom(reader) as XElement;

                                while (reader.Read())
                                {
                                    if (reader.Name == "x:c" || reader.Name == "c")
                                    {
                                        xeCol = XElement.ReadFrom(reader) as XElement;

                                        while (reader.Read())
                                        {
                                            if (reader.Name == "x:v" || reader.Name == "v")
                                            {
                                                i++;

                                                if (i % 1000 == 0)
                                                {
                                                    Console.WriteLine(i);
                                                }

                                                xeValue = XElement.ReadFrom(reader) as XElement;

                                                //values.Add(new KeyValuePair<string, string>(xeCol.Attribute("r").Value, xeValue.Value));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void Method23()
        {
            void Method20()
            {
                var doc = SpreadsheetDocument.Open(Shard.TinyFile, false);

                Sheets? sheets = doc.WorkbookPart?.Workbook.GetFirstChild<Sheets>();
                //Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "0121X14610010");
                Sheet? sheet = sheets?.Elements<Sheet>().FirstOrDefault(o => o.Name == "TestRecord");
                WorksheetPart worksheetpart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);

                var stream = worksheetpart.GetStream(FileMode.Open, FileAccess.Read);

                //Worksheet? worksheet = worksheetpart?.Worksheet;  //todo 这里做了序列化,有问题
                //var sdkXml = worksheet.OuterXml;

                //var fileStream = new FileStream("temp", FileMode.OpenOrCreate);
                //var fileStream = new MemoryStream();
                //fileStream.Write(Encoding.UTF8.GetBytes(sdkXml));
                //fileStream.Seek(0, SeekOrigin.Begin);

                //30s
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    XElement xeRow = null;
                    XElement xeCol = null;
                    XElement xeValue = null;

                    int i = 0;

                    var values = new List<KeyValuePair<string, string>>();

                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element)
                            continue;

                        if (reader.Name == "x:sheetData" || reader.Name == "sheetData")
                        {
                            while (reader.Read())
                            {
                                if (reader.Name == "x:row" || reader.Name == "row")
                                {
                                    xeRow = XElement.ReadFrom(reader) as XElement;

                                    while (reader.Read())
                                    {
                                        if (reader.Name == "x:c" || reader.Name == "c")
                                        {
                                            xeCol = XElement.ReadFrom(reader) as XElement;

                                            while (reader.Read())
                                            {
                                                if (reader.Name == "x:v" || reader.Name == "v")
                                                {
                                                    i++;

                                                    if (i % 1000 == 0)
                                                    {
                                                        Console.WriteLine(i);
                                                    }

                                                    xeValue = XElement.ReadFrom(reader) as XElement;

                                                    //values.Add(new KeyValuePair<string, string>(xeCol.Attribute("r").Value, xeValue.Value));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /*
         * 微软对大xml的建议是流式转换,LINQ TO SQL
         */
        //XStreamingElement root = new XStreamingElement("Root",
        //    from el in StreamCustomerItem("Source.xml")
        //    select new XElement("Item",
        //        new XElement("Customer", (string)el.Parent.Element("Name")),
        //        new XElement(el.Element("Key"))
        //    )
        //);
        //root.Save("Test.xml");
        //Console.WriteLine(File.ReadAllText("Test.xml"));

        //static IEnumerable<XElement> StreamCustomerItem(string uri)
        //{
        //    using (XmlReader reader = XmlReader.Create(uri))
        //    {
        //        XElement name = null;
        //        XElement item = null;

        //        reader.MoveToContent();

        //        // Parse the file, save header information when encountered, and yield the
        //        // Item XElement objects as they're created.
        //        // Loop through Customer elements.
        //        while (reader.Read())
        //        {
        //            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Customer")
        //            {
        //                // move to Name element
        //                while (reader.Read())
        //                {
        //                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Name")
        //                    {
        //                        name = XElement.ReadFrom(reader) as XElement;
        //                        break;
        //                    }
        //                }

        //                // loop through Item elements
        //                while (reader.Read())
        //                {
        //                    if (reader.NodeType == XmlNodeType.EndElement)
        //                        break;
        //                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
        //                    {
        //                        item = XElement.ReadFrom(reader) as XElement;
        //                        if (item != null)
        //                        {
        //                            XElement tempRoot = new XElement("Root", new XElement(name));
        //                            tempRoot.Add(item);
        //                            yield return item;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
