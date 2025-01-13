using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Collections.Generic;
using System;
using System.IO;
using OfficeOpenXml.Drawing;

namespace demo
{
    /// <summary>
    ///     尝试ep-plus的画表格
    /// </summary>
    internal class ExcelDrawing
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

        void Method1()
        {
            using (var excel = new ExcelPackage(new FileInfo(Shard.TinyFile)))
            {
                var sheet1 = excel.Workbook.Worksheets["Summary"];
                var sheet2 = excel.Workbook.Worksheets["0121X14610010"];

                var chartInfo = new ChartInfo()
                {
                    WorkSheet = sheet1,
                    Title = "图表标题",
                    XTitle = "X轴标题",
                    YTitle = "Y轴标题",
                    ChartAddress = sheet1.Cells[100, 2, 100, 2],
                    ChartSources = new List<ChartDataSourceInfo>()
                    {
                        new ChartDataSourceInfo()
                        {
                            Title="系列1",
                            XSource = sheet1.Cells[89, 2, 99, 2],
                            YSource = sheet1.Cells[89, 3, 99, 3],
                        },
                        new ChartDataSourceInfo()
                        {
                            Title="系列2",
                            XSource = sheet1.Cells[89, 2, 99, 2],
                            YSource = sheet1.Cells[89, 4, 99, 4],
                        }
                    }
                };

                var chart = chartInfo.WorkSheet.Drawings.AddChart(chartInfo.Title, eChartType.Line);
                chart.Title.Text = chartInfo.Title;
                chart.SetPosition(chartInfo.ChartAddress.Start.Row, 0, chartInfo.ChartAddress.Start.Column, 0);
                chart.SetSize(800, 400);
                chart.Legend.Position = eLegendPosition.Right;
                chart.Legend.Add();


                chart.XAxis.Title.Text = "CNC";
                chart.XAxis.Title.Font.Size = 10;

                chart.YAxis.Title.Text = "Value";
                chart.YAxis.Title.Font.Size = 10;

                chart.DisplayBlanksAs = eDisplayBlanksAs.Gap;

                //chart.ShowHiddenData = true;
                chart.XAxis.MinorUnit = 2;
                //chart.DataLabel.ShowPercent = true;
                chart.YAxis.MinValue = 3d;

                chart.YAxis.RemoveGridlines(true, true);

                foreach (var item in chartInfo.ChartSources)
                {
                    var chartSeries = chart.Series.Add(item.YSource, item.XSource);
                    chartSeries.Header = item.Title;
                }

                excel.Workbook.View.ActiveTab = 0;
                excel.SaveAs(new FileInfo(Shard.GetNewFileOutDir()));
            }
        }

