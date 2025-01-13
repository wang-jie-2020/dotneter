using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;

namespace demo
{
    internal class ExcelWriting
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
        ///     OPEN-XML OpenXmlWriter
        /// </summary>
        void Method1()
        {
            //5w rows - 100w cells = total 3s
            //注意:这里的string值是未考虑shard的
            SpreadsheetDocument xl = SpreadsheetDocument.Create(Shard.GetNewFileOutDir(), SpreadsheetDocumentType.Workbook);

            xl.AddWorkbookPart();
            WorksheetPart wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();

            OpenXmlWriter oxw = OpenXmlWriter.Create(wsp);
            oxw.WriteStartElement(new Worksheet());
            oxw.WriteStartElement(new SheetData());

            for (int i = 1; i <= 50000; ++i)
            {
                List<OpenXmlAttribute> oxa = new List<OpenXmlAttribute>();
                oxa.Add(new OpenXmlAttribute("r", null, i.ToString()));

                oxw.WriteStartElement(new Row(), oxa);

                for (int j = 1; j <= 20; ++j)
                {
                    oxa = new List<OpenXmlAttribute>();
                    oxa.Add(new OpenXmlAttribute("t", null, "str"));
                    oxw.WriteStartElement(new Cell(), oxa);
                    oxw.WriteElement(new DocumentFormat.OpenXml.Spreadsheet.CellValue(string.Format("R{0}C{1}", i, j)));
                    oxw.WriteEndElement();
                }

                oxw.WriteEndElement();
            }

            oxw.WriteEndElement();
            oxw.WriteEndElement();
            oxw.Close();

            oxw = OpenXmlWriter.Create(xl.WorkbookPart);
            oxw.WriteStartElement(new Workbook());
            oxw.WriteStartElement(new Sheets());
            oxw.WriteElement(new Sheet()
            {
                Name = "Sheet1",
                SheetId = 1,
                Id = xl.WorkbookPart.GetIdOfPart(wsp)
            });

            oxw.WriteEndElement();
            oxw.WriteEndElement();
            oxw.Close();

            xl.Close();
        }

        /// <summary>
        ///     OPEN-XML OpenXmlWriter
        /// </summary>
        void Method2()
        {
            //50w rows - 1000w cells total 22s
            //注意:这里的string值是未考虑shard的
            SpreadsheetDocument xl = SpreadsheetDocument.Create(Shard.GetNewFileOutDir(), SpreadsheetDocumentType.Workbook);

            xl.AddWorkbookPart();
            WorksheetPart wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();

            OpenXmlWriter oxw = OpenXmlWriter.Create(wsp);
            oxw.WriteStartElement(new Worksheet());
            oxw.WriteStartElement(new SheetData());

            for (int i = 1; i <= 500000; ++i)
            {
                List<OpenXmlAttribute> oxa = new List<OpenXmlAttribute>();
                oxa.Add(new OpenXmlAttribute("r", null, i.ToString()));

                oxw.WriteStartElement(new Row(), oxa);

                for (int j = 1; j <= 20; ++j)
                {
                    oxa = new List<OpenXmlAttribute>();
                    oxa.Add(new OpenXmlAttribute("t", null, "str"));
                    oxw.WriteStartElement(new Cell(), oxa);
                    oxw.WriteElement(new DocumentFormat.OpenXml.Spreadsheet.CellValue(string.Format("R{0}C{1}", i, j)));
                    oxw.WriteEndElement();
                }

                oxw.WriteEndElement();
            }

            oxw.WriteEndElement();
            oxw.WriteEndElement();
            oxw.Close();

            oxw = OpenXmlWriter.Create(xl.WorkbookPart);
            oxw.WriteStartElement(new Workbook());
            oxw.WriteStartElement(new Sheets());
            oxw.WriteElement(new Sheet()
            {
                Name = "Sheet1",
                SheetId = 1,
                Id = xl.WorkbookPart.GetIdOfPart(wsp)
            });

            oxw.WriteEndElement();
            oxw.WriteEndElement();
            oxw.Close();

            xl.Close();
        }

        /// <summary>
        ///     OPEN-XML +Sheet
        /// </summary>
        void Method3()
        {
            using (var spreadSheet = SpreadsheetDocument.Open(Shard.FileOut, true))
            {

                WorksheetPart newWorksheetPart = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet(new SheetData());
                Sheets sheets = spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = spreadSheet.WorkbookPart.GetIdOfPart(newWorksheetPart);

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                string sheetName = "mySheet" + sheetId;
                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
                sheets.Append(sheet);
            }
        }

