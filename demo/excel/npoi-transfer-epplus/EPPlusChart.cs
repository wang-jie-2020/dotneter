using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace magicodes_epplus_draw_test
{
    /// <summary>
    ///     尝试ep-plus的画表格
    /// </summary>
    public class EPPlusChart
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
            string file = @"D:\Code\dotnet\demo\Excel\T22111211C-RateCC-CAP&DCR01&DC-Rate01 - 副本.xlsx";
            var fileOut = Path.Combine(@"D:\Code\dotnet\demo\Excel", $"out -{DateTime.Now:yyMMddHHmmss}.xlsx");

            using (var excel = new ExcelPackage(new FileInfo(file)))
            {
                var sheet1 = excel.Workbook.Worksheets["Summary"];

                var chartInfo = new EPlusChartInfo()
                {
                    WorkSheet = sheet1,
                    Title = "图表标题",
                    XTitle = "X轴标题",
                    YTitle = "Y轴标题",
                    ChartAddress = sheet1.Cells[60, 2, 60, 20],
                };

                for (int row = 5; row <= 17; row++)
                {
                    var source = new EPlusChartDataSourceInfo
                    {
                        Title = sheet1.Cells[row, 3].Text,
                        XSource = sheet1.Cells[4, sheet1.Cells["LN1"].Start.Column, 4, sheet1.Cells["XW1"].Start.Column],
                        YSource = sheet1.Cells[row, sheet1.Cells["LN1"].Start.Column, row, sheet1.Cells["XW1"].Start.Column],
                    };
                    chartInfo.ChartSources.Add(source);
                }

                EPPlusExcelDrawerHelper.CreateLineChart(chartInfo);
                excel.SaveAs(new FileInfo(fileOut));
            }
        }

        void Method2()
        {
            string file = @"D:\Code\dotnet\demo\Excel\T22111211C-RateCC-CAP&DCR01&DC-Rate01&cycle01 - 副本.xlsx";
            var fileOut = Path.Combine(@"D:\Code\dotnet\demo\Excel", $"out -{DateTime.Now:yyMMddHHmmss}.xlsx");

            using (var excel = new ExcelPackage(new FileInfo(file)))
            {
                var sheet1 = excel.Workbook.Worksheets["Summary"];

                var chartInfo = new EPlusChartInfo()
                {
                    WorkSheet = sheet1,
                    Title = "图表标题1",
                    XTitle = "X轴标题",
                    YTitle = "Y轴标题",
                    ChartAddress = sheet1.Cells[60, 2, 60, 20],
                };

                for (int row = 5; row <= 9; row++)
                {
                    var source = new EPlusChartDataSourceInfo
                    {
                        Title = sheet1.Cells[row, 3].Text,
                        XSource = sheet1.Cells[4, sheet1.Cells["BCU1"].Start.Column, 4, sheet1.Cells["DGK1"].Start.Column],
                        YSource = sheet1.Cells[row, sheet1.Cells["BCU1"].Start.Column, row, sheet1.Cells["DGK1"].Start.Column],
                    };
                    chartInfo.ChartSources.Add(source);
                }

                EPPlusExcelDrawerHelper.CreateLineChart(chartInfo);
                excel.SaveAs(new FileInfo(fileOut));
            }
        }

        void Method3()
        {
            string file = @"D:\Code\dotnet\demo\Excel\T22071014C-RateCC-CC-Rate - 副本.xlsx";
            var fileOut = Path.Combine(@"D:\Code\dotnet\demo\Excel", $"out -{DateTime.Now:yyMMddHHmmss}.xlsx");

            using (var excel = new ExcelPackage(new FileInfo(file)))
            {
                var sheet1 = excel.Workbook.Worksheets["Summary"];

                {
                    var chartInfo = new EPlusChartInfo()
                    {
                        WorkSheet = sheet1,
                        Title = "图表标题",
                        XTitle = "X轴标题",
                        YTitle = "Y轴标题",
                        ChartAddress = sheet1.Cells[30, 2, 30, 20],
                    };

                    for (int row = 5; row <= 7; row++)
                    {
                        var source = new EPlusChartDataSourceInfo
                        {
                            Title = sheet1.Cells[row, 3].Text,
                            XSource = sheet1.Cells[4, sheet1.Cells["W1"].Start.Column, 4, sheet1.Cells["AO1"].Start.Column],
                            YSource = sheet1.Cells[row, sheet1.Cells["W1"].Start.Column, row, sheet1.Cells["AO1"].Start.Column],
                        };
                        chartInfo.ChartSources.Add(source);
                    }

                    EPPlusExcelDrawerHelper.CreateLineChart(chartInfo);
                }

                {
                    var chartInfo = new EPlusChartInfo()
                    {
                        WorkSheet = sheet1,
                        Title = "图表标题",
                        XTitle = "X轴标题",
                        YTitle = "Y轴标题",
                        ChartAddress = sheet1.Cells[30, 20, 30, 40],
                    };

                    for (int row = 12; row <= 14; row++)
                    {
                        var source = new EPlusChartDataSourceInfo
                        {
                            Title = sheet1.Cells[row, 3].Text,
                            XSource = sheet1.Cells[11, sheet1.Cells["W1"].Start.Column, 11, sheet1.Cells["AO1"].Start.Column],
                            YSource = sheet1.Cells[row, sheet1.Cells["W1"].Start.Column, row, sheet1.Cells["AO1"].Start.Column],
                        };
                        chartInfo.ChartSources.Add(source);
                    }

                    EPPlusExcelDrawerHelper.CreateLineChart(chartInfo);
                }

                {
                    var chartInfo = new EPlusChartInfo()
                    {
                        WorkSheet = sheet1,
                        Title = "图表标题",
                        XTitle = "X轴标题",
                        YTitle = "Y轴标题",
                        ChartAddress = sheet1.Cells[30, 40, 30, 60],
                    };

                    for (int row = 19; row <= 21; row++)
                    {
                        var source = new EPlusChartDataSourceInfo
                        {
                            Title = sheet1.Cells[row, 3].Text,
                            XSource = sheet1.Cells[18, sheet1.Cells["AP1"].Start.Column, 18, sheet1.Cells["BH1"].Start.Column],
                            YSource = sheet1.Cells[row, sheet1.Cells["AP1"].Start.Column, row, sheet1.Cells["BH1"].Start.Column],
                        };
                        chartInfo.ChartSources.Add(source);
                    }

                    EPPlusExcelDrawerHelper.CreateLineChart(chartInfo);
                }

                excel.SaveAs(new FileInfo(fileOut));
            }
        }
    }
}