        void Method2()
        {
            using (var excel = new ExcelPackage(new FileInfo(Shard.TinyFile)))
            {
                var sheet1 = excel.Workbook.Worksheets["Summary"];
                var sheet2 = excel.Workbook.Worksheets["0121X14610010"];
                var sheet3 = excel.Workbook.Worksheets["0121X14140002"];

                var chartInfo = new ChartInfo()
                {
                    WorkSheet = sheet1,
                    Title = "图表标题",
                    XTitle = "X轴标题",
                    YTitle = "Y轴标题",
                    ChartAddress = sheet1.Cells[100, 2, 100, 2],
                    ChartSources = new List<ChartDataSourceInfo>()
                    {
                        new ChartDataSourceInfo()
                        {
                            Title = sheet2.Name,
                            XSource = sheet2.Cells[3, 2, sheet2.Dimension.End.Row, 2],
                            YSource = sheet2.Cells[3, 3, sheet2.Dimension.End.Row, 3],
                        },
                        new ChartDataSourceInfo()
                        {
                            Title = sheet3.Name,
                            XSource = sheet3.Cells[3, 2, sheet3.Dimension.End.Row, 2],
                            YSource = sheet3.Cells[3, 3, sheet3.Dimension.End.Row, 3],
                        }
                    }
                };

                var chart = chartInfo.WorkSheet.Drawings.AddChart(chartInfo.Title, eChartType.XYScatterSmoothNoMarkers);
                chart.Title.Text = chartInfo.Title;
                chart.SetPosition(chartInfo.ChartAddress.Start.Row, 0, chartInfo.ChartAddress.Start.Column, 0);
                chart.SetSize(800, 400);
                chart.Legend.Position = eLegendPosition.Right;
                chart.Legend.Add();

                chart.XAxis.Title.Text = "CNC";
                chart.XAxis.Title.Font.Size = 10;

                chart.YAxis.Title.Text = "Value";
                chart.YAxis.Title.Font.Size = 10;

                chart.DisplayBlanksAs = eDisplayBlanksAs.Gap;

                //chart.ShowHiddenData = true;
                chart.XAxis.MinorUnit = 2;
                //chart.DataLabel.ShowPercent = true;
                chart.YAxis.MinValue = 3d;

                chart.YAxis.RemoveGridlines(true, true);
                chart.XAxis.RemoveGridlines(true, true);

                foreach (var item in chartInfo.ChartSources)
                {
                    var chartSeries = chart.Series.Add(item.YSource, item.XSource);
                    chartSeries.Header = item.Title;
                }

                excel.SaveAs(new FileInfo(Shard.GetNewFileOutDir()));
            }
        }

        void Method3()
        {
            using (var excel = new ExcelPackage(new FileInfo(Shard.DrawFile)))
            {
                var sheet1 = excel.Workbook.Worksheets["Summary"];

                foreach (var drawing in sheet1.Drawings)
                {
                    if (drawing is ExcelScatterChart chart)
                    {
                        foreach (var series in chart.Series)
                        {
                            if (series is ExcelScatterChartSerie excelChartSerie)
                            {
                                Console.WriteLine(excelChartSerie.HeaderAddress);
                                Console.WriteLine(excelChartSerie.XSeries);
                                Console.WriteLine(excelChartSerie.Series);

                            }
                        }
                    }
                }
            }
        }

        void Method4()
        {
            string file1 = @"D:\Code\dotnet\demo\Excel\T22111211C-RateCC-CAP&DCR01&DC-Rate01&cycle01.xlsx";
            string file2 = @"D:\Code\dotnet\demo\Excel\T22111211C-RateCC-CAP&DCR01&DC-Rate01.xlsx";
            var fileOut = Path.Combine(@"D:\Code\dotnet\demo\Excel", $"out -{DateTime.Now:yyMMddHHmmss}.xlsx");

            using (var excel = new ExcelPackage(new FileInfo(file2)))
            {
                var sheet1 = excel.Workbook.Worksheets["Summary"];

                var chartInfo = new ChartInfo()
                {
                    WorkSheet = sheet1,
                    Title = "图表标题",
                    XTitle = "X轴标题",
                    YTitle = "Y轴标题",
                    ChartAddress = sheet1.Cells[60, 2, 60, 20],
                };

                for (int row = 5; row <= 17; row++)
                {
                    var source = new ChartDataSourceInfo
                    {
                        Title = sheet1.Cells[row, 3].Text,
                        XSource = sheet1.Cells[4, sheet1.Cells["LN1"].Start.Column, 4, sheet1.Cells["XW1"].Start.Column],
                        YSource = sheet1.Cells[row, sheet1.Cells["LN1"].Start.Column, row, sheet1.Cells["XW1"].Start.Column],
                    };
                    chartInfo.ChartSources.Add(source);
                }

                var chart = chartInfo.WorkSheet.Drawings.AddChart(chartInfo.Title, eChartType.LineMarkersStacked);
                chart.Title.Text = chartInfo.Title;
                chart.SetPosition(chartInfo.ChartAddress.Start.Row, 0, chartInfo.ChartAddress.Start.Column, 0);
                chart.SetSize(800, 400);
                chart.Legend.Position = eLegendPosition.Right;
                chart.Legend.Add();

                chart.XAxis.Title.Text = "CNC";
                chart.XAxis.Title.Font.Size = 10;

                chart.YAxis.Title.Text = "Value";
                chart.YAxis.Title.Font.Size = 10;

                chart.DisplayBlanksAs = eDisplayBlanksAs.Gap;

                //chart.ShowHiddenData = true;
                chart.XAxis.MinorUnit = 2;
                //chart.DataLabel.ShowPercent = true;
                chart.YAxis.MinValue = 3d;

                chart.YAxis.RemoveGridlines(true, true);
                chart.XAxis.RemoveGridlines(true, true);

                foreach (var item in chartInfo.ChartSources)
                {
                    var chartSeries = chart.Series.Add(item.YSource, item.XSource);
                    chartSeries.Header = item.Title;
                }

                excel.SaveAs(new FileInfo(fileOut));
            }
        }