        /// <summary>
        ///     OPEN-XML +Sheet
        /// </summary>
        void Method4()
        {
            //注意:这里的string值是未考虑shard的
            using (var spreadSheet = SpreadsheetDocument.Open(Shard.FileOut, true))
            {
                WorksheetPart newWorksheetPart = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();

                OpenXmlWriter oxw = OpenXmlWriter.Create(newWorksheetPart);
                oxw.WriteStartElement(new Worksheet());
                oxw.WriteStartElement(new SheetData());

                for (int i = 1; i <= 50000; ++i)
                {
                    List<OpenXmlAttribute> oxa = new List<OpenXmlAttribute>();
                    oxa.Add(new OpenXmlAttribute("r", null, i.ToString()));

                    oxw.WriteStartElement(new Row(), oxa);

                    for (int j = 1; j <= 20; ++j)
                    {
                        oxa = new List<OpenXmlAttribute>();
                        oxa.Add(new OpenXmlAttribute("t", null, "str"));
                        oxw.WriteStartElement(new Cell(), oxa);
                        oxw.WriteElement(new DocumentFormat.OpenXml.Spreadsheet.CellValue(string.Format("R{0}C{1}", i, j)));
                        oxw.WriteEndElement();
                    }

                    oxw.WriteEndElement();
                }

                oxw.WriteEndElement();
                oxw.WriteEndElement();
                oxw.Close();

                Sheets sheets = spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = spreadSheet.WorkbookPart.GetIdOfPart(newWorksheetPart);

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                string sheetName = "mySheet" + sheetId;
                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
                sheets.Append(sheet);
            }
        }

        /// <summary>
        ///     已有的大表格是否有影响?  没有
        /// </summary>
        void Method5()
        {
            using (var spreadSheet = SpreadsheetDocument.Open(Shard.MiddlingFile, true))
            {
                WorksheetPart newWorksheetPart = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();

                OpenXmlWriter oxw = OpenXmlWriter.Create(newWorksheetPart);
                oxw.WriteStartElement(new Worksheet());
                oxw.WriteStartElement(new SheetData());

                for (int i = 1; i <= 50000; ++i)
                {
                    List<OpenXmlAttribute> oxa = new List<OpenXmlAttribute>();
                    oxa.Add(new OpenXmlAttribute("r", null, i.ToString()));

                    oxw.WriteStartElement(new Row(), oxa);

                    for (int j = 1; j <= 20; ++j)
                    {
                        oxa = new List<OpenXmlAttribute>();
                        oxa.Add(new OpenXmlAttribute("t", null, "str"));
                        oxw.WriteStartElement(new Cell(), oxa);
                        oxw.WriteElement(new DocumentFormat.OpenXml.Spreadsheet.CellValue(string.Format("R{0}C{1}", i, j)));
                        oxw.WriteEndElement();
                    }

                    oxw.WriteEndElement();
                }

                oxw.WriteEndElement();
                oxw.WriteEndElement();
                oxw.Close();

                Sheets sheets = spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = spreadSheet.WorkbookPart.GetIdOfPart(newWorksheetPart);

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                string sheetName = "mySheet" + sheetId;
                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
                sheets.Append(sheet);
            }
        }

        void Method10()
        {
            //5w rows - 100w cells = total 5s
            using (var excel = new ExcelPackage(new FileInfo(Shard.GetNewFileOutDir())))
            {
                var sheet = excel.Workbook.Worksheets.Add("test");


                for (int i = 1; i <= 50000; ++i)
                {
                    for (int j = 1; j <= 20; ++j)
                    {
                        var cell = sheet.Cells[i, j];
                        cell.Value = $"R{i}C{j}";
                    }
                }

                excel.Save();
            }
        }

        void Method11()
        {
            //50w rows - 1000w cells total 47s
            using (var excel = new ExcelPackage(new FileInfo(Shard.GetNewFileOutDir())))
            {
                var sheet = excel.Workbook.Worksheets.Add("test");


                for (int i = 1; i <= 500000; ++i)
                {
                    for (int j = 1; j <= 20; ++j)
                    {
                        var cell = sheet.Cells[i, j];
                        cell.Value = $"R{i}C{j}";
                    }
                }

                excel.Save();
            }
        }

        /// <summary>
        ///     已有的大表格是否有影响?  有
        /// </summary>
        void Method12()
        {
            //5w rows - 100w cells = total 161s
            using (var excel = new ExcelPackage(new FileInfo(Shard.MiddlingFile)))
            {
                var sheet = excel.Workbook.Worksheets.Add("test");

                for (int i = 1; i <= 50000; ++i)
                {
                    for (int j = 1; j <= 20; ++j)
                    {
                        var cell = sheet.Cells[i, j];
                        cell.Value = $"R{i}C{j}";
                    }
                }

                excel.Save();
            }
        }
    }
}
