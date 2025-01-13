using NPOI.SS.UserModel;
using NPOI.SS.UserModel.Charts;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace npoi_draw
{
    internal class NPOIChart
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

        private string file1 = @"D:\Code\dotnet\demo\Excel\T22111211C-RateCC-CAP&DCR01&DC-Rate01&cycle01 - 副本.xlsx";
        private string file2 = @"D:\Code\dotnet\demo\Excel\T22111211C-RateCC-CAP&DCR01&DC-Rate01 - 副本.xlsx";

        void Method1()
        {
            var fileOut = Path.Combine(@"D:\Code\dotnet\demo\Excel", $"out -{DateTime.Now:yyMMddHHmmss}.xlsx");

            var bars = new[]
            {
                "0122811200066",
                "0122811200098",
                "0FNCE840420101CAA0001993",
                "0FNCE840420101CAA0002251",
                "0FNCE840420101CAA0002360",
            };

            using (var stream = File.OpenRead(file1))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                DrawChart(workbook, bars);

                using (FileStream fs = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    workbook.Write(fs);
                }
            }
        }

        void Method2()
        {
            var fileOut = Path.Combine(@"D:\Code\dotnet\demo\Excel", $"out -{DateTime.Now:yyMMddHHmmss}.xlsx");

            var bars = new[]
            {
               "0122811200075",
               "0FNCE840420101CAA0002145",
               "0FNCE840420101CAA0002149",
               "0FNCE840420101CAA0002219",
               "0FNCE840420101CAA0002220",
               "0FNCE840420101CAA0002233",
               "0FNCE840420101CAA0002241",
               "0FNCE840420101CAA0002253",
               "0FNCE840420101CAA0002283",
               "0FNCE840420101CAA0002290",
               "0FNCE840420101CAA0002291",
               "0FNCE840420101CAA0002343",
               "0FNCE840420101CAA0002407"
            };

            using (var stream = File.OpenRead(file2))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                DrawChart(workbook, bars);

                using (FileStream fs = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    workbook.Write(fs);
                }
            }
        }

        protected virtual void DrawChart(IWorkbook workbook, string[] bars)
        {
            var theme = "充电";

            var dataBarCodes = bars.Select(o => new
            {
                Group = "",
                BarCode = o
            }).Distinct().ToList();

            /*
             *  ******************************************************************************************
             *                                      Summary图表
             *  ******************************************************************************************
             */
            var workSheet = workbook.GetSheet("Summary");
            if (workSheet == null)
            {
                return;
            }

            workSheet.CreateRow(workSheet.LastRowNum + 3 + 15);

            for (int i = 0; i <= workSheet.LastRowNum; i++)
            {
                var cell = workSheet.GetRow(i)?.GetCell(D1.Column);
                if (cell == null || cell.CellType != CellType.String)
                {
                    continue;
                }

                if (cell.StringCellValue.Contains($"倍率{theme}容量"))
                {
                    for (int j = 0; j <= workSheet.GetRow(i).LastCellNum; j++)
                    {
                        cell = workSheet.GetRow(i)?.GetCell(j);
                        if (cell == null || cell.CellType != CellType.String)
                        {
                            continue;
                        }

                        if (cell.StringCellValue.Contains($"倍率{theme}容量保持率"))
                        {
                            CreateCapacityRetention(i, j);
                        }
                    }

                    continue;
                }

                if (cell.StringCellValue.Contains($"倍率{theme}能量"))
                {
                    for (int j = 0; j <= workSheet.GetRow(i).LastCellNum; j++)
                    {
                        cell = workSheet.GetRow(i)?.GetCell(j);
                        if (cell == null || cell.CellType != CellType.String)
                        {
                            continue;
                        }

                        if (cell.StringCellValue.Contains($"倍率{theme}能量保持率"))
                        {
                            CreateEnergyRetention(i, j);
                        }
                    }

                    continue;
                }

                if (cell.StringCellValue.Contains($"{theme}前温度"))
                {
                    for (int j = 0; j <= workSheet.GetRow(i).LastCellNum; j++)
                    {
                        cell = workSheet.GetRow(i)?.GetCell(j);
                        if (cell == null || cell.CellType != CellType.String)
                        {
                            continue;
                        }

                        if (cell.StringCellValue.Contains($"{theme}温升"))
                        {
                            CreateTemperatureIncrease(i, j);
                        }
                    }

                    break;
                }
            }

            void CreateCapacityRetention(int row, int col)
            {
                var range = GetMergedCellRange(workSheet, workSheet.GetRow(row).GetCell(col));
                var firstCol = range.FirstColumn;
                var lastCol = range.LastColumn;

                //标题需要第一个倍率
                var firstRate = workSheet.GetRow(row + 1).GetCell(firstCol).StringCellValue;
                if (firstRate.IndexOf('-') > 0)
                {
                    firstRate = firstRate.Substring(0, firstRate.IndexOf('-'));
                }

                var lineChart = new ChartInfo<string, double>
                {
                    WorkSheet = workSheet,
                    Title = $"倍率{theme}容量保持率 vs {firstRate}",
                    ChartAddress = new CellRangeAddress(workSheet.LastRowNum - 15, workSheet.LastRowNum, B1.Column, G1.Column),
                    ChartSources = new List<ChartDataSourceInfo<string, double>>(),
                    XTitle = "Rate",
                    YTitle = "Capacity Retention"
                };

                for (int i = 0; i < dataBarCodes.Count; i++)
                {
                    lineChart.ChartSources.Add(new ChartDataSourceInfo<string, double>
                    {
                        Title = dataBarCodes[i].BarCode,
                        XSource = DataSources.FromStringCellRange(workSheet, new CellRangeAddress(row + 1, row + 1, firstCol, lastCol)),
                        YSource = DataSources.FromNumericCellRange(workSheet, new CellRangeAddress(row + 2 + i, row + 2 + i, firstCol, lastCol))
                    });
                }

                CreateLineChart(lineChart);
            }

            void CreateEnergyRetention(int row, int col)
            {
                var range = GetMergedCellRange(workSheet, workSheet.GetRow(row).GetCell(col));
                var firstCol = range.FirstColumn;
                var lastCol = range.LastColumn;

                //标题需要第一个倍率
                var firstRate = workSheet.GetRow(row + 1).GetCell(firstCol).StringCellValue;
                if (firstRate.IndexOf('-') > 0)
                {
                    firstRate = firstRate.Substring(0, firstRate.IndexOf('-'));
                }

                var lineChart = new ChartInfo<string, double>
                {
                    WorkSheet = workSheet,
                    Title = $"倍率{theme}能量保持率 vs {firstRate}",
                    ChartAddress = new CellRangeAddress(workSheet.LastRowNum - 15, workSheet.LastRowNum, H1.Column, M1.Column),
                    ChartSources = new List<ChartDataSourceInfo<string, double>>(),
                    XTitle = "Rate",
                    YTitle = "Energy Retention"
                };

                for (int i = 0; i < dataBarCodes.Count; i++)
                {
                    lineChart.ChartSources.Add(new ChartDataSourceInfo<string, double>
                    {
                        Title = dataBarCodes[i].BarCode,
                        XSource = DataSources.FromStringCellRange(workSheet, new CellRangeAddress(row + 1, row + 1, firstCol, lastCol)),
                        YSource = DataSources.FromNumericCellRange(workSheet, new CellRangeAddress(row + 2 + i, row + 2 + i, firstCol, lastCol))
                    });
                }

                CreateLineChart(lineChart);
            }

            void CreateTemperatureIncrease(int row, int col)
            {
                var range = GetMergedCellRange(workSheet, workSheet.GetRow(row).GetCell(col));
                var firstCol = range.FirstColumn;
                var lastCol = range.LastColumn;

                var lineChart = new ChartInfo<string, double>
                {
                    WorkSheet = workSheet,
                    Title = $"倍率{theme}温度升高",
                    ChartAddress = new CellRangeAddress(workSheet.LastRowNum - 15, workSheet.LastRowNum, N1.Column, S1.Column),
                    ChartSources = new List<ChartDataSourceInfo<string, double>>(),
                    XTitle = "Rate",
                    YTitle = "Temperature rise ℃"
                };

                for (int i = 0; i < dataBarCodes.Count; i++)
                {
                    lineChart.ChartSources.Add(new ChartDataSourceInfo<string, double>
                    {
                        Title = dataBarCodes[i].BarCode,
                        XSource = DataSources.FromStringCellRange(workSheet, new CellRangeAddress(row + 1, row + 1, firstCol, lastCol)),
                        YSource = DataSources.FromNumericCellRange(workSheet, new CellRangeAddress(row + 2 + i, row + 2 + i, firstCol, lastCol))
                    });
                }

                CreateLineChart(lineChart);
            }

            CellRangeAddress GetMergedCellRange(ISheet sheet, ICell cell)
            {
                for (int i = 0; i < workSheet.NumMergedRegions; i++)
                {
                    CellRangeAddress cellRangeAddress = workSheet.GetMergedRegion(i);
                    if (cellRangeAddress.FirstColumn <= cell.ColumnIndex &&
                        cellRangeAddress.LastColumn >= cell.ColumnIndex &&
                        cellRangeAddress.FirstRow <= cell.RowIndex &&
                        cellRangeAddress.LastRow >= cell.RowIndex)
                    {
                        return cellRangeAddress;
                    }
                }

                return new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex);
            }

            /*
             *  ******************************************************************************************
             *                                      BarCode图表
             *  ******************************************************************************************
             */
            workSheet.CreateRow(workSheet.LastRowNum + 3 + 1 + 15);
            for (int i = 0; i < dataBarCodes.Count; i++)
            {
                if (workSheet.GetRow(workSheet.LastRowNum - 15 - 1) == null)
                {
                    workSheet.CreateRow(workSheet.LastRowNum - 15 - 1)
                        .CreateCell(B1.Column + 6 * i).SetCellValue(dataBarCodes[i].BarCode);
                }
                else
                {
                    workSheet.GetRow(workSheet.LastRowNum - 15 - 1)
                        .CreateCell(B1.Column + 6 * i).SetCellValue(dataBarCodes[i].BarCode);
                }

                var chart = new ChartInfo<double, double>
                {
                    WorkSheet = workSheet,
                    Title = $"Capacity&Voltage curves during rate discharge",
                    ChartAddress = new CellRangeAddress(workSheet.LastRowNum - 15, workSheet.LastRowNum, B1.Column + i * 6, G1.Column + i * 6),
                    ChartSources = new List<ChartDataSourceInfo<double, double>>(),
                    XTitle = "Capacity/Ah",
                    YTitle = "Voltage/V"
                };

                var dataSheet = workbook.GetSheet(dataBarCodes[i].BarCode);
                if (dataSheet == null)
                {
                    continue;
                }

                for (int col = 0; col <= dataSheet.GetRow(0).LastCellNum; col++)
                {
                    var cell = dataSheet.GetRow(0)?.GetCell(col);
                    if (cell == null || cell.CellType != CellType.String || string.IsNullOrEmpty(cell.StringCellValue))
                    {
                        continue;
                    }

                    chart.ChartSources.Add(new ChartDataSourceInfo
                    {
                        Title = cell.StringCellValue,
                        XSource = DataSources.FromNumericCellRange(dataSheet,
                                    new CellRangeAddress(B3.Row, dataSheet.LastRowNum, col + 3, col + 3)),
                        YSource = DataSources.FromNumericCellRange(dataSheet,
                                    new CellRangeAddress(B3.Row, dataSheet.LastRowNum, col, col)),
                    });
                }

                CreateScatterChart(chart);
            }

            workSheet.CreateRow(workSheet.LastRowNum + 3 + 1 + 15);
            for (int i = 0; i < dataBarCodes.Count; i++)
            {
                if (workSheet.GetRow(workSheet.LastRowNum - 15 - 1) == null)
                {
                    workSheet.CreateRow(workSheet.LastRowNum - 15 - 1)
                        .CreateCell(B1.Column + 6 * i).SetCellValue(dataBarCodes[i].BarCode);
                }
                else
                {
                    workSheet.GetRow(workSheet.LastRowNum - 15 - 1)
                        .CreateCell(B1.Column + 6 * i).SetCellValue(dataBarCodes[i].BarCode);
                }

                var chart = new ChartInfo<double, double>
                {
                    WorkSheet = workSheet,
                    Title = $"Energy&Voltage curves during rate discharge",
                    ChartAddress = new CellRangeAddress(workSheet.LastRowNum - 15, workSheet.LastRowNum, B1.Column + i * 6, G1.Column + i * 6),
                    ChartSources = new List<ChartDataSourceInfo<double, double>>(),
                    XTitle = "Energy/Wh",
                    YTitle = "Voltage/V"
                };

                var dataSheet = workbook.GetSheet(dataBarCodes[i].BarCode);
                if (dataSheet == null)
                {
                    continue;
                }

                for (int col = 0; col <= dataSheet.GetRow(0).LastCellNum; col++)
                {
                    var cell = dataSheet.GetRow(0)?.GetCell(col);
                    if (cell == null || cell.CellType != CellType.String || string.IsNullOrEmpty(cell.StringCellValue))
                    {
                        continue;
                    }

                    chart.ChartSources.Add(new ChartDataSourceInfo
                    {
                        Title = cell.StringCellValue,
                        XSource = DataSources.FromNumericCellRange(dataSheet,
                            new CellRangeAddress(B3.Row, dataSheet.LastRowNum, col + 4, col + 4)),
                        YSource = DataSources.FromNumericCellRange(dataSheet,
                            new CellRangeAddress(B3.Row, dataSheet.LastRowNum, col, col)),
                    });
                }

                CreateScatterChart(chart);
            }

            workSheet.CreateRow(workSheet.LastRowNum + 3 + 1 + 15);
            for (int i = 0; i < dataBarCodes.Count; i++)
            {
                if (workSheet.GetRow(workSheet.LastRowNum - 15 - 1) == null)
                {
                    workSheet.CreateRow(workSheet.LastRowNum - 15 - 1)
                        .CreateCell(B1.Column + 6 * i).SetCellValue(dataBarCodes[i].BarCode);
                }
                else
                {
                    workSheet.GetRow(workSheet.LastRowNum - 15 - 1)
                        .CreateCell(B1.Column + 6 * i).SetCellValue(dataBarCodes[i].BarCode);
                }

                var chart = new ChartInfo<double, double>
                {
                    WorkSheet = workSheet,
                    Title = $"Temp.rising during rate discharge(Positive weldings)",
                    ChartAddress = new CellRangeAddress(workSheet.LastRowNum - 15, workSheet.LastRowNum, B1.Column + i * 6, G1.Column + i * 6),
                    ChartSources = new List<ChartDataSourceInfo<double, double>>(),
                    XTitle = "Capacity/Ah",
                    YTitle = "Temperature rising(℃)"
                };

                var dataSheet = workbook.GetSheet(dataBarCodes[i].BarCode);
                if (dataSheet == null)
                {
                    continue;
                }

                for (int col = 0; col <= dataSheet.GetRow(0).LastCellNum; col++)
                {
                    var cell = dataSheet.GetRow(0)?.GetCell(col);
                    if (cell == null || cell.CellType != CellType.String || string.IsNullOrEmpty(cell.StringCellValue))
                    {
                        continue;
                    }

                    chart.ChartSources.Add(new ChartDataSourceInfo
                    {
                        Title = cell.StringCellValue,
                        XSource = DataSources.FromNumericCellRange(dataSheet,
                            new CellRangeAddress(B3.Row, dataSheet.LastRowNum, col + 3, col + 3)),
                        YSource = DataSources.FromNumericCellRange(dataSheet,
                            new CellRangeAddress(B3.Row, dataSheet.LastRowNum, col + 2, col + 2)),
                    });
                }

                CreateScatterChart(chart);
            }
        }

        protected virtual IChart CreateLineChart<TX, TY>(ChartInfo<TX, TY> chartInfo)
        {
            return ExcelDrawerHelper.CreateLineChart(chartInfo);
        }

        protected virtual IChart CreateScatterChart<TX, TY>(ChartInfo<TX, TY> chartInfo)
        {
            return ExcelDrawerHelper.CreateScatterChart(chartInfo);
        }

        protected static CellAddress B1 = new CellAddress("B1");
        protected static CellAddress D1 = new CellAddress("D1");
        protected static CellAddress F1 = new CellAddress("F1");
        protected static CellAddress G1 = new CellAddress("G1");
        protected static CellAddress H1 = new CellAddress("H1");
        protected static CellAddress L1 = new CellAddress("L1");
        protected static CellAddress M1 = new CellAddress("M1");
        protected static CellAddress N1 = new CellAddress("N1");
        protected static CellAddress R1 = new CellAddress("R1");
        protected static CellAddress S1 = new CellAddress("S1");
        protected static CellAddress T1 = new CellAddress("T1");
        protected static CellAddress X1 = new CellAddress("X1");

        protected static CellAddress H4 = new CellAddress("H4");
        protected static CellAddress K4 = new CellAddress("K4");
        protected static CellAddress P4 = new CellAddress("P4");
        protected static CellAddress S4 = new CellAddress("S4");
        protected static CellAddress AB4 = new CellAddress("AB4");
        protected static CellAddress AE4 = new CellAddress("AE4");
        protected static CellAddress B3 = new CellAddress("B3");
        protected static CellAddress D3 = new CellAddress("D3");
        protected static CellAddress E3 = new CellAddress("E3");
        protected static CellAddress F3 = new CellAddress("F3");
        protected static CellAddress H3 = new CellAddress("H3");
        protected static CellAddress J3 = new CellAddress("J3");
        protected static CellAddress K3 = new CellAddress("K3");
        protected static CellAddress L3 = new CellAddress("L3");
        protected static CellAddress N3 = new CellAddress("N3");
        protected static CellAddress P3 = new CellAddress("P3");
        protected static CellAddress Q3 = new CellAddress("Q3");
        protected static CellAddress R3 = new CellAddress("R3");
        protected static CellAddress T3 = new CellAddress("T3");
        protected static CellAddress V3 = new CellAddress("V3");
        protected static CellAddress W3 = new CellAddress("W3");
        protected static CellAddress X3 = new CellAddress("X3");
    }
}