        void Method5()
        {
            string file1 = @"D:\Code\dotnet\demo\Excel\T22111211C-RateCC-CAP&DCR01&DC-Rate01&cycle01.xlsx";
            string file2 = @"D:\Code\dotnet\demo\Excel\T22111211C-RateCC-CAP&DCR01&DC-Rate01.xlsx";
            var fileOut = Path.Combine(@"D:\Code\dotnet\demo\Excel", $"out -{DateTime.Now:yyMMddHHmmss}.xlsx");

            using (var excel = new ExcelPackage(new FileInfo(file1)))
            {
                var sheet1 = excel.Workbook.Worksheets["Summary"];

                var chartInfo = new ChartInfo()
                {
                    WorkSheet = sheet1,
                    Title = "图表标题",
                    XTitle = "X轴标题",
                    YTitle = "Y轴标题",
                    ChartAddress = sheet1.Cells[60, 2, 60, 20],
                };

                for (int row = 5; row <= 9; row++)
                {
                    var source = new ChartDataSourceInfo
                    {
                        Title = sheet1.Cells[row, 3].Text,
                        //XSource = sheet1.Cells[4, sheet1.Cells["BCU1"].Start.Column, 4, sheet1.Cells["DGK1"].Start.Column],
                        //YSource = sheet1.Cells[row, sheet1.Cells["BCU1"].Start.Column, row, sheet1.Cells["DGK1"].Start.Column],
                        XSource = sheet1.Cells[4, sheet1.Cells["BCU1"].Start.Column, 4, sheet1.Cells["BGK1"].Start.Column],
                        YSource = sheet1.Cells[row, sheet1.Cells["BCU1"].Start.Column, row, sheet1.Cells["BGK1"].Start.Column],
                    };
                    chartInfo.ChartSources.Add(source);
                }

                var chart = chartInfo.WorkSheet.Drawings.AddChart(chartInfo.Title, eChartType.LineMarkersStacked);
                chart.Title.Text = chartInfo.Title;
                chart.SetPosition(chartInfo.ChartAddress.Start.Row, 0, chartInfo.ChartAddress.Start.Column, 0);
                chart.SetSize(800, 400);
                chart.Legend.Position = eLegendPosition.Right;
                chart.Legend.Add();

                chart.XAxis.Title.Text = "CNC";
                chart.XAxis.Title.Font.Size = 10;

                chart.YAxis.Title.Text = "Value";
                chart.YAxis.Title.Font.Size = 10;

                chart.DisplayBlanksAs = eDisplayBlanksAs.Gap;

                //chart.ShowHiddenData = true;
                chart.XAxis.MinorUnit = 2;
                //chart.DataLabel.ShowPercent = true;
                chart.YAxis.MinValue = 3d;

                chart.YAxis.RemoveGridlines(true, true);
                chart.XAxis.RemoveGridlines(true, true);

                foreach (var item in chartInfo.ChartSources)
                {
                    var chartSeries = chart.Series.Add(item.YSource, item.XSource);
                    chartSeries.Header = item.Title;
                }

                excel.SaveAs(new FileInfo(fileOut));
            }
        }

        void Method6()
        {
            string file = @"D:\Code\dotnet\demo\Excel\T22071014C-RateCC-CC-Rate - 副本.xlsx";
            var fileOut = Path.Combine(@"D:\Code\dotnet\demo\Excel", $"out -{DateTime.Now:yyMMddHHmmss}.xlsx");

            using (var excel = new ExcelPackage(new FileInfo(file)))
            {
                var sheet1 = excel.Workbook.Worksheets["Summary"];

                var chartInfo = new ChartInfo()
                {
                    WorkSheet = sheet1,
                    Title = "图表标题",
                    XTitle = "X轴标题",
                    YTitle = "Y轴标题",
                    ChartAddress = sheet1.Cells[30, 2, 30, 20],
                };

                for (int row = 5; row <= 7; row++)
                {
                    var source = new ChartDataSourceInfo
                    {
                        Title = sheet1.Cells[row, 3].Text,
                        //XSource = sheet1.Cells[4, sheet1.Cells["BCU1"].Start.Column, 4, sheet1.Cells["DGK1"].Start.Column],
                        //YSource = sheet1.Cells[row, sheet1.Cells["BCU1"].Start.Column, row, sheet1.Cells["DGK1"].Start.Column],
                        XSource = sheet1.Cells[4, sheet1.Cells["W1"].Start.Column, 4, sheet1.Cells["AO1"].Start.Column],
                        YSource = sheet1.Cells[row, sheet1.Cells["W1"].Start.Column, row, sheet1.Cells["AO1"].Start.Column],
                    };
                    chartInfo.ChartSources.Add(source);
                }

                var chart = chartInfo.WorkSheet.Drawings.AddChart(chartInfo.Title, eChartType.Line);
                chart.Title.Text = chartInfo.Title;
                chart.SetPosition(chartInfo.ChartAddress.Start.Row, 0, chartInfo.ChartAddress.Start.Column, 0);
                chart.SetSize(800, 400);
                chart.Legend.Position = eLegendPosition.Right;
                chart.Legend.Add();

                chart.XAxis.Title.Text = "CNC";
                chart.XAxis.Title.Font.Size = 10;

                chart.YAxis.Title.Text = "Value";
                chart.YAxis.Title.Font.Size = 10;

                chart.DisplayBlanksAs = eDisplayBlanksAs.Gap;

                //chart.ShowHiddenData = true;
                //chart.XAxis.MinorUnit = 2;
                //chart.DataLabel.ShowPercent = true;
                //chart.YAxis.MinValue = 3d;

                chart.YAxis.RemoveGridlines(true, true);
                chart.XAxis.RemoveGridlines(true, true);

                foreach (var item in chartInfo.ChartSources)
                {
                    var chartSeries = chart.Series.Add(item.YSource, item.XSource);
                    chartSeries.Header = item.Title;

                    if (chartSeries is ExcelLineChartSerie lcs)
                    {
                        lcs.Smooth = true;
                    }
                }

                excel.SaveAs(new FileInfo(fileOut));
            }
        }
    }

    public class ChartInfo
    {
        public ExcelWorksheet WorkSheet { get; set; }

        public string Title { get; set; }

        public ExcelRange ChartAddress { get; set; }

        public List<ChartDataSourceInfo> ChartSources { get; set; } = new List<ChartDataSourceInfo>();

        public string XTitle { get; set; }

        public string YTitle { get; set; }
    }

    public class ChartDataSourceInfo
    {
        public string Title { get; set; }

        public ExcelRange XSource { get; set; }

        public ExcelRange YSource { get; set; }
    }
}